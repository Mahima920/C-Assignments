using System;
using WEEK3.CustomException;

namespace WEEK3.Model
{
    /// <summary>
    /// Book class derived from Item.
    /// Demonstrates inheritance and polymorphism.
    /// </summary>
    public class Book : Item
    {
        private string _author;

        public string Author
        {
            get { return _author; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidItemDataException("Author cannot be null or empty.");
                }
                if (value.Length < 5)
                {
                    throw new InvalidItemDataException("Author must be at least 5 characters long.");
                }
                if (!char.IsUpper(value[0]))
                {
                    throw new InvalidItemDataException("Author must begin with a capital letter.");
                }
                _author = value;
            }
        }

        public Book(string title, string publisher, int publicationYear, string author)
        {
            Title = title;
            Publisher = publisher;
            PublicationYear = publicationYear;
            Author = author;
        }

        /// <summary>
        /// Overrides the base DisplayItems() method to display book-specific details.
        /// Demonstrates polymorphism.
        /// </summary>
        public override void DisplayItems()
        {
            Console.WriteLine("========== BOOK ==========");
            base.DisplayItems();
            Console.WriteLine($"Author: {_author}");
            Console.WriteLine("==========================");
        }
    }
}
