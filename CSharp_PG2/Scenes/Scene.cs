using System;
using System.Collections.Generic;
using CSharp_PG2.Entities;
using CSharp_PG2.Events;
using CSharp_PG2.Managers.Object;
using CSharp_PG2.Managers.Shader.Entity;
using OpenTK.Mathematics;

namespace CSharp_PG2.Scenes;

public abstract class Scene : IDrawable, IDisposable
{
    protected readonly Dictionary<string, Figure> Figures = new();

    private readonly Dictionary<string, List<IEventListener>> _events = new();
    
    private readonly Dictionary<string, IShaderConfigurable> _shaderConfigurables = new();

    public abstract void Setup();

    protected abstract Shader GetMainShader();
    
    public void Draw(Camera camera, Matrix4 projectionMatrix)
    {
        var shader = GetMainShader();
        shader.Use();
        shader.SetMatrix4("projection", projectionMatrix);
        shader.SetMatrix4("camPos", camera.GetViewMatrix());
        foreach (var figure in Figures.Values)
        {
            figure.Draw(camera, projectionMatrix);
        }
    }

    public void Dispose()
    {
        foreach (var figure in Figures.Values)
        {
            figure.Dispose();
        }
    } 
    
    public void AddShaderConfigurable(string name, IShaderConfigurable configurable)
    {
        _shaderConfigurables.Add(name, configurable);
    }
    
    public IShaderConfigurable GetShaderConfigurable(string name)
    {
        return _shaderConfigurables[name] ?? throw new ArgumentException($"Shader configurable '{name}' not found");
    }
    
    public void RemoveShaderConfigurable(string name)
    {
        _shaderConfigurables.Remove(name);
    }
    
    public void AddEventListener(string eventName, IEventListener listener)
    {
        if (!_events.ContainsKey(eventName))
        {
            _events.Add(eventName, new List<IEventListener>());
        }

        _events[eventName].Add(listener);
    }

    public void FireEvent<T>(T e) where T : IEvent
    {
        if (!_events.ContainsKey(e.GetEventName())) return;

        foreach (var listener in _events[e.GetEventName()])
        {
            listener.OnEvent(e);
        }
    }
}