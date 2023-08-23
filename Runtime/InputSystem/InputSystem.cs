using System.Collections.Generic;
using UnityEngine;

namespace YummyFrameWork
{
    public class InputSystem : MonoBehaviour
    {
        [SerializeField] private List<InputActionMap> inputActionMaps;

        public void SetActionMapActive<T>(bool value)
        {
            foreach (var inputActionMap in inputActionMaps)
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
            inputActionMaps.Add(inputActionMap);
        }

        public void RemoveActionMap<T>()
        {
            foreach (var inputActionMap in inputActionMaps)
            {
                if (inputActionMap is T)
                {
                    inputActionMaps.Remove(inputActionMap);
                    Destroy(inputActionMap);
                }
            }
        }
    }
}