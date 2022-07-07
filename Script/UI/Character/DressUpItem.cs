using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using FancyScrollView;
using OtherMgr;
using Script.Main;
using Script.Model;
using UI.Character.vo;
using UnityEngine;
using UnityEngine.UI;

public class DressUpItem : FancyGridViewCell<ItemData, Context>
{
    // [Header("普通背景")]
    public Image iconBg;
    public Image icon;
    public GameObject select;
    public GameObject newFlag;
    public GameObject eFlag;
    public GameObject lockFlag;
    public Animator equipAni;
    public GameObject equipAniP1;
    public GameObject equipAniP2;

    private DressUpVo _vo;
    private Color alpha50 = new Color(1f,1f,1f,0.5f);
    private Color alpha100 = new Color(1f,1f,1f,1f);
    /**未拥有,拥有, 选择*/
    private Color[] bgCl =
    {
        new Color(158/255f,204/255f,1f,68/255f)
        ,new Color(223/255f,242/255f,1f,181/255f)
        ,new Color(223/255f,242/255f,1f,255/255f)
    };
    private bool isSelect;
    public override void UpdateContent(ItemData vo)
    {
        _vo = (DressUpVo)vo;
        // message.text = itemData.Index.ToString();

        if (_vo.lockFlag)
        {
            icon.color = alpha50;
            iconBg.color = bgCl[0];
        }
        else
        {
            icon.color = alpha100;
            iconBg.color = Context.SelectedIndex == Index?alpha100:bgCl[1];
        }
        
        select.SetActive(Context.SelectedIndex == Index);
        
        if(Context.SelectedIndex == Index)EventCenter.ins.EventTrigger("DressUp_select_item", _vo);
        
        eFlag.SetActive(_vo.eFlag);
        newFlag.SetActive(_vo.newFlag);
        
        if (!String.IsNullOrEmpty(_vo.cfg.icon))
        {
            icon.gameObject.SetActive(true);
            icon.sprite = SpritesMgr.Get(PlayerBag.CharacterIconUrl, _vo.cfg.icon);
        }
        else
        {
            icon.gameObject.SetActive(false);
        }
        // image.color = selected
        //     ? new Color32(0, 255, 255, 100)
        //     : new Color32(255, 255, 255, 77);
        //
        if (_vo.playEquipAniOnce)
        {
            _vo.playEquipAniOnce = false;
            playEquipAni();
        }
    }
    
    public override void Initialize()
    {
        // icon.gameObject.SetActive(false);
        select.SetActive(false);
        newFlag.SetActive(false);
        eFlag.SetActive(false);
        lockFlag.SetActive(false);
        equipAni.enabled = false;
        equipAni.speed = 0;
        equipAniP1.SetActive(false);
        equipAniP2.SetActive(false);
    }

    public void playEquipAni()
    {
        equipAniP1.SetActive(true);
        equipAniP2.SetActive(true);
        equipAni.enabled = true;
        equipAni.Rebind();
        equipAni.speed = 1;
        equipAni.Play("DressUpItem_1");
        Global.instance.StartCoroutine(delayFun(1f));
    }
    
    private IEnumerator delayFun(float time)
    {
        yield return new WaitForSeconds(time);
        equipAni.enabled = false;
        equipAni.speed = 0;
        equipAniP1.SetActive(false);
        equipAniP2.SetActive(false);
    }
    
    
}
