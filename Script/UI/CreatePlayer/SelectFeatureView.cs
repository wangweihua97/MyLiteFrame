using System.Collections;
using System.Collections.Generic;
using EasingCore;
using Events;
using FancyScrollView;
using Player;
using Script.Main;
using Script.Mgr;
using UI.Base;
using UI.CreatePlayer.vo;
using UnityEngine;
/**选择脸型*/
public class SelectFeatureView : InOutUView
{
    [Header("列表组件")]
    public List<C_item> list;
    public override bool DefaultShow => false;
    
    private int oldIdx = 0;
    private int listIdx = 0;
    private int listLen = 0;
    private List<ItemData> dataList;
    
    public override void DoCreat()
    {
        base.DoCreat();
        CreatePlayerViewMgr.SelectFeatureView = this;
        initListData();
    }

    void initListData()
    {
        dataList = new List<ItemData>();
        DressVo vo;
        for (int i = 14001; i < 14006; i++)
        {
            vo = new DressVo();
            vo.cfg = ExcelMgr.TDCharacter.Get(i + "");
            dataList.Add(vo);
        }
        //
        listIdx = 0;
        listLen = dataList.Count;
        // list.UpdateContents(dataList);
        updateListSelect();
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
                    listIdx -= listLen;
                }
                updateListSelect();
                break;
            case KeyCode.J:
                EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
                playInOutAni(false, delegate()
                {
                    DoClose();
                    EventCenter.ins.EventTrigger("create_player_step3");
                });
                break;
            case KeyCode.K:
                EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
                playInOutAni(false, delegate()
                {
                    DoClose();
                    EventCenter.ins.EventTrigger("create_player_step1");
                });
                break;
        }
    }
}
