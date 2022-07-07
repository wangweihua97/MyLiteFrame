using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EasingCore;
using Events;
using FancyScrollView;
using OtherMgr;
using Player;
using Script.Excel.Table;
using Script.Main;
using Script.Mgr;
using Script.Model;
using Script.Tool;
using UI.Base;
using UI.Character.Item;
using UI.Character.vo;
using UI.TrainMode;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;
using UnityEngine.UI;
using Alignment = FancyScrollView.Alignment;

namespace UI.Character
{
    public class DressUpView : UView
    {
        // [Header("选择训练管卡的组件")]
        // public TrainModeSelectComponent TrainModeSelectComponent;
        // [Component]
        // [Header("难易度的组件")]
        // public GradeComponent GradeComponent;
        [Header("列表组件")]
        public GridView list;
        [Header("类型组")]
        public List<TypeBar> typeBar;
        [Header("购买面板")]
        public DressUpBuyPanel dressUpBuyPanel;
        [Header("详细面板")]
        public DressUpDetail dressUpDetail;
        [Header("入场动画")]
        public Animator enterAni;

        public override bool DefaultShow => false;

        private TypeBar curTypeBar;

        private List<ItemData> voList;

        private int listIdx = 0;
        private int listLen = 0;
        private int typeBarIdx = 0;
        private int typeBarLen = 5;
        /**0=typeBar操作,1=list操作*/
        private HandType handType = HandType.TypeBar;

        private Dictionary<string, List<ItemData>> typeListDic;
        private DressUpVo currSelect;
        private bool _isInit = true;
        
        public override void DoCreat()
        {
            //
            base.DoCreat();
            CharacterUIMgr.DressUpView = this;
            // TrainModeSelectComponent.InitData();
            EventCenter.ins.AddEventListener<KeyCode>("KeyDown",KeyDown);
            EventCenter.ins.AddEventListener<DressUpVo>("DressUp_select_item",SetCurrSelect);
            EventCenter.ins.AddEventListener<string>("DressUp_buy_item", updateListData);
            // var items = Enumerable.Range(0, 50)
            //     .Select(i => new ItemData(i))
            //     .ToArray();
            // listLen = items.Length;
            // list.UpdateContents(items);
            //
            initTab();
            //
            // initBagData();
        }

        void initTab()
        {
            for (int i = 0; i < typeBar.Count; i++)
            {
                typeBar[i].IsSelected = false;
            }
            curTypeBar = typeBar[0];
            curTypeBar.IsSelected = true;
            typeBarLen = typeBar.Count;
        }

        // void initBagData()
        // {
        //     if (null==GameVariable.PlayerBagModel)
        //     {
        //         GameVariable.PlayerBagModel = NativeStoreTool.Get<PlayerBagModel>("PlayerBagModel");
        //         if (null == GameVariable.PlayerBagModel)
        //         {
        //             GameVariable.PlayerBagModel = new PlayerBagModel();
        //             GameVariable.PlayerBagModel.gold = 5000;
        //             GameVariable.PlayerBagModel.DressUpBagData = new List<DressUpBagData>();
        //             GameVariable.PlayerBagModel.DressUpBagDataDic = new Dictionary<string, DressUpBagData>();
        //             NativeStoreTool.Set("PlayerBagModel" ,GameVariable.PlayerBagModel);
        //         }
        //         //
        //         if(GameVariable.PlayerBagModel.gold<=1000)GameVariable.PlayerBagModel.gold = 5000;
        //         //
        //         analysisList();
        //         // if (null==GameVariable.PlayerBagModel.DressUpBagData)GameVariable.PlayerBagModel.DressUpBagData = new List<DressUpBagData>();
        //         // if (null==GameVariable.PlayerBagModel.DressUpBagDataDic)GameVariable.PlayerBagModel.DressUpBagDataDic = new Dictionary<string, DressUpBagData>();
        //         // NativeStoreTool.Set("PlayerBagModel" ,GameVariable.PlayerBagModel);
        //     }
        // }

        void analysisList()
        {
            int len = PlayerBag.PlayerBagModel.DressUpBagData.Count;
            DressUpBagData dubData;
            for (int i = 0; i < len; i++)
            {
                dubData = PlayerBag.PlayerBagModel.DressUpBagData[i];
                PlayerBag.PlayerBagModel.DressUpBagDataDic.Add(dubData.TDCharacterID, dubData);
            }
        }

