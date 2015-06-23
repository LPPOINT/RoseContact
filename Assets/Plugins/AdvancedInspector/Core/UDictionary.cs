﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;

using UnityEngine;

namespace AdvancedInspector
{
    [Serializable]
    [ComVisible(false)]
    [DebuggerDisplay("Count = {Count}")]
    public class UDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IDictionary, ICollection, IEnumerable, ISerializable, IDeserializationCallback, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<TKey> keys = new List<TKey>();

        [SerializeField]
        private List<TValue> values = new List<TValue>();

        [NonSerialized]
        private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

        #region Implementation of ISerializationCallbackReceiver
        public void OnAfterDeserialize()
        {
            dictionary.Clear();
            for (int i = 0; i < keys.Count; i++)
                if (keys[i] != null && (!(keys[i] is UnityEngine.Object) || ((UnityEngine.Object)(object)keys[i])))
                    dictionary.Add(keys[i], values[i]);
        }

        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach (KeyValuePair<TKey, TValue> pair in dictionary)
            {
                if (pair.Key == null || (pair.Key is UnityEngine.Object && !((UnityEngine.Object)(object)pair.Key)))
                    continue;

                keys.Add(pair.Key);
                values.Add(pair.Value);
            }
        }
        #endregion

        #region Implementation of ISerializable
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            dictionary.GetObjectData(info, context);
        }
        #endregion

        #region Implementation of IDeserializationCallback
        public void OnDeserialization(object sender)
        {
            dictionary.OnDeserialization(sender);
        }
        #endregion

        #region Implementation IDictionary
        public bool IsFixedSize
        {
            get { return false; }
        }

        public ICollection<TKey> Keys
        {
            get { return dictionary.Keys; }
        }

        ICollection IDictionary.Keys
        {
            get { return dictionary.Keys; }
        }

        public ICollection<TValue> Values
        {
            get { return dictionary.Values; }
        }

        ICollection IDictionary.Values
        {
            get { return dictionary.Values; }
        }

        public TValue this[TKey key]
        {
            get { return dictionary[key]; }
            set { dictionary[key] = value; }
        }

        object IDictionary.this[object key]
        {
            get
            {
                if (!(key is TKey))
                    return null;

                return dictionary[(TKey)key];
            }
            set
            {
                if (!(key is TKey))
                    return;

                if (!(value is TValue) && value != null)
                    return;

                dictionary[(TKey)key] = (TValue)value;
            }
        }

        public void Add(TKey key, TValue value)
        {
            dictionary.Add(key, value);
        }

        void IDictionary.Add(object key, object value)
        {
            if (!(key is TKey))
                return;

            if (!(value is TValue) && value != null)
                return;

            dictionary.Add((TKey)key, (TValue)value);
        }

        public bool ContainsKey(TKey key)
        {
            return dictionary.ContainsKey(key);
        }

        bool IDictionary.Contains(object key)
        {
            if (!(key is TKey))
                return false;

            return dictionary.ContainsKey((TKey)key);
        }

        public bool Remove(TKey key)
        {
            return dictionary.Remove(key);
        }

        void IDictionary.Remove(object key)
        {
            if (!(key is TKey))
                return;

            dictionary.Remove((TKey)key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return dictionary.TryGetValue(key, out value);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ((IDictionary)dictionary).GetEnumerator();
        }
        #endregion

        #region Implementation ICollection
        public int Count
        {
            get { return dictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool IsSynchronized
        {
            get { return false; }
        }

        public object SyncRoot
        {
            get { return null; }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            dictionary.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            dictionary.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return dictionary.ContainsKey(item.Key) && dictionary[item.Key].Equals(item.Value);
        }

        void ICollection.CopyTo(Array array, int index) { }

        void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) { }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return dictionary.Remove(item.Key);
        }
        #endregion

        #region Implementation of IEnumerable
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dictionary.GetEnumerator();
        }
        #endregion
    }
}