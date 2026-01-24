import json                      # JSON file handling
import smtplib                   # Send emails using SMTP
import tkinter as tk             # Base Tkinter GUI window
from dataclasses import dataclass  # Task data structure
from datetime import datetime    # Date & time handling
from email.message import EmailMessage  # Email message builder
from pathlib import Path         # File path handling
from tkinter import messagebox, ttk  # GUI widgets
from typing import List, Optional  # Type hints

from apscheduler.schedulers.background import BackgroundScheduler # Background scheduler ma reminder check garna 

# ============================================================
# EMAIL SETTINGS (YOUR VALUES)
# ============================================================
SMTP_HOST = "smtp.gmail.com"
SMTP_PORT = 587
SMTP_USER = "mahimaadhikari456@gmail.com"
SMTP_PASS = "xxxx xxxx xxxx xxxxx" 
FROM_EMAIL = "mahimaadhikari456@gmail.com"
TO_EMAIL = "mahii20054@gmail.com"

# ============================================================
# APP SETTINGS
# ============================================================
TASK_FILE = Path("tasks.json")

# Light pink theme
PINK_BG = "#fadbe8"
PINK_PANEL = "#ffe6f0"
WHITE = "#ffffff"
TEXT = "#222222"

PRIORITY_LABELS = {1: "High", 2: "Medium", 3: "Low"}


# -------------------- MODEL --------------------
@dataclass # Task class automatic __init__ generate garcha.
class Task: #Task ko structure define garcha.
    id: int
    title: str
    due_at: Optional[datetime]
    priority: int
    completed: bool
    remind_before_minutes: int
    reminder_sent: bool #reminder already send bhayo ki bhayena (duplicate email rokna).


# -------------------- STORAGE --------------------
def _ensure_file() -> None:
    if not TASK_FILE.exists(): #file cha ki chaina check garcha.
        TASK_FILE.write_text("[]", encoding="utf-8") #chaina bhane empty JSON list [] le create garcha.


def _load_raw() -> list: 
    _ensure_file()
    return json.loads(TASK_FILE.read_text(encoding="utf-8")) #file read garera Python list of dict ma convert garcha.


def _save_raw(data: list) -> None:
    TASK_FILE.write_text(json.dumps(data, indent=2), encoding="utf-8")


def _parse_dt(s: Optional[str]) -> Optional[datetime]:
    return datetime.fromisoformat(s) if s else None #string iso format bata datetime object banaune.


def _to_iso(dt: Optional[datetime]) -> Optional[str]:
    return dt.isoformat(timespec="minutes") if dt else None


def list_tasks() -> List[Task]:
    data = _load_raw() #JSON list load
    tasks: List[Task] = [] #empty Task list.
    for t in data:
        tasks.append(
            Task(
                id=int(t["id"]),
                title=str(t["title"]),
                due_at=_parse_dt(t.get("due_at")),
                priority=int(t.get("priority", 2)),
                completed=bool(t.get("completed", False)),
                remind_before_minutes=int(t.get("remind_before_minutes", 60)),
                reminder_sent=bool(t.get("reminder_sent", False)),
            )
        )
    return tasks


def add_task(title: str, due_at: Optional[datetime], priority: int, remind_before_minutes: int) -> None:
    data = _load_raw()
    new_id = max([int(t["id"]) for t in data], default=0) + 1 # existing max id + 1 = new unique id.
    data.append(
        {
            "id": new_id,
            "title": title.strip(),
            "due_at": _to_iso(due_at),
            "priority": int(priority),
            "completed": False,
            "remind_before_minutes": int(remind_before_minutes),
            "reminder_sent": False,
        }
    )
    _save_raw(data)


def update_task(task_id: int, title: str, due_at: Optional[datetime], priority: int, remind_before_minutes: int) -> None:
    data = _load_raw()
    for t in data:
        if int(t["id"]) == int(task_id):
            t["title"] = title.strip()
            t["due_at"] = _to_iso(due_at)
            t["priority"] = int(priority)
            t["remind_before_minutes"] = int(remind_before_minutes)
            t["reminder_sent"] = False  # allow reminders again if edited
            break
    _save_raw(data)


def delete_task(task_id: int) -> None:
    data = _load_raw()
    data = [t for t in data if int(t["id"]) != int(task_id)]
    _save_raw(data)


