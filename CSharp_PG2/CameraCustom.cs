using CSharp_PG2.Managers.Shader.Entity;
using OpenTK.Mathematics;

namespace CSharp_PG2;

public class CameraCustom
{
    public Vector3 Position;
    public Vector3 Orientation = new Vector3(0.0f, 0.0f, -1.0f);
    public Vector3 Up = new Vector3(0.0f, 1.0f, 0.0f);

    private int _width, _height;
    
    private float _speed = 0.1f;
    private float _sensitivity = 100.0f;
    
    public CameraCustom(int width, int height, Vector3 position)
    {
        _width = width;
        _height = height;
        Position = position;
    }
    
    public void Matrix(float FOV, float nearPlane, float farPlane, Shader shader)
    {
        var view = new Matrix4();
        var projection = new Matrix4();
        
        view = Matrix4.LookAt(Position, Position + Orientation, Up);
    }
}/**/