using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace CSharp_PG2.Utils;

public class ConsoleWriter
{

    private Timer _timer;
    private readonly int _updateInterval;
    private Dictionary<string, string> _buffer = new Dictionary<string, string>();
    private int _startLine = Console.CursorTop;

    public ConsoleWriter(int updateInterval)
    {
        _updateInterval = updateInterval;
    }

    public void SetMessage(Dictionary<string, string> messages)
    {
        _buffer = messages;
    }
    
    public async Task Start()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        var pad = new string('=', 8);
        
        Console.WriteLine($"{pad} Debugger {pad}");
        Console.ResetColor();
        // Create a timer that triggers the callback method every x milliseconds
        _startLine = Console.CursorTop;
        _timer = new Timer(WriteToConsole, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(_updateInterval));

        // Use a Task.Delay to keep the application running
        await Task.Delay(Timeout.Infinite);
    }
    
    public void Stop()
    {
        Console.ResetColor();
        _timer?.Dispose();
    }
    
    private void WriteToConsole(object state)
    {
        Console.SetCursorPosition(0, _startLine);

        var i = 0;
        foreach (var (key, value) in _buffer)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write($"{key}: ");
            Console.ResetColor();
            Console.Write($"{PadWithSpace(value,key.Length+2)}");
            Console.SetCursorPosition(0, _startLine + i++ + 1);
        }
        
        // for (int i = 0; i < _buffer.Length; i++)
        // {
        //     
        //     Console.Write(PadWithSpace(_buffer[i]));
        //     Console.SetCursorPosition(0, _startLine + i + 1);
        // }
    }

    private string PadWithSpace(string s, int add = 0)
    {
        var diff = Console.WindowWidth - s.Length + add;
        if (diff < 0) diff = 0;
        return s + new string(' ', diff);
    }
    
}