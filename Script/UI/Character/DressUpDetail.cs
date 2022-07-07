using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Script.Excel.Table;
using Script.Main;
using Script.Mgr;
using Script.Model;
using UI.Character.vo;
using UnityEngine;
using UnityEngine.UI;

public class DressUpDetail : MonoBehaviour
{
    public Text item_name;
    public Text attr;
    public Text old_act;
    public Text new_act;
    public Text content;
    public Text suit_content;
    public Text flag_txt;
    public Text price_txt;
    public Image money_icon;
    public Image power_change;
    public Image btn_a;

    private DressUpVo _vo;

    private string txt1 = "卸载";
    private string txt2 = "穿着";
    private string txt3 = "购买";
    private string txt4 = "已拥有";
    private string clWhite = "#FFFFFF";
    private string clRed = "#FF6E6E";
    private Color blue = new Color(79/255f,236/255f,230/255f,1f);
    private Color white = new Color(1f,1f,1f,1f);
    private Color green = new Color(89/255f,253/255f,182/255f,1f);
    private Color red = new Color(1f,110/255f,110/255f,1f);
    private Vector3 v3 = new Vector3(0, -108,0);
    // private string[] attrTxt = {"攻击力:","防御力:"};
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setData(DressUpVo vo, int tipsID)
    {
        _vo = vo;
        if (null==_vo)
        {
            setType(false);
            //
            TDUITpis cfg = ExcelMgr.TDUITpis.Get(tipsID.ToString());
            item_name.text = cfg.name;//名字
            content.text = cfg.content.Replace("\\n", "\n");//说明
        }
        else
        {
            setType(true);
            //
            item_name.text = _vo.cfg.name;//名字
            content.text = _vo.cfg.des;//说明
            //套装说明
            if (Const.NULL_INT != _vo.cfg.group)
            {
                suit_content.text = ExcelMgr.TDSuit.Get(_vo.cfg.group.ToString()).description.Replace("\\n", "\n");
            }
            else
            {
                suit_content.text = "";
            }
            //
            if (_vo.eFlag)//已装备
            {
                flag_txt.text = txt1;//卸载
                price_txt.text = txt4;//已拥有
                price_txt.color = blue;//蓝色
            }
            else if (_vo.lockFlag)
            {
                flag_txt.text = txt3;//购买
                price_txt.color = white;//红色(200)/900
                string cl = PlayerBag.ContainsMoney(_vo.cfg.price) ? clWhite : clRed;
                price_txt.text = "<color=\""+cl+"\">"+_vo.cfg.price+"</color>/"+PlayerBag.PlayerBagModel.gold;
            }
            else
            {
                flag_txt.text = txt2;//穿着
                price_txt.text = txt4;//已拥有
                price_txt.color = blue;//蓝色
            }
            //
            float offsetX = PlayerBag.PlayerBagModel.gold.ToString().Length + _vo.cfg.price.ToString().Length + 1;
            offsetX *= 13;
            v3.x = 345 - offsetX;
            money_icon.gameObject.SetActive(_vo.lockFlag);
            money_icon.rectTransform.localPosition = v3;
            //属性,颜色
            attr.text = _vo.cfg.ptype + ":";
            string cId = PlayerMgr.instance.PlayerClothingMgr.GetCorrespondingId(_vo.cfg);
            int oldValue = ExcelMgr.TDCharacter.Get(cId).pnum;
            int newValue = _vo.cfg.pnum;
            // int newValue = 0;
            old_act.text = ""+oldValue;
            // old_act.text = ""+99;
            new_act.text = ""+newValue;
            if (oldValue>newValue)
            {
                new_act.color = red;
            }
            else if (oldValue<newValue)
            {
                new_act.color = green;
            }
            else
            {
                new_act.color = white;
            }
        }
        
    }

    private void setType(bool b)
    {
        attr.gameObject.SetActive(b);
        old_act.gameObject.SetActive(b);
        new_act.gameObject.SetActive(b);
        suit_content.gameObject.SetActive(b);
        flag_txt.gameObject.SetActive(b);
        price_txt.gameObject.SetActive(b);
        money_icon.gameObject.SetActive(b);
        power_change.gameObject.SetActive(b);
        btn_a.gameObject.SetActive(b);
    }
}
