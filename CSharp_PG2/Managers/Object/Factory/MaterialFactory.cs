using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using CSharp_PG2.Utils;

namespace CSharp_PG2.Managers.Object.Factory;

public static class MaterialFactory
{
    public static List<Material> FromFile(string path)
    {
        var reader = new StreamReader($"../../../Materials/{path}", Encoding.UTF8);
        Material? materialInstance = null;
        var materials = new List<Material>();
        char[] remove = { '\t' };
        while (reader.ReadLine() is { } materialLine)
        {
            var substrings = materialLine.Split(" ");
            switch (substrings[0])
            {
                case "newmtl":
                    if (materialInstance != null)
                    {
                        materials.Add(materialInstance);
                    }
                    materialInstance = new Material();
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
        
        if (materialInstance != null)
        {
            materials.Add(materialInstance);
        }

        return materials;
    }
}