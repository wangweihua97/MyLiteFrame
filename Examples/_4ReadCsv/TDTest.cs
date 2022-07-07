using System.Collections.Generic;
using Script.Excel.Table.Base;

namespace Examples.ReadCsv
{
    public class TDTest: TableData
    {
        public int myInt;
        public bool myBool;
        public double myDouble;
        public string myString;
        public List<string> myListString;
        public Dictionary<string, float> MyDictionary;
    }
}