        void setViewData()
        {
            typeListDic = new Dictionary<string, List<ItemData>>();
            voList = new List<ItemData>();
            // voList = new List<DressUpVo>();
            DressUpVo vo;
            foreach (var kvp in ExcelMgr.TDCharacter.GetDictionary())
            {
                if (kvp.Value.create)
                {
                    continue;
                }
                if (!kvp.Value.gender)//女
                {
                    vo = new DressUpVo();
                    vo.cfg = kvp.Value;
                    if (PlayerBag.PlayerBagModel.DressUpBagDataDic.ContainsKey(vo.cfg.Id))
                    {
                        vo.count = PlayerBag.PlayerBagModel.DressUpBagDataDic[vo.cfg.Id].count;
                        vo.eFlag = PlayerMgr.instance.PlayerClothingMgr.IsWore(vo.cfg.Id);
                        // PlayerMgr.instance.PlayerDressModel
                    }
                    else
                    {
                        vo.eFlag = false;
                        vo.count = 0;
                    }
                    vo.lockFlag = !PlayerBag.ContainsCharacterID(vo.cfg.Id);
                    vo.newFlag = false;
                    // voList[voList.Count] = vo;
                    voList.Add(vo);
                    if (!typeListDic.ContainsKey(vo.cfg.type+""))
                    {
                        typeListDic[vo.cfg.type+""] = new List<ItemData>();
                    }
                    typeListDic[vo.cfg.type + ""].Add(vo);
                }
                else//男
                {
                    
                }
                
                // typeListDic[vo.cfg.type + ""][typeListDic[vo.cfg.type + ""].Count] = vo;
            }
            orderList();
            // listLen = voList.Count;
            Debug.Log(typeListDic["" + curTypeBar.type]);
            listLen = typeListDic[""+curTypeBar.type].Count;
            list.UpdateContents(typeListDic[""+curTypeBar.type]);
            // ExcelMgr.TDMusic.GetDictionary();
        }

        public void SetData(TDTraining data)
        {
            // CurData = data;
            // GradeComponent.SetDegree(data.difficulty);
            // DetailsComponent.SetData(data);
        }
        
