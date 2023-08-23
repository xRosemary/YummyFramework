using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using LitJson;

namespace YummyFrameWork
{
    public class AssetChecker : MonoBehaviour
    {
        private List<AssetData> localData;
        private List<AssetData> serverData = new List<AssetData>();
        private Dictionary<string, string> dataToDownLoadDic = new Dictionary<string, string>();
        private string assetListPath;

        void Awake()
        {
            assetListPath = AssetConfig.AssetPath + "AssetData.json";
            localData = LoadAssetListFromLocal();
            MessageBus.Instance.Register(PublicAssetMessage.StartAssetCheck, () =>
            {
                MessageBus.Instance.Send<(string, UnityAction<string>)>(
                    PublicHttpMessage.SendHttpGet, (AssetConfig.ServerAssetListURL, OnHttpGetFinished
                ));
            });
        }

        void OnAssetCheckFinished()
        {
            MessageBus.Instance.Send(PublicAssetMessage.AssetDownLoadFinished);
            Debug.Log("���ݸ������");
            SaveAssetList();
        }

        void OnHttpGetFinished(string json)
        {
            Debug.Log(json);
            serverData = JsonMapper.ToObject<List<AssetData>>(json);

            List<AssetData> dataToDownLoad = serverData.Except(localData).ToList();
            List<AssetData> dataToDelete = localData.Except(serverData).ToList();

            // ɾ�����ڵ��ļ�
            foreach (AssetData data in dataToDelete)
            {
                File.Delete(AssetConfig.AssetPath + data.name);
            }

            // �����ļ���Ϣ��
            localData = localData.Except(dataToDelete).ToList();

            // ����Ƿ���Ҫ����
            if (dataToDownLoad.Count == 0)
            {
                OnAssetCheckFinished();
                return;
            }
            // ������Ҫ���µ��ļ�
            MessageBus.Instance.Send(PublicAssetMessage.DownLoadAssetBegin, dataToDownLoad.Count);
            foreach (AssetData data in dataToDownLoad)
            {
                dataToDownLoadDic.Add(data.name, data.md5);

                MessageBus.Instance.Send<(string, UnityAction<string, byte[]>)>(
                    PublicHttpMessage.DownLoadFile,
                    (AssetConfig.DownLoadAssetURL + data.name, OnDownLoadFinish)
                );
            }
        }

        void OnDownLoadFinish(string name, byte[] data)
        {
            string md5 = AssetUtil.GetMD5(data);
            if (md5 != dataToDownLoadDic[name])
            {
                Debug.LogWarning($"���Ϸ����ļ�: {name}");
                Debug.LogWarning($"MD5: {md5}");
                return;
            }
            Debug.Log($"�ѻ�ȡ�ļ�������: {name}");
            dataToDownLoadDic.Remove(name);
            File.WriteAllBytesAsync(AssetConfig.AssetPath + name, data);
            localData.Add(new AssetData(name, md5));
            MessageBus.Instance.Send(PublicAssetMessage.SingleAssetDownLoadFinished, name);

            if (dataToDownLoadDic.Count <= 0)
            {
                OnAssetCheckFinished();
            }
        }

        List<AssetData> LoadAssetListFromLocal()
        {
            List<AssetData> ret;
            if (!File.Exists(assetListPath))
            {
                Debug.Log("δ�ҵ�ԭʼ�ļ�");
                ret = new List<AssetData>();
            }
            else
            {
                string json = File.ReadAllText(assetListPath);
                ret = JsonMapper.ToObject<List<AssetData>>(json);
                Debug.Log("��ȡ�������ݳɹ�");
            }
            return ret;
        }

        void SaveAssetList()
        {
            string data = JsonMapper.ToJson(localData);
            File.WriteAllTextAsync(assetListPath, data);
            Debug.Log($"���ݱ���ɹ�: {data}");
        }

        void OnDestroy()
        {
            localData.Clear();
            serverData.Clear();
            dataToDownLoadDic.Clear();
        }
    }
}
