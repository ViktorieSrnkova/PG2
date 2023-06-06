using System;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace CSharp_PG2.Utils;

public class TitleUtils
{
    
    public static String GetPosition(Vector3 position)
    {
        // Round it to 2 decimals. Always include 2 decimals.
        position.X = (float)Math.Round(position.X, 2);
        position.Y = (float)Math.Round(position.Y, 2);
        position.Z = (float)Math.Round(position.Z, 2);
        return $"X: {position.X} Y: {position.Y} Z: {position.Z}";
    }
    
    public static String GetFps(float fps)
    {
        return $"FPS: {fps}";
    }
    
    public static String GetDeviceDetails()
    {
        return $"GPU: {GL.GetString(StringName.Renderer)} - CPU: {System.Environment.ProcessorCount} Cores";
    }
    
}