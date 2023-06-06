using System.IO;
using OpenTK.Graphics.OpenGL4;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;
using StbImageSharp;

namespace CSharp_PG2;

public class Texture
{
    private readonly int _handle;

    public static Texture LoadFromFile(string path) {
        var handle = GL.GenTexture();
        GL.ActiveTexture(TextureUnit.Texture0);
        GL.BindTexture(TextureTarget.Texture2D, handle);

        StbImage.stbi_set_flip_vertically_on_load(1);

        using (Stream stream = File.OpenRead(path)) {
            var image = ImageResult.FromStream(stream, ColorComponents.RedGreenBlueAlpha);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, image.Width, image.Height, 0,
                PixelFormat.Rgba, PixelType.UnsignedByte, image.Data);
        }

        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);

        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);

        return new Texture(handle);
    }

    private Texture(int glHandle) {
        _handle = glHandle;
    }

    public void Use(TextureUnit unit) {
        GL.ActiveTexture(unit);
        GL.BindTexture(TextureTarget.Texture2D, _handle);
    }
    
    public void Dispose() {
        GL.DeleteTexture(_handle);
    }
}