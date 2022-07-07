using System.Collections.Generic;
using Script.Excel.Table.Base;
using Script.Main;

namespace Script.Excel.Table
{
    public class TDActionFlow : TableData
    {
        public int levelID = Const.NULL_INT;
        public string music  = Const.NULL_STRING;
        public float delay = Const.NULL_FLOAT;
        public int story = Const.NULL_INT;
        public int step = Const.NULL_INT;
        public int voiceTips = Const.NULL_INT;
        public string text = Const.NULL_STRING;
        public int group = 0;
        public int textTime= 0;
        public string textSoundRes = Const.NULL_STRING;
        public int rhythmEffects  = Const.NULL_INT;
        public Dictionary<string,int> node;
        
        public int sensationTime = 0;
        
        public string body = Const.NULL_STRING;
        public string left = Const.NULL_STRING;
        public string right = Const.NULL_STRING;
        public string all = Const.NULL_STRING;
        public int bounsType = Const.NULL_INT; 
        public int bounsProp1 = Const.NULL_INT;
        public int bounsProp2 = Const.NULL_INT;
        public int bounsProp3 = Const.NULL_INT;
    }
}