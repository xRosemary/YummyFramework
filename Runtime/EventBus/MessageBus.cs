using System.Collections.Generic;
using UnityEngine.Events;
using System;

namespace YummyFrameWork
{
    public class MessageBus : Singleton<MessageBus>, IInitable
    {
        private Dictionary<string, IMessageHandler> messageDict;

        public void Init()
        {
            messageDict = new Dictionary<string, IMessageHandler>();

        }

        #region Register
        public void Register(string key, UnityAction action)
        {
            if (messageDict.TryGetValue(key, out var previousAction))
            {
                if (previousAction is MessageHandler messageHandler)
                {
                    messageHandler.action += action;
                }
            }
            else
            {
                messageDict.Add(key, new MessageHandler(action));
            }
        }

        public void Register<T>(string key, UnityAction<T> action)
        {
            if (messageDict.TryGetValue(key, out var previousAction))
            {
                if (previousAction is MessageHandler<T> messageHandler)
                {
                    messageHandler.action += action;
                }
            }
            else
            {
                messageDict.Add(key, new MessageHandler<T>(action));
            }
        }

        public void Register<R>(string key, Func<R> func)
        {
            if (messageDict.TryGetValue(key, out var previousAction))
            {
                if (previousAction is MessageWithRetHandler<R> messageHandler)
                {
                    messageHandler.func += func;
                }
            }
            else
            {
                messageDict.Add(key, new MessageWithRetHandler<R>(func));
            }
        }

        public void Register<T, R>(string key, Func<T, R> func)
        {
            if (messageDict.TryGetValue(key, out var previousAction))
            {
                if (previousAction is MessageWithRetHandler<T, R> messageHandler)
                {
                    messageHandler.func += func;
                }
            }
            else
            {
                messageDict.Add(key, new MessageWithRetHandler<T, R>(func));
            }
        }
        #endregion


        #region Remove
        public void Remove(string key, UnityAction action)
        {
            if (messageDict.TryGetValue(key, out var previousAction))
            {
                if (previousAction is MessageHandler messageHandler)
                {
                    messageHandler.action -= action;
                }
            }
        }

        public void Remove<T>(string key, UnityAction<T> action)
        {
            if (messageDict.TryGetValue(key, out var previousAction))
            {
                if (previousAction is MessageHandler<T> messageHandler)
                {
                    messageHandler.action -= action;
                }
            }
        }

        public void Remove<R>(string key, Func<R> func)
        {
            if (messageDict.TryGetValue(key, out var previousAction))
            {
                if (previousAction is MessageWithRetHandler<R> messageHandler)
                {
                    messageHandler.func -= func;
                }
            }
        }
        public void Remove<T, R>(string key, Func<T, R> func)
        {
            if (messageDict.TryGetValue(key, out var previousAction))
            {
                if (previousAction is MessageWithRetHandler<T, R> messageHandler)
                {
                    messageHandler.func -= func;
                }
            }
        }
        #endregion


        #region Send
        public void Send(string key)
        {
            if (messageDict.TryGetValue(key, out var previousAction))
            {
                (previousAction as MessageHandler)?.action.Invoke();
            }
        }

        public void Send<T>(string key, T data)
        {
            if (messageDict.TryGetValue(key, out var previousAction))
            {
                (previousAction as MessageHandler<T>)?.action.Invoke(data);
            }
        }

        public R SendWithRet<R>(string key)
        {
            if (messageDict.TryGetValue(key, out var previousAction))
            {
                if (previousAction is MessageWithRetHandler<R> messageHandler)
                {
                    return messageHandler.func.Invoke();
                }
            }

            return default(R);
        }

        public R SendWithRet<T, R>(string key, T data)
        {
            if (messageDict.TryGetValue(key, out var previousAction))
            {
                if (previousAction is MessageWithRetHandler<T, R> messageHandler)
                {
                    return messageHandler.func.Invoke(data);
                }
            }

            return default(R);
        }
        #endregion


        public void Clear()
        {
            messageDict.Clear();
        }
    }

}
