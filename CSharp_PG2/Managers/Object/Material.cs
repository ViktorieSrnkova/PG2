using System;
using System.Globalization;
using System.IO;
using System.Text;
using OpenTK.Mathematics;
using CSharp_PG2.Utils;

namespace CSharp_PG2.Managers.Object;

public class Material
{
    public string Name { get; set; } // newmtl

    public Vector3 AmbientColor { get; set; } // ambient color (RGB)
    public Vector3 DiffuseColor { get; set; } // diffuse color (RGB)
    public Vector3 SpecularColor { get; set; } // specular color (RGB)
    public float SpecularHighlight { get; set; } // aka shininess
    public float OpticalDensity { get; set; } // aka index of refraction
    public float Dissolve { get; set; } // 1.0 = opaque; 0.0 = fully transparent
    private string? TextureFile { get; set; } = null;

    private Material()
    {
    }

    public static Material FromFile(string path)
    {
        var reader = new StreamReader($"../../../Materials/{path}", Encoding.UTF8);
        var materialInstance = new Material();
        while (reader.ReadLine() is { } materialLine)
        {
            var substrings = materialLine.Split(" ");
            switch (substrings[0])
            {
                case "newmtl":
                    materialInstance.Name = substrings[1];
                    break;
                case "Ka":
                    var ambient = VertexUtils.FormatVector3FromFile(substrings);

                    if (ambient != null)
                    {
                        materialInstance.AmbientColor = ambient.Value;
                    }

                    break;
                case "Kd":
                    var diffuse = VertexUtils.FormatVector3FromFile(substrings);

                    if (diffuse != null)
                    {
                        materialInstance.DiffuseColor = diffuse.Value;
                    }

                    break;

                case "Ks":
                    var specular = VertexUtils.FormatVector3FromFile(substrings);

                    if (specular != null)
                    {
                        materialInstance.SpecularColor = specular.Value;
                    }

                    break;

                case "Ns":
                    var specularHighlight = Convert.ToSingle(substrings[1], CultureInfo.InvariantCulture);
                    materialInstance.SpecularHighlight = specularHighlight;

                    break;

                case "Ni":
                    var opticalDensity = Convert.ToSingle(substrings[1], CultureInfo.InvariantCulture);
                    materialInstance.OpticalDensity = opticalDensity;

                    break;
                
                case "d":
                    var dissolve = Convert.ToSingle(substrings[1], CultureInfo.InvariantCulture);
                    materialInstance.Dissolve = dissolve;

                    break;
                
                case "map_Kd":
                    var textureFile = substrings[1];
                    materialInstance.TextureFile = textureFile;

                    break;
            }
        }

        return materialInstance;
    }
}