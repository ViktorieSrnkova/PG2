using OpenTK.Mathematics;

namespace CSharp_PG2;

public class Figure : IDisposable
{
    private readonly Mesh _mesh;
    public Vector3 Position { get; set; } = Vector3.Zero;
    private Matrix4 _model = Matrix4.Identity;
    public bool IsVisible { get; set; } = true;
    
    public Figure(Mesh mesh, Vector3 position)
    {
        _mesh = mesh;
        Move(position);
    }
    
    public void Draw(Matrix4 view, Matrix4 projection)
    {
        _mesh.Draw(_model, view, projection);
    }
    
    public void Move(Vector3 position)
    {
        // _position += position;
        Position += position;
        
        _model *= Matrix4.CreateTranslation(position);
    }
    
    public void Rotate(float angle, Vector3 axis)
    {
        _model *= Matrix4.CreateFromAxisAngle(axis, angle);
    }


    public void Dispose()
    {
        _mesh.Dispose();
    }
}