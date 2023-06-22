using System;
using System.Collections.Generic;
using CSharp_PG2.Entities;
using CSharp_PG2.Entities.ShaderConfigurables;
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
            new Vector3(9f, 0f, 4.01f),
            new Vector3(-9f, 0f, 4f),
            new Vector3(6f, 1000f, 3f),
            new Vector3(-6f, 1000f, 3f)
        };

        for (int i = 0; i < lights.Length; i++)
        {
            var light = PointLight.Create(
                $"pointLight_{i}",
                i,
                lights[i],
                new Vector3(0.05f, 0.05f, 0.05f),
                new Vector3(1.8f, 0.8f, 0.8f),
                new Vector3(1, 1, 1)
            );
            
            light.Velocity = new Vector3((i%2==0?-1:1)*2, 0, 0);
            Figures.Add(light.GetName(), light);
        }

        var cubeMesh = ObjectManager.GetInstance().GetObject("cube").GetMesh();
        _player = new Figure(cubeMesh, new Vector3(0, 1, 0), "player")
        {
            IsStatic = true,
            IsCollidable = true,
            Velocity = Vector3.Zero
        };
        Figures.Add(_player.GetName(), _player);
        
        var groundMesh = ObjectManager.GetInstance().GetObject("ground").GetMesh();
        var ground = new Figure(groundMesh, Vector3.Zero, "ground")
        {
            IsStatic = true,
            IsCollidable = true,
            Velocity = Vector3.Zero
        };
        Figures.Add("ground", ground);

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