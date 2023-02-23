using System;
using JetBrains.Annotations;
using UnityEngine;

namespace UDT.Core
{
    /// <summary>
    /// This Component Data class is using for Casting to a form of Component Data that can be added to the IComponentBase.
    /// Do not extend this.
    /// </summary>
    [Serializable]
    public class ComponentDataBase : ScriptableObject
    {
    }
}