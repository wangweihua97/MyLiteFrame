using System;
using System.Collections.Generic;
using System.Reflection;
using Script.Excel.Conver;
using Script.Excel.Table;
using Script.Excel.Table.Base;
using Script.Main.Base;
using Script.Mgr;
using UnityEngine;

namespace Script.Excel
{
    public class BaseExcelMgr  : BaseGameFlow
    {
        public static ConverHelper ConverHelper;
        protected override void DoEnable()
        {
            base.DoEnable();
            ConverHelper = new ConverHelper();
        }
    }
}