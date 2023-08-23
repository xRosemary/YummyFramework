using System.Collections.Generic;
using UnityEngine;

namespace YummyFrameWork
{
    [System.Serializable]
    public class InputActionMap : MonoBehaviour
    {
        [SerializeField] protected List<InputAction> actionList;

        void Awake()
        {
            for(int i=0; i < actionList.Count; i++)
            {
                if (PrefabUtility.IsPartOfPrefabAsset(actionList[i]))
                {
                    // 是预制体则实例化
                    actionList[i] = Instantiate(actionList[i]); 
                }
            }
        }

        void OnDestroy()
        {
            foreach(var instance in actionList)
            {
                Destroy(instance);
            }
        }

        void Update()
        {
            foreach (var action in actionList)
            {
                action.Tick();
            }
        }
    }
}