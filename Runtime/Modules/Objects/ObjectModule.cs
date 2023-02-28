using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UDT.Reflection;
using UnityEngine;
using Component = UnityEngine.Component;

namespace UDT.Core
{
    public class ObjectModule : Singleton<ObjectModule>
    {
        [SerializeField]
        private List<StandardObject> _indexedObjects = new List<StandardObject>();
        [SerializeField]
        private SerializableDictionary<string, ObjectPool> _objectPools = new SerializableDictionary<string, ObjectPool>();
        private ObjectDefinition[] _resourceData = new ObjectDefinition[] { };
        private int UIDCounter = 0;
        /// <summary>
        /// Subcribe Systems to this event to get notified when a new Component is added to an Object,
        /// so they can make use of it.
        /// </summary>
        public static Action<StandardComponent> OnComponentAdded;  
        /// <summary>
        /// Subcribe Systems to this event to get notified when a Component is removed from an Object,
        /// </summary>
        public static Action<StandardComponent> OnComponentRemoved;
        /// <summary>
        /// Subcribe Systems to this event to get notified when a new Object is added to the scene,
        /// </summary>
        public static Action<StandardObject> OnObjectAdded;
        /// <summary>
        /// Subcribe Systems to this event to get notified when an Object is removed from the scene,
        /// </summary>
        public static Action<StandardObject> OnObjectRemoved;
 
        public override void Init()
        {
            // Get all prefabs in Resources folder
            _resourceData = Resources.LoadAll<ObjectDefinition>("");
        }

        /// <summary>
        /// Fill a pool with a certain amount of objects, deactivating them until they are spawned.
        /// </summary>
        /// <param name="poolName"></param>
        /// <param name="amount"></param>
        public static void FillPool(string poolName, int amount)
        {
            if (Instance._objectPools.ContainsKey(poolName))
            {
                for (int i = 0; i < amount; i++)
                {
                    Instance._objectPools[poolName].freeInstances.Add(GameObject.Instantiate(Instance._objectPools[poolName].prefab).GetComponent<StandardObject>());
                    Instance._objectPools[poolName].freeInstances[i].gameObject.SetActive(false);
                }
            }
        }
        
        /// <summary>
        /// Empty a pool, destroying all objects in it.
        /// </summary>
        /// <param name="poolName"></param>
        public static void FreePool(string poolName)
        {
            if (Instance._objectPools.ContainsKey(poolName))
            {
                foreach (var instance in Instance._objectPools[poolName].freeInstances)
                {
                    Destroy(instance.gameObject);
                }
                
                Instance._objectPools[poolName].freeInstances.Clear();
            }
        }
        
        /// <summary>
        /// Empty all pools, destroying all objects in them.
        /// </summary>
        public static void FreeAllPools()
        {
            foreach (var pool in Instance._objectPools.Values)
            {
                foreach (var instance in pool.freeInstances)
                {
                    Destroy(instance.gameObject);
                }
                
                pool.freeInstances.Clear();
            }
        }
        
        /// <summary>
        /// Empty a pool, destroying a certain amount of objects in it.
        /// </summary>
        /// <param name="poolName"></param>
        /// <param name="amount"></param>
        public static void FreePoolAmount(string poolName, int amount)
        {
            if (Instance._objectPools.ContainsKey(poolName))
            {
                for (int i = 0; i < amount; i++)
                {
                    if (Instance._objectPools[poolName].freeInstances.Count > 0)
                    {
                        Destroy(Instance._objectPools[poolName].freeInstances[0].gameObject);
                        Instance._objectPools[poolName].freeInstances.RemoveAt(0);
                    }
                }
            }
        }
        
        /// <summary>
        /// Handles the Instantiation of an Object, adds it to the Container and the IndexedObjects List
        /// </summary>
        /// <param name="instance"></param>
        public static void HandleInstantiatedObject(StandardObject instance)
        {
            instance.instanced = true;

            foreach (var previousObject in Instance._indexedObjects)
            {
                if(previousObject != null && previousObject.gameObject == instance)
                {
                    instance.UID = previousObject.UID;
                    return;
                }
            }
            
            Instance._indexedObjects.Add(instance);
            Instance.UIDCounter++;
            instance.UID = Instance.UIDCounter;

            if (instance.prefab != null&&Instance._objectPools.ContainsKey(instance.prefab.name))
            {
                Instance._objectPools[instance.prefab.name].freeInstances.Remove(instance);
                Instance._objectPools[instance.prefab.name].activeInstances.Add(instance);
                
                UpdateIIDs(instance.prefab.name);
            }
            else
            {
                if(instance.prefab == null) instance.prefab = instance.gameObject;
                
                if (!Instance._objectPools.ContainsKey(instance.prefab.name))
                    Instance._objectPools.Add(instance.prefab.name, new ObjectPool(instance.prefab));
                
                Instance._objectPools[instance.prefab.name].activeInstances.Add(instance);
                
                UpdateIIDs(instance.prefab.name);
            }
            
            OnObjectAdded?.Invoke(instance);
        }
        
