using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace YummyFrameWork
{
    class DelayTask : IComparable<DelayTask>
    {
        /// <summary>
        /// 延迟时间
        /// </summary>
        public float Duration;

        /// <summary>
        /// 执行时间
        /// </summary>
        public float ExecTime;

        /// <summary>
        /// 循环次数，小于等于0时无限循环，大于0时循环指定次数
        /// </summary>
        public int LoopTimes;

        /// <summary>
        /// 待延迟事件
        /// </summary>
        public UnityAction Action;

        /// <summary>
        /// 事件取消时
        /// </summary>
        public UnityAction OnCancel;

        public DelayTask(UnityAction Action, float ExecTime, float Duration, int LoopTimes, UnityAction OnCancel)
        {
            this.Duration = Duration;
            this.ExecTime = ExecTime;
            this.Action = Action;
            this.LoopTimes = LoopTimes;
            this.OnCancel = OnCancel;
        }

        public int CompareTo(DelayTask other)
        {
            return Comparer<float>.Default.Compare(ExecTime, other.ExecTime);
        }
    }
}
