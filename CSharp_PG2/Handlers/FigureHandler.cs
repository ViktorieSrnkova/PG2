using CSharp_PG2.Scenes;
using OpenTK.Mathematics;

namespace CSharp_PG2.Handlers;

public abstract class FigureHandler : ISetup, IDrawable, IDisposable
{

    private Scene _scene;

    protected FigureHandler(Scene scene)
    {
        _scene = scene;
    }
    
    public abstract void Setup();
    
    public abstract void Draw(Camera camera, Matrix4 projectionMatrix);

    public abstract void Dispose();
}