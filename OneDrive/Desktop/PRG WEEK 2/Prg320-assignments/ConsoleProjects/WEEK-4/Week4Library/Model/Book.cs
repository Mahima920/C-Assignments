using Week4Library.CustomExpectation;

namespace Week4Library.Model
{
    // This class stores information about a book in the library
    // It uses shared data like title, publisher, and year from the base class
    public class Book : LibraryItemBase
    {
        // Private variable to store the author name safely
        private string _author = "";

        // Public property to get and set the author name
        // This property also checks if the author name is valid
        public string Author
        {
            get => _author; // Returns the author name
            set
            {
                // Check if the author name is empty or too short
                if (string.IsNullOrWhiteSpace(value) || value.Trim().Length < 5)
                {
                    // Show an error if the author name is not valid
                    throw new InvalidItemException(
                        "Author name must be at least 5 characters long."
                    );
                }

                // Remove extra spaces before and after the name
                value = value.Trim();

                // Make the first letter uppercase
                _author = char.ToUpper(value[0]) + value.Substring(1);
            }
        }

        // Constructor that creates a new Book object
        // It receives values from the user
        // Common values are sent to the base class
        public Book(string title, string publisher, int year, string author)
            : base(title, publisher, year)
        {
            // Set the author using the property validation
            Author = author;
        }

        // Shows book details on the screen
        // This method replaces the base class method
        public override void DisplayInfo()
        {
            Console.WriteLine(
                $"[Book] Title: {Title}, Publisher: {Publisher}, Year: {PublicationYear}, Author: {Author}"
            );
        }
    }
}