        void KeyDown(KeyCode keyCode)
        {
            if(!IsTop() || !IsActive() || dressUpBuyPanel.IsShow())
                return;
            switch (keyCode)
            {
                case KeyCode.W:
                    // TrainModeSelectComponent.MoveUp();
                    AudioManager.PlayAudioEffectA("选中框移动");
                    if (handType==HandType.TypeBar)
                    {
                        handType = HandType.list;
                        listIdx = listLen - (5 - typeBarIdx);
                        listIdx = listIdx < 0 ? 0 : listIdx;
                        updateListSelect();
                        curTypeBar.SetWeaken(true);
                    }
                    else
                    {
                        listIdx-=5;
                        if (listIdx<0)
                        {
                            handType = HandType.TypeBar;
                            // typeBarIdx = listIdx+5;
                            //
                            updateTypeBarSelect();
                            switchListData();
                            SetCurrSelect(null);
                            listIdx = -1;//设置未选中
                            updateListSelect();
                            PlayerMgr.instance.PlayerClothingMgr.Reset();
                            //
                            curTypeBar.SetWeaken(false);
                        }
                        else
                        {
                            updateListSelect();
                        }
                    }
                    break;
                case KeyCode.A:
                    AudioManager.PlayAudioEffectA("选中框移动");
                    // TrainModeSelectComponent.MoveLeft();
                    if (handType==HandType.TypeBar)
                    {
                        typeBarIdx--;
                        if (typeBarIdx<0)
                        {
                            typeBarIdx += typeBarLen;
                        }
                        updateTypeBarSelect();
                        switchListData();
                        updateDetail();
                        curTypeBar.SetWeaken(false);
                    }
                    else
                    {
                        listIdx--;
                        if (listIdx<0)
                        {
                            listIdx += listLen;
                        }
                        updateListSelect();
                    }
                    break;
                case KeyCode.S:
                    AudioManager.PlayAudioEffectA("选中框移动");
                    // TrainModeSelectComponent.MoveDown();
                    if (handType==HandType.TypeBar)
                    {
                        handType = HandType.list;
                        listIdx = typeBarIdx>=listLen?listLen-1:typeBarIdx;
                        updateListSelect();
                        curTypeBar.SetWeaken(true);
                    }
                    else
                    {
                        listIdx+=5;
                        if (listIdx>=listLen)
                        {
                            // handType = HandType.TypeBar;
                            // typeBarIdx = listIdx-listLen;
                            // updateTypeBarSelect();
                            // SetCurrSelect(null);
                            // listIdx = -1;//设置未选中
                            // updateListSelect();
                            // PlayerMgr.instance.PlayerClothingMgr.Reset();
                            // curTypeBar.SetWeaken(false);
                            listIdx -= listLen;
                            updateListSelect();
                        }
                        else
                        {
                            updateListSelect();
                        }
                    }
                    break;
                case KeyCode.D:
                    AudioManager.PlayAudioEffectA("选中框移动");
                    // TrainModeSelectComponent.MoveRight();
                    if (handType==HandType.TypeBar)
                    {
                        typeBarIdx++;
                        if (typeBarIdx>=typeBarLen)
                        {
                            typeBarIdx -= typeBarLen;
                        }
                        updateTypeBarSelect();
                        switchListData();
                        updateDetail();
                        curTypeBar.SetWeaken(false);
                    }
                    else
                    {
                        listIdx++;
                        if (listIdx>=listLen)
                        {
                            listIdx -= listLen;
                        }
                        updateListSelect();
                    }
                    break;
                case KeyCode.J:
                    // 
                    AudioManager.PlayAudioEffectA("选中确认");
                    if (currSelect.eFlag)
                    {
                        PlayerMgr.instance.PlayerClothingMgr.Unload(currSelect.cfg);
                        currSelect.eFlag = false;
                        PlayerMgr.instance.PlayerClothingMgr.TrySave();
                        list.UpdateContents(typeListDic[""+curTypeBar.type]);
                        updateDetail();
                        CommonUIMgr.PopupFrame.ShowTips("  已卸载  ");
                    }
                    else if (PlayerBag.ContainsCharacterID(currSelect.cfg.Id))
                    {
                        if(!currSelect.eFlag)PlayerMgr.instance.PlayerClothingMgr.ChangeDressUp(currSelect.cfg);
                        //
                        string cId = PlayerMgr.instance.PlayerClothingMgr.GetCorrespondingId(currSelect.cfg);
                        // typeListDic[""+curTypeBar.type]
                        DressUpVo vo1=null;//老装备设置标记
                        int kLen = typeListDic["" + currSelect.cfg.type].Count;
                        for (int j = 0; j < kLen; j++)
                        {
                            vo1 = (DressUpVo)typeListDic["" + currSelect.cfg.type][j];//刷新字典
                            vo1.eFlag = false;
                            // if (vo1.cfg.Id != cId)
                            // {
                            //     vo1.eFlag = false;
                            // }
                        }
                        // 
                        PlayerMgr.instance.PlayerClothingMgr.TrySave();
                        currSelect.eFlag = true;
                        currSelect.playEquipAniOnce = true;
                        //
                        list.UpdateContents(typeListDic[""+curTypeBar.type]);
                        updateDetail();
                        switchListData();
                        CommonUIMgr.PopupFrame.ShowTips("  已装备  ");
                    }
                    else if (!PlayerBag.ContainsMoney(currSelect.cfg.price))
                    {
                        CommonUIMgr.PopupFrame.ShowTips("  金币不足  ");
                    }
                    else
                    {
                        dressUpBuyPanel.show(currSelect.cfg);
                    }
                    break;
                case KeyCode.H:
                    // StartCoroutine(OpenBaseTrainView());
                    break;
                case KeyCode.K:
                    DoClose();
                    HallViewMgr.MainView.CameraMoveBTypeOnce = true;
                    HallViewMgr.MainView.DoOpen();
                    break;
            }
            
        }

        void switchListData()
        {
            listLen = typeListDic[""+curTypeBar.type].Count;
            list.UpdateContents(typeListDic[""+curTypeBar.type]);
        }

        void updateListSelect()
        {
            list.UpdateSelection(listIdx);
            list.ScrollTo(listIdx, 0.4f, Ease.InOutQuint, Alignment.Upper);
        }

