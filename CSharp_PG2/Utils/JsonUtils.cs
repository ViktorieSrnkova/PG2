using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using CSharp_PG2.Exceptions.Utils;
using Newtonsoft.Json;

namespace CSharp_PG2.Utils;

public static class JsonUtils
{

    public static Dictionary<string, T> LoadFromFile<T>(string path)
    {
        string jsonString = File.ReadAllText(path);

        try
        {
            return JsonConvert.DeserializeObject<Dictionary<string, T>>(jsonString);
        }
        catch (Exception e)
        {
            throw new InvalidJsonFormatException($"Failed to convert '{path}' JSON to Dictionary");
        }
    }
}