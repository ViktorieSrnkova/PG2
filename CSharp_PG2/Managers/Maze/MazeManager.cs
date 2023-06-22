using System.Collections.Generic;
using System.IO;
using CSharp_PG2.Entities;
using OpenTK.Mathematics;

namespace CSharp_PG2.Managers.Maze;

public class MazeManager
{

    public static List<Figure> GetMazeFigures(int[,,] maze)
    {
        List<Figure> figures = new List<Figure>();

        int width = maze.GetLength(0);
        int height = maze.GetLength(1);
        int depth = maze.GetLength(2);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < depth; z++)
                {
                    int value = maze[x, y, z];

                    if (value == 1)
                    {
                        // Create a wall figure at the specified position
                        var wallEntity = new BasicEntity($"wall_({x},{y},{z})", new Vector3(x, y, z), "crate");
                        wallEntity.IsStatic = true;
                        figures.Add(wallEntity);
                    }
                    // Add more conditions as needed for different types of figures
                }
            }
        }

        return figures;
    }
    
    public static int[,,] MapMaze(int[,] maze, int height)
    {
        int width = maze.GetLength(0);
        int depth = maze.GetLength(1);
    
        int[,,] mappedMaze = new int[width, height, depth];

        for (var i = 0; i < height; i++)
        {
            for (var j = 0; j < depth; j++)
            {
                for (var k = 0; k < width; k++)
                {
                    mappedMaze[k, i, j] = maze[k, j] == 0 ? 0 : 1;
                }
            }
        }

        return mappedMaze;
    }

    public static int[,] GetMaze(string mazeName)
    {
        var lines = ReadLines($"../../../Mazes/{mazeName}.txt");
        var maze = ParseMaze(lines);

        return maze;
    }

    public static void PrintMaze(int[,] maze)
    {
        var width = maze.GetLength(0);
        var height = maze.GetLength(1);

        for (var i = 0; i < height; i++)
        {
            var line = "";
            for (var j = 0; j < width; j++)
            {
                var number = maze[j, i];
                line += number == 0 ? " " : number;
            }

            System.Console.WriteLine(line);
        }
    }

    private static List<string> ReadLines(string path)
    {
        // Read the file and save it to a list of strings
        var lines = new List<string>();
        using (var sr = new StreamReader(path))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                lines.Add(line);
            }
        }

        return lines;
    }

    private static int[,] ParseMaze(List<string> lines)
    {
        // Get the size of the maze
        var size = lines[0].Split();
        var width = lines[0].Length;
        var height = lines.Count;

        // Create the maze
        var maze = new int[width, height];

        // Fill the maze
        for (var i = 0; i < height; i++)
        {
            var line = lines[i];
            for (var j = 0; j < line.Length; j++)
            {
                maze[j, i] = int.Parse(line[j].ToString());
            }
        }

        return maze;
    }
}