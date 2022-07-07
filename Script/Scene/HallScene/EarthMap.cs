using System;
using System.Collections.Generic;
using DG.Tweening;
using Events;
using OtherMgr;
using Player;
using Script.Excel.Table;
using Script.Mgr;
using Script.Scene.Hall.Item;
using Tool.Others;
using UnityEngine;

namespace Script.Scene.Hall
{
    public class EarthMap : MonoBehaviour
    {
        public MapNode SelectNode;
        public MapNode LevelNode;

        public static EarthMap Instance;
        private Vector3 _initRotation;
        private List<Vector3> _nodes;
        private List<Vector3> _starRotaiton;
        private List<GameObject> _nodesGameObjects;
        public int CurLevelIndex;
        private int _playerMaxLevel;
        private bool _isActive;
        private MapLineModel _mapLine;
        private Coroutine _mapLineCoroutine;
        private Tweener _rotateTweener;
        private void Awake()
        {
            Instance = this;
            CurLevelIndex = 0;
            _isActive = false;
            _playerMaxLevel = PlayerInfo.Instance.PlayerData.PassMaxLevel;
            _nodes = new List<Vector3>();
            _nodesGameObjects = new List<GameObject>();
            _starRotaiton = new List<Vector3>();
            string mapRotationStr = ExcelMgr.TDGlobal.Get("1019").prop1;
            _initRotation= mapRotationStr.ToVector3();
            InitLevelNode();
        }

        public void Exit()
        {
            _isActive = false;
        }

        public void Init()
        {
            _isActive = true;
            if(_playerMaxLevel <= 0)
                transform.localRotation = Quaternion.Euler(_starRotaiton[0]);
            else
                transform.localRotation = Quaternion.Euler(_starRotaiton[_playerMaxLevel - 1]);
        }

        void Move(int value)
        {
            int NewCurLevelIndex = CurLevelIndex + value;
            if(NewCurLevelIndex < 0 || NewCurLevelIndex >= ExcelMgr.TDLevel.Count - 1)
                return;
            SelectLevel(NewCurLevelIndex);
        }

        /*void InitLevelNode()
        {
            foreach (var kvp in ExcelMgr.TDLevel.GetDictionary())
            {
                TDLevel level = kvp.Value;
                Vector3 rotation = new Vector3(level.position[0], level.position[1], level.position[2]);
                //if (Int32.Parse(level.Id) <= PlayerInfo.Instance.PlayerData.MaxLevel && ExcelMgr.TDLevel.Count)
                if (Int32.Parse(level.Id) <= 1012 && _playerMaxLevel < ExcelMgr.TDLevel.Count)
                {
                    _playerMaxLevel++;
                    GameObject go = Instantiate(LevelNode);
                    go.transform.parent = LevelNode.transform.parent;
                    go.transform.localPosition = LevelNode.transform.localPosition;
                    go.transform.localRotation = Quaternion.Euler(rotation);
                    _nodesGameObjects.Add(go);
                }
                _nodes.Add(rotation);
            }

            if (_playerMaxLevel >= ExcelMgr.TDLevel.Count - 1)
                _playerMaxLevel = ExcelMgr.TDLevel.Count - 1;
            
            LevelNode.transform.localRotation = Quaternion.Euler(_nodes[_playerMaxLevel]);
            LevelNode.SetActive(false);
            SelectNode.transform.localRotation = Quaternion.Euler(_nodes[_playerMaxLevel]);
            CurLevelIndex = _playerMaxLevel;
        }*/
        
        void InitLevelNode()
        {
            foreach (var kvp in ExcelMgr.TDLevel.GetDictionary())
            {
                TDLevel level = kvp.Value;
                Vector3 rotation = new Vector3(level.position[0], level.position[1], level.position[2]);
                Vector3 starRotation = new Vector3(level.rotation[0], level.rotation[1], level.rotation[2]);
                //if (Int32.Parse(level.Id) <= PlayerInfo.Instance.PlayerData.MaxLevel && ExcelMgr.TDLevel.Count)
                _nodes.Add(rotation);
                _starRotaiton.Add(starRotation);
            }
        }

        public void SelectLevel(int levelIndex)
        {
            Clear();
            if (levelIndex <= 0)
            {
                if (_mapLine != null)
                {
                    _mapLine.Dispose();
                }
                LevelNode.transform.localRotation = Quaternion.Euler(_nodes[0]);
                LevelNode.gameObject.SetActive(false);
                SelectNode.transform.localRotation = Quaternion.Euler(_nodes[0]);
                transform.DOLocalRotate(_starRotaiton[0], 0.5f);
                return;
            }
            else if (levelIndex >= ExcelMgr.TDLevel.Count)
            {
                LevelNode.transform.localRotation = Quaternion.Euler(_nodes[levelIndex - 1]);
                LevelNode.gameObject.SetActive(false);
                SelectNode.transform.localRotation = Quaternion.Euler(_nodes[levelIndex - 1]);
                CurLevelIndex = ExcelMgr.TDLevel.Count - 1;
                return;
            }
            
            CurLevelIndex = levelIndex;
            LevelNode.gameObject.SetActive(true);
            LevelNode.transform.localRotation = Quaternion.Euler(_nodes[levelIndex - 1]);
            SelectNode.gameObject.SetActive(false);
            SelectNode.transform.localRotation = Quaternion.Euler(_nodes[levelIndex]);
            _rotateTweener = transform.DOLocalRotate(_starRotaiton[levelIndex], 1f);
            _mapLine = PoolManager.CommonPool.Spawn("Prefab/MapLine", transform)
                .GetComponent<MapLineModel>();
            _mapLine.transform.localScale = new Vector3(0.2f, 0.1f, 0.1f);
            _mapLine.transform.localPosition = transform.InverseTransformPoint(LevelNode.GetIconPosition());
            _mapLine.Play(SelectNode.GetIconPosition() ,transform.InverseTransformPoint(SelectNode.GetIconPosition()), 0.5f);
            
            _mapLineCoroutine = WaitTimeMgr.WaitTime(0.5f,
                () =>
                {
                    _mapLineCoroutine = null;
                    SelectNode.gameObject.SetActive(true);
                });
        }

        void Clear()
        {
            if (_mapLineCoroutine != null)
            {
                WaitTimeMgr.CancelWait(ref _mapLineCoroutine);
                SelectNode.gameObject.SetActive(true);
            }
            if (_mapLine != null)
            {
                _mapLine.Dispose();
            }

            if (_rotateTweener != null && !_rotateTweener.IsComplete())
            {
                _rotateTweener.Kill(false);
            }
        }

        private void OnDestroy()
        {
            Instance = null;
            Clear();
            PoolManager.CommonPool.RemovePool("Prefab/MapLine");
        }
    }
}