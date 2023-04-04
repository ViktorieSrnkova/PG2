using System;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace CSharp_PG2
{
    class Program
    {
        static void Main(string[] args)
        {
            // Print
            Console.WriteLine("Hello World!");

            var gameWindowSettings = new GameWindowSettings
            {
                
            };

            var nativeWindowSettings = new NativeWindowSettings
            {
                Size = new OpenTK.Mathematics.Vector2i(800, 600),
                Title = "My Game"
            };

            using (var gameWindow = new Game(gameWindowSettings, nativeWindowSettings))
            {
                gameWindow.Run();
            }

        }
    }
}
