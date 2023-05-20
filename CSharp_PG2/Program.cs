using OpenTK.Windowing.Desktop;

namespace CSharp_PG2;

public static class Program
{
    private static void Main(string[] args)
    {
        var nativeWindowSettings = new NativeWindowSettings
        {
            Size = new OpenTK.Mathematics.Vector2i(800, 600),
            Title = "My Game"
        };

        using var gameWindow = new Game(GameWindowSettings.Default, nativeWindowSettings);
        gameWindow.Run();
    }
}