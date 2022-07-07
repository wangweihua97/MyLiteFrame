using System;
using OtherMgr;
using Script.Model;
using UI.Common.VO;
using UnityEngine;
using UnityEngine.UI;

namespace UI.SettlementView.item
{
    public class Item : MonoBehaviour
    {
        public Image icon;
        public Text name;
        public Text count;

        public void SetData(ItemVo vo)
        {
            string url = vo.cfg.icon;
            if (!String.IsNullOrEmpty(url))
            {
                icon.gameObject.SetActive(true);
                updateImg(url);
            }
            else
            {
                icon.gameObject.SetActive(false);
            }
            name.text = vo.cfg.name;
            count.text = "+"+ vo.count;
        }
        
        void updateImg(string url)
        {
            icon.sprite = SpritesMgr.Get(PlayerBag.CharacterIconUrl, url);
        }
    }
    
}