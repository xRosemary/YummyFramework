using System.Collections.Generic;
using UnityEngine;

namespace YummyFrameWork
{
    [System.Serializable]
    public class InputActionMap : MonoBehaviour
    {
        [SerializeField] protected List<InputAction> actionList;

        void Update()
        {
            foreach (var action in actionList)
            {
                action.Tick();
            }
        }
    }
}