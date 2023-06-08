using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp_PG2.Utils;

public class ConsoleReader
{
    private bool _isRunning;
    private Queue<TaskCompletionSource<string>> _inputTasks;

    public ConsoleReader()
    {
        _isRunning = false;
        _inputTasks = new Queue<TaskCompletionSource<string>>();
    }

    public Task<string> ReadLineAsync()
    {
        var inputTask = new TaskCompletionSource<string>();
        _inputTasks.Enqueue(inputTask);
        return inputTask.Task;
    }

    public void Start()
    {
        if (!_isRunning)
        {
            _isRunning = true;
            Task.Run(() => ReadInput());
        }
    }

    public void Stop()
    {
        _isRunning = false;
    }

    private void ReadInput()
    {
        while (_isRunning)
        {
            Console.WriteLine("Waiting for input...");
            string input = Console.ReadLine();

            lock (_inputTasks)
            {
                while (_inputTasks.Count > 0)
                {
                    var inputTask = _inputTasks.Dequeue();
                    inputTask.SetResult(input);
                }
            }
        }
    }
}