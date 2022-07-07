using System;
using FancyScrollView;
using OtherMgr;
using Script.Mgr;
using UIModel.TrainMode;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.U2D;
using UnityEngine.UI;

namespace UI.TrainMode
{
    public class SelectSceneItem : FancyScrollRectCell<SceneItemData, TrainModeScrollContext>
    {
        public Text Name;
        public Image Icon;
        public GameObject BeSelectedGO;
        public GameObject NotBeSelectedGO;
        public GameObject RandomGO;
        public GameObject BeLockedGO;
        private SceneItemData _data;
        private int _index = -1;
        private bool _isBeSelect = false;
        private int _priority = 4;
        private SortingGroup _sortingGroup;
        private Canvas _canvas;
        private Animator _animator;
        private CanvasGroup _canvasGroup;

        private float _intervalTime = 0.2f;
        private float _time = 0;
        private int _sceneLength = 0;
        private int _randomIndex = 0;

        public bool IsLocked
        {
            get { return _data.IsLocked; }
        }

        private void Update()
        {
            if (_data == null || !_data.IsRandom || !gameObject.activeSelf)
                return;
            _time += Time.deltaTime;
            if (_time > _intervalTime)
            {
                _time = 0;
                _randomIndex = (_randomIndex + 1) % _sceneLength;
                Icon.sprite = SpritesMgr.Get(ExcelMgr.TDScene.Get(_randomIndex).icon).Sprite;
            }
        }

        public override void Initialize()
        {
            _sortingGroup = GetComponent<SortingGroup>() ?? gameObject.AddComponent<SortingGroup>();
            _canvas = GetComponent<Canvas>() ?? gameObject.AddComponent<Canvas>();
            _canvasGroup = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
            _animator = GetComponent<Animator>();
            _sceneLength = ExcelMgr.TDScene.Count;
        }

        public override void UpdateContent(SceneItemData itemData)
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
            NotBeSelectedGO.SetActive(false);
            BeSelectedGO.SetActive(true);
        }
        
        public void BeLeft()
        {
            if(!_isBeSelect)
                return;
            _isBeSelect = false;
            NotBeSelectedGO.SetActive(true);
            BeSelectedGO.SetActive(false);
        }

        protected override void UpdatePosition(float normalizedPosition, float localPosition)
        {
            float scale = 1 - Mathf.Abs(normalizedPosition - 0.5f)*0.8f;
            transform.localScale = new Vector3(scale ,scale ,scale);

            int SortingPriority = normalizedPosition < 0.05f || normalizedPosition > 0.95f ? 4 :
                normalizedPosition < 0.2f || normalizedPosition > 0.8f ? 3 :
                normalizedPosition < 0.4f || normalizedPosition > 0.6f ? 2 : 1;
            SetSorting(SortingPriority);
            base.UpdatePosition(normalizedPosition, localPosition);
        }

        void SetSorting(int priority)
        {
            if(_priority == priority)
                return;
            _priority = priority;
            gameObject.SetActive(true);
            switch (priority)
            {
                case 1:
                    _sortingGroup.sortingLayerName = "CommonView";
                    _sortingGroup.sortingOrder = 53;
                    _canvas.sortingOrder = 3;
                    _canvasGroup.alpha = 1;
                    break;
                case 2:
                    _sortingGroup.sortingLayerName = "CommonView";
                    _sortingGroup.sortingOrder = 52;
                    _canvas.sortingOrder = 2;
                    _canvasGroup.alpha = 0.85f;
                    break;
                case 3:
                    _sortingGroup.sortingLayerName = "CommonView";
                    _sortingGroup.sortingOrder = 51;
                    _canvas.sortingOrder = 1;
                    _canvasGroup.alpha = 0.7f;
                    break;
                default:
                    gameObject.SetActive(false);
                    break;
                
            }
        }
        

        void InitItem()
        {
            Name.text = _data.Name;
            if (_data.IsLocked)
            {
                BeLockedGO.SetActive(true);
            }
            else
            {
                BeLockedGO.SetActive(false);
            }

            if (_data.IsRandom)
            {
                _time = 0;
                RandomGO.SetActive(true);
                Icon.sprite = SpritesMgr.Get("随机场景").Sprite;
                _animator.Play("SelectSceneItem");
            }
            else
            {
                RandomGO.SetActive(false);
                _animator.Rebind();
                Icon.sprite = SpritesMgr.Get(_data.Name).Sprite;
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