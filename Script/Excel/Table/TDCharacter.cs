using System.Collections.Generic;
using Script.Excel.Table.Base;
using Script.Main;

namespace Script.Excel.Table
{
    public class TDCharacter : TableData
    {
        public string name = "";
        public string des = "";
        public string icon = Const.NULL_STRING;
        public int type;
        public string maleid;
        public bool gender;
        public string model = Const.NULL_STRING;
        public List<string> texturing;
        public string color = Const.NULL_STRING;
        public float metallic;
        public bool create = false;
        public bool defaultch = false;
        public bool coach = false;
        public int index = Const.NULL_INT;
        public int price = Const.NULL_INT;
        public int unlock = Const.NULL_INT;
        public int group = Const.NULL_INT;
        public string ptype = Const.NULL_STRING;
        public int pnum = Const.NULL_INT;
    }
}