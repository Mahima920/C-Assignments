using System;

namespace WEEK3.CustomException
{
    /// <summary>
    /// Custom exception thrown when invalid item data is provided.
    /// </summary>
    public class InvalidItemDataException : Exception
    {
        public InvalidItemDataException(string message) : base(message)
        {
        }

        public InvalidItemDataException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}
