using CSharp_PG2.Managers.Shader.Entity;

namespace CSharp_PG2;

public interface IShaderConfigurable
{

    public void Setup(Shader shader);
    
    public void ConfigureShader(Camera camera, Shader shader);
    
}