        /// <summary>
        /// Removes the Object from the Container and the IndexedObjects List
        /// </summary>
        /// <param name="instance"></param>
        public static void HandleDestroyedObject(StandardObject instance)
        {
            if (Instance._indexedObjects.Contains(instance))
            {
                //Instance.RemoveAllWithUID(instance.UID);
                Instance._indexedObjects.Remove(instance);
            }
            
            instance.gameObject.SetActive(false);
            
            Instance._indexedObjects.Remove(instance);
            Instance._objectPools[instance.prefab.name].freeInstances.Add(instance);
            Instance._objectPools[instance.prefab.name].activeInstances.Remove(instance);
            
            instance.transform.SetParent(Instance._objectPools[instance.prefab.name].PoolObject.transform);
            
            UpdateIIDs(instance.prefab.name);
            
            OnObjectRemoved?.Invoke(instance);
        }
        
        /// <summary>
        /// Removes all Objects with the given UID
        /// </summary>
        /// <param name="UID"></param>
        private void RemoveAllWithUID(int UID)
        {
            foreach (var indexedObject in _indexedObjects)
            {
                if (indexedObject.UID == UID)
                {
                    StandardObject.Free(indexedObject);
                    break;
                }
            }
        }
        
        /// <summary>
        /// Get the IndexedObject with the given UID
        /// </summary>
        /// <param name="UID"></param>
        /// <returns></returns>
        public static StandardObject GetIndexedObject(int UID)
        {
            return Instance._indexedObjects.Find(x => x.UID == UID);
        }

        /// <summary>
        /// Spawn an instance of the Prefab with the given Name
        /// </summary>
        /// <param name="prefabName"></param>
        /// <returns></returns>
        public static StandardObject Instantiate(string prefabName, Transform target = null)
        {
            if (!Instance._objectPools.ContainsKey(prefabName))
            {
                Instance._objectPools.Add(prefabName, new ObjectPool(Resources.Load<GameObject>(prefabName)));
                if(Instance._objectPools[prefabName].prefab == null)    
                    return null;
            }
            
            if (Instance._objectPools[prefabName].freeInstances.Count > 0)
            {
                StandardObject instance = Instance._objectPools[prefabName].freeInstances[0];
                Instance._objectPools[prefabName].freeInstances.RemoveAt(0);
                instance.gameObject.SetActive(true);
                
                if(target != null)
                    instance.transform.SetParent(target);
                else
                    instance.transform.SetParent(null);
                
                instance.OnCreate();
                
                return instance;
            }
            else
            {
                var instance = GameObject.Instantiate(Instance._objectPools[prefabName].prefab).GetComponent<StandardObject>();
                instance.prefab = Instance._objectPools[prefabName].prefab;
                
                if(target != null)
                    instance.transform.SetParent(target);
                else
                    instance.transform.SetParent(null);
                
                instance.OnCreate();
                
                return instance;
            }
        }
        
        /// <summary>
        /// Destroy the Object with the given UID
        /// </summary>
        /// <param name="UID"></param>
        public static void Free(int UID)
        {
            StandardObject deletedObject = GetIndexedObject(UID);
            if (deletedObject != null)
            {
                StandardObject.Free(deletedObject);
            }
        }

        /// <summary>
        /// Select all Objects in the World
        /// </summary>
        /// <returns></returns>
        public static ObjectSelection SelectAll()
        {
            return new ObjectSelection(Instance._indexedObjects);
        }
        
        /// <summary>
        /// Select all Objects in the Pool with the given Name
        /// </summary>
        /// <param name="poolName"></param>
        /// <returns></returns>
        public static ObjectSelection SelectAll(string poolName, ObjectSelection originalSelection = null)
        {
            if (Instance._objectPools.ContainsKey(poolName))
            {
                if(originalSelection == null)
                    return new ObjectSelection(Instance._objectPools[poolName].activeInstances);
                else
                    return new ObjectSelection(originalSelection.FindAll(x => x.prefab.name == poolName));
            }
            else
            {
                return new ObjectSelection(null);
            }
        }
        
        /// <summary>
        /// Select all Objects with the given IBaseComponent
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static ObjectSelection SelectAll<T>(ObjectSelection originalSelection = null) where T : IComponentBase
        {
            if(originalSelection == null)
                return new ObjectSelection(Instance._indexedObjects.FindAll(x => x.HasComponent<T>()));
            else
                return new ObjectSelection(originalSelection.FindAll(x => x.HasComponent<T>()));
        }
        
