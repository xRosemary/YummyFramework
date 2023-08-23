using System.IO;
using UnityEditor;
using UnityEngine;

namespace YummyFrameWork
{
    public class AssetBundleExport
    {

        [MenuItem("AB锟斤拷锟斤拷锟斤拷/Windows")]
        static void ExportWindows()
        {
            ExportAssetBundle(BuildTarget.StandaloneWindows);
        }

        [MenuItem("AB锟斤拷锟斤拷锟斤拷/Mac")]
        static void ExportMac()
        {
            ExportAssetBundle(BuildTarget.StandaloneOSX);
        }

        [MenuItem("AB锟斤拷锟斤拷锟斤拷/IOS")]
        static void ExportIOS()
        {
            ExportAssetBundle(BuildTarget.iOS);
        }

        [MenuItem("AB锟斤拷锟斤拷锟斤拷/Android")]
        static void ExportAndroid()
        {
            ExportAssetBundle(BuildTarget.Android);
        }

        static void ExportAssetBundle(BuildTarget platform)
        {
            string dataPath = Application.dataPath + "/AssetBundle/";

            // 锟叫讹拷dataPath目录锟角凤拷锟斤拷锟�
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            BuildPipeline.BuildAssetBundles(dataPath, BuildAssetBundleOptions.None, platform);
        }
    }

}