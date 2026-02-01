using System;

namespace Week4Library.CustomExpectation
{
    // This custom error is used when user input is wrong or invalid
    // It helps show clear error messages in the program
    public class InvalidItemException : Exception
    {
        // Creates an error with a custom message
        // The message explains what the user did wrong
        public InvalidItemException(string message) : base(message)
        {
        }
    }
}
