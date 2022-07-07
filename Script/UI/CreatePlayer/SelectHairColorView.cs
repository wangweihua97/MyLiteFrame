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
public class SelectHairColorView : InOutUView
{
    [Header("颜色列表")]
    public List<Color_item> color_list;
    public override bool DefaultShow => false;

    private int oldColorIdx = 0;
    private int ColorListIdx = 0;
    private int ColorListLen = 0;
    private List<ItemData> dataColorList;

    public override void DoCreat()
    {
        base.DoCreat();
        CreatePlayerViewMgr.SelectHairColorView = this;
        // 
        initListData();
    }
    void initListData()
    {
        dataColorList = new List<ItemData>();
        DressVo vo;
        //发色
        for (int i = 12001; i < 12008; i++)
        {
            vo = new DressVo();
            vo.cfg = ExcelMgr.TDCharacter.Get(i + "");
            dataColorList.Add(vo);
        }
        updateColorListSelect();
        //颜色list
        ColorListIdx = -1;
        ColorListLen = dataColorList.Count;
        //
    }

    void updateColorListSelect()
    {
        if(oldColorIdx>=0) color_list[oldColorIdx].SetSelect(false);
        if(ColorListIdx>=0) {
            color_list[ColorListIdx].SetSelect(true);
            DressVo vo = (DressVo)dataColorList[ColorListIdx];
            PlayerMgr.instance.PlayerClothingMgr.ChangeDressUp(vo.cfg);
        }
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
                    ColorListIdx += ColorListIdx;
                }
                updateColorListSelect();
                break;
            case KeyCode.S:
                break;
            case KeyCode.D:
                ColorListIdx++;
                if (ColorListIdx>=ColorListLen)
                {
                    ColorListIdx = 0;
                }
                updateColorListSelect();
                break;
            case KeyCode.J:
                EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
                playInOutAni(false, delegate()
                {
                    DoClose();
                    EventCenter.ins.EventTrigger("create_player_step5");
                });
                break;
            case KeyCode.K:
                EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
                playInOutAni(false, delegate()
                {
                    DoClose();
                    EventCenter.ins.EventTrigger("create_player_step3");
                });
                break;
        }
        
    }
}
