using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace YummyFrameWork
{
    [Obsolete]
    public class TimeWheel : MonoSingleton<TimeWheel>, IInitable
    {
        private List<Queue<UnityAction>> slots;
        private int currentSlotIndex = 0;
        [SerializeField] private float slotDuration = 0.1f;
        [SerializeField] private int numSlots = 60;

        public void Init()
        {
            slots = new List<Queue<UnityAction>>();
            for (int i = 0; i < numSlots; i++)
            {
                slots.Add(new Queue<UnityAction>());
            }
            currentSlotIndex = 0;

            StartTimeWheelLoop();
        }

        public void AddDelayedAction(UnityAction action, float delay)
        {
            int delaySlots = Mathf.CeilToInt(delay / slotDuration);
            int targetSlotIndex = (currentSlotIndex + delaySlots) % slots.Count;
            slots[targetSlotIndex].Enqueue(action);
        }

        private void StartTimeWheelLoop()
        {
            StartCoroutine(TimeWheelLoop());
        }

        private IEnumerator TimeWheelLoop()
        {
            WaitForSeconds wait = new WaitForSeconds(slotDuration);
            while (true)
            {
                Queue<UnityAction> currentSlot = slots[currentSlotIndex];
                while (currentSlot.Count > 0)
                {
                    UnityAction action = currentSlot.Dequeue();
                    action.Invoke();
                }
                currentSlotIndex = (currentSlotIndex + 1) % slots.Count;
                yield return wait;
            }
        }
    }
}

