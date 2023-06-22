
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CSharp_PG2.Managers.Shader.Entity;
using CSharp_PG2.Managers.Texture;
using CSharp_PG2.Utils;
using CSharp_PG2.Utils;
using NUnit.Framework;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace CSharp_PG2;

public class Mesh : IDisposable
{
    // VAO = Vertex Array Object
    // VBO = Vertex Buffer Object
    // EBO = Element Buffer Object
    private readonly int _vao, _vbo, _ebo;

    private readonly Shader _shader;
    private Vertex[] _vertices;
    private readonly uint[] _indices;
    private readonly int _primitiveType;
    public List<FaceUtils.TextureUsage> TextureUsages { get; set; } = new List<FaceUtils.TextureUsage>();
    
    public Mesh(Shader shader, Vertex[] vertices, uint[] indices, Texture? texture = null,
        int primitiveType = (int)PrimitiveType.Triangles)
    {
        _shader = shader;
        _vertices = vertices;
        _indices = indices;
        _primitiveType = primitiveType;
        
        if (texture != null)
        {
            TextureUsages.Add(new FaceUtils.TextureUsage{Texture = texture});
        }
        else
        {
            TextureUsages.Add(new FaceUtils.TextureUsage());
        }

        // Generate VAO, VBO, EBO
        _vao = GL.GenVertexArray();
        _vbo = GL.GenBuffer();
        _ebo = GL.GenBuffer();

        var vertexSize = Marshal.SizeOf<Vertex>();

        // Bind VBO
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * vertexSize, _vertices, BufferUsageHint.StaticDraw);
        
        // Bind VAO
        GL.BindVertexArray(_vao);

        // Bind EBO
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices,
            BufferUsageHint.StaticDraw);

        // Set vertex attributes
        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, vertexSize, 0);

        GL.EnableVertexAttribArray(1);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, vertexSize,
            Marshal.OffsetOf<Vertex>("Normal"));

        GL.EnableVertexAttribArray(2);
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, vertexSize,
            Marshal.OffsetOf<Vertex>("TexCoord"));

        // Unbind VAO, VBO, EBO
        GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        GL.BindVertexArray(0);
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
    }

    public void Draw(Matrix4 model, Matrix4 view, Matrix4 projection)
    {
        // Texture?.Use(TextureUnit.Texture0);
        _shader.Use();
        
        _shader.SetMatrix4("model", model);
        _shader.SetMatrix4("view", view);
        _shader.SetMatrix4("proj", projection);

        GL.BindVertexArray(_vao);
        var current = 0;
        foreach (var textureUsage in TextureUsages)
        {
            textureUsage.Texture?.Use(TextureUnit.Texture0);
            textureUsage.Material?.SetMaterial(_shader);
            
            GL.DrawElements((PrimitiveType)_primitiveType, textureUsage.Length * 3 ?? _indices.Length, DrawElementsType.UnsignedInt, current * sizeof(uint));
            current += textureUsage.Length*3 ?? 0;
        }
        
        var length = _indices.Length;

        GL.BindVertexArray(0);
    }

    public Vertex[] GetVertices()
    {
        return _vertices;
    }

    public void SetVertices(Vertex[] vertices)
    {
        _vertices = vertices;
    }
    
    public uint[] GetIndices()
    {
        return _indices;
    }
    
    public Shader GetShader()
    {
        return _shader;
    }
    
    public void Dispose()
    {
        GL.DeleteVertexArray(_vao);
        GL.DeleteBuffer(_vbo);
        GL.DeleteBuffer(_ebo);

        foreach (var textureUsage in TextureUsages)
        {
            textureUsage.Texture?.Dispose();
        }
    }
    
    
}