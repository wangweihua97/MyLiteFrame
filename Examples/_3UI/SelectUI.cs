using System.Collections;
using System.Collections.Generic;
using Examples.UI;
using Script.Mgr;
using UnityEngine;

public class SelectUI : MonoBehaviour
{
    public void OpenA1()
    {
        AUIMgr.A1View.DoOpen();
    }
    
    public void OpenA2()
    {
        AUIMgr.A2View.DoOpen();
    }
    
    public void OpenA3()
    {
        AUIMgr.A3View.DoOpen();
    }
    
    public void OpenB1()
    {
        BUIMgr.B1View.DoOpen();
    }
    
    public void OpenB2()
    {
        BUIMgr.B2View.DoOpen();
    }
    
    public void OpenB3()
    {
        BUIMgr.B3View.DoOpen();
    }
    
    public void CloseTopUI()
    {
        var uuiMgr = RootUIMgr.instance.GetTop();
        if (uuiMgr == null)
        {
            Debug.Log("没有显示的UI");
            return;
        }

        var uiView = uuiMgr.GetTop();
        uiView.DoClose();
    }
}
