using System;
using System.Collections.Generic;
using Events;
using Script.Main;
using Script.Mgr;
using UnityEngine;

namespace OtherMgr
{
    public class AttackEffectMgr
    {
        public static void InitSkillEffect(GameFlowEvent attach)
        {
            List<string> paths = new List<string>();
            paths.Add("HitEffect");
            paths.Add(AttackEffectMgr.GetFullPath("Monster_Diss"));
            /*foreach (var kvp in ExcelMgr.TDSkill.GetDictionary())
            {
                AddSkillEffect(paths ,kvp.Value.skillEffects);
                AddSkillEffect(paths ,kvp.Value.bulletEffects);
                AddSkillEffect(paths ,kvp.Value.hitEffects);

            }*/
            PoolManager.EffectPool.poolCreatMultiGoHelper.AddGameObject(paths,attach);
        }

        static bool AddSkillEffect(List<string> paths ,string effectAddress)
        {
            if (effectAddress == Const.NULL_STRING)
                return false;
            string fullAddress = "AttackEffect/" + effectAddress + ".prefab";
            if (!AddressablesHelper.instance.IsAddressEnable("fullAddress"))
            {
                Debug.LogError("Effect文件夹下没有" + effectAddress);
                return false;
            }
            paths.Add(fullAddress);
            return true;
        }

        public static string GetFullPath(string effectPath)
        {
            return "AttackEffect/" + effectPath + ".prefab";
        }

        public static void Clear()
        {
            
        }
    }
}