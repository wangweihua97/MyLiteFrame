using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using Script.Main.Base;
using Script.Mgr;
using Tool.Others;
using UI.Base;
using UnityEngine;

public class UUIMgr : BaseUIMgr
{
    public int RootIndex;
    protected int ShowCount;
    private const int UiMgrMaxViewNum = 50;

    private DoubleLinkedList<UView> _doubleLinkedList;
    // Start is called before the first frame update
    protected void Add<TUI>(string ui,string label,Action callBack ,Action failCallBack = null) where TUI : UView
    {
        base.Add<TUI>(ui ,label ,callBack ,failCallBack);
    }

    #region lua测试

    public void AddLua(string ui,string label,Action callBack ,Action failCallBack = null)
    {
        base.AddLua(ui ,label ,callBack ,failCallBack);
    }

    #endregion
    
    
    public override void DoCreat()
    {
        base.DoCreat();
        _doubleLinkedList = new DoubleLinkedList<UView>();
    }
    
    public override void CreatUI<TUI>(TUI UI  ,string UIName)
    {
        base.CreatUI(UI ,UIName);
        _doubleLinkedList.PushBack(UI);
        Canvas canvas = UI.gameObject.GetComponent<Canvas>();
        canvas.sortingOrder = GetOrder(_doubleLinkedList.Size - 1);
        UI.Parent = this;
        UI.DoCreat();
        if (UI.DefaultShow)
        {
            UI.DoOpen();
            ShowCount++;
        }
        else
        {
            UI.Show(false);
        }
    }

    public void AddShowCount()
    {
        ShowCount++;
    }
    
    public void DecreaseShowCount()
    {
        ShowCount--;
    }


    public int GetUIOrder(UView ui)
    {
        return RootIndex * 50 + _doubleLinkedList.Find(ui);
    }
    
    public int GetIndex(UView ui)
    {
        return _doubleLinkedList.Find(ui);
    }

    public void Top(UView ui)
    {
        RootUIMgr.instance.Top(GetType());
        int index = GetIndex(ui);
        _doubleLinkedList.MoveToBack(index);
        ui.SetOrder(GetOrder(_doubleLinkedList.Size - 1));
        Refresh();
    }
    
    public void Head(UView ui)
    {
        _doubleLinkedList.MoveToHead(_doubleLinkedList.Find(ui));
        if(ShowCount == 0)
            RootUIMgr.instance.Head(this.GetType());
    }
    
    public void End(UView ui)
    {
        _doubleLinkedList.MoveToBack(_doubleLinkedList.Find(ui));
    }

    int GetOrder(int index)
    {
        return RootIndex * UiMgrMaxViewNum + index;
    }

    public void Refresh()
    {
        int i = 0;
        foreach (var uView in _doubleLinkedList)
        {
            if (uView.IsActive())
            {
                uView.SetOrder(GetOrder(i));
            }
            i++;
        }
    }

    public bool IsTop()
    {
        return RootUIMgr.instance.IsTop(GetType());
    }
    
    public bool UIIsTop(UView ui)
    {
        return _doubleLinkedList.IsEnd(ui);
    }

    public UView GetTop()
    {
        return _doubleLinkedList.End.Value;
    }

    public bool IsLayerTop()
    {
        return RootUIMgr.instance.IsLayerTop(GetType() ,sortingLayerName);
    }

    public override void DoDestroy()
    {
        base.DoDestroy();
        _doubleLinkedList = null;
    }
}