        /// <summary>
        /// Select all Objects within a given Radius
        /// </summary>
        /// <param name="position"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static ObjectSelection SelectInRadius(Vector3 position, float radius, ObjectSelection originalSelection = null)
        {
            if(originalSelection == null)
                return new ObjectSelection(Instance._indexedObjects.FindAll(x => Vector3.Distance(x.transform.position, position) <= radius));
            else
                return new ObjectSelection(originalSelection.FindAll(x => Vector3.Distance(x.transform.position, position) <= radius));
        }

        //TODO: Implement box selection
        public static ObjectSelection BoxSelection(Vector3 position, Vector3 size, ObjectSelection originalSelection = null)
        {
            if (originalSelection == null)
                return new ObjectSelection(Instance._indexedObjects.FindAll(x => x.transform.position.x > position.x - size.x && x.transform.position.x < position.x + size.x && x.transform.position.y > position.y - size.y && x.transform.position.y < position.y + size.y));
            else
                return new ObjectSelection(originalSelection.FindAll(x => x.transform.position.x > position.x - size.x && x.transform.position.x < position.x + size.x && x.transform.position.y > position.y - size.y && x.transform.position.y < position.y + size.y));
        }

        private static void UpdateIIDs(string poolName)
        {
            var pool = Instance._objectPools[poolName];
            for (int i = 0; i < pool.activeInstances.Count; i++)
            {
                pool.activeInstances[i].IID = i;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static StandardObject CreateInstanceFromData(string dataName)
        {
            StandardObject instance = null;
            ObjectDefinition resource = null;
            
            for (int i = 0; i < Instance._resourceData.Length; i++)
            {
                if (Instance._resourceData[i].name == dataName)
                {
                    resource = Instance._resourceData[i];
                }
            }

            if (resource == null)
            {
                return null;
            }

            if (resource.prefab == null)
                instance = new GameObject(resource.name).AddComponent<StandardObject>();
            else
            {
                var go = GameObject.Instantiate(resource.prefab);
                instance = go.GetComponent<StandardObject>();
                if (instance == null)
                {
                    instance = go.AddComponent<StandardObject>();
                }
            }

            foreach (var componentData in resource.ComponentData)
            {
                var componentDataToUse = componentData;
                if (componentData.intantiate) componentDataToUse = ScriptableObject.Instantiate(componentData);
                
                var componentType = componentData.GetType().BaseType.GetGenericArguments()[0];
                
                bool found = false;
                foreach(var component in instance.Components.Keys)
                {
                    if(componentType == component.GetType())
                    {
                        found = true;
                        component.Data = componentData;
                        break;
                    }
                }
                if(!found)
                    instance.AddComponent(componentType, componentDataToUse, componentData.GetAttachedGOPath());
            }

            
            foreach (var system in resource.SubscribedSystems)
            {
                var addObjectMethod = Type.GetType(system)?.GetMethod("AddObject", BindingFlags.Public | BindingFlags.Static);
                addObjectMethod?.Invoke(null, new object[] { instance });
            }

            return instance;
        }
        
        /// <summary>
        /// Set the Data of a Component on a given Instance
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="data"></param>
        public static void SetComponentData(StandardObject instance, ComponentDataBase data)
        {
            if (data.intantiate) data = ScriptableObject.Instantiate(data);
            var key = instance.Components.Keys.First(c => c.GetType() == data.GetType().GetGenericArguments()[0]);
            if (key != null) instance.Components[key] = data;
        }
    }
    
    /// <summary>
    /// An Object Pool contains a List of Free and Active Objects that are instantiated based off an original Prefab
    /// </summary>
    [Serializable]
    public class ObjectPool
    {
        public GameObject prefab;
        public GameObject PoolObject;
        public List<StandardObject> freeInstances;
        public List<StandardObject> activeInstances;

        public ObjectPool(GameObject prefab)
        {
            this.prefab = prefab;
            this.freeInstances = new List<StandardObject>();
            this.activeInstances = new List<StandardObject>();
            PoolObject = new GameObject() { name = prefab.name + " Pool" };
            PoolObject.AddComponent<ObjectPoolRoot>().pool = this;
        }
    }

    /// <summary>
    /// An Object Selection is a List of Objects that can be instantiated, freed, or otherwise manipulated in bulk
    /// </summary>
    public class ObjectSelection : List<StandardObject>
    {
        public ObjectSelection(List<StandardObject> objects) : base(objects)
        {
        }
        
        /// <summary>
        /// Duplicates all Objects in the Selection
        /// </summary>
        public void Instantiate()
        {
            for (int i = 0; i < this.Count; i++)
            {
                StandardObject.Instantiate(this[i]);
            }
        }
        
        /// <summary>
        /// Free all Objects in the Selection
        /// </summary>
        public void Free()
        {
            for (int i = 0; i < this.Count; i++)
            {
                StandardObject.Free(this[i]);
            }
        }
        
        /// <summary>
        /// Add an IBaseComponent to all Objects in the Selection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void AddIComponent<T>() where T : StandardComponent, IComponentBase
        {
            for (int i = 0; i < this.Count; i++)
            {
                this[i].AddComponent<T>();
            }
        }
        
        /// <summary>
        /// Add a Unity Component to all Objects in the Selection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void AddComponent<T>() where T : Component
        {
            for (int i = 0; i < this.Count; i++)
            {
                this[i].gameObject.AddComponent<T>();
            }
        }
        
        public ComponentSelection<T> GetIComponents<T>() where T : MonoBehaviour, IComponentBase
        {
            ComponentSelection<T> selection = new ComponentSelection<T>(new List<IComponentBase>());

            foreach (var instance in this)
            {
                selection.Add(instance.GetIComponent<T>());
            }

            return selection;
        }
        
        /// <summary>
        /// Call a Method in all IBaseComponents of all Objects in the Selection
        /// </summary>
        /// <param name="methodName"></param>
        public void CallMethodInIComponents(string methodName, object[] args)
        {
            for (int i = 0; i < this.Count; i++)
            {
                foreach (var component in this[i].Components)
                {
                    component.CallMethod(methodName, args);
                }
            }
        }
        
        /// <summary>
        /// Call a Method in all Unity Components of all Objects in the Selection
        /// </summary>
        /// <param name="methodName"></param>
        public void CallMethodInComponents(string methodName, object[] args)
        {
            for (int i = 0; i < this.Count; i++)
            {
                foreach (var component in this[i].Components)
                {
                    component.CallMethod(methodName, args);
                }
            }
        }
        
        public ObjectSelection Append(ObjectSelection selection)
        {
            for (int i = 0; i < selection.Count; i++)
            {
                this.Add(selection[i]);
            }
            return this;
        }
        
        public ObjectSelection Append(StandardObject obj)
        {
            this.Add(obj);
            return this;
        }
        
        public ObjectSelection Append(List<StandardObject> objects)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                this.Add(objects[i]);
            }
            return this;
        }
        
