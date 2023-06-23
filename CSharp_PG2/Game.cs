using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CSharp_PG2.Managers.Shader;
using CSharp_PG2.Managers.Shader.Entity;
using CSharp_PG2.Scenes;
using CSharp_PG2.Utils;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace CSharp_PG2;

class Game : GameWindow
{
    private const float FOV = 90;

    private bool _mouseGrabbed = false;
    private Matrix4 _projection;
    private readonly Camera _camera;

    private Vector2 _lastMousePosition;

    private int _frameCount;
    private Stopwatch _timer = new Stopwatch();
    private double _previousTime = 0.0;
    private double _previousTimeFps;

    private static readonly DebugProc OnDebugMessageDebugProc = OnDebugMessage;

    private Shader _shader;

    private Audio _backgroundAudio;

    private ConsoleWriter _consoleWriter = new ConsoleWriter(50);

    private int fps = 0;

    private int _up = 1;

    private float _ambientIntensity = 0.8f;

    private Scene _scene;

    public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
        _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FOV), 1f, 0.1f, 100f);
        _camera = new Camera(new Vector3(0, 1, -1));
        _backgroundAudio = new Audio();
        _backgroundAudio.Load("../../../Music/13zw5-ay2qo.wav");
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        _timer.Start();
        _backgroundAudio.Play();
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

        // Transparency
        GL.Enable(EnableCap.Blend);
        GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        // Enable face culling
        GL.Enable(EnableCap.CullFace);

        // Set up debug output callback
        GL.DebugMessageCallback(DebugCallback, IntPtr.Zero);

        _shader = ShaderManager.GetInstance().GetShader("default");

        if (_shader == null)
        {
            throw new Exception("Unable to load shader");
        }

        _shader.Use();
        _scene = new DefaultScene(_camera);
        _scene.Setup();

        _shader.SetVector3("camPos", _camera.Position);

        _timer.Start();
        // _consoleWriter.Start();

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
            case Keys.F:
                WindowState = WindowState == WindowState.Fullscreen ? WindowState.Normal : WindowState.Fullscreen;
                break;
        }

        _scene.OnKeyDown(e);
    }

    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        double currentTime = _timer.Elapsed.TotalSeconds; //stopwatch se resetuje 
        if (_previousTime == 0.0)
        {
            _previousTime = currentTime;
        }

        var deltaTime = currentTime - _previousTime;

        _shader.Use();
        _shader.SetVector3("camPos", _camera.Position);
        _scene.Draw((float)deltaTime, _camera, _projection);
        _shader.Use();

        _scene.HandleKeyboardInput(KeyboardState,(float) deltaTime);
        _previousTime = currentTime;

        
        _consoleWriter.SetMessage(GetInfo());

        _frameCount++;
        if (_timer.ElapsedMilliseconds - _previousTimeFps >= 1000)
        {
            fps = _frameCount;
            Title =
                $"FPS: {_frameCount} - GPU: {GL.GetString(StringName.Renderer)} - CPU: {System.Environment.ProcessorCount} Cores";
            _frameCount = 0;
            _previousTimeFps = _timer.ElapsedMilliseconds;
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
            { "X", x },
            { "Y", y },
            { "Z", z },
            { "FPS", fps.ToString() },
            { "VSync", Context.SwapInterval == 1 ? "On" : "Off" },
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
        _scene.OnMouseMove(MouseState, e);
    }

    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);

       _scene.OnMouseWheel(MouseState, e);
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

    protected override void OnUnload()
    {
        _backgroundAudio.Dispose();
        base.OnUnload();
    }
}