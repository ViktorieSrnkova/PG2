using System;
using CSharp_PG2.Managers.Shader.Entity;
using OpenTK.Mathematics;

namespace CSharp_PG2.Entities.ShaderConfigurables;

public class Spotlight : IShaderConfigurable
{

    public const string Name = "Spotlight";
    
    public Vector3 Color { get; set; }
    public float Intensity { get; set; }

    public Spotlight(Vector3 color, float intensity)
    {
        Color = color;
        Intensity = intensity;
    }

    public void Setup(Shader shader)
    {
        var flashLightIntensity = Color * Intensity;
        shader.SetVector3("spotLight.ambient", new Vector3(0.0f, 0.0f, 0.0f));
        shader.SetVector3("spotLight.diffuse", flashLightIntensity);
        shader.SetVector3("spotLight.specular", new Vector3(1.0f, 1.0f, 1.0f));
        shader.SetFloat("spotLight.constant", 1.0f);
        shader.SetFloat("spotLight.linear", 0.09f);
        shader.SetFloat("spotLight.quadratic", 0.032f);
        shader.SetFloat("spotLight.cutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
        shader.SetFloat("spotLight.outerCutOff", (float)Math.Cos(MathHelper.DegreesToRadians(17.5f)));
    }

    public void ConfigureShader(Camera camera, Shader shader)
    {
        shader.SetVector3("spotLight.position", camera.Position);
        shader.SetVector3("spotLight.direction", camera.Front);
        
    }

    public static Spotlight MakeDefault()
    {
        return new Spotlight(new Vector3(1.0f, 1.0f, 1.0f), 0.8f);
    }
}