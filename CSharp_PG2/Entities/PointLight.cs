using System;
using System.Runtime.Remoting;
using CSharp_PG2.Entities.Core;
using CSharp_PG2.Managers.Object;
using OpenTK.Mathematics;

namespace CSharp_PG2.Entities;

public class PointLight : Figure
{
    public const string ObjectName = "cube";
    
    public Vector3 Color { get; set; }
    public float Intensity { get; set; }

    public PointLight(string name, Mesh mesh, Vector3 position, Vector3 color, float intensity) : base(mesh, position, name)
    {
        Color = color;
        Intensity = intensity;
    }

    public override void Draw(Camera camera, Matrix4 projection)
    {
        var shader = Mesh?.GetShader();
        shader?.Use();
        shader?.SetVector3("light.diffuse", Color);
        base.Draw(camera, projection);
    }

    public static PointLight Create(string name, Vector3 position, Vector3 color, float intensity)
    {
        var mesh = GetMesh();
        return new PointLight(name, mesh, position, color, intensity);
    }
    
    private static Mesh GetMesh()
    {
        var obj = ObjectManager.GetInstance().GetObject(ObjectName);

        if (obj == null)
        {
            throw new NullReferenceException();
        }

        obj.Shader = "light";

        return obj.GetMesh();
    }
    
}