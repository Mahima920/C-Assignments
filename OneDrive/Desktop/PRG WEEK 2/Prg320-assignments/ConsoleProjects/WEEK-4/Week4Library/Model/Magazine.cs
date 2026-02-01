using Week4Library.CustomExpectation;

namespace Week4Library.Model
{
    // This class stores information about a magazine item
    // It uses shared data from the base library class
    public class Magazine : LibraryItemBase
    {
        // Stores the magazine issue number
        public int IssueNumber { get; set; }

        // Creates a magazine with title, publisher, year, and issue number
        public Magazine(string title, string publisher, int year, int issueNumber)
            : base(title, publisher, year)
        {
            // Make sure the issue number is a valid positive value
            if (issueNumber <= 0)
            {
                // Show error if issue number is not valid
                throw new InvalidItemException(
                    "Issue number must be greater than zero."
                );
            }

            // Save the issue number
            IssueNumber = issueNumber;
        }

        // Shows magazine details on the screen
        public override void DisplayInfo()
        {
            Console.WriteLine(
                $"[Magazine] Title: {Title}, Publisher: {Publisher}, Year: {PublicationYear}, Issue: {IssueNumber}"
            );
        }
    }
}
