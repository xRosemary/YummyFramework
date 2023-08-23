using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

namespace YummyFrameWork
{
    public class HttpRequest : MonoSingleton<HttpRequest>
    {
        public void SendHttpGet(string url, UnityAction<string> callback)
        {
            Debug.Log($"尝试发送GET请求: {url}");
            StartCoroutine(HttpGet(url, callback));
        }

        public void DownloadFile(string url, UnityAction<string, byte[]> callback)
        {
            Debug.Log($"尝试下载文件: {url}");
            StartCoroutine(Download(url, callback));
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

        IEnumerator Download(string url, UnityAction<string, byte[]> callback)
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
