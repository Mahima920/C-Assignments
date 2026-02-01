using System;

namespace Week4Library.CustomExpectation
{
    // This custom error is used when the same item is added more than once
    // It helps prevent duplicate books, magazines, or newspapers
    public class DuplicateItemException : Exception
    {
        // Creates an error message that explains the duplicate problem
        public DuplicateItemException(string message) : base(message)
        {
        }
    }
}
