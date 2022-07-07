using System.Collections.Generic;
using FancyScrollView;
using UIModel.TrainMode;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TrainMode
{
    public class ScrollViewItem : FancyScrollRectCell<TrainScrollItemData, TrainModeScrollContext>
    {
        public Text DescribeText;
        public GameObject UnLockGO;
        public GameObject LockGO;
        public GameObject UnLockSelectedImg;
        public GameObject UnLockNoSelectedImg;
        public GameObject LockedSelectedImg;
        public GameObject LockedNoSelectedImg;
        public List<StarItem> StarItems;
        private TrainScrollItemData _data;
        private int _index = -1;
        private bool _isBeSelect = false;
        

        public bool IsLocked
        {
            get { return _data.IsLocked; }
        }
        
        public override void Initialize()
        {
            ;
        }

        public void Log()
        {
            Debug.Log("---------------------");
        }

        public override void UpdateContent(TrainScrollItemData itemData)
        {
            if (_index != Index)
            {
                _data = itemData;
                InitItem();
            }
            _index = Index;
            if (Context.SelectedIndex == _index && _data.Degree == TrainModeSelectComponent.degreeIndex)
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

            /*var wave = Mathf.Sin(normalizedPosition * Mathf.PI * 2) * 65;
            transform.localPosition += Vector3.right * wave;*/
        }

        void InitItem()
        {
            DescribeText.text = _data.Describe;
            if (_data.IsLocked)
            {
                LockGO.SetActive(true);
                UnLockGO.SetActive(false);
                DescribeText.color = new Color(1,1,1,0.5f);
            }
            else
            {
                LockGO.SetActive(false);
                UnLockGO.SetActive(true);
                DescribeText.color = new Color(0.113f , 0.277f ,0.39f ,1);
                InitStarNum();
            }
        }

        void InitStarNum()
        {
            for (int i = 0; i < StarItems.Count; i++)
            {
                if (i < _data.GetStarNumber)
                    StarItems[i].IsLocked = false;
                else
                    StarItems[i].IsLocked = true;
            }
        }
    }
}