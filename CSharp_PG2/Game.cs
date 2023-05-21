using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CSharp_PG2.Utils;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace CSharp_PG2;

class Game : GameWindow
{
    private const float FOV = 90;

    private readonly float[] _vertices =
    {   //COORDINATES           /   Normals     /    TexCoord
        -0.5f, 0.0f,  0.5f,     0.0f, -1.0f, 0.0f, 	 0.0f, 0.0f,      // Bottom side
        -0.5f, 0.0f, -0.5f,     0.0f, -1.0f, 0.0f,	 0.0f, 5.0f,      // Bottom side
        0.5f, 0.0f, -0.5f,      0.0f, -1.0f, 0.0f,	 5.0f, 5.0f,      // Bottom side
        0.5f, 0.0f,  0.5f,      0.0f, -1.0f, 0.0f,	 5.0f, 0.0f,      // Bottom side

        -0.5f, 0.0f,  0.5f,     -0.8f, 0.5f,  0.0f,	 0.0f, 0.0f,      // Left Side
        -0.5f, 0.0f, -0.5f,     -0.8f, 0.5f,  0.0f,	 5.0f, 0.0f,      // Left Side
        0.0f, 0.8f,  0.0f,      -0.8f, 0.5f,  0.0f,	 5.0f, 0.0f,      // Left Side

        -0.5f, 0.0f, -0.5f,     0.0f, 0.5f, -0.8f,	 5.0f, 0.0f,       // Non-facing side
        0.5f, 0.0f, -0.5f,      0.0f, 0.5f, -0.8f,	 0.0f, 0.0f,       // Non-facing side
        0.0f, 0.8f,  0.0f,      0.0f, 0.5f, -0.8f,	 5.0f, 0.0f,       // Non-facing side

        0.5f, 0.0f, -0.5f,      0.8f, 0.5f,  0.0f,	 0.0f, 0.0f,       // Right side
        0.5f, 0.0f,  0.5f,      0.8f, 0.5f,  0.0f,	 5.0f, 0.0f,       // Right side
        0.0f, 0.8f,  0.0f,      0.8f, 0.5f,  0.0f,	 5.0f, 0.0f,       // Right side

        0.5f, 0.0f,  0.5f,      0.0f, 0.5f,  0.8f,	 5.0f, 0.0f,       // Facing side
        -0.5f, 0.0f,  0.5f,     0.0f, 0.5f,  0.8f, 	 0.0f, 0.0f,       // Facing side
        0.0f, 0.8f,  0.0f,      0.0f, 0.5f,  0.8f,	 5.0f, 0.0f,       // Facing side
    };

    private readonly uint[] _indices =
    {
        0, 1, 2, // Bottom side
        0, 2, 3, // Bottom side
        4, 6, 5, // Left side
        7, 9, 8, // Non-facing side
        10, 12, 11, // Right side
        13, 15, 14 // Facing side
    };

    private readonly float[] _lightVertices =
    {
        //COORDINATES//         //color                         //poz
        -0.1f + 1, -0.1f + 1, 0.1f + 1,     1.0f, 1.0f, 1.0f,         0.9f, 0.9f,
        -0.1f + 1, -0.1f + 1, -0.1f + 1,    1.0f, 1.0f, 1.0f,         0.9f, 0.9f,
        0.1f + 1, -0.1f + 1, -0.1f +1,     1.0f, 1.0f, 1.0f,         0.9f, 0.9f,
        0.1f + 1, -0.1f + 1, 0.1f + 1,      1.0f, 1.0f, 1.0f,         0.9f, 0.9f,
        -0.1f + 1, 0.1f + 1, 0.1f + 1,      1.0f, 1.0f, 1.0f,         0.9f, 0.9f,
        -0.1f + 1, 0.1f + 1, -0.1f + 1,     1.0f, 1.0f, 1.0f,         0.9f, 0.9f,
        0.1f + 1, 0.1f + 1, -0.1f + 1,      1.0f, 1.0f, 1.0f,         0.9f, 0.9f,
        0.1f + 1, 0.1f + 1, 0.1f + 1,        1.0f, 1.0f, 1.0f,        0.9f, 0.9f

    };

    private readonly uint[] _lightIndices =
    {
        0, 1, 2,
        0, 2, 3,
        0, 4, 7,
        0, 7, 3,
        3, 7, 6,
        3, 6, 2,
        2, 6, 5,
        2, 5, 1,
        1, 5, 4,
        1, 4, 0,
        4, 5, 6,
        4, 6, 7
    };
    

    private bool _mouseGrabbed = false;
    private Matrix4 _projection;
    private Matrix4 _model = Matrix4.Identity;
    private readonly Camera _camera;

    private Vector2 _lastMousePosition;

    private int _frameCount;
    private Stopwatch _timer = new Stopwatch();

    private Mesh _mesh;
    private Mesh _mesh2;

    private static readonly DebugProc OnDebugMessageDebugProc = OnDebugMessage;

    private Shader _shader;

