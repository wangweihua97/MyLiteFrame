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

/**选择发型*/
public class SelectHairView : InOutUView
{
    [Header("头像列表")]
    public List<C_item> list;
    public override bool DefaultShow => false;
    
    private int oldIdx = 0;
    private int listIdx = 0;
    private int listLen = 0;
    private List<ItemData> dataList;

    public override void DoCreat()
    {
        base.DoCreat();
        CreatePlayerViewMgr.SelectHairView = this;
        // 
        initListData();
    }
    void initListData()
    {
        dataList = new List<ItemData>();
        DressVo vo;
        //发型
        for (int i = 15001; i < 15006; i++)
        {
            vo = new DressVo();
            vo.cfg = ExcelMgr.TDCharacter.Get(i + "");
            dataList.Add(vo);
        }
        //
        listIdx = 0;
        listLen = dataList.Count;
        updateListSelect();
        //
    }
    
    void updateListSelect()
    {
        list[oldIdx].SetSelect(false);
        list[listIdx].SetSelect(true);
        DressVo vo = (DressVo)dataList[listIdx];
        PlayerMgr.instance.PlayerClothingMgr.ChangeDressUp(vo.cfg);
        oldIdx = listIdx;
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
                listIdx--;
                if (listIdx<0)
                {
                    listIdx += listLen;
                }
                updateListSelect();
                break;
            case KeyCode.S:
                break;
            case KeyCode.D:
                listIdx++;
                if (listIdx>=listLen)
                {
                    listIdx = 0;
                }
                updateListSelect();
                break;
            case KeyCode.J:
                EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
                playInOutAni(false, delegate()
                {
                    DoClose();
                    EventCenter.ins.EventTrigger("create_player_step4");
                });
                break;
            case KeyCode.K:
                EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
                playInOutAni(false, delegate()
                {
                    DoClose();
                    EventCenter.ins.EventTrigger("create_player_step2");
                });
                
                break;
        }
        
    }
}
