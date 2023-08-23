using UnityEngine;

namespace YummyFrameWork
{
    [System.Serializable]
    public class InputAction
    {
        [SerializeField] protected KeyCode keyCode;
        [SerializeField] protected EInputType inputMode;
        [SerializeField] protected string message;

        public void Tick()
        {
            switch (inputMode)
            {
                case EInputType.KeyUp:
                    if (Input.GetKeyUp(keyCode))
                    {
                        MessageBus.Instance.Send(message);
                    }
                    break;
                case EInputType.KeyDown:
                    if (Input.GetKeyDown(keyCode))
                    {
                        MessageBus.Instance.Send(message);
                    }
                    break;
                case EInputType.Pressed:
                    if (Input.GetKey(keyCode))
                    {
                        MessageBus.Instance.Send(message);
                    }
                    break;
                case EInputType.Axis:
                    MessageBus.Instance.Send(message, Input.GetAxis(message));
                    break;
                default:
                    break;
            }
        }
    }
}
