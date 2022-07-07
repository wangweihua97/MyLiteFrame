using System;
using System.Collections;
using System.Collections.Generic;
using FancyScrollView;
using OtherMgr;
using Script.Excel.Table;
using Script.Model;
using UI.CreatePlayer.vo;
using UnityEngine;
using UnityEngine.UI;

public class C_item : FancyGridViewCell<ItemData, Context>
{
    public Image bg;
    public Image icon;
    public GameObject select;

    private DressVo _vo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Initialize()
    {
        select.SetActive(false);
        icon.gameObject.SetActive(false);
    }
    public override void UpdateContent(ItemData vo)
    {
        _vo = (DressVo) vo;
        SetData(_vo.cfg);
        SetSelect(Context.SelectedIndex == Index);
    }

    public void SetData(TDCharacter cfg)
    {
        string url = cfg.icon;
        if (!String.IsNullOrEmpty(url))
        {
            icon.gameObject.SetActive(true);
            updateImg(url);
        }
        else
        {
            icon.gameObject.SetActive(false);
        }
    }

    void updateImg(string url)
    {
        icon.sprite = SpritesMgr.Get(PlayerBag.CharacterIconUrl, url);
    }

    public void SetSelect(bool b)
    {
        select.SetActive(b);
    }
}
