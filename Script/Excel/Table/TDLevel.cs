using System.Collections.Generic;
using Script.Excel.Table.Base;
using Script.Main;

namespace Script.Excel.Table
{

    public class TDLevel : TableData
    {
        public string name = Const.NULL_STRING;
        public string icon = Const.NULL_STRING;
        public string desc = Const.NULL_STRING;
        public List<string> battle;
        public int speed = Const.NULL_INT;
        public string map= Const.NULL_STRING;
        public List<float> position;
        public List<float> rotation;
        public int bonus = Const.NULL_INT;
        public int S  = Const.NULL_INT;
        public int A  = Const.NULL_INT;
        public int B = Const.NULL_INT;
        public int C  = Const.NULL_INT;
        public int age18 = Const.NULL_INT;
        public int age60 = Const.NULL_INT;
        public string music = Const.NULL_STRING;
        public int _time = Const.NULL_INT;
        public int _weather = Const.NULL_INT;
        public int _bouns1Type = Const.NULL_INT;
        public string _bouns1ID = Const.NULL_STRING;
        public int _Amount1 = Const.NULL_INT;
        public int _bouns2Type = Const.NULL_INT;
        public string _bouns2ID = Const.NULL_STRING;
        public int _Amount2 = Const.NULL_INT;
        public int _bouns3Type = Const.NULL_INT;
        public string _bouns3ID = Const.NULL_STRING;
        public int _Amount3 = Const.NULL_INT;
        public List<string> sceneBlock;
        public List<int> born;
        public List<string> reward;
    }
}