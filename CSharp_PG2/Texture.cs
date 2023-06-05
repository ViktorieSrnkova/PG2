using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.IO;
using SixLabors.ImageSharp.Advanced;

namespace CSharp_PG2;
public class Texture
{
    private readonly int _handle;
    public int LoadTexture(string path)
    {
        if (!File.Exists("Texture/" + path))
        {
            throw new FileNotFoundException("File not found at 'Textures/" + path + "'");
        }

        int textureId = GL.GenTexture();
        GL.BindTexture(TextureTarget.Texture2D, textureId);

        using (Image<Rgba32> image = Image.Load<Rgba32>(path))
        {
            image.Mutate(x => x.Flip(FlipMode.Vertical)); // Flip vertically as OpenGL expects the origin to be at the bottom-left

            var pixelMemoryGroup = image.GetPixelMemoryGroup();
            var pixelData = pixelMemoryGroup[0].Span.ToArray();

            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                image.Width,
                image.Height,
                0,
                OpenTK.Graphics.OpenGL4.PixelFormat.Rgba,
                PixelType.UnsignedByte,
                pixelData);

            pixelData = null; // Release the array to free memory
        }
        
        GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.LinearMipmapLinear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);
        
        return textureId;
    }
    private Texture(int glHandle) {
        _handle = glHandle;
    }
    public void DeleteTexture()
    {
        GL.DeleteTexture(_handle);
    }

    public void Use(TextureUnit unit)
    {
        GL.ActiveTexture(unit);
        GL.BindTexture(TextureTarget.Texture2D, _handle);
    }
}