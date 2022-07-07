using System;
using System.Collections.Generic;
using System.Text;
using OtherMgr;
using Player;
using Script.Excel.Table;
using Script.Mgr;
using UI.Base;
using UnityEngine;

namespace UI.TrainMode
{
    public class ActGroupCpmponent : UComponent
    {
        [SerializeField] private int _rowNum;
        [SerializeField] private int _columnNum;
        [SerializeField] private GameObject _content;
        [Header("每行有多少数据")]
        [SerializeField] private List<int> _itemNumberInEveryRow;

        public float PlayInterval = 1.68f;
        private float time = 0;
        private TDAction[][] _map;
        private ActItem[] _actItems;
        private bool _bodyIsLeft = true;
        private ValueTuple<int ,int> _curIndex = new ValueTuple<int, int>(0, 0);
        private ValueTuple<int ,int> _temp;
        public override void DoCreat()
        {
            base.DoCreat();
            CreatActItemList();
        }

        public void SetPosture(bool isLeft)
        {
            _bodyIsLeft = isLeft;
            if(_bodyIsLeft)
                InitMap("左");
            else
                InitMap("右");
        }

        public override void DoOpen()
        {
            base.DoOpen();
            SelectIconLeave();
            _curIndex = new ValueTuple<int, int>(0, 0);
            Select();
        }

        private void Update()
        {
            time += Time.deltaTime;
            if (time >= PlayInterval)
            {
                time = 0;
                PlayerMgr.instance.playerAnimationMgr.PlayAnimation(_map[_curIndex.Item1][_curIndex.Item2]
                    .actAnimation);
            }
        }

        void InitMap(string body)
        {
            _map = new TDAction[_rowNum][];
            for (int i = 0; i < _rowNum; i++)
            {
                _map[i] = new TDAction[_columnNum];
            }
            _temp = new ValueTuple<int, int>(0, 0);
            foreach (var kvp in ExcelMgr.TDAction.GetDictionary())
            {
                if (kvp.Value.body.Equals(body) && kvp.Value.isInSpectrum)
                {
                    InsertData(kvp.Value);
                }
            }
            InitActItemList();
        }

        void InitActItemList()
        {
            for (int i = 0; i < _rowNum; i++)
            {
                for (int j = 0; j < _columnNum; j++)
                {
                    _actItems[i * _columnNum + j].SetData(_map[i][j]);
                }
            }
        }

        void CreatActItemList()
        {
            _actItems = new ActItem[100];
            foreach (var actItem in _content.GetComponentsInChildren<ActItem>())
            {
                StringBuilder sb = new StringBuilder();    //我们抓取当前字符当中的123
                foreach (char c in actItem.gameObject.name)
                {
                    if (Convert.ToInt32(c) >= 48 && Convert.ToInt32(c) <= 57)
                    {
                        sb.Append(c);
                    }
                }

                if (sb.Length <= 0)
                    _actItems[0] = actItem;
                else
                {
                    _actItems[Int32.Parse(sb.ToString())] = actItem;
                }
            }
        }

        void Select()
        {
            _actItems[_curIndex.Item1 * _columnNum + _curIndex.Item2].BeSelected();
            if (PlayerMgr.instance.playerAnimationMgr != null && _map != null)
            {
                PlayerMgr.instance.playerAnimationMgr.PlayAnimation(_map[_curIndex.Item1][_curIndex.Item2].actAnimation);
                time = 0;
            }
        }
        
        void SelectIconLeave()
        {
            _actItems[_curIndex.Item1 * _columnNum + _curIndex.Item2].BeLeft();
        }
        
        public void MoveUp()
        {
            if(_curIndex.Item1 <= 0 || _map[_curIndex.Item1 - 1][_curIndex.Item2] == null)
                return;
            ChangeSelectIndex(-1 ,0);
        }
        
        public void MoveLeft()
        {
            if(_curIndex.Item2 <= 0 || _map[_curIndex.Item1][_curIndex.Item2 - 1] == null)
                return;
            ChangeSelectIndex(0, -1);
        }
        
        public void MoveDown()
        {
            if (_curIndex.Item1 + 1 == 2)
            {
                if (0==_curIndex.Item2)
                {
                    // _curIndex.Item2 = 1;
                    ChangeSelectIndex(1, 1);
                    return;
                } 
                else if (6==_curIndex.Item2)
                {
                    // _curIndex.Item2 = 5;
                    ChangeSelectIndex(1, -1);
                    return;
                }
            }
            if (_curIndex.Item1 + 1 == 3)
            {
                if (0==_curIndex.Item2 || 1==_curIndex.Item2)
                {
                    // _curIndex.Item2 = 2;
                    ChangeSelectIndex(1, 1);
                    return;
                }
                else if (6==_curIndex.Item2 || 5==_curIndex.Item2)
                {
                    // _curIndex.Item2 = 4;
                    ChangeSelectIndex(1, -1);
                    return;
                }
            }
            if(_curIndex.Item1 + 1 >= _rowNum || _map[_curIndex.Item1 + 1][_curIndex.Item2] == null)
                return;
            ChangeSelectIndex(1, 0);
        }
        
        public void MoveRight()
        {
            if(_curIndex.Item2 + 1 >= _columnNum || _map[_curIndex.Item1][_curIndex.Item2 + 1] == null)
                return;
            ChangeSelectIndex(0, 1);
        }

        void ChangeSelectIndex(int x, int y)
        {
            SelectIconLeave();
            _curIndex.Item1 += x;
            _curIndex.Item2 += y;
            Select();
            AudioManager.PlayAudioEffectA("选中框移动");
        }

        public ActItem GetItem()
        {
            return _actItems[_curIndex.Item1 * _columnNum + _curIndex.Item2];
        }

        // public int idxX()
        // {
        //     return _curIndex.Item1;
        // }
        // public int idxY()
        // {
        //     return _curIndex.Item1;
        // }

        public TDAction GetSelectData()
        {
            return _map[_curIndex.Item1][_curIndex.Item2];
        }

        void InsertData(TDAction tdAction)
        {
            if(_temp.Item1 >= _rowNum)
                return;
            int startIndex = (_columnNum - _itemNumberInEveryRow[_temp.Item1]) / 2;
            _map[_temp.Item1][startIndex + _temp.Item2] = tdAction;
            _temp.Item2++;
            if (_temp.Item2 >= _itemNumberInEveryRow[_temp.Item1])
            {
                _temp.Item1++;
                _temp.Item2 = 0;
            }
        }
    }
}