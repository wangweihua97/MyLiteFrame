using System.Collections.Generic;
using Script.Excel.Table.Base;

namespace Script.Excel.Table
{
    public class TestTable : TableData
    {
        public string Name { get; set; }
        public float Value { get; set; }
        public List<int> ListA { get; set; }
        public Dictionary<string,bool> Dic { get; set; }
    }
}