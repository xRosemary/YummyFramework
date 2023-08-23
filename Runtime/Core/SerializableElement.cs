using UnityEngine;
namespace YummyFrameWork {
    [System.Serializable]
    public struct SerializableElement<K, V>
    {
        public K Key;
        public V Value;
    }
}
