using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime;
using System.Runtime.InteropServices;
using CSharp_PG2.Managers.Object;
using CSharp_PG2.Managers.Shader;
using CSharp_PG2.Managers.Shader.Entity;
using CSharp_PG2.Managers.Texture;
using CSharp_PG2.Utils;
using Microsoft.VisualBasic;
using OpenTK.Audio.OpenAL;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace CSharp_PG2;

class Game : GameWindow
{

    private const float FOV = 90;

    private readonly float[] _vertices =
    {
        -1, 1, 1, 0, 0, 0, 0.0f, 0.0f,
        -1, -1, 1, 0, 0, 0, 0.0f, 0.0f,
        -1, 1, -1, 0, 0, 0, 0.0f, 0.0f,
        -1, -1, -1, 0, 0, 0, 0.0f, 0.0f,
        1, 1, 1, 0, 0, 0, 0.0f, 0.0f,
        1, -1, 1, 0, 0, 0, 0.0f, 0.0f,
        1, 1, -1, 0, 0, 0, 0.0f, 0.0f,
        1, -1, -1, 0, 0, 0, 0.0f, 0.0f,
    };

    private readonly uint[] _indices =
    {
        5, 3, 1,
        3, 8, 4,
        7, 6, 8,
        2, 8, 6,
        1, 4, 2,
        5, 2, 6,
        5, 7, 3,
        3, 7, 8,
        7, 5, 6,
        2, 4, 8,
        1, 3, 4,
        5, 1, 2

    };

    private readonly float[] _lightVertices =
    {
        //COORDINATES//         //normals                         //poz

        //front face
        -0.1f, -0.1f, -0.1f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f,
        0.1f, -0.1f, -0.1f, 0.0f, 0.0f, -1.0f, 1.0f, 0.0f,
        0.1f, 0.1f, -0.1f, 0.0f, 0.0f, -1.0f, 1.0f, 1.0f,
        -0.1f, 0.1f, -0.1f, 0.0f, 0.0f, -1.0f, 0.0f, 1.0f,

        //right face
        0.1f, -0.1f, -0.1f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f,
        0.1f, -0.1f, 0.1f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
        0.1f, 0.1f, 0.1f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
        0.1f, 0.1f, -0.1f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f,

        //left face
        -0.1f, -0.1f, 0.1f, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f,
        -0.1f, -0.1f, -0.1f, -1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
        -0.1f, 0.1f, -0.1f, -1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
        -0.1f, 0.1f, 0.1f, -1.0f, 0.0f, 0.0f, 0.0f, 1.0f,

        //bottom face
        -0.1f, -0.1f, 0.1f, 0.0f, -1.0f, 0.0f, 0.0f, 0.0f,
        0.1f, -0.1f, 0.1f, 0.0f, -1.0f, 0.0f, 1.0f, 0.0f,
        0.1f, -0.1f, -0.1f, 0.0f, -1.0f, 0.0f, 1.0f, 1.0f,
        -0.1f, -0.1f, -0.1f, 0.0f, -1.0f, 0.0f, 0.0f, 1.0f,

        //top face
        -0.1f, 0.1f, -0.1f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f,
        0.1f, 0.1f, -0.1f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f,
        0.1f, 0.1f, 0.1f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f,
        -0.1f, 0.1f, 0.1f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f,

        //back face
        0.1f, -0.1f, 0.1f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
        -0.1f, -0.1f, 0.1f, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f,
        -0.1f, 0.1f, 0.1f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f,
        0.1f, 0.1f, 0.1f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f,
    };

    private readonly uint[] _lightIndices =
    {
        0, 1, 2, 2, 3, 0, // Front face
        4, 5, 6, 6, 7, 4, // Right face
        8, 9, 10, 10, 11, 8, // Left face
        12, 13, 14, 14, 15, 12, // Bottom face
        16, 17, 18, 18, 19, 16, // Top face
        20, 21, 22, 22, 23, 20, // Back face
    };
    
private Vector3[] _pointLightPositions= {
new Vector3(0.7f,0.2f,2.0f),
new Vector3(2.3f,-0.3f,-4.0f),
new Vector3(-4.0f,0.3f,-6.0f),
new Vector3(0.0f,0.0f,-3.0f)
};

    private bool _mouseGrabbed = false;
    private Matrix4 _projection;
    private Matrix4 _model = Matrix4.Identity;
    private readonly Camera _camera;

    private Vector2 _lastMousePosition;