def set_completed(task_id: int, completed: bool) -> None:
    data = _load_raw()
    for t in data:
        if int(t["id"]) == int(task_id):
            t["completed"] = bool(completed)
            if completed:
                t["reminder_sent"] = True  # stop reminders when done
            else:
                t["reminder_sent"] = False  # allow reminders again if incomplete
            break
    _save_raw(data)


def mark_reminder_sent(task_id: int) -> None:
    data = _load_raw()
    for t in data:
        if int(t["id"]) == int(task_id):
            t["reminder_sent"] = True
            break
    _save_raw(data)


# -------------------- EMAIL + REMINDERS --------------------
def send_email(subject: str, body: str) -> None:
    msg = EmailMessage()
    msg["From"] = FROM_EMAIL
    msg["To"] = TO_EMAIL
    msg["Subject"] = subject
    msg.set_content(body)

    with smtplib.SMTP(SMTP_HOST, SMTP_PORT) as s:
        s.starttls()
        s.login(SMTP_USER, SMTP_PASS)
        s.send_message(msg)


def tasks_needing_reminder(now: datetime) -> List[Task]:
    # Small tolerance so it doesn't feel late
    tolerance_seconds = 10
    due_tasks: List[Task] = []

    for t in list_tasks():
        if t.completed or t.reminder_sent or not t.due_at:
            continue

        remind_at_ts = t.due_at.timestamp() - (t.remind_before_minutes * 60)

        # send at first check after remind time
        if (now.timestamp() + tolerance_seconds) >= remind_at_ts and now < t.due_at:
            due_tasks.append(t)

    return due_tasks


def run_reminder_check() -> None:
    now = datetime.now()
    for t in tasks_needing_reminder(now):
        try:
            send_email(
                subject=f"Task Reminder: {t.title}",
                body=(
                    "Reminder!\n\n"
                    f"Task: {t.title}\n"
                    f"Due: {t.due_at.strftime('%Y-%m-%d %H:%M')}\n"
                ),
            )
            mark_reminder_sent(t.id)
            print(f"Reminder sent for task #{t.id}: {t.title}")
        except Exception as e:
            print("Email error:", e)


# -------------------- UI HELPERS --------------------
def parse_due(dt_str: str) -> Optional[datetime]:
    s = dt_str.strip() # spaces remove
    if not s:
        return None #blank means no due date
    return datetime.strptime(s, "%Y-%m-%d %H:%M")


def parse_remind_before(text: str) -> int:
    """
    Accepts:
      - '12h', '1.5h'
      - '60m'
      - '90' (minutes)
    """
    s = text.strip().lower().replace(" ", "")
    if not s:
        return 60
    try:
        if s.endswith("h"):
            return max(0, int(float(s[:-1]) * 60))
        if s.endswith("m"):
            return max(0, int(float(s[:-1])))
        return max(0, int(float(s)))
    except ValueError:
        raise ValueError("Invalid reminder format. Use 12h or 60m.")


