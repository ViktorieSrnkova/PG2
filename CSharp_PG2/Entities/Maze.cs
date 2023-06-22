using System;
using System.Collections.Generic;
using System.Linq;
using CSharp_PG2.Managers.Object.Entity;
using CSharp_PG2.Managers.Shader;
using CSharp_PG2.Managers.Texture;
using OpenTK.Mathematics;

namespace CSharp_PG2.Entities;

public class Maze : Figure
{

    private float[][] _base;
    private Dictionary<string, Vector3> _vertices;
    private Dictionary<string, uint> _indices;
    private uint _counter;
    private Mesh? _mesh;

    public Maze(string name = "maze") : base(name)
    {
        
    }

    public override void Draw(Camera camera, Matrix4 projection)
    {
        var mesh = _mesh ??= GetMesh();

        mesh.Draw(Matrix4.Identity, camera.GetViewMatrix(), projection);
    }

    private Mesh GetMesh()
    {
        Clear();

        var shader = ShaderManager.GetInstance().GetShader("default");
        
        // Create vertices and indices
        var indices = new List<uint>();
        
        // Connect it
        for (var i = 0; i < _base.Length-1; i++)
        {
            for (var j = 0; j < _base[i].Length-1; j++)
            {
                var lower = new Vector2(i, j);
                var upper = new Vector2(i+1, j+1);

                var points = new List<Vector2>()
                {
                    lower, new Vector2(lower.X, upper.Y), new Vector2(upper.X, lower.Y),
                    upper, new Vector2(lower.X, upper.Y), new Vector2(upper.X, lower.Y)
                };
                
                foreach (var point in points)
                {
                    var height = _base[(int)point.X][(int)point.Y];
                    indices.Add(GetIndex(new Vector3(point.X, height, point.Y)));
                }
            }   
        }

        // Converts points to Vertex[]
        var vertexes = new List<Vertex>();
        foreach (var item in _vertices.Values)
        {
            vertexes.Add(new Vertex
            {
                Position = item,
                Normal = new Vector3(0, 1, 0),
                TexCoord = new Vector2(0, 0)
            });
        }
        
        
        return new Mesh(shader, vertexes.ToArray(), indices.ToArray(), TextureManager.GetInstance().GetTexture("environment:ground"));
    }

    private uint GetIndex(Vector3 point)
    {
        var format = "({0}, {1}, {2})";
        if (_vertices.ContainsKey(point.ToString(format)))
        {
            return _indices[point.ToString(format)];
        }
        
        _vertices.Add(point.ToString(format), point);
        _indices.Add(point.ToString(format), _counter);
        
        return _counter++;
    }
    
    private void Clear()
    {
        GenerateBase();
        _vertices = new Dictionary<string, Vector3>();
        _indices = new Dictionary<string, uint>();
        _counter = 0;
    }
    
    private void GenerateBase(int highestPoint = 1)
    {
        //Generate 10x10 grid with middle point will be highest hill and will circle down to 0 to the edges
        _base = new float[10][];
        for (var i = 0; i < 10; i++)
        {
            _base[i] = new float[10];
            for (var j = 0; j < 10; j++)
            {
                _base[i][j] = 0;
            }
        }
        
        _base[5][5] = highestPoint;
        
        for (var i = 0; i < 10; i++)
        {
            for (var j = 0; j < 10; j++)
            {
                if (_base[i][j] == 0)
                {
                    var distance = Math.Sqrt(Math.Pow(i - 5, 2) + Math.Pow(j - 5, 2));
                    _base[i][j] = (float) (highestPoint - distance / 5);
                }
            }
        }
    }
    
}