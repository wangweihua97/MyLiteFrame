using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using Script.Excel.Table.Base;
using Script.Mgr;
using Script.Model;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Script.Excel
{
    public class CsvTable<T> where T :  TableData ,new()
    {
        public string TbName;
        public AsyncOperationHandle<IList<TextAsset>> handle;

        public int Count
        {
            get
            {
                return Dictionary.Count;
            }
        }
        private Dictionary<string ,T> Dictionary = new Dictionary<string,T>();
        private List<string> Keys;
        private CsvReadState _state = CsvReadState.NoReaded;
        private CsvReader<T> csvReader;
        private Dictionary<string, int> fieldKVP;
        
        /// <summary>
        /// 初始化函数
        /// </summary>
        /// <param name="tbName">表的名字，要和build名字对应</param>
        public  CsvTable(string tbName ,Action callBack ,Action failCallBack = null)
        {
            this.TbName = tbName;
            csvReader = new CsvReader<T>(this);
            InitFieldKVP();
            _state = CsvReadState.Reaing;
            if (AddressablesHelper.instance.ContainsKey(tbName) && AddressablesHelper.instance.GetHandle(tbName).IsValid())
            {
                handle = AddressablesHelper.instance.GetHandle(tbName).Convert<IList<TextAsset>>();
                ReadText(callBack);
                return;
            }

            handle = AddressablesHelper.instance.LoadAssetsAsync<TextAsset>(tbName, "");
            handle.Completed += obj =>
            {
                if (obj.Status == AsyncOperationStatus.Succeeded)
                {
                    Debug.Log("读取------------------"+tbName +"表//" );
                        
                    ReadText(callBack);
                }
                else
                {
                    if (failCallBack != null)
                        failCallBack.Invoke();
                    _state = CsvReadState.NoReaded;
                }
            };
        }
        /// <summary>
        /// 读取表中每行的数据
        /// </summary>
        public void ReadData()
        {
            if (!csvReader.dataIndex.ContainsKey("Id"))
            {
                Debug.LogError(TbName+"CSV表没有Id数据");
                return;
            }
            if(csvReader.datas[csvReader.dataIndex["Id"]].Length <= 0)
                return;
            T t = new T();
            Type type = typeof(T);
            FieldInfo[] fieldInfos = type.GetFields();
            foreach (var field in fieldInfos)
            {
                if (!csvReader.dataIndex.ContainsKey(field.Name) || csvReader.datas[csvReader.dataIndex[field.Name]].Length <= 0)
                    continue;
                field.SetValue(t ,GetFieldValue(fieldKVP[field.Name] ,field.Name));
            }
            t.Id = csvReader.datas[csvReader.dataIndex["Id"]];
            if(!Dictionary.ContainsKey(csvReader.datas[csvReader.dataIndex["Id"]]))
                Dictionary.Add(csvReader.datas[csvReader.dataIndex["Id"]] ,t);
        }

        /// <summary>
        /// 读取表格结束后执行
        /// </summary>
        public void ReadEnd()
        {
            Keys = new List<string>(Dictionary.Keys);
            csvReader = null;
        }

        /// <summary>
        /// 根据index得到数据
        /// </summary>
        /// <param name="index">要得到数据的位置</param>
        /// <returns>数据</returns>
        public T Get(int index)
        {
            return Dictionary[Keys[index]];
        }
        /// <summary>
        /// 根据Id得到数据
        /// </summary>
        /// <param name="id">Id值</param>
        /// <returns>数据</returns>
        public T Get(string id)
        {
            if (!ContainsKey(id))
            {
                return new T();
            }
            return Dictionary[id];
        }

        /// <summary>
        /// 是否含有键
        /// </summary>
        public bool ContainsKey(string id)
        {
            return Dictionary.ContainsKey(id);
        }
        
        /// <summary>
        /// 得到表的读取状态
        /// </summary>
        public CsvReadState GetState()
        {
            return _state;
        }

        /// <summary>
        /// 得到键值对字典
        /// </summary>
        public Dictionary<string, T> GetDictionary()
        {
            return Dictionary;
        }
        
        /// <summary>
        /// 得到所有的键值
        /// </summary>
        public List<string> GetKeys()
        {
            return Keys;
        }

        /// <summary>
        /// 开始读取Csv表格
        /// </summary>
        void ReadText(Action callBack)
        {
            _state = CsvReadState.Readed;
            TextAsset textAsset = handle.Result[0];
            MemoryStream ms = new MemoryStream(textAsset.bytes);
            StreamReader sr = new StreamReader(ms);
            while (sr.Peek() >= 0)
            {
                csvReader.ReadLine(sr.ReadLine());
            }

            csvReader.ReadEnd();
            ReadEnd();
            sr.Dispose();
            ms.Dispose();
            callBack.Invoke();
        }
        
        /// <summary>
        /// 初始化成员变量的类型键值对
        /// </summary>
        void InitFieldKVP()
        {
            fieldKVP = new Dictionary<string, int>();
            Type type = typeof(T);
            FieldInfo[] fieldInfos = type.GetFields();
            foreach (var field in fieldInfos)
            {
                Type fieldType = field.FieldType;
                if (fieldType.FullName.Equals(typeof(int).FullName))
                {
                    fieldKVP.Add(field.Name ,1);
                }
                else if (fieldType.FullName.Equals(typeof(float).FullName))
                {
                    fieldKVP.Add(field.Name ,2);
                }
                else if (fieldType.FullName.Equals(typeof(bool).FullName))
                {
                    fieldKVP.Add(field.Name ,3);
                }
                else if (fieldType.FullName.Equals(typeof(string).FullName))
                {
                    fieldKVP.Add(field.Name ,4);
                }
                else if (fieldType.FullName.Equals(typeof(double).FullName))
                {
                    fieldKVP.Add(field.Name ,5);
                }
                else if (fieldType.FullName.Equals(typeof(List<int>).FullName))
                {
                    fieldKVP.Add(field.Name ,6);
                }
                else if (fieldType.FullName.Equals(typeof(List<bool>).FullName))
                {
                    fieldKVP.Add(field.Name ,7);
                }
                else if (fieldType.FullName.Equals(typeof(List<float>).FullName))
                {
                    fieldKVP.Add(field.Name ,8);
                }
                else if (fieldType.FullName.Equals(typeof(List<double>).FullName))
                {
                    fieldKVP.Add(field.Name ,9);
                }
                else if (fieldType.FullName.Equals(typeof(List<string>).FullName))
                {
                    fieldKVP.Add(field.Name ,10);
                }
                else if (fieldType.FullName.Equals(typeof(Dictionary<string,int>).FullName))
                {
                    fieldKVP.Add(field.Name ,11);
                }
                else if (fieldType.FullName.Equals(typeof(Dictionary<string,bool>).FullName))
                {
                    fieldKVP.Add(field.Name ,12);
                }
                else if (fieldType.FullName.Equals(typeof(Dictionary<string,float>).FullName))
                {
                    fieldKVP.Add(field.Name ,13);
                }
                else if (fieldType.FullName.Equals(typeof(Dictionary<string,string>).FullName))
                {
                    fieldKVP.Add(field.Name ,14);
                }
                else if (fieldType.FullName.Equals(typeof(Dictionary<string,double>).FullName))
                {
                    fieldKVP.Add(field.Name ,15);
                }
            }
        }

        /// <summary>F
        /// 根据键值对类型得值
        /// </summary>
        object GetFieldValue(int type, string name)
        {
            switch (type)
            {
                case 1:
                    return ExcelMgr.ConverHelper.ToInt(csvReader.datas[csvReader.dataIndex[name]]);
                case 2:
                    return ExcelMgr.ConverHelper.ToFloat(csvReader.datas[csvReader.dataIndex[name]]);
                case 3:
                    return ExcelMgr.ConverHelper.ToBool(csvReader.datas[csvReader.dataIndex[name]]);
                case 4:
                    return ExcelMgr.ConverHelper.ToString(csvReader.datas[csvReader.dataIndex[name]]);
                case 5:
                    return ExcelMgr.ConverHelper.ToDouble(csvReader.datas[csvReader.dataIndex[name]]);
                case 6:
                    return ExcelMgr.ConverHelper.ToList<int>(csvReader.datas[csvReader.dataIndex[name]]);
                case 7:
                    return ExcelMgr.ConverHelper.ToList<bool>(csvReader.datas[csvReader.dataIndex[name]]);
                case 8:
                    return ExcelMgr.ConverHelper.ToList<float>(csvReader.datas[csvReader.dataIndex[name]]);
                case 9:
                    return ExcelMgr.ConverHelper.ToList<double>(csvReader.datas[csvReader.dataIndex[name]]);
                case 10:
                    return ExcelMgr.ConverHelper.ToList<string>(csvReader.datas[csvReader.dataIndex[name]]);
                case 11:
                    return ExcelMgr.ConverHelper.ToDictionary<int>(csvReader.datas[csvReader.dataIndex[name]]);
                case 12:
                    return ExcelMgr.ConverHelper.ToDictionary<bool>(csvReader.datas[csvReader.dataIndex[name]]);
                case 13:
                    return ExcelMgr.ConverHelper.ToDictionary<float>(csvReader.datas[csvReader.dataIndex[name]]);
                case 14:
                    return ExcelMgr.ConverHelper.ToDictionary<string>(csvReader.datas[csvReader.dataIndex[name]]);
                case 15:
                    return ExcelMgr.ConverHelper.ToDictionary<double>(csvReader.datas[csvReader.dataIndex[name]]);
            }
            return null;
        }

        /// <summary>
        /// 清空所有数据
        /// </summary>
        public void Clear()
        {
            Keys.Clear();
            fieldKVP.Clear();
            Dictionary.Clear();
            AddressablesHelper.instance.Release(handle);
        }
    }
}