    private int _frameCount;
    private Stopwatch _timer = new Stopwatch();
    private double _previousTime;

    private Dictionary<String, Figure> _figures = new Dictionary<string, Figure>();

    private static readonly DebugProc OnDebugMessageDebugProc = OnDebugMessage;

    private Shader _shader;

    private Mesh _ground;
    private Mesh _pyramid;
    private Mesh _cube;
    private Mesh _glass_cube;
    
    private Audio _backgroundAudio;
    private Audio _footstepSound;
    private bool _isFootstepSoundPlaying;

    private readonly uint[] _groundIndices =
    {
        0, 1, 2,
        0, 2, 3
    };

    private ConsoleWriter _consoleWriter = new ConsoleWriter(50);

    private int fps = 0;

    private int _up = 1;

    private Vector3 _lightPosition = new Vector3(0, 0.5f, 0);
    private float _ambientIntensity = 0.8f;
    private readonly float[] _groundVertices =
    {
        -10.0f, 0.0f, -10.0f, 0, 0, 1, 0.0f, 0.0f,
        -10.0f, 0.0f, 10.0f, 0, 0, 1, 0.0f, 10.0f,
        10.0f, 0.0f, 10.0f, 0, 0, 1, 10.0f, 10.0f,
        10.0f, 0.0f, -10.0f, 0, 0, 1, 10.0f, 0.0f
    };

    public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
        : base(gameWindowSettings, nativeWindowSettings)
    {
        _projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(FOV), 1f, 0.1f, 100f);
        _camera = new Camera(new Vector3(0, 1, -1));
        _backgroundAudio = new Audio();
        _backgroundAudio.Load("../../../Music/zia3f-wub88.wav");
       
        _footstepSound = new Audio();
        _footstepSound.Load("../../../Music/duck_feet.wav");
        _isFootstepSoundPlaying = false;
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        _timer.Start();
        _previousTime = _timer.Elapsed.TotalSeconds;
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
         //GL.Enable(EnableCap.Blend);
         //GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);

        // Enable face culling
        // GL.Enable(EnableCap.CullFace);

        // Set up debug output callback
        GL.DebugMessageCallback(DebugCallback, IntPtr.Zero);

        var lightShader = ShaderManager.GetInstance().GetShader("light");
        if (lightShader == null) {throw new Exception("Unable to load shader 'light'"); }

        
        
        var lightPosition = new Vector3(1.0f, 1.0f, 1.0f);
        var lightColor = new Vector3(1.0f, 1.0f, 1.0f);
        var diffuseColor = lightColor * 0.8f; // decrease the influence
        var ambientColor = lightColor * 0.8f; // low influence
        // var lightColor = new Vector3(220.0f, 0.0f, 0.0f);
        // var lightModel = Matrix4.CreateTranslation(lightPosition);


        _shader = ShaderManager.GetInstance().GetShader("default");

        if (_shader == null)
        {
            throw new Exception("Unable to load shader");
        }

        var flashLightIntensity = lightColor * _ambientIntensity;
        _shader.Use();
        _shader.SetVector3("dirLight.direction", new Vector3(-0.2f, -1.0f, -0.3f));
        _shader.SetVector3("dirLight.ambient", new Vector3(0.1f, 0.1f, 0.1f));
        _shader.SetVector3("dirLight.diffuse", new Vector3(0.4f, 0.4f, 0.4f));
        _shader.SetVector3("dirLight.specular", new Vector3(0.5f, 0.5f, 0.5f));
        
        _shader.SetVector3("pointLights[0].position", _pointLightPositions[0]);
        _shader.SetVector3("pointLights[0].ambient", new Vector3(0.05f, 0.05f, 0.05f));
        _shader.SetVector3("pointLights[0].diffuse", new Vector3(1.8f, 0.8f, 0.8f));
        _shader.SetVector3("pointLights[0].specular", new Vector3(1.0f, 1.0f, 1.0f));
        _shader.SetFloat("pointLights[0].constant", 1.0f);
        _shader.SetFloat("pointLights[0].linear", 0.09f);
        _shader.SetFloat("pointLights[0].quadratic", 0.032f);
     
        _shader.SetVector3("pointLights[1].position", _pointLightPositions[1]);
        _shader.SetVector3("pointLights[1].ambient", new Vector3(0.05f, 0.05f, 0.05f));
        _shader.SetVector3("pointLights[1].diffuse", new Vector3(0.8f, 1.8f, 0.8f));
        _shader.SetVector3("pointLights[1].specular", new Vector3(1.0f, 1.0f, 1.0f));
        _shader.SetFloat("pointLights[1].constant", 1.0f);
        _shader.SetFloat("pointLights[1].linear", 0.09f);
        _shader.SetFloat("pointLights[1].quadratic", 0.032f);

