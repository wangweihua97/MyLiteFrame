using System;
using System.Collections.Generic;
using EasingCore;
using FancyScrollView;
using UI.Base;
using UIModel.TrainMode;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TrainMode
{
    public class ScrollViewComponent : FancyScrollRect<TrainScrollItemData, TrainModeScrollContext>
    {
        [SerializeField] float cellSize = 100f;
        [SerializeField] GameObject cellPrefab = default;
        [SerializeField] Alignment alignmentDropdown = default;

        protected override float CellSize => cellSize;
        protected override GameObject CellPrefab => cellPrefab;
        public int DataCount => ItemsSource.Count;

        public float PaddingTop
        {
            get => paddingHead;
            set
            {
                paddingHead = value;
                Relayout();
            }
        }

        public float PaddingBottom
        {
            get => paddingTail;
            set
            {
                paddingTail = value;
                Relayout();
            }
        }

        public float Spacing
        {
            get => spacing;
            set
            {
                spacing = value;
                Relayout();
            }
        }

        public void OnCellClicked(Action<int> callback)
        {
            Context.OnCellClicked = callback;
        }

        public void ScrollTo(int index)
        {
            ScrollTo(index, 0.3f, Ease.InOutQuint, (Alignment)alignmentDropdown);
        }

        public void UpdateData(IList<TrainScrollItemData> items)
        {
            UpdateContents(items);
        }

        public void ScrollTo(int index, float duration, Ease easing, Alignment alignment = Alignment.Upper)
        {
            UpdateSelection(index);
            ScrollTo(index, duration, easing, GetAlignment(alignment));
        }

        public void JumpTo(int index, Alignment alignment = Alignment.Upper)
        {
            UpdateSelection(index);
            JumpTo(index, GetAlignment(alignment));
        }

        float GetAlignment(Alignment alignment)
        {
            switch (alignment)
            {
                case Alignment.Upper: return 0.0f;
                case Alignment.Middle: return 0.5f;
                case Alignment.Lower: return 1.0f;
                default: return GetAlignment(Alignment.Middle);
            }
        }

        void UpdateSelection(int index)
        {
            /*if (Context.SelectedIndex == index)
            {
                return;
            }*/

            Context.SelectedIndex = index;
            Refresh();
        }

        public void DoRefresh()
        {
            base.Refresh();
        }
    }

    public enum Alignment
    {
        Upper,
        Middle,
        Lower,
    }
    
    public class TrainModeScrollContext : FancyScrollRectContext
    {
        public int SelectedIndex = -1;
        public Action<int> OnCellClicked;
    }
}