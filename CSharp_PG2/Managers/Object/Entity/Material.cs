using OpenTK.Mathematics;

using CSharp_PG2.Managers.Shader.Entity;

namespace CSharp_PG2.Managers.Object.Entity;

public class Material
{
    public string Name { get; set; } // newmtl

    public Vector3 AmbientColor { get; set; } = new Vector3(1, 1, 1); // ambient color (RGB)
    public Vector3 DiffuseColor { get; set; } =  new Vector3(1, 1, 1); // diffuse color (RGB)
    public Vector3 SpecularColor { get; set; } =  new Vector3(1, 1, 1); // specular color (RGB)
    public float SpecularHighlight { get; set; } = 1;// aka shininess
    public float OpticalDensity { get; set; } = 1;// aka index of refraction
    public float Dissolve { get; set; } = 1; // 1.0 = opaque; 0.0 = fully transparent
    public string? TextureFile { get; set; } = null;

    public void SetMaterial(Shader.Entity.Shader shader)
    {
        shader.SetVector3("material.ambient", new Vector3(1, 0.5f, 0.31f));
        shader.SetVector3("material.diffuse", new Vector3(1, 0.5f, 0.31f));
        shader.SetVector3("material.specular", new Vector3(0.5f, 0.5f, 0.5f));
        shader.SetFloat("material.shininess", 32.0f);
    }
    
}