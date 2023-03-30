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

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
                    : base(gameWindowSettings, nativeWindowSettings)
        {
        }
        protected override void OnLoad()
        {
            base.OnLoad();
            GL.ClearColor(Color4.Black);
            GL.Enable(EnableCap.DepthTest);
            timer.Start();
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
