using System;

namespace CSharp_PG2.Exceptions.Managers.Texture;

public class TextureNotFoundException : Exception
{
    public TextureNotFoundException()
    {
    }

    public TextureNotFoundException(string message) : base(message)
    {
    }

    public TextureNotFoundException(string message, Exception innerException) : base(message, innerException)
    {
    }
}