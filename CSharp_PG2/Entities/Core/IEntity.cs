using OpenTK.Mathematics;

namespace CSharp_PG2.Entities.Core;

public interface IEntity : IDrawable, IDisposable
{

    public string GetName();
    
    public Vector3 GetPosition();
    
    public void SetPosition(Vector3 position);

}