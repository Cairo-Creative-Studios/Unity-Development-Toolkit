//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using UnityEngine;

public static class ComponentExtensionss
{
    public static Component CopyComponent(this Component original, GameObject destination)
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        // Copied fields can be restricted with BindingFlags
        System.Reflection.FieldInfo[] fields = type.GetFields(); 
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy;
    }
}