        void updateTypeBarSelect()
        {
            if(curTypeBar)curTypeBar.IsSelected=false;
            curTypeBar = typeBar[typeBarIdx];
            curTypeBar.IsSelected=true;
        }

        void updateListData(string id)
        {
            DressUpVo vo=null;
            DressUpVo vo1=null;
            int len = voList.Count;
            int kLen;
            for (int i = 0; i < len; i++)
            {
                vo = (DressUpVo)voList[i];
                if (vo.cfg.Id==id)
                {
                    vo.count = 1;//刷新list
                    vo.lockFlag = false;
                    if (vo.cfg.type == curTypeBar.type)//刷列表
                    {
                        orderList();
                        list.UpdateContents(typeListDic[""+curTypeBar.type]);
                        listIdx = typeListDic["" + curTypeBar.type].IndexOf(vo);
                        updateListSelect();
                    }
                    break;
                    /*kLen = typeListDic["" + vo.cfg.type].Count;
                    for (int j = 0; j < kLen; j++)
                    {
                        vo1 = (DressUpVo)typeListDic["" + vo.cfg.type][j];//刷新字典
                        if (vo1.cfg.Id == id)
                        {
                            vo1.count = 1;
                            if (vo1.cfg.type == curTypeBar.type)//刷列表
                            {
                                list.UpdateContents(typeListDic[""+curTypeBar.type]);
                            }
                            break;
                        }
                    }*/
                    //
                    // break;
                }
            }
            //刷详细
            if (null!=currSelect&&currSelect.cfg.Id == id)
            {
                currSelect.count = 1;;
                updateDetail();
            }
            //
        }

        public override void DoOpen()
        {
            if (_isInit)
            {
                _isInit = false;
                analysisList();
                //
                setViewData();
                //
                listIdx = -1;
                updateListSelect();
                updateDetail();
            }
            base.DoOpen();
            AudioManager.PlayAudioEffectA("打开弹窗");
            EventCenter.ins.EventTrigger("CameraMove" ,ExcelMgr.TDCameraLocation.Get("CharacterClothingView").location);
            
            GraduallyShow(gameObject, 0.3f);
            Global.instance.StartCoroutine(delayFun(0.5f));
        }
        private IEnumerator delayFun(float time)
        {
            yield return new WaitForSeconds(time);
            enterAni.enabled = false;
        }
        
        public override void DoClose()
        {
            base.DoClose();
            AudioManager.PlayAudioEffectA("返回");
            PlayerMgr.instance.PlayerClothingMgr.Reset();
        }

        public override void DoDestory()
        {
            base.DoDestory();
            EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown",KeyDown);
            EventCenter.ins.RemoveEventListener<DressUpVo>("DressUp_select_item",SetCurrSelect);
            EventCenter.ins.RemoveEventListener<string>("DressUp_buy_item", updateListData);
        }

        private void SetCurrSelect(DressUpVo vo)
        {
            if (currSelect != vo)
            {
                currSelect = vo;
                updateDetail();
                if (null!=currSelect)
                {
                    PlayerMgr.instance.PlayerClothingMgr.ChangeDressUp(currSelect.cfg);
                }
            }
        }

        /**手,头,衣,裤,鞋*/
        // private int[] typeBar2type = {3, 4, 5, 6, 7};
        void updateDetail()
        {
            int type = -1;
            if (null==currSelect)
            {
                type = typeBarIdx + 1;
            }
            dressUpDetail.setData(currSelect, type);
        }

        /**排序,lockFlag->Index*/
        void orderList()
        {
            foreach (var list in typeListDic.Values)
            {
                list.Sort(delegate(ItemData vo, ItemData vo1)
                {
                    DressUpVo vvo = (DressUpVo) vo;
                    DressUpVo vvo1 = (DressUpVo) vo1;
                    if ((vvo.lockFlag && vvo1.lockFlag) || (!vvo.lockFlag && !vvo1.lockFlag))
                    {
                        return vvo.Index - vvo1.Index;
                    }
                    if (vvo.lockFlag)
                    {
                        return 0;
                    }
                    if (vvo1.lockFlag)
                    {
                        return -1;
                    }
                    return 0;
                });
            }
        }
    }
    
    enum HandType
    {
        TypeBar,
        list,
    }
}