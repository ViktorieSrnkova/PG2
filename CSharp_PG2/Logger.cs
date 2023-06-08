using System;

namespace CSharp_PG2;

public class Logger
{
    private readonly string _name;

    public Logger(string name)
    {
        _name = name;
    }
    
    public void Log(string message, string prefix = "LOG", ConsoleColor? prefixColorBackground = null, ConsoleColor? prefixColor = null)
    {
        if (prefixColorBackground != null)
        {
            Console.BackgroundColor = prefixColorBackground.Value;
        }
        if (prefixColor != null)
        {
            Console.ForegroundColor = prefixColor.Value;
        }
        Console.Write($" {prefix} ");
        Console.ResetColor();
        
        Console.BackgroundColor = ConsoleColor.DarkGreen;
        Console.ForegroundColor = ConsoleColor.Black;
        Console.Write($" {_name} ");
        Console.ResetColor();
        
        Console.Write($"\t{message}\n");
        Console.ResetColor();
    }
    
    public void Info(string message)
    {
        Log(message, "INFO", ConsoleColor.Blue, ConsoleColor.Black);
    }
    
    public void Warn(string message)
    {
        Log(message, "WARN", ConsoleColor.DarkYellow, ConsoleColor.Black);
    }
    
    public void Error(string message)
    {
        Log(message, "NOPE", ConsoleColor.Red, ConsoleColor.Black);
    }
}