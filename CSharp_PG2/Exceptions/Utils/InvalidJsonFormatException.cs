using System;

namespace CSharp_PG2.Exceptions.Utils;

public class InvalidJsonFormatException : Exception
{
    public InvalidJsonFormatException()
    {
    }

    public InvalidJsonFormatException(string message)
        : base(message)
    {
    }

    public InvalidJsonFormatException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}