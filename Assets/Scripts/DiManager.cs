using System;
using System.Collections.Generic;

public class DiManager
{
    private readonly Dictionary<Type, object> instances = new();

    public static DiManager Instance { get; } = new();

    private DiManager()
    {
    }

    public void Initialize()
    {
        instances.Clear();
    }

    public void Bind<T>(T instance)
    {
        instances.Add(typeof(T), instance);
    }

    public T Resolve<T>() => (T)instances[typeof(T)];
}
