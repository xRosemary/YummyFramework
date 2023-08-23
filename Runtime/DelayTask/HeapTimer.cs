using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace YummyFrameWork
{
    class HeapTimer : MonoSingleton<HeapTimer>, IInitable
    {
        private Heap<DelayTask> tasks;

        public void Init()
        {
            tasks = new Heap<DelayTask>();
        }

        /// <summary>
        /// 添加延迟任务
        /// </summary>
        /// <param name="action">待延迟事件</param>
        /// <param name="delay">延迟时间</param>
        /// <param name="loopTimes">循环次数，小于等于0时无限循环，大于0时循环指定次数</param>
        /// <param name="onCancel">事件取消时</param>
        public void AddTask(UnityAction action, float delay, int loopTimes = 1, UnityAction onCancel = null)
        {
            float executeTime = Time.time + delay;
            DelayTask task = new DelayTask(action, executeTime, delay, loopTimes, onCancel);
            tasks.Insert(task);
        }

        public void Stop()
        {
            while(tasks.Count > 0)
            {
                DelayTask task = tasks.Top();
                task.OnCancel?.Invoke();
                tasks.Pop();
            }
        }

        public void FixedUpdate()
        {
            float currentTime = Time.time;
            while (tasks.Count > 0)
            {
                DelayTask task = tasks.Top();
                if (task.ExecTime > currentTime)
                {
                    break;
                }

                tasks.Pop();
                ExecTask(task);
            }
        }

        private void ExecTask(DelayTask task)
        {
            task.Action.Invoke();

            if(task.LoopTimes == 1)
            {
                return;
            }

            if (task.LoopTimes > 1)
            {
                task.LoopTimes--;
            }

            task.ExecTime = Time.time + task.Duration;
            tasks.Insert(task);
        }
    }
}
