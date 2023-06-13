using OpenTK.Mathematics;

namespace CSharp_PG2;

public interface IDrawable
{

    public void Draw(Camera camera, Matrix4 projectionMatrix);

}