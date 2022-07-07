using System.Collections.Generic;
using Script.Excel.Table.Base;
using Script.Main;

namespace Script.Excel.Table
{
    public class TDTraining : TableData
    { 
        public string name;
        public string degree;
        public string battleId;
        public List<string> bodyparts;
        public List<string> basicmovement;
        public int time;
        public string monster;
        public int monsterHP = 0;
        public float calorie;
        public int difficulty;
        public int starpoint1;
        public int starpoint2;
        public int starpoint3;
        public int starpoint4;
        public int starpoint5;
        public int S  = Const.NULL_INT;
        public int A  = Const.NULL_INT;
        public int B = Const.NULL_INT;
        public int C  = Const.NULL_INT;
        public int age18 = Const.NULL_INT;
        public int age60 = Const.NULL_INT;
    }
}