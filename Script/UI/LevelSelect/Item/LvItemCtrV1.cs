using System;
using System.Collections;
using System.Collections.Generic;
using FancyScrollView;
using UI.Base;
using UI.CreatePlayer.vo;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LevelSelect.Item
{
    public class LvItemCtrV1 : FancyScrollRectCell<ItemData, Context>
    {
        public Image bg;
        public Image bg1;
        public Image select;
        public Image select1;
        public Text txt;

        private LvItemVo _vo;

        private Color[] txtColorList =
        {
            new Color(27/255f , 61/255f ,103/255f ,1f)
            ,new Color(6/255f , 32/255f ,65/255f ,240/255f)
            ,new Color(6/255f , 32/255f ,65/255f ,220/255f)
            ,new Color(6/255f , 32/255f ,65/255f ,200/255f)
            ,new Color(6/255f , 32/255f ,65/255f ,180/255f)
        };

        public void SetSelect(bool b)
        {
            select.gameObject.SetActive(b);
            select1.gameObject.SetActive(b);
        }
        
        public override void Initialize()
        {
            select.gameObject.SetActive(false);
            select1.gameObject.SetActive(false);
            // bg1.SetActive(true);
        }
        public override void UpdateContent(ItemData vo)
        {
            _vo = (LvItemVo) vo;
            SetAct(-1 != _vo.type);
            if (-1 != _vo.type)
            {
                SetSelect(Context.SelectedIndex == Index);
                //
                /*bg.gameObject.SetActive(2 == _vo.type);
                bg1.gameObject.SetActive(0 == _vo.type);*/
                if (1 == _vo.type)
                {
                    bg.gameObject.SetActive(false);
                    bg1.gameObject.SetActive(true);
                }
                else
                {
                    bg.gameObject.SetActive(true);
                    bg1.gameObject.SetActive(false);
                }
                txt.text = Index - 3 + "";
                if (-1 != Context.SelectedIndex)
                {
                    int offsetIdx=0;
                    if (Context.SelectedIndex == Index)
                    {
                        offsetIdx = Context.SelectedIndex - Index;
                        SetAlpha(1);
                    }
                    else if (Context.SelectedIndex > Index)
                    {
                        offsetIdx = Context.SelectedIndex - Index;
                        SetAlpha(1 - offsetIdx * 0.2f);
                    }
                    else
                    {
                        offsetIdx = Index - Context.SelectedIndex;
                        SetAlpha(1 - offsetIdx * 0.2f);
                    }
                    //
                    SetTxtColor(txtColorList[Mathf.Min(txtColorList.Length-1, Mathf.Max(offsetIdx,0))]);
                }
            }
            
            
        }
        
        protected override void UpdatePosition(float normalizedPosition, float localPosition)
        {
            base.UpdatePosition(normalizedPosition, localPosition);
        }

        void SetAct(bool b)
        {
            bg.gameObject.SetActive(b);
            bg1.gameObject.SetActive(b);
            select.gameObject.SetActive(b);
            select1.gameObject.SetActive(b);
            txt.gameObject.SetActive(b);
        }

        /**0-1alpha*/
        void SetAlpha(float alpha)
        {
            bg.color = new Color(1f,1f,1f, alpha);
            bg1.color = new Color(1f,1f,1f, alpha);
            select.color = new Color(1f,1f,1f, alpha);
            select1.color = new Color(1f,1f,1f, alpha);
            // txt.color = new Color(61/255f,89/255f,96/255f, alpha);
        }

        void SetTxtColor(Color cl)
        {
            txt.color = cl;
        }
    }
}
