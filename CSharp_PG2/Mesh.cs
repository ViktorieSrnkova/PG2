using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace CSharp_PG2
{
    public class Mesh
    {
        // VAO = Vertex Array Object
        // VBO = Vertex Buffer Object
        // EBO = Element Buffer Object
        private readonly int _vao, _vbo, _ebo;
        
        private readonly Shader _shader;
        private readonly Vertex[] _vertices;
        private readonly uint[] _indices;
        private readonly int _primitiveType;
        
        public Mesh(Shader shader, Vertex[] vertices, uint[] indices, int primitiveType = (int)PrimitiveType.Triangles)
        {
            _shader = shader;
            _vertices = vertices;
            _indices = indices;
            _primitiveType = primitiveType;
            
            // Generate VAO, VBO, EBO
            _vao = GL.GenVertexArray();
            _vbo = GL.GenBuffer();
            _ebo = GL.GenBuffer();
            
            var vertexSize = Marshal.SizeOf<Vertex>();
            
            // Bind VAO
            GL.BindVertexArray(_vao);
            
            // Bind VBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * vertexSize, _vertices, BufferUsageHint.StaticDraw);
            
            // Bind EBO
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices, BufferUsageHint.StaticDraw);
            
            // Set vertex attributes
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, vertexSize, 0);

            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, vertexSize, Marshal.OffsetOf<Vertex>("Color"));

            // Unbind VAO, VBO, EBO
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }
        
        public void Draw(Matrix4 model, Matrix4 view, Matrix4 projection)
        {
            _shader.Use();
            
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
    }
}