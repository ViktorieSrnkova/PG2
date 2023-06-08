using System.Collections.Generic;

namespace CSharp_PG2.Managers.Object;

public class ObjectManager
{
    
    private static ObjectManager? _instance;
    
    private static Dictionary<string, Object> _objects = new Dictionary<string, Object>();
    
    private ObjectManager() {}
    
    public static ObjectManager GetInstance()
    {
        return _instance ??= new ObjectManager();
    }
    
    public Object? GetObject(string name)
    {
        if (_objects.TryGetValue(name, out var o))
        {
            return o;
        }
        
        var obj = Object.FromFile(name);
        if (obj != null)
        {
            _objects.Add(name, obj);
        }

        return obj;
    }
    
    // private void LoadObject(string path)
    
}