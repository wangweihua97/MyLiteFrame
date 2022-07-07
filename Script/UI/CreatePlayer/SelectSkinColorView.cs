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

public class SelectSkinColorView : InOutUView
{
    [Header("颜色列表")]
    public List<Color_item> color_list;
    
    private int oldColorIdx = 0;
    private int ColorListIdx = 0;
    private int ColorListLen = 0;
    private List<ItemData> dataColorList;
    public override bool DefaultShow => false;
    
    public override void DoCreat()
    {
        base.DoCreat();
        CreatePlayerViewMgr.SelectSkinColorView = this;
        initListData();
    }
    void initListData()
    {
        dataColorList = new List<ItemData>();
        DressVo vo;
        //肤色
        for (int i = 11001; i < 11006; i++)
        {
            vo = new DressVo();
            vo.cfg = ExcelMgr.TDCharacter.Get(i + "");
            dataColorList.Add(vo);
        }
        //
        ColorListIdx = 0;
        ColorListLen = dataColorList.Count;
        updateListSelect();
        //当前
    }
    void updateListSelect()
    {
        color_list[oldColorIdx].SetSelect(false);
        color_list[ColorListIdx].SetSelect(true);
        DressVo vo = (DressVo)dataColorList[ColorListIdx];
        PlayerMgr.instance.PlayerClothingMgr.ChangeDressUp(vo.cfg);
        oldColorIdx = ColorListIdx;
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
                ColorListIdx--;
                if (ColorListIdx<0)
                {
                    ColorListIdx += ColorListLen;
                }
                updateListSelect();
                break;
            case KeyCode.S:
                break;
            case KeyCode.D:
                ColorListIdx++;
                if (ColorListIdx>=ColorListLen)
                {
                    ColorListIdx = 0;
                }
                updateListSelect();
                break;
            case KeyCode.J:
                EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
                playInOutAni(false, delegate()
                {
                    DoClose();
                    EventCenter.ins.EventTrigger("create_player_step2");
                });
                break;
            case KeyCode.K:
                EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
                playInOutAni(false, delegate()
                {
                    DoClose();
                    EventCenter.ins.EventTrigger("create_player_step0");
                });
                break;
        }
    }
}
