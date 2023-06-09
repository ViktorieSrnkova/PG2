using System;
using System.Collections.Generic;
using System.Linq;
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
            new Vector3(6f, 10f, 6f),
            new Vector3(-6f, 25f, 6f),
            new Vector3(6f, 35f, 0f)
        };

        var ambient = new Vector3(0.05f, 0.05f, 0.05f);
        var diffuse = new Vector3(1.8f, 0.8f, 0.8f);
        var specular = new Vector3(1, 1, 1);
        for (int i = 0; i < lights.Length; i++)
        {
            var light = PointLight.Create(
                $"pointLight_{i}",
                i,
                lights[i],
                ambient,
                diffuse,
                specular
                );

            light.Velocity = new Vector3(0, 0, 0);
            Figures.Add(light.GetName(), light);
        }
        
        var circularLight = PointLightCircle.CreateCircular("pointLight_circular", 3, Vector3.UnitY*5, ambient, diffuse, specular);

        circularLight.Radius = 10;
        Figures.Add(circularLight.GetName(), circularLight);

        var ghost = new Ghost("ghost", new Vector3(5,5,5));
        Figures.Add(ghost.GetName(), ghost);
        
        var ghostCircular = new GhostCircular("ghost_circular", new Vector3(10, 0,10), 0, -1, 3);
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

        var mazeFigures = MazeManager.GetMazeReady("maze", new Vector3(0, 0, 0), height:2);
        mazeFigures.ForEach(figure => Figures.Add(figure.GetName(), figure));
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