using System.IO;
using UnityEditor;
using UnityEngine;

namespace YummyFrameWork
{
    public class AssetBundleExport
    {

        [MenuItem("AB������/Windows")]
        static void ExportWindows()
        {
            ExportAssetBundle(BuildTarget.StandaloneWindows);
        }

        [MenuItem("AB������/Mac")]
        static void ExportMac()
        {
            ExportAssetBundle(BuildTarget.StandaloneOSX);
        }

        [MenuItem("AB������/IOS")]
        static void ExportIOS()
        {
            ExportAssetBundle(BuildTarget.iOS);
        }

        [MenuItem("AB������/Android")]
        static void ExportAndroid()
        {
            ExportAssetBundle(BuildTarget.Android);
        }

        static void ExportAssetBundle(BuildTarget platform)
        {
            string dataPath = Application.dataPath + "/AssetBundle/";

            // �ж�dataPathĿ¼�Ƿ����
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            BuildPipeline.BuildAssetBundles(dataPath, BuildAssetBundleOptions.None, platform);
        }
    }

}