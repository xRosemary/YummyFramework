using System;
using UnityEngine.Events;

namespace YummyFrameWork
{
    public interface IMessageHandler
    {
    }

    public class MessageHandler : IMessageHandler
    {
        public UnityAction action;
        public MessageHandler(UnityAction action)
        {
            this.action += action;
        }
    }

    public class MessageHandler<T> : IMessageHandler
    {
        public UnityAction<T> action;

        public MessageHandler(UnityAction<T> action)
        {
            this.action += action;
        }
    }

    public class MessageWithRetHandler<R> : IMessageHandler
    {
        public Func<R> func;

        public MessageWithRetHandler(Func<R> func)
        {
            this.func += func;
        }
    }

    public class MessageWithRetHandler<T, R> : IMessageHandler
    {
        public Func<T, R> func;

        public MessageWithRetHandler(Func<T, R> func)
        {
            this.func += func;
        }
    }

}
