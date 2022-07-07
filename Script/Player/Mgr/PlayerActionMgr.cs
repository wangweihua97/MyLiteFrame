using System.Collections.Generic;
using Script.Excel.Table;
using Script.Mgr;

namespace Player
{
    public static class PlayerActionMgr
    {
        static bool IsInit = false;
        public static Dictionary<string, string> ActionDictionary;

        public static void Init()
        {
            if(IsInit)
                return;
            IsInit = true;
            ActionDictionary = new Dictionary<string, string>();
            foreach (var kvp in ExcelMgr.TDAction.GetDictionary())
            {
                TDAction action = kvp.Value;
                string key = action.body + "_" + action.left + "_" + action.right + "_" + action.all;
                if(key.Length <= 4 || ActionDictionary.ContainsKey(key))
                    continue;
                ActionDictionary.Add(key ,action.Id);
            }
        }

        public static string Get(string key)
        {
            return ActionDictionary[key];
        }
        
        //得到动画数据
        public static TDAction GetActionData(string body ,string left, string right, string all)
        {
            string key = body+ "_" + left + "_" + right + "_" + all;
            TDAction data = ExcelMgr.TDAction.Get(PlayerActionMgr.Get(key));
            return data;
        }
    }
}