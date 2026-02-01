using Week4Library.Interface;
using Week4Library.CustomExpectation;

namespace Week4Library.Model
{
    // This is the main base class for all library items
    // Book, Magazine, and Newspaper all use this class
    // It stores common data like title, publisher, and year
    public abstract class LibraryItemBase : ILibraryItem
    {
        // Private variable to store the item title
        private string _title = "";

        // Private variable to store the publisher name
        private string _publisher = "";

        // Private variable to store the publication year
        private int _year;

        // Public property to get and set the title safely
        public string Title
        {
            get => _title; // Returns the title
            set => _title = ValidateText(value, "Title"); // Checks and saves the title
        }

        // Public property to get and set the publisher safely
        public string Publisher
        {
            get => _publisher; // Returns the publisher name
            set => _publisher = ValidateText(value, "Publisher"); // Checks and saves publisher
        }

        // Public property to get and set the publication year
        public int PublicationYear
        {
            get => _year; // Returns the year
            set
            {
                // Check if the year is exactly 4 digits
                if (value < 1000 || value > 9999)
                    throw new InvalidItemException(
                        "Publication year must be a 4-digit year (1000-9999)."
                    );

                // Save the year if valid
                _year = value;
            }
        }

        // Constructor used by child classes to set common values
        protected LibraryItemBase(string title, string publisher, int year)
        {
            // Set title using validation
            Title = title;

            // Set publisher using validation
            Publisher = publisher;

            // Set publication year using validation
            PublicationYear = year;
        }

        // This method checks text input for title and publisher
        // It makes sure the text is not empty
        // It checks that the text has at least 5 characters
        // It also removes extra spaces and capitalizes the first letter
        protected string ValidateText(string value, string fieldName)
        {
            // Check if text is empty or only spaces
            if (string.IsNullOrWhiteSpace(value))
                throw new InvalidItemException($"{fieldName} cannot be empty.");

            // Remove extra spaces at the beginning and end
            value = value.Trim();

            // Check minimum length requirement
            if (value.Length < 5)
                throw new InvalidItemException(
                    $"{fieldName} must be at least 5 characters long."
                );

            // Make the first letter uppercase and return the result
            return char.ToUpper(value[0]) + value.Substring(1);
        }

        // All child classes must define how their information is displayed
        public abstract void DisplayInfo();
    }
}
