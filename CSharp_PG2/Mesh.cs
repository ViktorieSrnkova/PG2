using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using CSharp_PG2.Managers.Texture;
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
    private readonly Vertex[] _vertices;
    private readonly uint[] _indices;
    private readonly int _primitiveType;
    private readonly Texture? _texture;

    public Mesh(Shader shader, Vertex[] vertices, uint[] indices, Texture? texture = null,
        int primitiveType = (int)PrimitiveType.Triangles)
    {
        _shader = shader;
        _vertices = vertices;
        _indices = indices;
        _primitiveType = primitiveType;
        _texture = texture;

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
        _shader.Use();

        _texture?.Use(TextureUnit.Texture0);

        var modelLocation = GL.GetUniformLocation(_shader.Handle, "model");
        GL.UniformMatrix4(modelLocation, false, ref model);

        var viewLocation = GL.GetUniformLocation(_shader.Handle, "view");
        GL.UniformMatrix4(viewLocation, false, ref view);

        var projectionLocation = GL.GetUniformLocation(_shader.Handle, "proj");
        GL.UniformMatrix4(projectionLocation, false, ref projection);


        GL.BindVertexArray(_vao);
        GL.DrawElements((PrimitiveType)_primitiveType, _indices.Length, DrawElementsType.UnsignedInt, 0);
        GL.BindVertexArray(0);
    }

    public static Tuple<List<Vector3>, List<Vector2>, List<Vector3>> ReadObject(string path)
    {
        var vertexIndices = new List<uint>();
        var uvIndices = new List<uint>();
        var normalIndices = new List<uint>();
        var tempVertices = new List<Vector3>();
        var tempUVs = new List<Vector2>();
        var tempNormals = new List<Vector3>();

        using (var file = new StreamReader(path))
        {
            while (!file.EndOfStream)
            {
                var line = file.ReadLine();
                if (line == null) continue;

                var lineParts = line.Replace(".", ",").Split(' ');
                var lineHeader = lineParts[0];

                switch (lineHeader)
                {
                    case "v":
                        var vertex = new Vector3(float.Parse(lineParts[1]), float.Parse(lineParts[2]),
                            float.Parse(lineParts[3]));
                        tempVertices.Add(vertex);
                        break;
                    case "vt":
                        var uv = new Vector2(float.Parse(lineParts[2]), float.Parse(lineParts[1]));
                        tempUVs.Add(uv);
                        break;
                    case "vn":
                        var normal = new Vector3(float.Parse(lineParts[1]), float.Parse(lineParts[2]),
                            float.Parse(lineParts[3]));
                        tempNormals.Add(normal);
                        break;
                    case "f":
                        for (var i = 1; i <= 3; i++)
                        {
                            var vertexParts = lineParts[i].Split('/');
                            var vertexIndex = uint.Parse(vertexParts[0]);
                            var uvIndex = uint.Parse(vertexParts[1]);
                            var normalIndex = uint.Parse(vertexParts[2]);

                            vertexIndices.Add(vertexIndex);
                            uvIndices.Add(uvIndex);
                            normalIndices.Add(normalIndex);
                        }

                        break;
                }
            }
        }

        // Unroll from indirect to direct vertex specification
        var outVertices = vertexIndices.Select(vertexIndex => tempVertices[(int)vertexIndex - 1]).ToList();
        var outUVs = uvIndices.Select(uvIndex => tempUVs[(int)uvIndex - 1]).ToList();
        var outNormals = normalIndices.Select(normalIndex => tempNormals[(int)normalIndex - 1]).ToList();

        return new Tuple<List<Vector3>, List<Vector2>, List<Vector3>>(outVertices, outUVs, outNormals);
    }

    public void Dispose()
    {
        GL.DeleteVertexArray(_vao);
        GL.DeleteBuffer(_vbo);
        GL.DeleteBuffer(_ebo);

        _texture?.Dispose();
    }
}