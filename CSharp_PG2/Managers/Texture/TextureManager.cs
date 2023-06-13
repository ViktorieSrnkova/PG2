using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CSharp_PG2.Exceptions.Managers.Texture;
using CSharp_PG2.Exceptions.Utils;
using CSharp_PG2.Utils;


namespace CSharp_PG2.Managers.Texture;

public class TextureManager : IDisposable
{
    private const string MissingTexture = "global:missing_texture";
    
    private static TextureManager? _instance;

    private Dictionary<string, string> _atlas = new Dictionary<string, string>();

    private Dictionary<string, Texture> _textures = new Dictionary<string, Texture>();

    private readonly Logger _logger = new Logger("TextureManager");
    
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
        if (!_textures.ContainsKey(name))
        {
            throw new TextureNotFoundException($"Texture '{name}' not found");
        }
        
        return _textures.TryGetValue(name, out var texture) ? texture : null;
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
                _logger.Warn($"Unable to load texture '{name}', using missing texture");
                missing.Add(name);
            }
        }
        
        foreach (var name in missing)
        {
            _textures.Add(name, _textures[MissingTexture]);
        }
        
        _logger.Info($"Loaded {_textures.Count} textures to memory");
    }

    public void Dispose()
    {
        foreach (var texture in _textures)
        {
            texture.Value.Dispose();
        }
    }
}