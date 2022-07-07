using System.Collections.Generic;
using FancyScrollView;
using UIModel.TrainMode;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TrainMode
{
    public class MusicSelectScrollItem : FancyScrollRectCell<MusicItemData, TrainModeScrollContext>
    {
        public Text DescribeText;
        public GameObject UnLockGO;
        public GameObject LockGO;
        public GameObject UnLockSelectedImg;
        public GameObject UnLockNoSelectedImg;
        public GameObject LockedSelectedImg;
        public GameObject LockedNoSelectedImg;
        public Text Time;
        public Text PlayCount;
        private MusicItemData _data;
        private int _index = -1;
        private bool _isBeSelect = false;
        

        public bool IsLocked
        {
            get { return _data.IsLocked; }
        }
        
        public override void Initialize()
        {
            Time.gameObject.SetActive(false);
        }

        public override void UpdateContent(MusicItemData itemData)
        {
            if (_index != Index)
            {
                _data = itemData;
                InitItem();
            }
            _index = Index;
            if (Context.SelectedIndex == _index)
                BeSelected();
            else
                BeLeft();
        }

        public void BeSelected()
        {
            if(_isBeSelect)
                return;
            _isBeSelect = true;
            if (IsLocked)
            {
                LockedSelectedImg.SetActive(true);
                LockedNoSelectedImg.SetActive(false);
            }
            else
            {
                UnLockSelectedImg.SetActive(true);
                UnLockNoSelectedImg.SetActive(false);
            }
        }
        
        public void BeLeft()
        {
            if(!_isBeSelect)
                return;
            _isBeSelect = false;
            if (IsLocked)
            {
                LockedSelectedImg.SetActive(false);
                LockedNoSelectedImg.SetActive(true);
            }
            else
            {
                UnLockSelectedImg.SetActive(false);
                UnLockNoSelectedImg.SetActive(true);
            }
        }

        protected override void UpdatePosition(float normalizedPosition, float localPosition)
        {
            base.UpdatePosition(normalizedPosition, localPosition);
        }

        void InitItem()
        {
            DescribeText.text = _data.Name;
            if (_data.IsLocked)
            {
                LockGO.SetActive(true);
                UnLockGO.SetActive(false);
                PlayCount.gameObject.SetActive(false);
                Color white = new Color(1,1,1,0.5f);
                DescribeText.color = white;
                Time.color = white;
                SetChildrenTextColor(Time.gameObject, white);
            }
            else
            {
                LockGO.SetActive(false);
                UnLockGO.SetActive(true);
                // PlayCount.gameObject.SetActive(false);
                PlayCount.gameObject.SetActive(true);
                Color color = new Color(0.113f, 0.277f, 0.39f, 1);
                DescribeText.color = color;
                Time.color = color;
                SetChildrenTextColor(Time.gameObject, color);
            }

            if (_data.IsRandom)
            {
                Time.text = "??";
            }
            else
            {
                Time.text = _data.Time.ToString();
            }

            InitPlayCount();
        }

        void InitPlayCount()
        {
            if(_data.IsLocked)
                return;
            if (_data.IsRandom)
            {
                PlayCount.text = "******";
            }
            else
            {
                PlayCount.text = _data.PlayCount.ToString();
            }
        }

        void SetChildrenTextColor(GameObject go ,Color color)
        {
            foreach (var text in go.GetComponentsInChildren<Text>())
            {
                text.color = color;
            }
        }
    }
}