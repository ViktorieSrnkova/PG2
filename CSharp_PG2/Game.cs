using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CSharp_PG2.Managers.Object;
using CSharp_PG2.Managers.Texture;
using CSharp_PG2.Utils;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace CSharp_PG2;

class Game : GameWindow
{
    private const float FOV = 90;
    
    private readonly float[] _vertices =
    {
        -0.5f, 0.0f,  0.5f,     0.83f, 0.70f, 0.44f,	0.0f, 0.0f,
        -0.5f, 0.0f, -0.5f,     0.83f, 0.70f, 0.44f,	5.0f, 0.0f,
        0.5f, 0.0f, -0.5f,     0.83f, 0.70f, 0.44f,	0.0f, 0.0f,
        0.5f, 0.0f,  0.5f,     0.83f, 0.70f, 0.44f,	5.0f, 0.0f,
        0.0f, 0.8f,  0.0f,     0.92f, 0.86f, 0.76f,	2.5f, 5.0f
    };

    private readonly uint[] _indices =
    {
        0, 1, 2,
        0, 2, 3,
        0, 1, 4,
        1, 2, 4,
        2, 3, 4,
        3, 0, 4
    };

    private readonly float[] _lightVertices =
    {
        //COORDINATES//         //color                         //poz
        -0.1f + 1, -0.1f + 1, 0.1f + 1, 1.0f, 1.0f, 1.0f, 0.9f, 0.9f,
        -0.1f + 1, -0.1f + 1, -0.1f + 1, 1.0f, 1.0f, 1.0f, 0.9f, 0.9f,
        0.1f + 1, -0.1f + 1, -0.1f + 1, 1.0f, 1.0f, 1.0f, 0.9f, 0.9f,
        0.1f + 1, -0.1f + 1, 0.1f + 1, 1.0f, 1.0f, 1.0f, 0.9f, 0.9f,
        -0.1f + 1, 0.1f + 1, 0.1f + 1, 1.0f, 1.0f, 1.0f, 0.9f, 0.9f,
        -0.1f + 1, 0.1f + 1, -0.1f + 1, 1.0f, 1.0f, 1.0f, 0.9f, 0.9f,
        0.1f + 1, 0.1f + 1, -0.1f + 1, 1.0f, 1.0f, 1.0f, 0.9f, 0.9f,
        0.1f + 1, 0.1f + 1, 0.1f + 1, 1.0f, 1.0f, 1.0f, 0.9f, 0.9f
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

    private Dictionary<String, Figure> _figures = new Dictionary<string, Figure>();

    private static readonly DebugProc OnDebugMessageDebugProc = OnDebugMessage;

    private Shader _shader;
    
    private Mesh _ground;
    private readonly uint[] _groundIndices =
    {
        0, 1, 2,
        0, 2, 3
    };
    
    private ConsoleWriter _consoleWriter = new ConsoleWriter(50);

    private int fps = 0;

    private readonly float[] _groundVertices =
    {
        -10.0f, 0.0f, -10.0f, 0, 0, 0, 0.0f, 0.0f,
        -10.0f, 0.0f, 10.0f, 0, 0, 0, 0.0f, 10.0f,
        10.0f, 0.0f, 10.0f, 0, 0, 0, 10.0f, 10.0f,
        10.0f, 0.0f, -10.0f, 0, 0, 0, 10.0f, 0.0f
    };

    public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
        _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FOV), 1f, 0.1f, 100f);
        _camera = new Camera(new Vector3(0, 1, -1));
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

        var groundTexture = TextureManager.GetInstance().GetTexture("environment:ground");
        _ground = new Mesh(_shader, VertexUtils.ConvertToVertices(_groundVertices), _groundIndices, groundTexture);

        var vertices = VertexUtils.ConvertToVertices(_vertices);
        var texture = TextureManager.GetInstance().GetTexture("structures:wall");
        var mesh = new Mesh(_shader, vertices, _indices, texture);
        _figures.Add("triangle", new Figure(mesh, new Vector3(3, 0, 0)));
        
        var test = Mesh.ReadObject("../../../Objects/cube_alt.obj");

        var lightVertices = VertexUtils.ConvertToVertices(_lightVertices);
        mesh = new Mesh(_shader, lightVertices, _lightIndices);
        _figures.Add("cube", new Figure(mesh, new Vector3(0, 0, 0)));

        _timer.Start();
        Console.WriteLine("OnLoad");
        _consoleWriter.Start();

        var obj = ObjectManager.GetInstance().GetObject("cube_alt");
        
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

        var viewMatrix = _camera.GetViewMatrix();
        _ground.Draw(_model, viewMatrix, _projection);
        foreach (var figure in _figures)
        {
            figure.Value.Draw(_camera.GetViewMatrix(), _projection);
        }

        var position = _camera.Position;

        _consoleWriter.SetMessage(GetInfo());
        
        _frameCount++;
        if (this._timer.ElapsedMilliseconds >= 1000)
        {
            fps = _frameCount;
            this.Title =
                $"FPS: {this._frameCount} - GPU: {GL.GetString(StringName.Renderer)} - CPU: {System.Environment.ProcessorCount} Cores";
            _frameCount = 0;
            _timer.Restart();
        }

        SwapBuffers();
    }

    private Dictionary<string, string> GetInfo()
    {
        var position = _camera.Position;
        var x = $"{position.X:0.00}";
        var y = $"{position.Y:0.00}";
        var z = $"{position.Z:0.00}";
        var info = new Dictionary<string, string>
        {
            { "X", x},
            { "Y", y},
            { "Z", z},
            { "FPS", fps.ToString() },
            { "VSync", Context.SwapInterval == 1 ? "On" : "Off" },
            { "Mouse", _mouseGrabbed ? "Grabbed" : "Normal" }
        };

        return info;
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
        
        if (KeyboardState.IsKeyDown(Keys.Space))
        {
            _camera.Position += _camera.ProcessInput(Camera.Direction.Up, (float)deltaTime);
        }
        
        if (KeyboardState.IsKeyDown(Keys.LeftShift))
        {
            _camera.Position += _camera.ProcessInput(Camera.Direction.Down, (float)deltaTime);
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