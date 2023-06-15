using UDT.Reflection;
using UnityEngine;

namespace UDT.Core.Utils
{
    public class UtilityData : ScriptableObject
    {
        
    }
    
    public class UtilityData<T> : UtilityData where T : Utility, new()
    {
        public T Data { get; private set; }
        
        public T NewUtility()
        {
            var util = new T();
            util.SetProperty("Data", this);
            return util;
        }
    }
}