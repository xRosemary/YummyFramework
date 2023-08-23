using UnityEngine;

static class AssetConfig
{
    public const string ServerAssetListURL = "http://localhost:8000/api/Asset/AssetList/";
    public const string DownLoadAssetURL = "http://localhost:8000/media/uploads/";
    public static string AssetPath = Application.dataPath + "/AssetBundle/";
}