using System.Collections.Generic;
using CSharp_PG2.Managers.Object.Factory;

namespace CSharp_PG2.Managers.Object;

public class ObjectManager
{
    
    private static ObjectManager? _instance;
    
    private static Dictionary<string, Entity.Figure> _objects = new Dictionary<string, Entity.Figure>();
    
    private ObjectManager() {}
    
    public static ObjectManager GetInstance()
    {
        return _instance ??= new ObjectManager();
    }
    
    public Entity.Figure? GetObject(string name)
    {
        if (_objects.TryGetValue(name, out var o))
        {
            return o;
        }
        
        var obj = FigureFactory.FromFile(name);
        if (obj != null)
        {
            _objects.Add(name, obj);
        }

        return obj;
    }
    
    // private void LoadObject(string path)
    
}