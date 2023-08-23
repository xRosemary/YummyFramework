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
            Debug.Log("数据更新完毕");
            SaveAssetList();
        }

        void OnHttpGetFinished(string json)
        {
            Debug.Log(json);
            serverData = JsonMapper.ToObject<List<AssetData>>(json);

            List<AssetData> dataToDownLoad = serverData.Except(localData).ToList();
            List<AssetData> dataToDelete = localData.Except(serverData).ToList();

            // 删除过期的文件
            foreach (AssetData data in dataToDelete)
            {
                File.Delete(AssetConfig.AssetPath + data.name);
            }

            // 更新文件信息表
            localData = localData.Except(dataToDelete).ToList();

            // 检查是否需要跟新
            if (dataToDownLoad.Count == 0)
            {
                OnAssetCheckFinished();
                return;
            }
            // 下载需要更新的文件
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
                Debug.LogWarning($"不合法的文件: {name}");
                Debug.LogWarning($"MD5: {md5}");
                return;
            }
            Debug.Log($"已获取文件数据流: {name}");
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
                Debug.Log("未找到原始文件");
                ret = new List<AssetData>();
            }
            else
            {
                string json = File.ReadAllText(assetListPath);
                ret = JsonMapper.ToObject<List<AssetData>>(json);
                Debug.Log("读取本地数据成功");
            }
            return ret;
        }

        void SaveAssetList()
        {
            string data = JsonMapper.ToJson(localData);
            File.WriteAllTextAsync(assetListPath, data);
            Debug.Log($"数据保存成功: {data}");
        }

        void OnDestroy()
        {
            localData.Clear();
            serverData.Clear();
            dataToDownLoadDic.Clear();
        }
    }
}