        public ObjectSelection Append(GameObject[] objects)
        {
            for (int i = 0; i < objects.Length; i++)
            {
                this.Add(objects[i].GetComponent<StandardObject>());
            }
            return this;
        }
        
        public ObjectSelection FindAll(string tag)
        {
            return new ObjectSelection(base.FindAll(x => x.tag == tag));
        }
        
        public ObjectSelection FindAll(Vector2 position, Vector2 size)
        {
            return new ObjectSelection(base.FindAll(x => x.transform.position.x > position.x - size.x && x.transform.position.x < position.x + size.x && x.transform.position.y > position.y - size.y && x.transform.position.y < position.y + size.y));
        }
        
        public ObjectSelection FindAll(Vector2 position, float radius)
        {
            return new ObjectSelection(base.FindAll(x => Vector2.Distance(x.transform.position, position) < radius));
        }
        
        public ObjectSelection FindAll(Vector2 position, float radius, string tag)
        {
            return new ObjectSelection(base.FindAll(x => Vector2.Distance(x.transform.position, position) < radius && x.tag == tag));
        }
        
        public void RemoveAll(string tag)
        {
            base.RemoveAll(x => x.tag == tag);
        }
        
        public void RemoveAll(ObjectSelection selection)
        {
            base.RemoveAll(x => selection.Contains(x));
        }

        public void SetComponentData(ComponentDataBase data)
        {
            foreach (var instance in this)
            {
                ObjectModule.SetComponentData(instance, data);
            }
        }
    }

    public class ComponentSelection : List<IComponentBase>
    {
        public ComponentSelection(List<IComponentBase> components) : base(components)
        {
        }

        /// <summary>
        /// Get all the Objects that these Components are Attached to
        /// </summary>
        /// <returns></returns>
        public ObjectSelection GetObjects()
        {
            ObjectSelection objects = new ObjectSelection(new List<StandardObject>());
            foreach (var component in this)
            {
                objects.Add(component.Object);
            }

            return objects;
        }

        /// <summary>
        /// Call a method on all of the selected Components
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        public void CallMethod(string method, object[] args)
        {
            for (int i = 0; i < this.Count; i++)
                this[i].CallMethod(method, args);
        }
    }
    public class ComponentSelection<T> : ComponentSelection
    {
        public ComponentSelection(List<IComponentBase> components) : base(components)
        {
        }
    }
}