using System;

namespace CSharp_PG2.Exceptions.Managers.Texture;

public class BootstrapFailedException : Exception
{
    public BootstrapFailedException()
    {
    }

    public BootstrapFailedException(string message) : base(message)
    {
    }

    public BootstrapFailedException(string message, Exception innerException) : base(message, innerException)
    {
    }
}