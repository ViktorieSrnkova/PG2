using CSharp_PG2.Containers;
using OpenTK.Mathematics;

namespace CSharp_PG2.Managers.Object.Entity;

public class Face
{

    public Vector3 VertexIndices { get; set; } = new Vector3();

    public Vector3Nullable TextureIndices { get; set; } = new Vector3Nullable();
    
    public Vector3 NormalIndices { get; set; } = new Vector3();

    public Material? Material { get; set; } = null;

}