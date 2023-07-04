using UnityEngine;
using UDT.Core;


namespace UDT.Core.Utils
{
    public class Utility
    {
    }
    
    /// <summary>
    /// A utility is a generic object that can be paired with Utility Data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Utility<TUtility, TData> : Utility, IUID where TUtility : Utility, new() where TData : UtilityData
    {
        public new TData Data { get; set; }
        public int UID { get; set; }
        
        public Utility()
        {
            UID = ObjectModule.UIDCounter++;
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