using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Mathematics;

namespace CSharp_PG2
{
    class Game : GameWindow
    {
        private int frameCount;
        private Stopwatch timer = new Stopwatch();
        
        private int _vertexBufferObject;
        
        private int _vertexArrayObject;

        private Shader _shader;
        
        private float[] _vertices = {
            -0.5f, -0.5f, 0.0f, //Bottom-left vertex
            0.5f, -0.5f, 0.0f, //Bottom-right vertex
            0.0f,  0.5f, 0.0f  //Top vertex
        };

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
                    : base(gameWindowSettings, nativeWindowSettings)
        {
        }
        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(Color4.Black);
            GL.Enable(EnableCap.DepthTest);
            _vertexBufferObject = GL.GenBuffer();
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
            
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
            _vertexArrayObject = GL.GenVertexArray();
            GL.BindVertexArray(_vertexArrayObject);
            
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);
            
            _shader = new Shader("Shaders/shader.vert", "Shaders/shader.frag");
            _shader.Use();
            timer.Start();
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(0, 0, e.Width, e.Height);
        }
        
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            if (KeyboardState.IsKeyDown(OpenTK.Windowing.GraphicsLibraryFramework.Keys.Escape))
            {
                this.Close();
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            // Render your game here

            _shader.Use();
            GL.BindVertexArray(_vertexArrayObject);
            
            GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
            
            this.SwapBuffers();
            this.frameCount++;
            if (this.timer.ElapsedMilliseconds >= 1000)
            {
                this.Title = $"FPS: {this.frameCount} - GPU: {GL.GetString(StringName.Renderer)} - CPU: {System.Environment.ProcessorCount} Cores";
                this.frameCount = 0;
                this.timer.Restart();
            }
        }






    }
}
