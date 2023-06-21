using CSharp_PG2.Entities;
using CSharp_PG2.Entities.ShaderConfigurables;
using CSharp_PG2.Managers.Shader;
using CSharp_PG2.Managers.Shader.Entity;
using OpenTK.Mathematics;

namespace CSharp_PG2.Scenes;

public class DefaultScene : Scene
{
    private const string MainShaderName = "default";
    
    public override void Setup()
    {
        var light = PointLight.Create("pointLight", new Vector3(220, 0, 0), new Vector3(1, 1, 1), 1);
        AddShaderConfigurable("ambient", new AmbientLight(new Vector3(1,1,1)));
        Figures.Add(light.GetName(), light);

        var maze = new Maze("Maze");
        Figures.Add(maze.GetName(), maze);
    }

    protected override Shader GetMainShader()
    {
        return ShaderManager.GetInstance().GetShader("default");
    }
}