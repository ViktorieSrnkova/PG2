using System;
using System.Collections.Generic;
using CSharp_PG2.Managers.Shader.Factory;

namespace CSharp_PG2.Managers.Shader;

using Entity;
public class ShaderManager
{
    
    private static ShaderManager? _instance;
    
    private static Dictionary<string, Shader> _shaders = new Dictionary<string, Shader>();
    
    private readonly Logger _logger = new Logger("ShaderManager");

    private ShaderManager()
    {
        _shaders = ShaderFactory.LoadShaderFromAtlas(ShaderFactory.LoadAtlas("../../../Settings/Atlas/shaders.json"));
    }
    
    public static ShaderManager GetInstance()
    {
        return _instance ??= new ShaderManager();
    }
    
    public Shader GetShader(string name)
    {
        if (_shaders.TryGetValue(name, out var s))
        {
            return s;
        }
        
        _logger.Error($"Shader '{name}' not found");
        throw new Exception($"Shader '{name}' not found");
    }
    
}