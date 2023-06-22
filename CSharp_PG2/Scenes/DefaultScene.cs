using System;
using System.Collections.Generic;
using CSharp_PG2.Entities;
using CSharp_PG2.Entities.Ghost;
using CSharp_PG2.Entities.ShaderConfigurables;
using CSharp_PG2.Managers.Maze;
using CSharp_PG2.Managers.Object;
using CSharp_PG2.Managers.Shader;
using CSharp_PG2.Managers.Shader.Entity;
using CSharp_PG2.Utils;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace CSharp_PG2.Scenes;

public class DefaultScene : Scene
{
    private const string MainShaderName = "default";

    private Spotlight _spotlight;
    private DirLight _dirLight;

    private bool _mouseGrabbed = false;
    private Vector2 _lastMousePosition;

    private Figure _player;
    
    public DefaultScene(Camera camera) : base(camera)
    {
    }
    
    public override void Setup()
    {
        _spotlight = Spotlight.MakeDefault();
        _dirLight = DirLight.MakeDefault();
        
        AddShaderConfigurable(Spotlight.Name, _spotlight);
        AddShaderConfigurable(DirLight.Name, _dirLight);
        
        var lights = new Vector3[]
        {
            new Vector3(3f, 20f, 3f),
            new Vector3(-3f, 20f, 3f),
            new Vector3(3f, 20f, 0f),
            new Vector3(-3f, 20f, 0f)
        };

        for (int i = 0; i < lights.Length; i++)
        {
            var light = PointLight.Create(
                $"pointLight_{i}",
                i,
                lights[i],
                new Vector3(0.05f, 0.05f, 0.05f),
                new Vector3(1f, 0.8f, 0.8f),
                new Vector3(1, 1, 1)
            );

            light.Velocity = new Vector3(0, 5, 0);
            Figures.Add(light.GetName(), light);
        }

        var ghost = new Ghost("ghost", new Vector3(5,5,5));
        Figures.Add(ghost.GetName(), ghost);
        
        var ghostCircular = new GhostCircular("ghost_circular", Vector3.Zero, 0, -1, 30);
        Figures.Add(ghostCircular.GetName(), ghostCircular);
        
        var ghostSquare = new GhostSquare("ghost_square", Vector3.Zero);
        Figures.Add(ghostSquare.GetName(), ghostSquare);
        
        var ghostZ = new GhostLineZAxis("ghost_z_axis", new Vector3(5, 0, 3));
        Figures.Add(ghostZ.GetName(), ghostZ);
        
        var ground = new Ground("ground", 500)
        {
            IsStatic = true,
            IsCollidable = true,
            Velocity = Vector3.Zero
        };
        Figures.Add("ground", ground);

        var maze = MazeManager.GetMaze("maze");
        var mappedMaze = MazeManager.MapMaze(maze, 3);
        var figures = MazeManager.GetMazeFigures(mappedMaze);
        
        foreach (var figure in figures)
        {
            Figures.Add(figure.GetName(), figure);
        }

        // var maze = new Maze("Maze");
        // Figures.Add(maze.GetName(), maze);
    }

    protected override Shader GetMainShader()
    {
        return ShaderManager.GetInstance().GetShader(MainShaderName);
    }

    public override void OnMouseWheel(MouseState state, MouseWheelEventArgs e)
    {
        _spotlight.AdjustIntensity(e.OffsetY > 0);
    }

    public override void HandleKeyboardInput(KeyboardState state, float deltaTime)
    {
        if (state.IsKeyDown(Keys.W))
        {
            Camera.Position += Camera.ProcessInput(Camera.Direction.Forward, (float)deltaTime);
        }

        if (state.IsKeyDown(Keys.S))
        {
            Camera.Position += Camera.ProcessInput(Camera.Direction.Backward, (float)deltaTime);
        }

        if (state.IsKeyDown(Keys.A))
        {
            Camera.Position += Camera.ProcessInput(Camera.Direction.Left, (float)deltaTime);
        }

        if (state.IsKeyDown(Keys.D))
        {
            Camera.Position += Camera.ProcessInput(Camera.Direction.Right, (float)deltaTime);
        }

        if (state.IsKeyDown(Keys.Space))
        {
            Camera.Position += Camera.ProcessInput(Camera.Direction.Up, (float)deltaTime);
        }

        if (state.IsKeyDown(Keys.LeftShift))
        {
            Camera.Position += Camera.ProcessInput(Camera.Direction.Down, (float)deltaTime);
        }
        
        
    }

    public override void OnKeyDown(KeyboardKeyEventArgs e)
    {
        switch (e.Key)
        {
            case Keys.Q:
                Camera.Follow(Camera.IsFollowing() ? null : _player);
                break;
        }
    }
}