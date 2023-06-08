using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CSharp_PG2.Exceptions.Managers.Texture;
using CSharp_PG2.Exceptions.Utils;
using CSharp_PG2.Utils;


namespace CSharp_PG2.Managers.Texture;

public class TextureManager
{
    private const string MISSING_TEXTURE = "global:missing_texture";
    
    private static TextureManager? _instance;

    private Dictionary<string, string> _atlas = new Dictionary<string, string>();

    private Dictionary<string, Texture> _textures = new Dictionary<string, Texture>();

    private Logger _logger = new Logger("TextureManager");
    
    private TextureManager()
    {
        LoadAtlas();
        LoadTextures();
    }

    public static TextureManager GetInstance()
    {
        return _instance ??= new TextureManager();
    }

    public Texture? GetTexture(string name)
    {
        if (!_atlas.TryGetValue(name, out var path))
        {
            Console.WriteLine($"TextureManager: Unable to find texture of name '{name}'");
            return null;
        }
        
        if (_textures.TryGetValue(name, out var texture))
        {
            return texture;
        }
        
        path = "../../../Textures/" + path;

        try
        {
            texture = Texture.FromFile(path);
        }
        catch (Exception e)
        {
            throw new TextureNotFoundException($"Unable to load texture '{path}'", e);
        }

        if (texture != null)
        {
            _textures.Add(name, texture);
        }
        return texture;
    }

    private void LoadAtlas()
    {
        Dictionary<string, string> json;
        try
        {
            json = JsonUtils.LoadFromFile<string>("../../../Settings/Atlas/textures.json");
        }
        catch (InvalidJsonFormatException e)
        {
            throw new BootstrapFailedException("Failed to load Atlas textures.json", e);
        }

        // Loop through json and add to atlas those which value is string
        _atlas = json;
        
        
        _logger.Info($"Loaded {_atlas.Count} textures to atlas");
    }
    
    private void LoadTextures()
    {
        var missing = new List<string>();
        foreach (var (name, path) in _atlas)
        {
            var filePath = "../../../Textures/" + path;
            try
            {
                var texture = Texture.FromFile(filePath);
                if (texture != null)
                {
                    _textures.Add(name, texture);
                }
            }
            catch (Exception e)
            {
                _logger.Error($"Unable to load texture '{name}', using missing texture");
                missing.Add(name);
            }
        }
        
        foreach (var name in missing)
        {
            _textures.Add(name, _textures[MISSING_TEXTURE]);
        }
        
        _logger.Info($"Loaded {_textures.Count} textures to memory");
    }
}