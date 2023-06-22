using System;
using System.Collections.Generic;
using CSharp_PG2.Entities;
using CSharp_PG2.Entities.ShaderConfigurables;
using CSharp_PG2.Managers.Object;
using CSharp_PG2.Managers.Shader;
using CSharp_PG2.Managers.Shader.Entity;
using CSharp_PG2.Utils;
using OpenTK.Mathematics;

namespace CSharp_PG2.Scenes;

public class DefaultScene : Scene
{
    private const string MainShaderName = "default";

    private Spotlight _spotlight;
    private DirLight _dirLight;
    
    public override void Setup()
    {
        _spotlight = Spotlight.MakeDefault();
        _dirLight = DirLight.MakeDefault();
        
        AddShaderConfigurable(Spotlight.Name, _spotlight);
        AddShaderConfigurable(DirLight.Name, _dirLight);

        var lights = new Vector3[]
        {
            new Vector3(0.7f, 2f, 2.0f),
            new Vector3(2.3f, 3f, -4.0f),
            new Vector3(-4.0f, 3f, -6.0f),
            new Vector3(0.0f, 5f, -3.0f)
        };

        for (int i = 0; i < lights.Length; i++)
        {
            var light = PointLight.Create(
                $"pointLight_{i}",
                i,
                lights[i],
                new Vector3(0.05f, 0.05f, 0.05f),
                new Vector3(10*i, 0.8f, 0.8f),
                new Vector3(1, 1, 1)
            );
            
            Figures.Add(light.GetName(), light);
        }

        var ground = ObjectManager.GetInstance().GetObject("ground").GetMesh();
        Figures.Add("ground", new Figure(ground, Vector3.Zero, "ground"));

        // var maze = new Maze("Maze");
        // Figures.Add(maze.GetName(), maze);
    }

    protected override Shader GetMainShader()
    {
        return ShaderManager.GetInstance().GetShader(MainShaderName);
    }
}