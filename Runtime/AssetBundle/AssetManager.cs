using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace YummyFrameWork
{
    class AssetManager
    {
        private static AssetBundle mainAsset;
        private static AssetBundleManifest manifest;

        /// <summary>
        /// 已打开的AB包
        /// </summary>
        private static Dictionary<string, AssetBundle> loadedDependencies = new Dictionary<string, AssetBundle>();

        public static T Load<T>(string bundleName, string assetName) where T : Object
        {
            LoadDependencies(bundleName);
            if (!loadedDependencies.ContainsKey(bundleName))
            {
                var assetBundle = LoadFromFile(bundleName);
                if (assetBundle == null)
                {
                    Debug.Log($"{bundleName} 加载资源失败");
                    return null;
                }

                loadedDependencies.Add(bundleName, assetBundle);
            }

            var obj = loadedDependencies[bundleName].LoadAsset<T>(assetName);
            Debug.Log($"{obj.name} 加载资源成功");

            return obj is GameObject ? Object.Instantiate(obj) : obj;
        }

        public static void Unload(string assetBundle)
        {
            if (loadedDependencies.ContainsKey(assetBundle))
            {
                loadedDependencies[assetBundle].Unload(false);
                loadedDependencies.Remove(assetBundle);
            }
        }

        public static async Task<T> LoadAsync<T>(string bundleName, string assetName) where T : Object
        {
            LoadDependencies(bundleName);
            if (!loadedDependencies.ContainsKey(bundleName))
            {
                var assetBundle = await LoadFromFileAsync(bundleName);
                if (assetBundle == null)
                {
                    Debug.Log($"{bundleName} 加载资源失败");
                    return null;
                }

                loadedDependencies.Add(bundleName, assetBundle);
            }

            var request = loadedDependencies[bundleName].LoadAssetAsync<T>(assetName);
            if (!request.isDone)
            {
                await Task.Yield();
            }

            if (request.asset == null) return null;
            Debug.Log($"{request.asset.name} 加载资源成功");
            return request.asset is GameObject ? (T)Object.Instantiate(request.asset) : (T)request.asset;
        }

        public static async Task<AsyncOperation> LoadSceneAsync(string bundleName, string sceneName)
        {
            LoadDependencies(bundleName);
            if (!loadedDependencies.ContainsKey(bundleName))
            {
                var assetBundle = await LoadFromFileAsync(bundleName);
                if (assetBundle == null)
                {
                    Debug.Log($"{bundleName} 加载资源失败");
                    return null;
                }

                loadedDependencies.Add(bundleName, assetBundle);
            }

            return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        }

        public static void Clear()
        {
            AssetBundle.UnloadAllAssetBundles(true);
            loadedDependencies.Clear();
            manifest = null;
            mainAsset = null;
        }

        private static void LoadDependencies(string bundleName)
        {
            LoadMainAssetBundle();
            string[] dependencies = manifest.GetAllDependencies(bundleName);
            foreach (string dependency in dependencies)
            {
                if (loadedDependencies.ContainsKey(dependency)) continue;
                var assetBundle = LoadFromFile(dependency);
                loadedDependencies.Add(dependency, assetBundle);
            }
        }

        private static void LoadMainAssetBundle()
        {
            if (mainAsset != null) return;
            mainAsset = LoadFromFile("AssetBundle");
            if(mainAsset == null)
            {
                Debug.Log("读取mainAsset失败");
            }
            manifest = mainAsset.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
            if (mainAsset == null)
            {
                Debug.Log("读取manifest失败");
            }
        }

        private static AssetBundle LoadFromFile(string assetBundle)
        {
            string path = AssetConfig.AssetPath + assetBundle;
         
            if (!File.Exists(path))
            {
                return null;
            }

            return AssetBundle.LoadFromFile(path); 
        }

        private static async Task<AssetBundle> LoadFromFileAsync(string assetBundle)
        {
            string path = AssetConfig.AssetPath + assetBundle;
            if (!File.Exists(path))
            {
                return null;
            }

            AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(path);
            if (!request.isDone)
            {
                await Task.Yield();
            }

            return request.assetBundle;
        }
    }
}