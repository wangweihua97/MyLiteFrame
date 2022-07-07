using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DG.Tweening;
using Script.Main.Base;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Base
{
    public class UView : BaseGameFlow
    {
        public virtual bool DefaultShow { get; set; }
        private List<UComponent> _uComponents;
        [HideInInspector] public CanvasGroup CanvasGroup;

        private bool _isShowAnimation = false;
        private float _showAnimationSpeed;
        public int index;
        public bool IsShow
        {
            get { return _isShow; }
            set { _isShow = value; }
        }

        public int SortingOrder
        {
            get { return _sortingOrder; }
            set { _sortingOrder = value; }
        }

        public UUIMgr Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        private Canvas _canvas;
        private bool _isShow = false;
        private int _sortingOrder;
        private UUIMgr _parent;

        

        void InitUComponent()
        {
            _uComponents = new List<UComponent>();
            Type type = this.GetType();
            FieldInfo[] fieldInfos = type.GetFields();
            foreach (var field in fieldInfos)
            {
                foreach (var customAttribute in field.CustomAttributes)
                {
                    if(customAttribute.AttributeType == typeof(ComponentAttribute))
                    {
                        this.AddUComponent((UComponent)field.GetValue(this));
                    }
                }
            }
        }
        
        void AddUComponent(UComponent uComponent)
        {
            _uComponents.Add(uComponent);
        }
        
        protected void GraduallyShow(float time = 0.5f)
        {
            CanvasGroup.alpha = 0;
            _isShowAnimation = true;
            _showAnimationSpeed = 1 / time;
            
        }

        protected void GraduallyShow(GameObject go, float time = 0.5f, float? fadeEnd = null)
        {
            Transform[] transforms =  go.GetComponentsInChildren<Transform>();
            foreach (var transformChild in transforms)
            {
                Image image = transformChild.gameObject.GetComponent<Image>();
                if (image)
                { 
                    float end = fadeEnd??image.color.a;
                    if (end >= 0.3f)
                    {
                        image.color = new Color(image.color.r,image.color.g, image.color.b,0.3f);
                        image.DOFade(end,time);
                    }
                }
                Text text = transformChild.gameObject.GetComponent<Text>();
                if (text)
                {
                    float end = fadeEnd??text.color.a;
                    if (end >= 0.3f)
                    {
                        text.color = new Color(text.color.r ,text.color.g ,text.color.b,0.3f);
                        text.DOFade(end,time);
                    }
                }
            }
        }

        protected void GraduallyVanish(GameObject go ,float time = 0.5f)
        {
            Transform[] transforms =  go.GetComponentsInChildren<Transform>();
            foreach (var transformChild in transforms)
            {
                Image image = transformChild.gameObject.GetComponent<Image>();
                if (image)
                {
                    image.color = new Color(1,1,1,image.color.a);
                    image.DOFade(0,time);
                }
                Text text = transformChild.gameObject.GetComponent<Text>();
                if (text)
                {
                    text.color = new Color(text.color.r ,text.color.g ,text.color.b,text.color.a);
                    text.DOFade(0,time);
                }
            }
        }

        public void SetOrder(int order)
        {
            _canvas.sortingOrder = order;
        }

        public int GetIndex()
        {
            return _parent.GetIndex(this);
        }

        public void Top()
        {
            _parent.Top(this);
        }
        public void Head()
        {
            _parent.Head(this);
        }
        

        /// <summary>
        /// 当前页面是否显示
        /// </summary>
        public virtual void Show(bool show)
        {
            if(show && IsShow != show)
                _parent.AddShowCount();
            else if(!show && IsShow != show)
                _parent.DecreaseShowCount();
            
            if (show)
            {
                Top();
            }
            else if (!show)
            {
                Head();
            }
            
            SetActive(show);
            IsShow = show;
        }

        /// <summary>
        /// 设置物体是否激活
        /// </summary>
        ///
        void SetActive(bool isActive)
        {
            if (gameObject == null)
            {
                return;
            }

            if (gameObject.activeSelf != isActive)
            {
                gameObject.SetActive(isActive);
            }
        }

        /// <summary>
        /// 该页面是不是在此Layer中的的最上层
        /// </summary>
        public bool IsTop()
        {
            return _parent.UIIsTop(this) && Parent.IsLayerTop();
        }
        
        /// <summary>
        /// 该页面是不是当前UIMgr的最上层
        /// </summary>
        public bool IsParentTop()
        {
            return _parent.UIIsTop(this);
        }


        public bool IsRootTop()
        {
            return _parent.UIIsTop(this) && Parent.IsTop();
        }


        /// <summary>
        /// 该页面是不是被激活
        /// </summary>
        public bool IsActive()
        {
            return gameObject.activeInHierarchy;
        }

        /// <summary>
        /// 页面创建时执行
        /// </summary>
        public virtual void DoCreat()
        {
            InitUComponent();
            _canvas = GetComponent<Canvas>();
            RectTransform rectTransform = transform as RectTransform;
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.offsetMin = new Vector2(0.0f, 0.0f);
            rectTransform.offsetMax = new Vector2(0.0f, 0.0f);
            transform.localRotation = Quaternion.Euler(Vector3.zero);

            foreach (var component in _uComponents)
            {
                component.DoCreat();
            }

            CanvasGroup = GetComponent<CanvasGroup>() ??
                          gameObject.AddComponent<CanvasGroup>();
        }

        protected override void DoUpdata()
        {
            base.DoUpdata();
            if (_isShowAnimation)
                ShowAnimation();
        }

        void ShowAnimation()
        {
            CanvasGroup.alpha += Time.deltaTime * _showAnimationSpeed;
            if (CanvasGroup.alpha >= 1)
            {
                CanvasGroup.alpha = 1;
                _isShowAnimation = false;
            }
        }

        /// <summary>
        /// 页面打开时
        /// </summary>
        public virtual void DoOpen()
        {
            if (!IsShow || !IsRootTop())
            {
                Show(true);
                foreach (var component in _uComponents)
                {
                    component.DoOpen();
                }
            }
            if(CanvasGroup != null)
                CanvasGroup.alpha = 1;

        }

        /// <summary>
        /// 页面关闭时
        /// </summary>
        public virtual void DoClose()
        {
            if (IsShow)
            {
                Show(false);
                foreach (var component in _uComponents)
                {
                    component.DoClose();
                }
            }

            _isShowAnimation = false;

        }

        /// <summary>
        /// 页面摧毁时执行
        /// </summary>
        public virtual void DoDestory()
        {
            foreach (var component in _uComponents)
            {
                component.DoDestory();
            }
        }
    }

    [AttributeUsage(AttributeTargets.Field)]
    class ComponentAttribute:Attribute
    {
    }
}
