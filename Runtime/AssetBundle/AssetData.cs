using System;

namespace YummyFrameWork
{
    [Serializable]
    public struct AssetData
    {
        public string name;
        public string md5;

        public AssetData(string name, string md5)
        {
            this.md5 = md5;
            this.name = name;
        }

        public static bool operator ==(AssetData a, AssetData b)
        {
            return a.name == b.name && a.md5 == b.md5;
        }

        public static bool operator !=(AssetData a, AssetData b)
        {
            return !(a == b);
        }

        private bool Equals(AssetData other)
        {
            return md5 == other.md5 && name == other.name;
        }

        public override bool Equals(object obj)
        {
            return obj is AssetData other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(md5, name);
        }
    }
}
