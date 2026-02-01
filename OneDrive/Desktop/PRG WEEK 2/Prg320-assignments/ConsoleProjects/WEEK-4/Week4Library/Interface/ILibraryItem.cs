namespace Week4Library.Interface
{
    // This interface defines the basic rules for all library items
    // Any class that uses this interface must follow these rules
    public interface ILibraryItem
    {
        // Stores or returns the title of the library item
        string Title { get; set; }

        // Stores or returns the publisher name
        string Publisher { get; set; }

        // Stores or returns the year the item was published
        int PublicationYear { get; set; }

        // Every library item must have a way to show its details
        // Each class decides how the information is displayed
        void DisplayInfo();
    }
}
