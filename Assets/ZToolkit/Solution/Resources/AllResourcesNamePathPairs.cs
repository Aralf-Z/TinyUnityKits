/*

*/

using System.Collections.Generic;

namespace ZToolKit
{
    public class AllResourcesNamePathPairs
    {
        public Dictionary<string, string> namePathDic;

        public AllResourcesNamePathPairs()
        {
            namePathDic = new();
        }

        public void AddPair(string resName, string path)
        {
            if (namePathDic.ContainsKey(resName))
            {
                namePathDic[resName] = path;
                return;
            }
            namePathDic.Add(resName,path);
        }

        public void RemovePair(string resName, string path)
        {
            namePathDic.Remove(resName);
        }

        public bool ContainsRes(string resName)
        {
            return namePathDic.ContainsKey(resName);
        }
    }
}