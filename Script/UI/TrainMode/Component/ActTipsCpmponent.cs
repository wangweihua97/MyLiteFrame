using System;
using System.Collections;
using OtherMgr;
using Script.Excel.Table;
using Tool.Others;
using UI.Base;
using UIModel;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TrainMode
{
    public class ActTipsCpmponent : UComponent
    {
        [SerializeField] private Image _image;
        [SerializeField] private Text _name;
        [SerializeField] private Text _desc;
        [SerializeField] private RectTransform _arrow;

        private TDAction _data;
        private int arrow = 0;

        private int axleX = -300;
        private int axleY = -70;

        public void SetData(TDAction data)
        {
            this._data = data;
            Refrsh();
        }

        void Refrsh()
        {
            SpriteModel spriteModel = SpritesMgr.Get(_data.icon);
            if(spriteModel.Sprite == null)
                Debug.LogError(_data.icon);
            _image.sprite = spriteModel.Sprite;
            Vector3 scale = spriteModel.IsOverturn ? new Vector3(-1, 1, 1) : new Vector3(1, 1, 1);
            _image.rectTransform.localScale = scale;

            _name.text = _data.name;
            _desc.text = StringTool.Formatting(_data.description);
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(_desc.rectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_desc.transform.parent.transform as RectTransform);
            //
            
        }

        void updateArrow()
        {
            switch (arrow)
            {
                case 1:
                    //左上
                    _arrow.anchorMin = new Vector2(0, 1);
                    _arrow.anchorMax = new Vector2(0, 1);
                    _arrow.anchoredPosition = new Vector2(-15,70);
                    _arrow.eulerAngles = new Vector3(0,0,180);
                    break;
                case 2:
                    //右上
                    _arrow.anchorMin = new Vector2(1, 1);
                    _arrow.anchorMax = new Vector2(1, 1);
                    _arrow.anchoredPosition = new Vector2(5,70);
                    _arrow.eulerAngles = new Vector3(0,0,90);
                    break;
                case 3:
                    //左下
                    _arrow.anchorMin = new Vector2(0, 0);
                    _arrow.anchorMax = new Vector2(0, 0);
                    _arrow.anchoredPosition = new Vector2(-15,-15);
                    _arrow.eulerAngles = new Vector3(0,0,270);
                    break;
                case 4:
                    //右下
                    _arrow.anchorMin = new Vector2(1, 0);
                    _arrow.anchorMax = new Vector2(1, 0);
                    _arrow.anchoredPosition = new Vector2(5,-15);
                    _arrow.eulerAngles = new Vector3(0,0,0);
                    break;
            }
        }

        private Vector3 tmpV3=new Vector3(50,50,0);
        public void SetXY(ActItem ai)
        {
            RectTransform rt = (RectTransform) ai.transform;
            Vector3 wv3 = rt.TransformPoint(Vector3.zero);
            Vector3 lv3 = gameObject.transform.parent.transform.InverseTransformPoint(wv3);
            int tgX = (int) lv3.x;
            int tgY = (int) (lv3.y - 18);
            // int tgX = (int) rt.anchoredPosition.x + 70;
            // int tgY = (int) (rt.anchoredPosition.y - 18) - 70;
            RectTransform bg = (RectTransform) _desc.transform.parent.transform;
            int w = (int) (bg.sizeDelta.x * 0.5);
            int h = (int) ((bg.sizeDelta.y + 40) * 0.5);
            RectTransform lrt = (RectTransform) gameObject.transform;
            tmpV3.x = bg.sizeDelta.x;
            tmpV3.y = bg.sizeDelta.y + 40;
            lrt.sizeDelta = tmpV3;
            // gameObject.transform
            int offset = 50;
            arrow = 0;
            if (tgX<=axleX)
            {
                if (tgY<=axleY)
                {
                    arrow = 3;
                    // tmpV3.x = w + offset;
                    // tmpV3.y = h + offset;
                    // wv3 = rt.TransformPoint(tmpV3);
                    // lv3 = gameObject.transform.parent.transform.InverseTransformPoint(wv3);
                    // gameObject.transform.localPosition = lv3;
                    gameObject.transform.localPosition = new Vector3(tgX+w+offset, tgY+h+offset);
                }
                else
                {
                    arrow = 1;
                    // tmpV3.x = w + offset;
                    // tmpV3.y = -h - offset;
                    // wv3 = rt.TransformPoint(tmpV3);
                    // lv3 = gameObject.transform.parent.transform.InverseTransformPoint(wv3);
                    // gameObject.transform.localPosition = lv3;
                    gameObject.transform.localPosition = new Vector3(tgX+w+offset, tgY-h);
                }
            }
            else
            {
                if (tgY<=axleY)
                {
                    arrow = 4;
                    // tmpV3.x = -w - offset;
                    // tmpV3.y = h + offset;
                    // wv3 = rt.TransformPoint(tmpV3);
                    // lv3 = gameObject.transform.parent.transform.InverseTransformPoint(wv3);
                    // gameObject.transform.localPosition = lv3;
                    gameObject.transform.localPosition = new Vector3(tgX-w-offset, tgY+h+offset);
                }
                else
                {
                    arrow = 2;
                    // tmpV3.x = -w - offset;
                    // tmpV3.y = -h - offset;
                    // wv3 = rt.TransformPoint(tmpV3);
                    // lv3 = gameObject.transform.parent.transform.InverseTransformPoint(wv3);
                    // gameObject.transform.localPosition = lv3;
                    gameObject.transform.localPosition = new Vector3(tgX-w-offset, tgY-h);
                }
            }
            updateArrow();
        }

        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
        
    }
}