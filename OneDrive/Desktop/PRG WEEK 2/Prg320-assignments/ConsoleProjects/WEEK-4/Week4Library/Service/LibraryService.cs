using Week4Library.Interface;
using Week4Library.Model;
using Week4Library.CustomExpectation;

namespace Week4Library.Service
{
    // This class manages all library actions like add, remove, search, sort, save, and load
    public class LibraryService
    {
        // This list stores all library items (books, magazines, newspapers)
        private List<ILibraryItem> _items = new();

        // This file is used to save and load library data
        private const string FileName = "libraryFile.json";

        // When the program starts, this loads saved data from the file
        public LibraryService()
        {
            LoadData();
        }

        // Adds a new item to the library after checking for duplicates
        public void AddItem(ILibraryItem item)
        {
            if (CheckForDuplicates(item))
                throw new DuplicateItemException("This item already exists (same type + same details).");

            _items.Add(item);
            SaveData();
        }

        // Displays all items currently in the library
        public void DisplayAllItems()
        {
            if (_items.Count == 0)
            {
                Console.WriteLine("No items in the library yet.");
                return;
            }

            Console.WriteLine($"Total items: {_items.Count}");

            // Loop through each item and show its details
            for (int i = 0; i < _items.Count; i++)
            {
                Console.Write($"{i + 1}. ");
                _items[i].DisplayInfo();
            }
        }

        // Removes an item from the library using its title
        public void RemoveItem(string title)
        {
            // Check if the title is empty
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Title cannot be empty.");
                return;
            }

            // Find the item that matches the title (ignores case)
            var item = _items.FirstOrDefault(x =>
                x.Title.Equals(title.Trim(), StringComparison.OrdinalIgnoreCase));

            if (item == null)
            {
                Console.WriteLine("Item not found.");
                return;
            }

            // Remove the item and save the updated list
            _items.Remove(item);
            SaveData();
            Console.WriteLine("Item removed successfully.");
        }

        // Searches for items that contain the given title
        public void SearchByTitle(string title)
        {
            // Find all items that match the title
            var results = _items
                .Where(x => x.Title.Contains(title ?? "", StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (results.Count == 0)
            {
                Console.WriteLine("No items found with that title.");
                return;
            }

            Console.WriteLine($"Found {results.Count} item(s):");

            // Show each matching item
            foreach (var item in results)
                item.DisplayInfo();
        }

        // Searches only books by author name
        public void SearchByAuthor(string author)
        {
            // Filter only Book objects and match the author name
            var results = _items
                .OfType<Book>()
                .Where(b => b.Author.Contains(author ?? "", StringComparison.OrdinalIgnoreCase))
                .ToList();

            if (results.Count == 0)
            {
                Console.WriteLine("No books found with that author.");
                return;
            }

            Console.WriteLine($"Found {results.Count} book(s):");

            // Display all matching books
            foreach (var book in results)
                book.DisplayInfo();
        }

        // Sorts all items alphabetically by title
        public void SortByTitle()
        {
            _items = _items.OrderBy(x => x.Title).ToList();
            Console.WriteLine("Items sorted by title.");
        }

        // Sorts all items by publication year
        public void SortByYear()
        {
            _items = _items.OrderBy(x => x.PublicationYear).ToList();
            Console.WriteLine("Items sorted by year.");
        }

        // Checks if the new item already exists in the library
        // Step 1: Check same item type
        // Step 2: Check same title, publisher, and year
        // Step 3: Check type-specific detail
        private bool CheckForDuplicates(ILibraryItem newItem)
        {
            foreach (var existing in _items)
            {
                // Skip if item types are different
                if (existing.GetType() != newItem.GetType())
                    continue;

                // Check common fields
                bool sharedMatch =
                    existing.Title.Equals(newItem.Title, StringComparison.OrdinalIgnoreCase) &&
                    existing.Publisher.Equals(newItem.Publisher, StringComparison.OrdinalIgnoreCase) &&
                    existing.PublicationYear == newItem.PublicationYear;

                if (!sharedMatch)
                    continue;

                // Check extra fields based on item type
                if (existing is Book b1 && newItem is Book b2)
                    return b1.Author.Equals(b2.Author, StringComparison.OrdinalIgnoreCase);

                if (existing is Magazine m1 && newItem is Magazine m2)
                    return m1.IssueNumber == m2.IssueNumber;

                if (existing is Newspaper n1 && newItem is Newspaper n2)
                    return n1.Editor.Equals(n2.Editor, StringComparison.OrdinalIgnoreCase);
            }

            return false;
        }

        // Saves all library items into the file
        private void SaveData()
        {
            try
            {
                using StreamWriter writer = new StreamWriter(FileName);

                // Write each item as one line in the file
                foreach (var item in _items)
                {
                    writer.WriteLine(SerializeItem(item));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving data: " + ex.Message);
            }
        }

        // Loads library items from the file when the program starts
        private void LoadData()
        {
            try
            {
                if (!File.Exists(FileName))
                    return;

                _items.Clear();

                // Read each line and convert it back into an object
                foreach (string line in File.ReadAllLines(FileName))
                {
                    var item = DeserializeItem(line);
                    if (item != null)
                        _items.Add(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading data: " + ex.Message);
            }
        }

        // Converts a library object into a text format for saving
        private string SerializeItem(ILibraryItem item)
        {
            if (item is Book b)
                return $"BOOK|{b.Title}|{b.Publisher}|{b.PublicationYear}|{b.Author}";

            if (item is Magazine m)
                return $"MAGAZINE|{m.Title}|{m.Publisher}|{m.PublicationYear}|{m.IssueNumber}";

            if (item is Newspaper n)
                return $"NEWSPAPER|{n.Title}|{n.Publisher}|{n.PublicationYear}|{n.Editor}";

            return "";
        }

        // Converts saved text back into a library object
        private ILibraryItem? DeserializeItem(string line)
        {
            var parts = line.Split('|');

            // Make sure the data format is correct
            if (parts.Length < 5) return null;

            string type = parts[0];
            string title = parts[1];
            string publisher = parts[2];
            int year = int.Parse(parts[3]);
            string last = parts[4];

            // Create the correct object based on item type
            return type switch
            {
                "BOOK" => new Book(title, publisher, year, last),
                "MAGAZINE" => new Magazine(title, publisher, year, int.Parse(last)),
                "NEWSPAPER" => new Newspaper(title, publisher, year, last),
                _ => null
            };
        }
    }
}
