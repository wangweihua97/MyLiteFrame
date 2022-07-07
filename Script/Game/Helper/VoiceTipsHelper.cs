using System.Collections.Generic;
using Events;
using Ghost;
using Script.Excel.Table;
using Script.Mgr;
using Script.Model;

namespace Script.Game
{
    public class VoiceTipsHelper
    {
        private Dictionary<int ,VoiceTipsGroup> _dic;
        public void Init()
        {
            _dic = new Dictionary<int, VoiceTipsGroup>();
            foreach (var kvp in ExcelMgr.TDVoiceTips.GetDictionary())
            {
                TDVoiceTips voiceTips = kvp.Value;
                if (!_dic.ContainsKey(voiceTips.type))
                {
                    _dic.Add(voiceTips.type ,new VoiceTipsGroup(voiceTips.type));
                }
                _dic[voiceTips.type].AddId(voiceTips);
            }
        }

        public void VoiceTips(ActionModel actionModel, GradeType grade)
        {
            if(!actionModel.voiceTips)
                return;
            VoiceTipsGroup voiceTipsGroup = _dic[GetVoiceTipsGroup(grade)];
            string id = voiceTipsGroup.GetRandomId();
            TDVoiceTips voiceTips = ExcelMgr.TDVoiceTips.Get(id);
            TipModel tipModel = new TipModel(voiceTips.text ,voiceTips.soundRes);
            EventCenter.ins.EventTrigger("PlayTip",tipModel);
        }

        int GetVoiceTipsGroup(GradeType grade)
        {
            switch (grade)
            {
                case GradeType.Perfect:
                    return 2; 
                case GradeType.VeryGood:
                    return 2;
                case GradeType.Miss:
                    return 4;
                default:
                    return 3;
            }
        }
        
        public void Clear()
        {
            _dic.Clear();
        }


        class VoiceTipsGroup
        {
            public int type;
            public List<string> ids = new List<string>();
            public VoiceTipsGroup(int type)
            {
                this.type = type;
            }

            public void AddId(TDVoiceTips data)
            {
                ids.Add(data.Id);
            }

            public string GetRandomId()
            {
                int random = UnityEngine.Random.Range(0 ,ids.Count);
                return ids[random];
            }
        }
    }
}