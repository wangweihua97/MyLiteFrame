using System;
using System.Linq;
using System.Reflection;
using Script.DB.DBBase;
using SQLite4Unity3d;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Script.DB
{
    public static class DBManager
    {
        public static BaseDBManager Instance
        {
            get
            {
                if(_baseDbManager == null)
                    _baseDbManager = new BaseDBManager();
                return _baseDbManager;
            }
        }
        
        private static BaseDBManager _baseDbManager;
  
        public static void CheckTable()
        {
            if(_baseDbManager == null)
                _baseDbManager = new BaseDBManager();
            var dbTableTypes = GetDBTableAttributedClass();
            foreach (var dbTableType in dbTableTypes)
            {
                if (!Instance.Exists(dbTableType))
                {
                    Instance.CreateTable(dbTableType);
                    var attribute = dbTableType.GetCustomAttribute<DBTableAttribute>();
                    if (attribute.DefaultAdd)
                    {
                        var defaultValue = Activator.CreateInstance(dbTableType);
                        Instance.Insert( defaultValue,true);
                    }
                }
            }
        }

        public static void DeleteAllTable()
        {
            var dbTableTypes = GetDBTableAttributedClass();
            foreach (var dbTableType in dbTableTypes)
            {
                Instance.DropTable(dbTableType);
            }

            Debug.Log("已经删除所有表格");
        }

        public static Type[] GetDBTableAttributedClass()
        {
            Assembly asm = Assembly.GetAssembly(typeof(DBTableAttribute));
            Type[] types = asm.GetExportedTypes();
 
            Func<Attribute[], bool> IsMyAttribute = o =>
            {
                foreach(Attribute a in o)
                {
                    if (a is DBTableAttribute)
                        return true;
                }
                return false;
            };
 
            Type[] dbTableTypes = types.Where(o =>
                {
                    return IsMyAttribute(Attribute.GetCustomAttributes(o,true));
                }
            ).ToArray();

            return dbTableTypes;
        }
    }
        
}