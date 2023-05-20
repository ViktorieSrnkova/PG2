using OpenTK.Graphics.ES11;
using StbImageSharp;

namespace CSharp_PG2;

public class Texture
{
    private readonly int _textureHandle;

    // public static Texture Load(string path)
    // {
    //     var handle = GL.GenTexture();
    //     
    //     // Slots are used to bind textures to a specific texture unit.
    //     // We use Texture0 as it is the default texture unit. 
    //     GL.ActiveTexture(TextureUnit.Texture0);
    //     GL.BindTexture(TextureTarget.Texture2D, handle);
    //     
    //     StbImage.stbi_set_flip_vertically_on_load(1);
    // }
}