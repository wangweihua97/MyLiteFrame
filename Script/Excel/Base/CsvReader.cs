using System.Collections.Generic;
using System.Text;
using Script.Excel.Table.Base;
using UnityEngine;
using UnityEngine.Events;

namespace Script.Excel
{
    public class CsvReader<T> where T :  TableData ,new()
    {
        private bool isWaitUnquote = false;
        private int curLine = 0;
        private int curCol = 0;
        private int col = 0;
        public Dictionary<string, int> dataIndex;
        public string[] datas;
        StringBuilder _stringBuilder = new StringBuilder(10);
        public CsvTable<T> CsvTable;
        
        /// <summary>
        /// 初始化
        /// </summary>
        public CsvReader(CsvTable<T> csvTable)
        {
            curLine = 0;
            dataIndex = new Dictionary<string, int>();
            this.CsvTable = csvTable;
        }

        /// <summary>
        /// 一行一行的读取
        /// </summary>
        public void ReadLine(string str)
        {
            if (curLine == 0)
                ReadDesc(str);
            else if (curLine == 1)
                ReadField(str);
            else
                ReadData(str);
            if (!isWaitUnquote)
                curLine++;
        }

        /// <summary>
        /// 读取第一行的描述
        /// </summary>
        void ReadDesc(string str)
        {
            for (int i = 0; i < str.Length ; i++)
            {
                char cur = str[i];
                switch (cur)
                {
                    case '\"':
                        if (i < str.Length - 1 && str[i + 1] == '\"')
                        {
                            i++;
                            continue;
                        }
                        if (isWaitUnquote)
                            isWaitUnquote = false;
                        else
                            isWaitUnquote = true;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 读取变量
        /// </summary>
        void ReadField(string str)
        {
            string[] sArry = str.Split(',');
            col = sArry.Length;
            datas = new string[col];
            for (int i = 0; i < col; i++)
            {
                if (dataIndex.ContainsKey(sArry[i]))
                {
                    dataIndex[sArry[i]] = i;
                }
                else
                {
                    dataIndex.Add(sArry[i] ,i);
                }
                    
            }
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        void ReadData(string str)
        {
            for (int i = 0; i < str.Length ; i++)
            {
                char cur = str[i];
                switch (cur)
                {
                    case '\"':
                        if (i < str.Length - 1 && str[i + 1] == '\"')
                        {
                            _stringBuilder.Append('\"');
                            i++;
                            continue;
                        }
                        if (isWaitUnquote)
                            isWaitUnquote = false;
                        else
                            isWaitUnquote = true;
                        break;
                    case ',':
                        if(isWaitUnquote)
                            _stringBuilder.Append(cur);
                        else
                        {
                            datas[curCol] = _stringBuilder.ToString();
                            curCol++;
                            _stringBuilder.Clear();
                        }
                        break;
                    default:
                        _stringBuilder.Append(cur);
                        break;
                }
            }
            if (!isWaitUnquote)
            {
                datas[curCol] = _stringBuilder.ToString();
                curCol = 0;
                CsvTable.ReadData();
                _stringBuilder.Clear();
            }
        }
        
        /// <summary>
        /// 读取完成后执行
        /// </summary>
        public void ReadEnd()
        {
            
        }
    }
}