# -------------------- UI --------------------
class TodoApp(ttk.Frame):
    def __init__(self, master: tk.Tk):
        super().__init__(master, padding=12) #base frame init
        self.master = master
        self.selected_task_id: Optional[int] = None

        self._style()
        self._build()
        self.refresh()

    def _style(self) -> None:
        self.master.title("To-Do List Application")
        self.master.geometry("980x560")
        self.master.configure(bg=PINK_BG)

        style = ttk.Style()
        try:
            style.theme_use("clam")
        except Exception:
            pass

        style.configure(".", background=PINK_BG, foreground=TEXT)
        style.configure("TFrame", background=PINK_BG)
        style.configure("TLabel", background=PINK_BG, foreground=TEXT)

        style.configure("TEntry", fieldbackground=WHITE, foreground=TEXT)
        style.configure("TCombobox", fieldbackground=WHITE, foreground=TEXT)

        style.configure("Treeview", background=WHITE, fieldbackground=WHITE, foreground=TEXT, rowheight=26)
        style.configure("Treeview.Heading", background=PINK_PANEL, foreground=TEXT)

        style.configure("TButton", padding=6)

    def _build(self) -> None:
        top = ttk.Frame(self)
        top.pack(fill="x", pady=(0, 10))


        ttk.Label(top, text="To-Do List", font=("Segoe UI", 16, "bold"), anchor="center").pack(fill="x")

        main = ttk.Frame(self)
        main.pack(fill="both", expand=True)

        # LEFT: list
        left = ttk.Frame(main)
        left.pack(side="left", fill="both", expand=True)

        columns = ("id", "title", "due", "priority", "status")
        self.tree = ttk.Treeview(left, columns=columns, show="headings", height=18)

        self.tree.heading("id", text="ID")
        self.tree.heading("title", text="Title")
        self.tree.heading("due", text="Due (YYYY-MM-DD HH:MM)")
        self.tree.heading("priority", text="Priority")
        self.tree.heading("status", text="Status")

        self.tree.column("id", width=55, anchor="center")
        self.tree.column("title", width=460, anchor="w")
        self.tree.column("due", width=200, anchor="w")
        self.tree.column("priority", width=90, anchor="center")
        self.tree.column("status", width=140, anchor="center")

        self.tree.pack(fill="both", expand=True)
        self.tree.bind("<<TreeviewSelect>>", self.on_select)

        btn_row = ttk.Frame(left)
        btn_row.pack(fill="x", pady=(10, 0))

        ttk.Button(btn_row, text="Refresh", command=self.refresh).pack(side="left")
        ttk.Button(btn_row, text="Mark Complete", command=self.mark_complete).pack(side="left", padx=8)
        ttk.Button(btn_row, text="Mark Incomplete", command=self.mark_incomplete).pack(side="left")

        # RIGHT: form
        right = ttk.LabelFrame(main, text="Task Details", padding=12)
        right.pack(side="right", fill="y", padx=(12, 0))

        ttk.Label(right, text="Title").grid(row=0, column=0, sticky="w")
        self.title_var = tk.StringVar()
        ttk.Entry(right, textvariable=self.title_var, width=38).grid(row=1, column=0, sticky="ew", pady=(0, 10))

        ttk.Label(right, text="Due (optional) - YYYY-MM-DD HH:MM").grid(row=2, column=0, sticky="w")
        self.due_var = tk.StringVar()
        ttk.Entry(right, textvariable=self.due_var, width=38).grid(row=3, column=0, sticky="ew", pady=(0, 10))

        ttk.Label(right, text="Priority").grid(row=4, column=0, sticky="w")
        self.priority_combo = ttk.Combobox(
            right, values=["1 - High", "2 - Medium", "3 - Low"], state="readonly", width=35
        )
        self.priority_combo.current(1)
        self.priority_combo.grid(row=5, column=0, sticky="ew", pady=(0, 10))

        ttk.Label(right, text="Remind Before (e.g., 12h or 60m)").grid(row=6, column=0, sticky="w")
        self.remind_var = tk.StringVar(value="60m")
        ttk.Entry(right, textvariable=self.remind_var, width=38).grid(row=7, column=0, sticky="ew", pady=(0, 12))

        ttk.Button(right, text="Add Task", command=self.add_new).grid(row=8, column=0, sticky="ew")
        ttk.Button(right, text="Update Task", command=self.update_selected).grid(row=9, column=0, sticky="ew", pady=6)
        ttk.Button(right, text="Delete Task", command=self.delete_selected).grid(row=10, column=0, sticky="ew")

        ttk.Separator(right).grid(row=11, column=0, sticky="ew", pady=12)
        ttk.Button(right, text="Clear Form", command=self.clear_form).grid(row=12, column=0, sticky="ew")

        self.status_var = tk.StringVar(value="Ready")
        tk.Label(self.master, textvariable=self.status_var, bg=PINK_BG, fg=TEXT, anchor="w").pack(
            side="bottom", fill="x", padx=10, pady=6
        )

        self.pack(fill="both", expand=True)

    def _selected_id(self) -> Optional[int]:
        sel = self.tree.selection()
        if not sel:
            return None
        values = self.tree.item(sel[0], "values")
        return int(values[0])

    def refresh(self) -> None:
        now = datetime.now()

        for item in self.tree.get_children():
            self.tree.delete(item)

        tasks = list_tasks()

        # Overdue incomplete first, then normal incomplete, then completed
        def sort_key(t: Task):
            overdue = (not t.completed) and (t.due_at is not None) and (t.due_at < now)
            return (t.completed, not overdue, t.due_at is None, t.due_at or datetime.max, t.priority)

        tasks.sort(key=sort_key)

        for t in tasks:
            due = t.due_at.strftime("%Y-%m-%d %H:%M") if t.due_at else ""
            priority = PRIORITY_LABELS.get(t.priority, str(t.priority))

            is_overdue = (not t.completed) and (t.due_at is not None) and (t.due_at < now)

            if t.completed:
                status = "Complete"
            elif is_overdue:
                status = "Not Done (Overdue)"
            else:
                status = "Incomplete"

            self.tree.insert("", "end", values=(t.id, t.title, due, priority, status))

        self.selected_task_id = None
        self.status_var.set("Tasks refreshed.")

    def on_select(self, _event=None) -> None:
        task_id = self._selected_id()
        if task_id is None:
            return

        selected = next((t for t in list_tasks() if t.id == task_id), None)
        if not selected:
            return

        self.selected_task_id = task_id
        self.title_var.set(selected.title)
        self.due_var.set(selected.due_at.strftime("%Y-%m-%d %H:%M") if selected.due_at else "")
        self.priority_combo.current(max(0, min(2, selected.priority - 1)))

        if selected.remind_before_minutes % 60 == 0:
            self.remind_var.set(f"{selected.remind_before_minutes // 60}h")
        else:
            self.remind_var.set(f"{selected.remind_before_minutes}m")

    def clear_form(self) -> None:
        self.selected_task_id = None
        self.title_var.set("")
        self.due_var.set("")
        self.priority_combo.current(1)
        self.remind_var.set("60m")
        self.status_var.set("Form cleared.")

    def _validate_form(self):
        title = self.title_var.get().strip()
        if not title:
            messagebox.showerror("Validation", "Title cannot be empty.")
            return None

        try:
            remind = parse_remind_before(self.remind_var.get())
        except ValueError:
            messagebox.showerror("Validation", "Remind Before must be like 12h or 60m (example: 1.5h).")
            return None

        due_str = self.due_var.get().strip()
        try:
            due_at = parse_due(due_str) if due_str else None
        except ValueError:
            messagebox.showerror("Validation", "Due date must be: YYYY-MM-DD HH:MM (example: 2026-01-20 21:30)")
            return None

        priority = int(self.priority_combo.get().split("-")[0].strip())
        return title, due_at, priority, remind

    def add_new(self) -> None:
        data = self._validate_form()
        if not data:
            return
        title, due_at, priority, remind = data
        add_task(title, due_at, priority, remind)
        self.refresh()
        self.clear_form()
        self.status_var.set("Task added.")

    def update_selected(self) -> None:
        task_id = self._selected_id()
        if task_id is None:
            messagebox.showinfo("Update", "Select a task first.")
            return
        data = self._validate_form()
        if not data:
            return
        title, due_at, priority, remind = data
        update_task(task_id, title, due_at, priority, remind)
        self.refresh()
        self.status_var.set("Task updated.")

    def delete_selected(self) -> None:
        task_id = self._selected_id()
        if task_id is None:
            messagebox.showinfo("Delete", "Select a task first.")
            return
        if messagebox.askyesno("Confirm", "Delete this task?"):
            delete_task(task_id)
            self.refresh()
            self.clear_form()
            self.status_var.set("Task deleted.")

    def mark_complete(self) -> None:
        task_id = self._selected_id()
        if task_id is None:
            messagebox.showinfo("Complete", "Select a task first.")
            return
        set_completed(task_id, True)
        self.refresh()
        self.status_var.set("Marked complete.")

    def mark_incomplete(self) -> None:
        task_id = self._selected_id()
        if task_id is None:
            messagebox.showinfo("Incomplete", "Select a task first.")
            return
        set_completed(task_id, False)
        self.refresh()
        self.status_var.set("Marked incomplete (shows as Incomplete / Overdue).")


def main():
    scheduler = BackgroundScheduler(daemon=True) #background scheduler start
    scheduler.add_job(run_reminder_check, "interval", seconds=5)
    scheduler.start() # scheduler run

    root = tk.Tk() # main window create
    TodoApp(root) # UI attach
    root.mainloop()


if __name__ == "__main__": # file directly run huda main() call hunchha.
    main()