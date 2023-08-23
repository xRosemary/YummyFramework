using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

namespace YummyFrameWork
{
    public class HttpRequest : MonoSingleton<HttpRequest>, IInitable
    {
        public void Init()
        {
            MessageBus.Instance.Register(PublicHttpMessage.SendHttpGet, ((string, UnityAction<string>) requestInfo) =>
            {
                Debug.Log($"尝试发送GET请求: {requestInfo.Item1}");
                StartCoroutine(HttpGet(requestInfo.Item1, requestInfo.Item2));
            });

            MessageBus.Instance.Register(PublicHttpMessage.DownLoadFile, ((string, UnityAction<string, byte[]>) requestInfo) =>
            {
                Debug.Log("尝试下载文件: " + requestInfo.Item1);
                StartCoroutine(DownloadFile(requestInfo.Item1, requestInfo.Item2));
            });
        }

        IEnumerator HttpGet(string url, UnityAction<string> callback)
        {
            using UnityWebRequest request = UnityWebRequest.Get(url);
            request.timeout = 5;
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log($"[GET] 请求已被接收: {url}");
                string json = request.downloadHandler.text;
                if (callback != null)
                {
                    callback.Invoke(json);
                }
            }
        }

        IEnumerator DownloadFile(string url, UnityAction<string, byte[]> callback)
        {
            using UnityWebRequest request = UnityWebRequest.Get(url);
            request.timeout = 5;
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log($"文件下载成功: {url}");
                byte[] data = request.downloadHandler.data;
                string fileName = Path.GetFileName(url);
                if (callback != null)
                {
                    callback.Invoke(fileName, data);
                }
            }
        }
    }
}
