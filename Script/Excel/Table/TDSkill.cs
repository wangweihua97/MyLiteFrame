using Script.Excel.Table.Base;
using Script.Main;

namespace Script.Excel.Table
{
    public class TDSkill : TableData
    {
        public string desc = Const.NULL_STRING;
        public int type = 0;
        public string monsterID= Const.NULL_STRING;
        public string Action= Const.NULL_STRING;
        public int forwardTime;
        public int hitTime;
        public int behindTime;
        public int endTime;
        public string skillEffects= Const.NULL_STRING;
        public string skillPoint= Const.NULL_STRING;
        public string bulletEffects= Const.NULL_STRING;
        public string bulletPoint= Const.NULL_STRING;
        public float bulletFlySpeed = Const.NULL_FLOAT;
        public int bulletFlyType = 0;
        public string hitEffects= Const.NULL_STRING;
        public string hitPoint= Const.NULL_STRING;

        public string buffEffects= Const.NULL_STRING;
        public string buffPoint= Const.NULL_STRING;

    }
}