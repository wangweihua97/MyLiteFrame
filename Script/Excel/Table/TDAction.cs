using Script.Excel.Table.Base;

namespace Script.Excel.Table
{
    public class TDAction : TableData
    {
        public string animationID = "";
        public string name = "";
        public string description = "";
        public int actionType = 0;
        public string actAnimation = "";
        public float fTime = 0;
        public float bTime = 0;
        public float aTime = 0;
        public float time = 0;
        public string icon = "";
        public string body = "";
        public string left = "";
        public string right = "";
        public string all = "";
        public bool isInBattle;
        public bool isInSpectrum;
        public float gradeS = 0;
        public int pointS = 0;
        public float gradeA = 0;
        public int pointA = 0;
        public float gradeB = 0;
        public int pointB = 0;
        public float gradeC = 0;
        public int pointC = 0;
    }
}