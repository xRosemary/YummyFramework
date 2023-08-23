using System.Collections.Generic;
using UnityEngine;

namespace YummyFrameWork
{
    public class InputSystem : MonoBehaviour
    {
        [SerializeField] private List<InputActionMap> actionMapPrefabs;

        void Awake()
        {
            for(int i=0; i < actionMapPrefabs.Count; i++)
            {
                if (PrefabUtility.IsPartOfPrefabAsset(actionMapPrefabs[i]))
                {
                    // 是预制体则实例化
                    actionMapPrefabs[i] = Instantiate(actionMapPrefabs[i]); 
                }
            }
        }

        void OnDestroy()
        {
            foreach(var instance in actionMapPrefabs)
            {
                Destroy(instance);
            }
        }
        
        public void SetActionMapActive<T>(bool value)
        {
            foreach (var inputActionMap in actionMapPrefabs)
            {
                if (inputActionMap is T)
                {
                    inputActionMap.gameObject.SetActive(value);
                }
            }
        }

        public void InsertActionMap<T>(T actionMapPrefab) where T : InputActionMap
        {
            T inputActionMap = Instantiate(actionMapPrefab);
            actionMapPrefabs.Add(inputActionMap);
        }

        public void RemoveActionMap<T>()
        {
            foreach (var inputActionMap in actionMapPrefabs)
            {
                if (inputActionMap is T)
                {
                    actionMapPrefabs.Remove(inputActionMap);
                    Destroy(inputActionMap);
                }
            }
        }
    }
}