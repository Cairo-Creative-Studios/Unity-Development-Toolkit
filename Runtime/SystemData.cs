using UnityEngine;

namespace UDT.Core
{
    public class SystemData : ScriptableObject
    {
        /// <summary>
        /// Get the System Data for the given System Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetSystemData<T>() where T : SystemData
        {
            return Resources.Load<T>("");
        }
    }
}