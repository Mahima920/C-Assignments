using Week4Library.CustomExpectation;

namespace Week4Library.Model
{
    // This class stores information about a newspaper item
    // It uses common fields from the base library class
    public class Newspaper : LibraryItemBase
    {
        // Stores the editor name privately
        private string _editor = "";

        // Allows getting and setting the editor name
        public string Editor
        {
            get => _editor; // Returns the editor name
            set
            {
                // Check if the editor name is valid
                if (string.IsNullOrWhiteSpace(value) || value.Trim().Length < 5)
                {
                    // Show error if editor name is too short or empty
                    throw new InvalidItemException(
                        "Editor name must be at least 5 characters long."
                    );
                }

                // Remove extra spaces from the name
                value = value.Trim();

                // Make the first letter uppercase
                _editor = char.ToUpper(value[0]) + value.Substring(1);
            }
        }

        // Creates a newspaper object with title, publisher, year, and editor
        public Newspaper(string title, string publisher, int year, string editor)
            : base(title, publisher, year)
        {
            // Save the editor name
            Editor = editor;
        }

        // Shows newspaper details on the screen
        public override void DisplayInfo()
        {
            Console.WriteLine(
                $"[Newspaper] Title: {Title}, Publisher: {Publisher}, Year: {PublicationYear}, Editor: {Editor}"
            );
        }
    }
}