        _shader.SetVector3("pointLights[2].position", _pointLightPositions[2]);
        _shader.SetVector3("pointLights[2].ambient", new Vector3(0.05f, 0.05f, 0.05f));
        _shader.SetVector3("pointLights[2].diffuse", new Vector3(0.8f, 0.8f, 1.8f));
        _shader.SetVector3("pointLights[2].specular", new Vector3(1.0f, 1.0f, 1.0f));
        _shader.SetFloat("pointLights[2].constant", 1.0f);
        _shader.SetFloat("pointLights[2].linear", 0.09f);
        _shader.SetFloat("pointLights[2].quadratic", 0.032f);
        
        _shader.SetVector3("pointLights[3].position", _pointLightPositions[3]);
        _shader.SetVector3("pointLights[3].ambient", new Vector3(0.05f, 0.05f, 0.05f));
        _shader.SetVector3("pointLights[3].diffuse", new Vector3(0.8f, 0.8f, 0.8f));
        _shader.SetVector3("pointLights[3].specular", new Vector3(1.0f, 1.0f, 1.0f));
        _shader.SetFloat("pointLights[3].constant", 1.0f);
        _shader.SetFloat("pointLights[3].linear", 0.09f);
        _shader.SetFloat("pointLights[3].quadratic", 0.032f);
        
        _shader.SetVector3("spotLight.position", _camera.Position);
        _shader.SetVector3("spotLight.direction", _camera.Front);
        _shader.SetVector3("spotLight.ambient", new Vector3(0.0f, 0.0f, 0.0f));
        _shader.SetVector3("spotLight.diffuse", flashLightIntensity);
        _shader.SetVector3("spotLight.specular", new Vector3(1.0f, 1.0f, 1.0f));
        _shader.SetFloat("spotLight.constant", 1.0f);
        _shader.SetFloat("spotLight.linear", 0.09f);
        _shader.SetFloat("spotLight.quadratic", 0.032f);
        _shader.SetFloat("spotLight.cutOff", (float)Math.Cos(MathHelper.DegreesToRadians(12.5f)));
        _shader.SetFloat("spotLight.outerCutOff", (float)Math.Cos(MathHelper.DegreesToRadians(17.5f)));
        _shader.SetVector3("camPos", _camera.Position);
         //_shader.SetMatrix4("model", lightModel);

        var groundTexture = TextureManager.GetInstance().GetTexture("environment:ground");
        var crateTexture = TextureManager.GetInstance().GetTexture("structures:crate");

        _ground = new Mesh(_shader, VertexUtils.ConvertToVertices(_groundVertices), _groundIndices, groundTexture);
        
        _cube = new Mesh(lightShader, VertexUtils.ConvertToVertices(_lightVertices), _lightIndices, groundTexture);
       
        
        _figures.Add("lightCube1", new Figure(_cube, lightColor * _pointLightPositions[0]));
        _figures.Add("lightCube2", new Figure(_cube, lightColor * _pointLightPositions[1]));
        _figures.Add("lightCube3", new Figure(_cube, lightColor * _pointLightPositions[2]));
        _figures.Add("lightCube4", new Figure(_cube, lightColor * _pointLightPositions[3]));
        
        var obj = ObjectManager.GetInstance().GetObject("ghost_shaded");
        if (obj != null)
        {
            var mesh = obj.GetMesh();
           // mesh.TextureUsages.Add(new FaceUtils.TextureUsage{Texture = crateTexture});
            var diff = new Vector3(-4f, -1.1f, -5f);
            var diff2=new Vector3(-4f, -1.1f, 5f);
            var diff3=new Vector3(4f, -1.1f, 5f);
            var diff4=new Vector3(4f, -1.1f, -5f);
            _figures.Add("cube", new Figure(mesh, lightPosition - diff));
            _figures.Add("cube2", new Figure(mesh, lightPosition - diff2));
            _figures.Add("cube3", new Figure(mesh, lightPosition - diff3));
            _figures.Add("cube4", new Figure(mesh, lightPosition - diff4));
            
            
        }
        var grnd = ObjectManager.GetInstance().GetObject("ground");
        if (grnd != null)
        {
            var mesh = grnd.GetMesh();
            mesh.TextureUsages.Add(new FaceUtils.TextureUsage { Texture = groundTexture });
            var diff = new Vector3(0f, 2f, 0f);
            _figures.Add("ground", new Figure(mesh, lightPosition-diff));


        }
        var glass = ObjectManager.GetInstance().GetObject("ghost_shaded");
        if (glass != null)
        {
            var mesh = glass.GetMesh();
            mesh.TextureUsages.Add(new FaceUtils.TextureUsage { Texture = groundTexture });
            var diff = new Vector3(0f, -1f, 0f);
            _figures.Add("glass", new Figure(mesh, lightPosition-diff));


        }

