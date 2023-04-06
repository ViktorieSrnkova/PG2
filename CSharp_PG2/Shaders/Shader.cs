using System;
using System.IO;
using OpenTK.Graphics.OpenGL4;

namespace CSharp_PG2.Shaders
{
    
    public class Shader
    {

        public int ID { get; private set; }

        public Shader(string vsPath, string fsPath)
        {
            int vsID = GL.CreateShader(ShaderType.VertexShader);
            string vsSource = File.ReadAllText(vsPath);
            GL.ShaderSource(vsID, vsSource);
            GL.CompileShader(vsID);
            
            // Check for errors
            string vsLog = GL.GetShaderInfoLog(vsID);
            if (!string.IsNullOrEmpty(vsLog))
            {
                throw new Exception($"Vertex shader compilation failed: {vsLog}");
            }

            // Load and compile fragment shader
            int fsID = GL.CreateShader(ShaderType.FragmentShader);
            string fsSource = File.ReadAllText(fsPath);
            GL.ShaderSource(fsID, fsSource);
            GL.CompileShader(fsID);

            // Check for errors
            string fsLog = GL.GetShaderInfoLog(fsID);
            if (!string.IsNullOrEmpty(fsLog))
            {
                throw new Exception($"Fragment shader compilation failed: {fsLog}");
            }

            // Create shader program and link shaders
            ID = GL.CreateProgram();
            GL.AttachShader(ID, vsID);
            GL.AttachShader(ID, fsID);
            GL.LinkProgram(ID);

            // Check for errors
            string programLog = GL.GetProgramInfoLog(ID);
            if (!string.IsNullOrEmpty(programLog))
            {
                throw new Exception($"Shader program linking failed: {programLog}");
            }

            // Cleanup
            GL.DeleteShader(vsID);
            GL.DeleteShader(fsID);
            
        }
        
        public void Activate()
        {
            GL.UseProgram(ID);
        }

        public void Destroy()
        {
            GL.DeleteProgram(ID);
        }
        
        public void Clear()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
        
    }
}