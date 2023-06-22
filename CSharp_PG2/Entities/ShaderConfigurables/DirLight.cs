using CSharp_PG2.Managers.Shader.Entity;
using OpenTK.Mathematics;

namespace CSharp_PG2.Entities.ShaderConfigurables;

public class DirLight : IShaderConfigurable
{

    public const string Name = "DirLight";
    
    public Vector3 Direction { get; set; }
    public Vector3 Ambient { get; set; }
    public Vector3 Diffuse { get; set; }
    public Vector3 Specular { get; set; }

    public DirLight(Vector3 direction, Vector3 ambient, Vector3 diffuse, Vector3 specular)
    {
        Direction = direction;
        Ambient = ambient;
        Diffuse = diffuse;
        Specular = specular;
    }

    public void Setup(Shader shader)
    {
        shader.SetVector3("dirLight.direction", Direction);
        shader.SetVector3("dirLight.ambient", Ambient);
        shader.SetVector3("dirLight.diffuse", Diffuse);
        shader.SetVector3("dirLight.specular", Specular);
    }

    public void ConfigureShader(Camera camera, Shader shader)
    {
        // Do nothing
    }

    public static DirLight MakeDefault()
    {
        return new DirLight(
            new Vector3(-0.2f, -1.0f, -0.3f),
            new Vector3(0.1f, 0.1f, 0.1f),
            new Vector3(0.4f, 0.4f, 0.4f),
            new Vector3(0.5f, 0.5f, 0.5f)
        );
    }
}