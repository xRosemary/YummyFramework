using System.IO;
using UnityEditor;
using UnityEngine;

namespace YummyFrameWork
{
    public class AssetBundleExport
    {

        [MenuItem("AB包导出/Windows")]
        static void ExportWindows()
        {
            ExportAssetBundle(BuildTarget.StandaloneWindows);
        }

        [MenuItem("AB包导出/Mac")]
        static void ExportMac()
        {
            ExportAssetBundle(BuildTarget.StandaloneOSX);
        }

        [MenuItem("AB包导出/IOS")]
        static void ExportIOS()
        {
            ExportAssetBundle(BuildTarget.iOS);
        }

        [MenuItem("AB包导出/Android")]
        static void ExportAndroid()
        {
            ExportAssetBundle(BuildTarget.Android);
        }

        static void ExportAssetBundle(BuildTarget platform)
        {
            string dataPath = Application.dataPath + "/AssetBundle/";

            // 判断dataPath目录是否存在
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            BuildPipeline.BuildAssetBundles(dataPath, BuildAssetBundleOptions.None, platform);
        }
    }

}