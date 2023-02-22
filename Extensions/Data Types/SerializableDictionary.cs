
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SerializableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    private readonly Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

    [SerializeField]
    private List<DictionaryItem> items = new List<DictionaryItem>();

    private bool invalidFlag;

    public TValue this[TKey key]
    {
        get
        {
            if (dictionary.ContainsKey(key))
                return dictionary[key];
            else
            {
                Debug.LogWarning("Key " + key + " doesn't exist!");
                return default(TValue);
            }
        }

        set
        {
            if (!dictionary.ContainsKey(key))
                dictionary.Add(key, value);
            else
                dictionary[key] = value;
        }
    }

    public ICollection<TKey> Keys
    {
        get { return dictionary.Keys; }
    }

    public ICollection<TValue> Values
    {
        get { return dictionary.Values; }
    }

    public void Add(TKey key, TValue value)
    {
        dictionary.Add(key, value);
    }

    public bool ContainsKey(TKey key)
    {
        return dictionary.ContainsKey(key);
    }

    public bool Remove(TKey key)
    {
        return dictionary.Remove(key);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return dictionary.TryGetValue(key, out value);
    }

    public void Clear()
    {
        dictionary.Clear();
    }

    public int Count
    {
        get { return dictionary.Count; }
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly
    {
        get { return (dictionary as ICollection<KeyValuePair<TKey, TValue>>).IsReadOnly; }
    }

    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
    {
        (dictionary as ICollection<KeyValuePair<TKey, TValue>>).Add(item);
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
    {
        return (dictionary as ICollection<KeyValuePair<TKey, TValue>>).Contains(item);
    }

    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        (dictionary as ICollection<KeyValuePair<TKey, TValue>>).CopyTo(array, arrayIndex);
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
    {
        return (dictionary as ICollection<KeyValuePair<TKey, TValue>>).Remove(item);
    }

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
    {
        return (dictionary as IEnumerable<KeyValuePair<TKey, TValue>>).GetEnumerator();
    }

    public IEnumerator GetEnumerator()
    {
        return dictionary.GetEnumerator();
    }

    public void OnBeforeSerialize()
    {
        if (invalidFlag)
        {
            return;
        }
        else
        {
            items.Clear();
        }

        foreach (var pair in dictionary)
        {
            items.Add(new DictionaryItem(pair.Key, pair.Value));
        }
    }

    public void OnAfterDeserialize()
    {
        dictionary.Clear();

        invalidFlag = false;

        for (var i = 0; i < items.Count; ++i)
        {
            if (items[i] != null)
            {
                if (!(dictionary.ContainsKey(items[i].key)))
                {
                    dictionary.Add(items[i].key, items[i].value);
                }
                else
                {
                    invalidFlag = true;
                    continue;
                }
            }
        }

        if (!invalidFlag)
        {
            items.Clear();
        }
    }

    /// <summary>
    /// Clones the other Dictionary into this one.
    /// </summary>
    /// <param name="from">From.</param>
    public void Clone(SerializableDictionary<TKey, TValue> from)
    {
        foreach(TKey key in from)
        {
            Add(key, from[key]);
        }
    }

    public TKey KeyAt(int index)
    {
        return items[index].key;
    }
    
    public TValue ValueAt(int index)
    {
        return items[index].value;
    }

    [Serializable]
    public class DictionaryItem
    {
        public TKey key;
        public TValue value;

        public DictionaryItem(TKey key, TValue value)
        {
            this.key = key;
            this.value = value;
        }
    }
}