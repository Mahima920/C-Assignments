using System;
using WEEK3.CustomException;

namespace WEEK3.Model
{
    /// <summary>
    /// Magazine class derived from Item.
    /// Demonstrates inheritance and polymorphism.
    /// </summary>
    public class Magazine : Item
    {
        private int _issueNumber;

        public int IssueNumber
        {
            get { return _issueNumber; }
            set
            {
                if (value <= 0)
                {
                    throw new InvalidItemDataException("Issue number must be a positive integer.");
                }
                _issueNumber = value;
            }
        }

        public Magazine(string title, string publisher, int publicationYear, int issueNumber)
        {
            Title = title;
            Publisher = publisher;
            PublicationYear = publicationYear;
            IssueNumber = issueNumber;
        }

        /// <summary>
        /// Overrides the base DisplayItems() method to display magazine-specific details.
        /// Demonstrates polymorphism.
        /// </summary>
        public override void DisplayItems()
        {
            Console.WriteLine("====== MAGAZINE ======");
            base.DisplayItems();
            Console.WriteLine($"Issue Number: {_issueNumber}");
            Console.WriteLine("=====================");
        }
    }
}
