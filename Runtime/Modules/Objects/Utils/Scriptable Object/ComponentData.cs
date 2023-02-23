using System;
using JetBrains.Annotations;
using UnityEngine;

namespace UDT.Core
{
    /// <summary>
    /// This Component Data class is using for Casting to a form of Component Data that can be added to the IComponentBase.
    /// </summary>
    [Serializable]
    public class ComponentDataBase : ScriptableObject
    {
        public Type ComponentType;
    }
    
    /// <summary>
    /// Component Data that can be passed to the Component with the given Type upon creation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ComponentData<T> : ComponentDataBase where T : IComponentBase
    {
        public ComponentData()
        {
            ComponentType = typeof(T);
        }
    }
}