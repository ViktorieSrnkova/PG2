using OpenTK.Mathematics;

namespace CSharp_PG2
{
    
public class Camera
{
    public enum Direction
    {
        Forward,
        Backward,
        Left,
        Right,
        Up,
        Down
    }

    public Vector3 Position { get; set; }
    private Vector3 _front;
    private Vector3 _right;
    private Vector3 _up;

    private float _yaw = -90.0f;
    private float _pitch = 0.0f;
    private float _roll = 0.0f;

    private const float MovementSpeed = 1.0f;
    private const float MouseSensitivity = 0.25f;

    public Camera(Vector3 position)
    {
        Position = position;
        _up = Vector3.UnitY;
        UpdateCameraVectors();
    }

    public Matrix4 GetViewMatrix()
    {
        return Matrix4.LookAt(Position, Position + _front, _up);
    }

    public Vector3 ProcessInput(Direction direction, float deltaTime)
    {
        var velocity = MovementSpeed * deltaTime;

        return direction switch
        {
            Direction.Forward => _front * velocity,
            Direction.Backward => -_front * velocity,
            Direction.Left => -_right * velocity,
            Direction.Right => _right * velocity,
            Direction.Up => _up * velocity,
            Direction.Down => -_up * velocity,
            _ => Vector3.Zero
        };
    }

    public void ProcessMouseMovement(float xOffset, float yOffset, bool constraintPitch = true)
    {
        xOffset *= MouseSensitivity;
        yOffset *= MouseSensitivity;

        _yaw += xOffset;
        _pitch += yOffset;

        if (constraintPitch)
        {
            if (_pitch > 89.0f)
                _pitch = 89.0f;
            if (_pitch < -89.0f)
                _pitch = -89.0f;
        }

        UpdateCameraVectors();
    }

    private void UpdateCameraVectors()
    {
        Vector3 front;
        front.X = (float)(MathHelper.Cos(MathHelper.DegreesToRadians(_yaw)) *
                          MathHelper.Cos(MathHelper.DegreesToRadians(_pitch)));
        front.Y = (float)MathHelper.Sin(MathHelper.DegreesToRadians(_pitch));
        front.Z = (float)(MathHelper.Sin(MathHelper.DegreesToRadians(_yaw)) *
                          MathHelper.Cos(MathHelper.DegreesToRadians(_pitch)));

        _front = Vector3.Normalize(front);
        _right = Vector3.Normalize(Vector3.Cross(_front, Vector3.UnitY));
        _up = Vector3.Normalize(Vector3.Cross(_right, _front));
    }
}
}