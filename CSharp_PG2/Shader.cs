using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace CSharp_PG2
{
    public class Shader
    {
        private readonly int _handle;

        public Shader(string vertexPath, string fragmentPath)
        {
            var vertexShaderSource = File.ReadAllText(vertexPath);
            var fragmentShaderSource = File.ReadAllText(fragmentPath);

            // Compile Vertex Shader
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            GL.ShaderSource(vertexShader, vertexShaderSource);
            CompileShader(vertexShader);

            // Compile Fragment Shader
            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            GL.ShaderSource(fragmentShader, fragmentShaderSource);
            CompileShader(fragmentShader);

            _handle = GL.CreateProgram();
            GL.AttachShader(_handle, vertexShader);
            GL.AttachShader(_handle, fragmentShader);

            LinkProgram(_handle);

            GL.DetachShader(_handle, vertexShader);
            GL.DetachShader(_handle, fragmentShader);
            GL.DeleteShader(fragmentShader);
            GL.DeleteShader(vertexShader);
        }

        public void Use()
        {
            GL.UseProgram(_handle);
        }

        private static void CompileShader(int shader)
        {
            GL.CompileShader(shader);

            GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
            if (success == 0)
            {
                var infoLog = GL.GetShaderInfoLog(shader);
                Console.WriteLine(infoLog);
            }
        }

        private static void LinkProgram(int program)
        {
            GL.LinkProgram(program);

            GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
            if (code != (int)All.True)
            {
                throw new Exception($"Error occurred whilst linking Program({program})");
            }
        }

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                GL.DeleteProgram(_handle);

                disposedValue = true;
            }
        }

        ~Shader()
        {
            GL.DeleteProgram(_handle);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}