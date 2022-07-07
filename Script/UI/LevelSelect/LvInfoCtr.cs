using System.Collections;
using System.Collections.Generic;
using OtherMgr;
using Script.Model;
using UI.CreatePlayer.vo;
using UnityEngine;
using UnityEngine.UI;

public class LvInfoCtr : MonoBehaviour
{
    public Image Icon;
    public Text LvName;
    public Text LvDes;
    public GameObject mask;
    public GameObject luck;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetData(LvItemVo vo)
    {
        if (-1 != vo.type)
        {
            LvName.text = vo.cfg.name;
            LvDes.text = vo.cfg.desc;
            Icon.sprite = SpritesMgr.Get(PlayerBag.LvIconUrl, vo.cfg.icon);
            mask.SetActive(2==vo.type);
            luck.SetActive(2==vo.type);
        }
        
    }
}
