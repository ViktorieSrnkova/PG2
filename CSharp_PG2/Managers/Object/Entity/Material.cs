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
    public string? TextureFile { get; set; } = null;
    
}