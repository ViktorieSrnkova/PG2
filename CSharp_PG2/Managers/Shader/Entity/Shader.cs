using System;
using System.Collections.Generic;
using System.IO;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;

namespace CSharp_PG2.Managers.Shader.Entity;

public class Shader
{
    private Logger _logger = new Logger("Shader");
    public readonly int Handle;
    private readonly Dictionary<string, int> _uniformLocations;
    private readonly Dictionary<string, int> _counter = new();

    public Shader(string vertexPath, string fragmentPath)
    {
        var vertexShaderSource = File.ReadAllText(vertexPath);
        var fragmentShaderSource = File.ReadAllText(fragmentPath);

        // Compile Vertex Shader
        var vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, vertexShaderSource);
        CompileShader(vertexShader);

        // Compile Fragment Shader
        var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, fragmentShaderSource);
        CompileShader(fragmentShader);

        Handle = GL.CreateProgram();
        GL.AttachShader(Handle, vertexShader);
        GL.AttachShader(Handle, fragmentShader);

        LinkProgram(Handle);

        GL.DetachShader(Handle, vertexShader);
        GL.DetachShader(Handle, fragmentShader);
        GL.DeleteShader(fragmentShader);
        GL.DeleteShader(vertexShader);
        
        GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out var numberOfUniforms);
        _uniformLocations = new Dictionary<string, int>();
        for (var i = 0; i < numberOfUniforms; i++) {
            var key = GL.GetActiveUniform(Handle, i, out _, out _);
            var location = GL.GetUniformLocation(Handle, key);
            _uniformLocations.Add(key, location);
        }
    }

    public void Use()
    {
        GL.UseProgram(Handle);
    }

    private static void CompileShader(int shader)
    {
        GL.CompileShader(shader);

        GL.GetShader(shader, ShaderParameter.CompileStatus, out int success);
        if (success == 0)
        {
            var infoLog = GL.GetShaderInfoLog(shader);
            Console.WriteLine(infoLog);
        }
    }

    private void AddToCounter(string name)
    {
        if (!_counter.ContainsKey(name))
        {
            _counter.Add(name, 0);
        }

        _counter[name] += 1;
    }
    
    public int GetAttribLocation(string attribName) {
        return GL.GetAttribLocation(Handle, attribName);
    }

    public void SetInt(string name, int data) {
        if (!_uniformLocations.ContainsKey(name))
        {
            return;
        };
        GL.Uniform1(_uniformLocations[name], data);
        AddToCounter(name);
    }

    public void SetFloat(string name, float data) {
        if (!_uniformLocations.ContainsKey(name))
        {
            return;
        }
        GL.Uniform1(_uniformLocations[name], data);
        AddToCounter(name);
    }

    public void SetMatrix4(string name, Matrix4 data)
    {
        if (!_uniformLocations.ContainsKey(name))
        {
            return;
        }
        GL.UniformMatrix4(_uniformLocations[name], false, ref data);
        AddToCounter(name);
    }

    public void SetVector3(string name, Vector3 data) {
        if (!_uniformLocations.ContainsKey(name))
        {
            return;
        }
        GL.Uniform3(_uniformLocations[name], data);
        AddToCounter(name);
    }
    
    public void SetVector4(string name, Vector4 data) {
        if (!_uniformLocations.ContainsKey(name))
        {
            return;
        }
        GL.Uniform4(_uniformLocations[name], data);
        AddToCounter(name);
    }
    
    private static void LinkProgram(int program)
    {
        GL.LinkProgram(program);

        GL.GetProgram(program, GetProgramParameterName.LinkStatus, out var code);
        if (code != (int)All.True)
        {
            throw new Exception($"Error occurred whilst linking Program({program})");
        }
    }

    ~Shader()
    {
        Dispose();
    }


    public void Dispose()
    {
        GL.DeleteProgram(Handle);
        GC.SuppressFinalize(this);
    }
}