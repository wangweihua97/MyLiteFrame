using System.Collections.Generic;
using Script.Mgr;
using UnityEngine;

namespace Script.Main.Base
{
    public class BaseGameFlowMgr
    {
        public static Dictionary<string ,BaseGameFlow> Dictionary = new Dictionary<string, BaseGameFlow>();
        protected static List<string> removeList = new List<string>();
        protected static Dictionary<string ,BaseGameFlow> AddDic = new Dictionary<string ,BaseGameFlow>();

        public static void AddRemoveList(string go)
        {
            removeList.Add(go);
        }
        
        public static void AddToAddList(string key, BaseGameFlow baseGameFlow)
        {
            if (AddDic.ContainsKey(key))
                AddDic[key] = baseGameFlow;
            else
                AddDic.Add(key ,baseGameFlow);
        }
        
        public static void Clear()
        {
            if(removeList.Count > 0)
            {
                foreach (var item in removeList)
                {
                    if(Dictionary.ContainsKey(item))
                        Dictionary.Remove(item);
                }
                removeList.Clear();
            }
            
            if(AddDic.Count > 0)
            {
                foreach (var item in AddDic)
                {
                    Dictionary.Add(item.Key ,item.Value);
                }
                AddDic.Clear();
            }
            
            
        }
    }
}