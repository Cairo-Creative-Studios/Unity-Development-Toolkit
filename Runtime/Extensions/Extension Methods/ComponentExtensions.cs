//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

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
    /// <summary>
    /// Destroys all components that are required by the given MonoBehaviour
    /// Based on user PizzaPies Answer: https://answers.unity.com/questions/1445663/how-to-auto-remove-the-component-that-was-required.html
    /// </summary>
    /// <param name="monoInstanceCaller"></param>
    public static void DestroyWithRequiredComponents(this MonoBehaviour monoInstanceCaller)
    {
        MemberInfo memberInfo = monoInstanceCaller.GetType();
        RequireComponent[] requiredComponentsAtts = Attribute.GetCustomAttributes(memberInfo, typeof(RequireComponent), true) as RequireComponent[];
        var monoInstance = monoInstanceCaller.gameObject;
        List<Type> typesToDestroy = new List<Type>();
        
        foreach (RequireComponent rc in requiredComponentsAtts)
        {
            if (rc != null && monoInstanceCaller.GetComponent(rc.m_Type0) != null)
            {
                typesToDestroy.Add(rc.m_Type0);
            }
        }
        
        Object.DestroyImmediate(monoInstanceCaller);
        
        foreach (Type type in typesToDestroy)
        {
            Object.DestroyImmediate(monoInstance.GetComponent(type));
        }
    }
}