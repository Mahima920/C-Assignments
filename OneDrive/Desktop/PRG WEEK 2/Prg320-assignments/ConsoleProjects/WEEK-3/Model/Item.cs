using System;
using WEEK3.CustomException;

namespace WEEK3.Model
{
    /// <summary>
    /// Abstract base class representing a library item.
    /// Demonstrates encapsulation, abstraction, and validation logic.
    /// </summary>
    public abstract class Item
    {
        private string _title;
        private string _publisher;
        private int _publicationYear;

        public string Title
        {
            get { return _title; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidItemDataException("Title cannot be null or empty.");
                }
                if (value.Length < 5)
                {
                    throw new InvalidItemDataException("Title must be at least 5 characters long.");
                }
                if (!char.IsUpper(value[0]))
                {
                    throw new InvalidItemDataException("Title must begin with a capital letter.");
                }
                _title = value;
            }
        }

        public string Publisher
        {
            get { return _publisher; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new InvalidItemDataException("Publisher cannot be null or empty.");
                }
                if (value.Length < 6)
                {
                    throw new InvalidItemDataException("Publisher must be at least 6 characters long.");
                }
                if (!char.IsUpper(value[0]))
                {
                    throw new InvalidItemDataException("Publisher must begin with a capital letter.");
                }
                _publisher = value;
            }
        }

        public int PublicationYear
        {
            get { return _publicationYear; }
            set
            {
                if (value < 1000 || value > 9999)
                {
                    throw new InvalidItemDataException("PublicationYear must be a valid four-digit year.");
                }
                _publicationYear = value;
            }
        }

        /// <summary>
        /// Virtual method to display item details. 
        /// Can be overridden by derived classes.
        /// </summary>
        public virtual void DisplayItems()
        {
            Console.WriteLine($"Title: {_title}");
            Console.WriteLine($"Publisher: {_publisher}");
            Console.WriteLine($"Publication Year: {_publicationYear}");
        }
    }
}
