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

public class Color_item : FancyGridViewCell<ItemData, Context>
{
    public Image bg;
    public Image bg_border;
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
        bg.gameObject.SetActive(true);
    }
    public override void UpdateContent(ItemData vo)
    {
        _vo = (DressVo) vo;
        SetData(_vo.cl);
        SetSelect(Context.SelectedIndex == Index);
    }

    public void SetData(Color cl)
    {
        bg.color = cl;
    }

    public void SetSelect(bool b)
    {
        select.SetActive(b);
    }
}
