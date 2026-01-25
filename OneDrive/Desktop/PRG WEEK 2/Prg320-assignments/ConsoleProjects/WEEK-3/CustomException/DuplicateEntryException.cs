using System;

namespace WEEK3.CustomException
{
    /// <summary>
    /// Custom exception thrown when a duplicate entry is detected in the library.
    /// </summary>
    public class DuplicateEntryException : Exception
    {
        public DuplicateEntryException(string message) : base(message)
        {
        }

        public DuplicateEntryException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
