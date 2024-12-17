using System;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator
{
    private ServiceLocator() { }

    private readonly Dictionary<string, IService> _services = new();

    public static ServiceLocator Current;

    public static void Init()
    {
        Current = new ServiceLocator();
    }

    public T Get<T>() where T : IService
    {
        string key = typeof(T).Name;
        if (!_services.ContainsKey(key))
        {
            Debug.Log($"Service {key} didn't registered");
            throw new InvalidOperationException();
        }
        return (T)_services[key];
    }

    public void Register<T>(T service) where T : IService
    {
        string key = typeof (T).Name;
        if(_services.ContainsKey(key))
        {
            Debug.Log($"Service {key} is already registered");
            return;
        }
        _services.Add(key, service);
    }

    public void Unregister<T>(T service) where T : IService
    {
        string key = typeof (T).Name;
        if(!_services.ContainsKey(key))
        {
            Debug.Log($"Service {key} didnt register yed");
            return;
        }
        _services.Remove(key);
    }
}
