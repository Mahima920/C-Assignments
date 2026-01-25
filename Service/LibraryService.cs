using System;
using System.Collections.Generic;
using WEEK3.Model;
using WEEK3.CustomException;
using System.Linq;

namespace WEEK3.Service
{
    /// <summary>
    /// Service layer for managing library items.
    /// Demonstrates encapsulation and the service pattern.
    /// </summary>
    public class LibraryService
    {
        private List<Item> _items;

        public LibraryService()
        {
            _items = new List<Item>();
        }

        /// <summary>
        /// Adds an item to the library after checking for duplicates.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <exception cref="DuplicateEntryException">Thrown when a duplicate entry is found.</exception>
        public void AddItem(Item item)
        {
            if (item == null)
            {
                throw new InvalidItemDataException("Item cannot be null.");
            }

            // Check for duplicate entries by comparing Title, Publisher, and PublicationYear
            foreach (var existingItem in _items)
            {
                if (existingItem.Title == item.Title &&
                    existingItem.Publisher == item.Publisher &&
                    existingItem.PublicationYear == item.PublicationYear)
                {
                    throw new DuplicateEntryException(
                        $"Duplicate entry found: Title='{item.Title}', Publisher='{item.Publisher}', Year={item.PublicationYear}");
                }
            }

            _items.Add(item);
        }

        /// <summary>
        /// Displays all items in the library.
        /// Demonstrates polymorphism by calling the overridden DisplayItems() method.
        /// </summary>
        public void DisplayAllItems()
        {
            if (_items.Count == 0)
            {
                Console.WriteLine("\nNo items in the library.");
                return;
            }

            Console.WriteLine("\n========================================");
            Console.WriteLine("        LIBRARY INVENTORY");
            Console.WriteLine("========================================");

            foreach (var item in _items)
            {
                item.DisplayItems();
                Console.WriteLine();
            }

            Console.WriteLine("========================================");
            Console.WriteLine($"Total Items: {_items.Count}");
            Console.WriteLine("========================================\n");
        }

        /// <summary>
        /// Gets the total number of items in the library.
        /// </summary>
        public int GetTotalItems()
        {
            return _items.Count;
        }

        /// <summary>
        /// Gets all items of a specific type.
        /// </summary>
        public List<T> GetItemsByType<T>() where T : Item
        {
            return _items.OfType<T>().ToList();
        }
    }
}
