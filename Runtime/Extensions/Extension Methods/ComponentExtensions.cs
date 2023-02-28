//Script Developed for The Cairo Engine, by Richy Mackro (Chad Wolfe), on behalf of Cairo Creative Studios

using System;
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
    /// Taken from user PizzaPie: https://answers.unity.com/questions/1445663/how-to-auto-remove-the-component-that-was-required.html
    /// </summary>
    /// <param name="monoInstanceCaller"></param>
    public static void DestroyRequiredComponents(this MonoBehaviour monoInstanceCaller)
    {
        MemberInfo memberInfo = monoInstanceCaller.GetType();
        RequireComponent[] requiredComponentsAtts = Attribute.GetCustomAttributes(memberInfo, typeof(RequireComponent), true) as RequireComponent[];
        foreach (RequireComponent rc in requiredComponentsAtts)
        {
            if (rc != null && monoInstanceCaller.GetComponent(rc.m_Type0) != null)
            {
                Object.DestroyImmediate(monoInstanceCaller.GetComponent(rc.m_Type0));
            }
        }
    }
}