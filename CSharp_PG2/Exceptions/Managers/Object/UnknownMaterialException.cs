using System;

namespace CSharp_PG2.Exceptions.Managers.Object;

public class UnknownMaterialException : Exception
{
    public UnknownMaterialException()
    {
    }

    public UnknownMaterialException(string message)
        : base(message)
    {
    }

    public UnknownMaterialException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}