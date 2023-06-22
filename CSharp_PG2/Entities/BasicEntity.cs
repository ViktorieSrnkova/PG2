using CSharp_PG2.Managers.Object;
using OpenTK.Mathematics;

namespace CSharp_PG2.Entities;

public class BasicEntity : Figure
{

    public BasicEntity(string name, Vector3 position, string objectName) : base(GetMash(objectName), position, name)
    {
        
    }
    
    private static Mesh GetMash(string objectName)
    {
        return ObjectManager.GetInstance().GetObject(objectName).GetMesh();
    }
    
}