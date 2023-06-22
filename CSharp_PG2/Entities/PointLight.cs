using System;
using System.Runtime.Remoting;
using CSharp_PG2.Entities.Core;
using CSharp_PG2.Managers.Object;
using CSharp_PG2.Managers.Shader;
using CSharp_PG2.Managers.Shader.Entity;
using OpenTK.Mathematics;

namespace CSharp_PG2.Entities;

public class PointLight : Figure
{
    public const string ObjectName = "cube";

    public Vector3 Ambient { get; set; }
    public Vector3 Diffuse { get; set; }
    public Vector3 Specular { get; set; }
    public float Constant { get; set; } = 1f;
    public float Linear { get; set; } = 0.09f;
    public float Quadratic { get; set; } = 0.032f;
    
    private int _index;

    private Shader _mainShader;

    public PointLight(string name, int index, Mesh mesh, Vector3 position, Vector3 ambient, Vector3 diffuse, Vector3 specular) : base(mesh, position, name)
    {
        _index = index;
        Ambient = ambient;
        Diffuse = diffuse;
        Specular = specular;
        Setup();
    }

    private void Setup()
    {
        _mainShader = ShaderManager.GetInstance().GetShader("default");
        _mainShader.SetVector3($"pointLights[{_index}].position", Position);
        _mainShader.SetVector3($"pointLights[{_index}].ambient", Ambient);
        _mainShader.SetVector3($"pointLights[{_index}].diffuse", Diffuse);
        _mainShader.SetVector3($"pointLights[{_index}].specular", Specular);
        _mainShader.SetFloat($"pointLights[{_index}].constant", Constant);
        _mainShader.SetFloat($"pointLights[{_index}].linear", Linear);
        _mainShader.SetFloat($"pointLights[{_index}].quadratic",Quadratic);
    }

    public override void Draw(Camera camera, Matrix4 projection)
    {
        // base.Draw(camera, projection);
    }

    public static PointLight Create(string name, int index, Vector3 position, Vector3 ambient, Vector3 diffuse, Vector3 specular)
    {
        var mesh = GetMesh();
        return new PointLight(name, index, mesh, position, ambient, diffuse, specular);
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