    public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
        _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FOV), 1f, 0.1f, 100f);
        _camera = new Camera(new Vector3(0, 0, 3));
    }

    protected override void OnLoad()
    {
        base.OnLoad();

        // Set clear color to black
        GL.ClearColor(new Color4(0.07f, 0.13f, 0.17f, 1.0f));

        // Output base information
        Console.WriteLine(
            $"OpenGL Version: {GL.GetString(StringName.Version)}, GLSL Version: {GL.GetString(StringName.ShadingLanguageVersion)}");

        // Enable debug
        GL.DebugMessageCallback(OnDebugMessageDebugProc, IntPtr.Zero);
        GL.Enable(EnableCap.DebugOutput);

        // Enable debug output
        GL.Enable(EnableCap.DebugOutput);
        GL.Enable(EnableCap.DebugOutputSynchronous);

        // Enable depth testing
        GL.Enable(EnableCap.DepthTest);

        // Set up debug output callback
        GL.DebugMessageCallback(DebugCallback, IntPtr.Zero);

        _shader = new Shader("../../../Shaders/shader.vert", "../../../Shaders/shader.frag");

        var vertices = VertexUtils.ConvertToVertices(_vertices);
        _mesh = new Mesh(_shader, vertices, _indices);
        
        var lightVertices = VertexUtils.ConvertToVertices(_lightVertices);
        _mesh2 = new Mesh(_shader, lightVertices, _lightIndices);
        
        _timer.Start();
        Console.WriteLine("OnLoad");

        SwapBuffers();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);

        GL.Viewport(0, 0, e.Width, e.Height);
        UpdateProjectionMatrix(e);
    }

    protected override void OnKeyDown(KeyboardKeyEventArgs e)
    {
        base.OnKeyDown(e);

        switch (e.Key)
        {
            case Keys.Escape:
                Close();
                break;
            case Keys.V:
                ChangeVSync(Context.SwapInterval == 1 ? 0 : 1);
                break;
            case Keys.G:
                _mouseGrabbed = !_mouseGrabbed;
                CursorState = _mouseGrabbed ? CursorState.Grabbed : CursorState.Normal;
                break;
            case Keys.C:
                // center camera
                _camera.Position = new Vector3(0, 0, 3);
                break;
        }
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        HandleKeyboardInput(e.Time);

        _mesh.Draw(_model, _camera.GetViewMatrix(), _projection);
        _mesh2.Draw(_model, _camera.GetViewMatrix(), _projection);

        _frameCount++;
        if (this._timer.ElapsedMilliseconds >= 1000)
        {
            this.Title =
                $"FPS: {this._frameCount} - GPU: {GL.GetString(StringName.Renderer)} - CPU: {System.Environment.ProcessorCount} Cores";
            this._frameCount = 0;
            this._timer.Restart();
        }

        SwapBuffers();
    }

    private static void DebugCallback(DebugSource source, DebugType type, int id,
        DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
    {
        string messageString = Marshal.PtrToStringAnsi(message, length);
        Console.WriteLine($"{severity} {type} | {messageString}");

        if (type == DebugType.DebugTypeError)
            throw new Exception(messageString);
    }

    // Debugger handler based on official docs
    private static void OnDebugMessage(
        DebugSource source,
        DebugType type,
        int id,
        DebugSeverity severity,
        int length,
        IntPtr pMessage,
        IntPtr pUserParam)
    {
        string message = Marshal.PtrToStringAnsi(pMessage, length);
        Console.WriteLine("[{0} source={1} type={2} id={3}] {4}", severity, source, type, id, message);
        if (type == DebugType.DebugTypeError)
        {
            throw new Exception(message);
        }
    }

    private void ChangeVSync(int vsync)
    {
        Context.SwapInterval = vsync;
        Console.WriteLine("VSync: " + (vsync == 1 ? "On" : "Off"));
    }

    protected override void OnMouseMove(MouseMoveEventArgs e)
    {
        base.OnMouseMove(e);

        if (_mouseGrabbed)
        {
            var mouseDelta = MouseState.Position - _lastMousePosition;

            // Limit speed
            if (mouseDelta.Length > 100)
            {
                mouseDelta = mouseDelta.Normalized() * 100;
            }

            _camera.ProcessMouseMovement(mouseDelta.X, mouseDelta.Y);
        }

        _lastMousePosition = MouseState.Position;
    }

    private void HandleKeyboardInput(double currentTime)
    {
        var deltaTime = 0.001f;
        if (KeyboardState.IsKeyDown(Keys.W))
        {
            _camera.Position += _camera.ProcessInput(Camera.Direction.Forward, (float)deltaTime);
        }

        if (KeyboardState.IsKeyDown(Keys.S))
        {
            _camera.Position += _camera.ProcessInput(Camera.Direction.Backward, (float)deltaTime);
        }

        if (KeyboardState.IsKeyDown(Keys.A))
        {
            _camera.Position += _camera.ProcessInput(Camera.Direction.Left, (float)deltaTime);
        }

        if (KeyboardState.IsKeyDown(Keys.D))
        {
            _camera.Position += _camera.ProcessInput(Camera.Direction.Right, (float)deltaTime);
        }
    }

    private void UpdateProjectionMatrix(ResizeEventArgs e)
    {
        var aspectRatio = (float)e.Width / e.Height;
        var projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
            MathHelper.DegreesToRadians(FOV), // The vertical field of view in radians
            aspectRatio, // The aspect ratio of the window
            0.1f, // Near clipping plane
            100f // Far clipping plane
        );
        _projection = projectionMatrix;
    }
}