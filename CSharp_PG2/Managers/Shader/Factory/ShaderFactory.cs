using System.Collections.Generic;
using CSharp_PG2.Exceptions.Managers.Texture;
using CSharp_PG2.Exceptions.Utils;
using CSharp_PG2.Utils;

namespace CSharp_PG2.Managers.Shader.Factory;

using Entity;

public static class ShaderFactory
{

    private const string RootPath = "../../../Shaders";
    
    public static Dictionary<string, Dictionary<string, string>> LoadAtlas(string path)
    {
        Dictionary<string, Dictionary<string, string>> json;
        try
        {
            json = JsonUtils.LoadFromFile<Dictionary<string, string>>(path);
        }
        catch (InvalidJsonFormatException e)
        {
            throw new BootstrapFailedException("Failed to load Atlas shaders.json", e);
        }

        // Loop through json and add to atlas those which value is string
        return json;
    }
    
    public static Dictionary<string, Shader> LoadShaderFromAtlas(Dictionary<string, Dictionary<string, string>> atlas)
    {
        var shaders = new Dictionary<string, Shader>();
        
        
        foreach (var (key, value) in atlas)
        {
            var vertex = value["vertex"];
            var fragment = value["fragment"];
            var shader = new Shader($"{RootPath}/{vertex}", $"{RootPath}/{fragment}");
            shaders.Add(key, shader);
        }

        return shaders;
    }

}