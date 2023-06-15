using UnityEngine;

namespace UDT.Core.Utils
{
    public class Utility
    {
        public object Data;
    }
    
    /// <summary>
    /// A utility is a generic object that can be paired with Utility Data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Utility<TUtility, TData> : Utility where TUtility : Utility, new() where TData : UtilityData
    {
        public new TData Data { get; private set; }
        
        public Utility()
        {
            Data = ScriptableObject.CreateInstance<TData>();
        }
        
        /// <summary>
        /// Creates a new instance of the Utility
        /// </summary>
        /// <returns></returns>
        public static TUtility Create()
        {
            return new TUtility();
        }
    } 
}