        _timer.Start();
        _consoleWriter.Start();

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
            case Keys.F:
                WindowState = WindowState == WindowState.Fullscreen ? WindowState.Normal : WindowState.Fullscreen;
                break;
        }
        if (e.Key == Keys.W || e.Key == Keys.A || e.Key == Keys.S || e.Key == Keys.D)
        {
            if (!_isFootstepSoundPlaying)
            {
                _isFootstepSoundPlaying = true;
                _footstepSound.Play();
            }
        }
    }
    protected override void OnKeyUp(KeyboardKeyEventArgs e)
    {
        base.OnKeyUp(e);
        
        if (e.Key == Keys.W || e.Key == Keys.A || e.Key == Keys.S || e.Key == Keys.D)
        {
            if (_isFootstepSoundPlaying)
            {
                _isFootstepSoundPlaying = false;
                _footstepSound.Stop();
            }
        }
    }
    
    protected override void OnRenderFrame(FrameEventArgs e)
    {
        base.OnRenderFrame(e);
        //ALC.MakeContextCurrent(_backgroundAudio.Context); -- this makes the background play but sped up+ footsteps also sped up
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        
        double currentTime = _timer.Elapsed.TotalSeconds; //stopwatch se resetuje 
        double res = currentTime - _previousTime;
        if (res < 0)
        {
            res += 1;
        }
        HandleKeyboardInput(res);
        _previousTime = currentTime;
        
        var viewMatrix = _camera.GetViewMatrix();
        //_ground.Draw(_model, viewMatrix, _projection);
        _shader.SetVector3("camPos", _camera.Position);
        _shader.SetVector3("spotLight.direction", _camera.Front);

        // random vector3 between 1-2
        
        _shader.SetVector3("light.position", _lightPosition);
        foreach (var figure in _figures)
        {
            figure.Value.Draw(_camera, _projection);
            
            if (figure.Key == "lightCube")
            {
                figure.Value.SetPosition(_lightPosition);
            }
        }
        
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
        var names = new List<string>();
        foreach (var figure in _figures)
        {
            var pos = figure.Value.Position;
            names.Add($"{figure.Key}[{pos.X};{pos.Y};{pos.Z}]");
        }

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
    }
    protected override void OnMouseWheel(MouseWheelEventArgs e)
    {
        base.OnMouseWheel(e);
    
        // Adjust the ambient intensity based on the scroll direction
        if (e.OffsetY > 0)
        {
            // Scroll up, increase intensity
            _ambientIntensity += 0.1f;
            if (_ambientIntensity > 1.0f)
                _ambientIntensity = 1.0f;
        }
        else
        {
            // Scroll down, decrease intensity
            _ambientIntensity -= 0.1f;
            if (_ambientIntensity < 0.0f)
                _ambientIntensity = 0.0f;
        }
    
        // Update the ambient color with the new intensity
        var ambientColore = new Vector3(1f,1f,1f) * _ambientIntensity;
        _shader.SetVector3("spotLight.diffuse", ambientColore);
    }

    private void HandleKeyboardInput(double deltaTime)
    {
       
        
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
        
        if (KeyboardState.IsKeyDown(Keys.Up))
        {
            _lightPosition += new Vector3(0, (float)deltaTime, 0);
        }
        
        if (KeyboardState.IsKeyDown(Keys.Down))
        {
            _lightPosition -= new Vector3(0, (float)deltaTime, 0);
        }
        
        if (KeyboardState.IsKeyDown(Keys.Left))
        {
            _lightPosition -= new Vector3((float)deltaTime, 0, 0);
        }
        
        if (KeyboardState.IsKeyDown(Keys.Right))
        {
            _lightPosition += new Vector3((float)deltaTime, 0, 0);
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
    protected override void OnUnload() {
        
        _backgroundAudio.Dispose();
        _footstepSound.Dispose();
        base.OnUnload();
    }
}