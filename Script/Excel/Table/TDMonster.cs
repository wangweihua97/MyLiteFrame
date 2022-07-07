using Script.Excel.Table.Base;
using Script.Main;

namespace Script.Excel.Table
{
    public class TDMonster : TableData
    {
        public string name;
        public string desc;
        public string map;
        
        public int ability1 = Const.NULL_INT;
        public int a1Prop1;
        public int a1Prop2;
        public int weight1;
        public string skill1;

        public int ability2 = Const.NULL_INT;
        public int a2Prop1;
        public int a2Prop2;
        public int weight2;
        public string skill2;

        public int ability3 = Const.NULL_INT;
        public int a3Prop1;
        public int a3Prop2;
        public int weight3;
        public string skill3;

        public string dead = Const.NULL_STRING;
        public string leisure = Const.NULL_STRING;
        public string born = Const.NULL_STRING;
        public string reborn = Const.NULL_STRING;
        public float deviation = 0;
    }
}