using System.Collections;
using System.Collections.Generic;
using EasingCore;
using Events;
using FancyScrollView;
using Player;
using Script.Main;
using Script.Mgr;
using Tool.Others;
using UI.Base;
using UI.CreatePlayer.vo;
using UnityEngine;

public class SelectConfirmView : InOutUView
{
    // [Header("颜色列表")]
    // public List<Color_item> color_list;
    
    public override bool DefaultShow => false;
    
    public override void DoCreat()
    {
        base.DoCreat();
        CreatePlayerViewMgr.SelectConfirmView = this;
        initListData();
    }
    void initListData()
    {
        
        //当前
    }
    
    // 
    public override void DoOpen()
    {
        base.DoOpen();
        // EventCenter.ins.AddEventListener<KeyCode>("KeyDown",KeyDown);
    }
    protected override void enterStageComplete()
    {
        EventCenter.ins.AddEventListener<KeyCode>("KeyDown",KeyDown);
    }
    public override void DoClose()
    {
        EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
        base.DoClose();
    }
    void KeyDown(KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.W:
                break;
            case KeyCode.A:
                break;
            case KeyCode.S:
                break;
            case KeyCode.D:
                break;
            case KeyCode.J:
                EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
                playInOutAni(false, delegate()
                {
                    DoClose();
                    EventCenter.ins.EventTrigger("create_player_step7");
                });
                break;
            case KeyCode.K:
                EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
                playInOutAni(false, delegate()
                {
                    DoClose();
                    EventCenter.ins.EventTrigger("create_player_step5");
                });
                break;
        }
    }
}
