using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using OpenTK.Mathematics;

namespace CSharp_PG2
{
    class Game : GameWindow
    {
        private int frameCount;
        private Stopwatch timer = new Stopwatch();
        
        private int _vbo;
        
        private int _vao;
        
        private static DebugProc _debugProcCallback = DebugCallback;
        private static GCHandle _debugProcCallbackHandle;

        private Shader _shader;

        private float[] _vertices;

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
                    : base(gameWindowSettings, nativeWindowSettings)
        {
        }
        protected override void OnLoad()
        {
            base.OnLoad();
            
            GL.ClearColor(Color4.Black);
            GL.Enable(EnableCap.DepthTest);
            _vbo = GL.GenBuffer();
            
            _shader = new Shader("Shaders/basic/basic.vert", "Shaders/basic/basic.frag");

            GL.BindBuffer(BufferTarget.ArrayBuffer, _vbo);
            
            _vertices = GenerateAnnulus(0.0f, 0.0f, 0.5f, 100);
            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices, BufferUsageHint.StaticDraw);
            _vao = GL.GenVertexArray();
            GL.BindVertexArray(_vao);
            
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);


            _debugProcCallbackHandle = GCHandle.Alloc(_debugProcCallback);

            // Enable debug output
            GL.Enable(EnableCap.DebugOutput);
            GL.Enable(EnableCap.DebugOutputSynchronous);

            // Set up debug output callback
            GL.DebugMessageCallback(DebugCallback, IntPtr.Zero);

            _shader.Use();
            timer.Start();
            Console.WriteLine("OnLoad");
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

            _shader.Use();
            GL.BindVertexArray(_vbo);
            
            GL.DrawArrays(PrimitiveType.Triangles, 0, _vertices.Length);
            
            this.SwapBuffers();
            this.frameCount++;
            if (this.timer.ElapsedMilliseconds >= 1000)
            {
                this.Title = $"FPS: {this.frameCount} - GPU: {GL.GetString(StringName.Renderer)} - CPU: {System.Environment.ProcessorCount} Cores";
                this.frameCount = 0;
                this.timer.Restart();
            }
        }
        
        private static void DebugCallback(DebugSource source, DebugType type, int id,
            DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
        {
            string messageString = Marshal.PtrToStringAnsi(message, length);
            Console.WriteLine($"{severity} {type} | {messageString}");

            if (type == DebugType.DebugTypeError)
                throw new Exception(messageString);
        }

        public float[] GenerateAnnulus(float centerX, float centerY, float radius, int numVertices)
        {
            float[] vertices = new float[numVertices * 3];
            
            var angleStep = (float)(2.0f * Math.PI / numVertices);
            
            // Compute the coordinates of the outer vertices
            for (int i = 0; i < numVertices; i++)
            {
                var angle = i * angleStep;
                var x = centerX + radius * (float)Math.Cos(angle);
                var y = centerY + radius * (float)Math.Sin(angle);
                vertices[i * 3] = x;
                vertices[i * 3 + 1] = y;
                vertices[i * 3 + 2] = 0.0f; // z-coordinate is zero (assuming 2D plane)
            }
    
            return vertices;
        }
    }
}
