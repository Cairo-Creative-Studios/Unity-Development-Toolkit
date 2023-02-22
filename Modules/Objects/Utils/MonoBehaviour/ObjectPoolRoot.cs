using NaughtyAttributes;
using UnityEngine;

namespace UDT.Core
{
    public class ObjectPoolRoot : MonoBehaviour
    {
        [Button("Instantiate All")]
        public void InstantiateAll()
        {
            while (pool.freeInstances.Count>0)
            {
                StandardObject.Instantiate(pool.freeInstances[0]);
            }
        }
        
        public ObjectPool pool;
    }
}