using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public sealed class PrefabAttribute : Attribute
{
    // Fields.
    private readonly string name;
    private readonly bool persistent;

    // Properties.
    public string Name
    {
        get { return this.name; }
    }

    public bool Persistent
    {
        get { return this.persistent; }
    }

    // Constructors.
    public PrefabAttribute(string name)
    {
        this.name = name;
        this.persistent = false;
    }

    public PrefabAttribute(string name, bool persistent)
    {
        this.name = name;
        this.persistent = persistent;
    }

}