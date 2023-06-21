using CSharp_PG2.Managers.Shader.Entity;
using OpenTK.Mathematics;

namespace CSharp_PG2.Entities.ShaderConfigurables;

public class AmbientLight : IShaderConfigurable
{
    
    public Vector3 Color { get; set; }
    
    public AmbientLight(Vector3 color)
    {
        Color = color;
    }
    
    public void ConfigureShader(Shader shader)
    {
        shader.SetVector3("light.ambient", Color);
    }
}