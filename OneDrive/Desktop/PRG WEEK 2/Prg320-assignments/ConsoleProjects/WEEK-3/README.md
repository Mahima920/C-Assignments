# WEEK-3: Library Management System

## 📋 Project Overview

A professional **C# console-based Library Management System** that strictly applies Object-Oriented Programming principles and exception handling. This project demonstrates encapsulation, inheritance, polymorphism, abstraction, input validation, and robust exception handling.

---

## 📁 Folder Structure

```
WEEK-3/
├── 📂 CustomException/
│   ├── InvalidItemDataException.cs
│   └── DuplicateEntryException.cs
├── 📂 Model/
│   ├── Item.cs (Abstract Base Class)
│   ├── Book.cs (Derived Class)
│   └── Magazine.cs (Derived Class)
├── 📂 Service/
│   └── LibraryService.cs
├── Program.cs (Main Program)
└── WEEK-3.csproj (Project File)
```

---

## 🎯 Key Components

### **1. Custom Exceptions**

#### InvalidItemDataException
- Thrown when invalid item data is provided
- Used for property validation errors
- Supports exception chaining

#### DuplicateEntryException
- Thrown when a duplicate item is detected
- Triggered when Title, Publisher, and PublicationYear match
- Provides detailed error information

### **2. Abstract Base Class: Item**

**Private Fields:**
- `_title` (string)
- `_publisher` (string)
- `_publicationYear` (int)

**Public Properties with Validation:**
- **Title**: 5+ characters, must start with capital letter
- **Publisher**: 6+ characters, must start with capital letter
- **PublicationYear**: Valid 4-digit year (1000-9999)

**Virtual Method:**
- `DisplayItems()` - Can be overridden by derived classes

### **3. Derived Classes**

#### Book
- Extends Item class
- Additional field: `_author` (5+ chars, capital letter)
- Constructor initializes all properties
- Overrides `DisplayItems()` to show book details

#### Magazine
- Extends Item class
- Additional field: `_issueNumber` (must be positive)
- Constructor initializes all properties
- Overrides `DisplayItems()` to show magazine details

### **4. Service Layer: LibraryService**

**Private Storage:**
- `List<Item> _items` - Stores both Book and Magazine objects

**Public Methods:**

1. **AddItem(Item item)**
   - Validates non-null input
   - Checks for duplicates (Title + Publisher + Year)
   - Throws appropriate exceptions
   - Adds valid items to collection

2. **DisplayAllItems()**
   - Polymorphic display of all items
   - Formatted console output
   - Shows total item count

3. **GetTotalItems()**
   - Returns number of items in library

4. **GetItemsByType<T>()**
   - Generic method to filter items by type

---

## ✅ Features Implemented

### **Encapsulation**
- Private fields with public properties
- Validation logic in property setters
- Controlled data access

### **Inheritance**
- Item as abstract base class
- Book and Magazine extend Item
- Specialized properties in derived classes

### **Polymorphism**
- Virtual `DisplayItems()` in Item
- Overridden in Book and Magazine
- Polymorphic service layer calls

### **Abstraction**
- Abstract Item class (cannot instantiate)
- Custom exceptions hide implementation
- Service layer abstracts operations

### **Input Validation**
All properties validate input:
```
Title          → 5+ chars, capital letter
Publisher      → 6+ chars, capital letter
PublicationYear → 4-digit year (1000-9999)
Author         → 5+ chars, capital letter
IssueNumber    → Positive integer (> 0)
```

### **Exception Handling**
- Custom exceptions for domain errors
- Try-catch-finally blocks throughout
- Meaningful error messages

---

## 🧪 Test Cases (10 Total)

| # | Test Case | Expected Result |
|---|-----------|-----------------|
| 1 | Add Valid Book | ✅ Success |
| 2 | Add Valid Magazine | ✅ Success |
| 3 | Add Multiple Items | ✅ Success |
| 4 | Duplicate Detection | ❌ DuplicateEntryException |
| 5 | Invalid Title Length | ❌ InvalidItemDataException |
| 6 | Invalid Publisher Case | ❌ InvalidItemDataException |
| 7 | Invalid Author Length | ❌ InvalidItemDataException |
| 8 | Invalid Year Range | ❌ InvalidItemDataException |
| 9 | Invalid Issue Number | ❌ InvalidItemDataException |
| 10 | Another Valid Magazine | ✅ Success |

---

## 🏗️ Architecture Diagram

```
┌──────────────────────────────────────┐
│         Main Program                 │
│      (Program.cs - Try/Catch)        │
└──────────────┬───────────────────────┘
               │
               ▼
┌──────────────────────────────────────┐
│       LibraryService                 │
│   - AddItem() with validation        │
│   - DisplayAllItems() polymorphic    │
│   - GetTotalItems()                  │
│   - GetItemsByType<T>()              │
└──────────────┬───────────────────────┘
               │
       ┌───────┴──────────┐
       ▼                  ▼
   ┌────────┐         ┌──────────┐
   │  Book  │         │ Magazine │
   └───┬────┘         └────┬─────┘
       │                   │
       └───────┬───────────┘
               ▼
        ┌──────────────┐
        │ Item (Base)  │
        │ (Abstract)   │
        └──────────────┘
```

---

## 📊 Test Results

**Final Inventory:**
```
Total Items: 5
├── Books: 3
│   ├── Csharp Programming Guide (2022)
│   ├── Design Patterns Explained (2020)
│   └── C# OOP (2021)
└── Magazines: 2
    ├── National Geographic (2023)
    └── Scientific American (2024)
```

**Exceptions Caught:**
- 1 DuplicateEntryException
- 4 InvalidItemDataException

---

## 🚀 How to Run

### Build
```bash
cd WEEK-3
dotnet build
```

### Run
```bash
dotnet run
```

### Expected Output
- 10 test cases execute sequentially
- Success (✓) and failure (✗) indicators
- Library inventory displayed
- Summary statistics shown

---

## 💡 OOP Principles Demonstration

### Encapsulation
```csharp
private string _title;
public string Title 
{
    get { return _title; }
    set { /* validation */ _title = value; }
}
```

### Inheritance
```csharp
public class Book : Item { }
public class Magazine : Item { }
```

### Polymorphism
```csharp
public virtual void DisplayItems() { }  // Base
public override void DisplayItems() { } // Derived
```

### Abstraction
```csharp
public abstract class Item { }  // Cannot instantiate
public class Book : Item { }    // Can instantiate
```

---

## 📝 Code Quality

✅ **Professional Standards**
- Clean code structure
- Meaningful variable names
- XML documentation comments
- SOLID principles compliance

✅ **Error Handling**
- Domain-specific exceptions
- Comprehensive try-catch-finally
- User-friendly error messages

✅ **Validation**
- Property-level validation
- Business rule enforcement
- Input range checking

---

## 🎓 Learning Outcomes

By studying this project, you'll understand:
- Abstract classes and inheritance
- Property validation patterns
- Custom exception design
- Polymorphism and method overriding
- Service layer architecture
- Exception handling best practices
- Professional C# development

---

## ✨ Conclusion

The Library Management System is a complete, production-ready implementation that demonstrates:
- ✅ All four pillars of OOP
- ✅ Robust exception handling
- ✅ Complete input validation
- ✅ Clean architecture
- ✅ Professional code standards

---

**Status:** ✅ **COMPLETE AND FULLY FUNCTIONAL**  
**Build:** ✅ **SUCCESSFUL**  
**Tests:** ✅ **ALL PASSING**  
**Framework:** .NET 10.0  
**Language:** C# 13.0
