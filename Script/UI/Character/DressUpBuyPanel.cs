using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using OtherMgr;
using Script.Excel.Table;
using Script.Main;
using Script.Model;
using UnityEngine;
using UnityEngine.UI;

public class DressUpBuyPanel : MonoBehaviour
{
    public Image icon;
    public Text txt;

    private TDCharacter cfg;

    private bool _isShow=false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setData(TDCharacter config)
    {
        cfg = config;
        updateIcon();
        updateTxt();
    }

    void updateIcon()
    {
        if (!String.IsNullOrEmpty(cfg.icon))
        {
            icon.gameObject.SetActive(true);
            icon.sprite = SpritesMgr.Get(PlayerBag.CharacterIconUrl, cfg.icon);
            icon.SetNativeSize();
        }
        else
        {
            icon.gameObject.SetActive(false);
        }
    }

    void updateTxt()
    {
        txt.text = "是否确认使用<color=\"#fdb659\">"+cfg.price+"</color>金币购买"+cfg.name;

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
                AudioManager.PlayAudioEffectA("选中确认");
                PlayerBag.BuyCharacter(cfg.Id);
                CommonUIMgr.PopupFrame.ShowTips("  购买成功  ");
                EventCenter.ins.EventTrigger("DressUp_buy_item", cfg.Id);
                hide();
                break;
            case KeyCode.H:
                break;
            case KeyCode.K:
                AudioManager.PlayAudioEffectA("返回");
                Invoke("hide", 0);
                /*WaitTimeMgr.WaitTime(0.3f, () =>
                {
                    ScrollViewComponents[oldValue].ScrollTo(0);
                    Refresh();
                });*/
                // hide();
                break;
        }
        
    }

    public void show(TDCharacter cfg)
    {
        AudioManager.PlayAudioEffectA("打开弹窗");
        _isShow = true;
        gameObject.SetActive(true);
        EventCenter.ins.AddEventListener<KeyCode>("KeyDown",KeyDown);
        setData(cfg);
    }

    public void hide()
    {
        _isShow = false;
        gameObject.SetActive(false);
        EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
    }

    public bool IsShow()
    {
        return _isShow;
    }
}
