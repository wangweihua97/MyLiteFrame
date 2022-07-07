using System.Collections;
using Events;
using Player;
using Script.Mgr;
using UI.CreatePlayer;
using UI.Loading;
using UI.Main;
using UnityEngine;

namespace Script.Main
{
    public class CreatePlayerViewMgr : UUIMgr
    {
        /**性别*/
        public static SelectSexView SelectSexView;
        /**脸型*/
        public static SelectFeatureView SelectFeatureView;
        /**瞳孔色*/
        public static SelectEyeColorView SelectEyeColorView;
        /**肤色*/
        public static SelectSkinColorView SelectSkinColorView;
        /**发型*/
        public static SelectHairView SelectHairView;
        /**发色*/
        public static SelectHairColorView SelectHairColorView;
        /**确认*/
        public static SelectConfirmView SelectConfirmView;
        
        public override string Name => "CreatePlayerViewMgr";
        public override string sortingLayerName => "CommonView";

        public override void DoCreat()
        {
            base.DoCreat();
            GameFlowTaskGroup gameFlowTaskGroup = FlowTaskFactory.CreatTaskGroup();
            Add<SelectSexView>("SelectSexView", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("SelectSexView", "UIView"));
            Add<SelectFeatureView>("SelectFeatureView", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("SelectFeatureView", "UIView"));
            Add<SelectEyeColorView>("SelectEyeColorView", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("SelectEyeColorView", "UIView"));
            Add<SelectSkinColorView>("SelectSkinColorView", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("SelectSkinColorView", "UIView"));
            Add<SelectHairView>("SelectHairView", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("SelectHairView", "UIView"));
            Add<SelectHairColorView>("SelectHairColorView", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("SelectHairColorView", "UIView"));
            Add<SelectConfirmView>("SelectConfirmView", "UIView" ,gameFlowTaskGroup.CompleteATask);
            gameFlowTaskGroup.Add(GetHandle("SelectConfirmView", "UIView"));
            gameFlowTaskGroup.Attach(GameFlowMgr.LoadPrepareData90Per);
            //
            EventCenter.ins.AddEventListener("enter_game_befor_check",checkEnterGame);
            EventCenter.ins.AddEventListener("move_p1_scene_camera_complete",createPlayerCameraMoveComplete);
            //
            EventCenter.ins.AddEventListener("create_player_step0", step0);
            EventCenter.ins.AddEventListener("create_player_step1", step1);
            EventCenter.ins.AddEventListener("create_player_step2", step2);
            EventCenter.ins.AddEventListener("create_player_step3", step3);
            EventCenter.ins.AddEventListener("create_player_step4", step4);
            EventCenter.ins.AddEventListener("create_player_step5", step5);
            EventCenter.ins.AddEventListener("create_player_step6", step6);
            EventCenter.ins.AddEventListener("create_player_step7", step7);
        }
        public override void DoDestroy()
        {
            EventCenter.ins.RemoveEventListener("enter_game_befor_check",checkEnterGame);
            EventCenter.ins.RemoveEventListener("move_p1_scene_camera_complete",createPlayerCameraMoveComplete);
            //
            EventCenter.ins.RemoveEventListener("create_player_step0", step0);
            EventCenter.ins.RemoveEventListener("create_player_step1", step1);
            EventCenter.ins.RemoveEventListener("create_player_step2", step2);
            EventCenter.ins.RemoveEventListener("create_player_step3", step3);
            EventCenter.ins.RemoveEventListener("create_player_step4", step4);
            EventCenter.ins.RemoveEventListener("create_player_step5", step5);
            EventCenter.ins.RemoveEventListener("create_player_step6", step6);
            EventCenter.ins.RemoveEventListener("create_player_step7", step7);
            base.DoDestroy();
        }

        void step0()
        {
            SelectSexView.DoOpen();
        }
        void step1()
        {
            //镜头
            EventCenter.ins.EventTrigger("CameraMove" ,ExcelMgr.TDCameraLocation.Get("SelectSexView").location);
            // 
            SelectSkinColorView.DoOpen();
        }
        void step2()
        {
            //镜头
            EventCenter.ins.EventTrigger("CameraMove" ,ExcelMgr.TDCameraLocation.Get("SelectHairView").location);
            SelectFeatureView.DoOpen();
        }
        void step3()
        {
            SelectHairView.DoOpen();
        }
        void step4()
        {
            SelectHairColorView.DoOpen();
        }
        void step5()
        {
            //镜头
            EventCenter.ins.EventTrigger("CameraMove" ,ExcelMgr.TDCameraLocation.Get("SelectEyeColorView").location);
            SelectEyeColorView.DoOpen();
        }
        void step6()
        {
            // 镜头
             EventCenter.ins.EventTrigger("CameraMove" ,ExcelMgr.TDCameraLocation.Get("SelectSexView").location);
            //提示确认
            SelectConfirmView.DoOpen();
            //确认ok再
            //保存人物设置
            // PlayerMgr.instance.PlayerClothingMgr.TrySave();
            // EventCenter.ins.EventTrigger("createPlayer_complete");
            // Global.SceneMgr.StartScene("HallScene" ,LoadingViewMgr.LoadingView);
        }
        
        void step7()
        {
            //确认ok
            //保存人物设置
            PlayerMgr.instance.PlayerClothingMgr.TrySave();
            PlayerInfo.Instance.CreatPlayerInfo();
            EventCenter.ins.EventTrigger("createPlayer_complete");
            // Global.SceneMgr.StartScene("HallScene" ,LoadingViewMgr.LoadingView);
        }

        public void checkEnterGame()
        {
            bool isCreated = false;
            if (isCreated)
            {
                Global.SceneMgr.StartScene("HallScene" ,LoadingViewMgr.LoadingView);
            }
            else
            {
                //发送移动镜头
                EventCenter.ins.EventTrigger("move_p1_scene_camera");
            }
            
        }

        /**移动镜头完成*/
        void createPlayerCameraMoveComplete()
        {
            Global.instance.StartCoroutine(delayEnter(1f));
        }
        
        private IEnumerator delayEnter(float time)
        {
            yield return new WaitForSeconds(time);
            step0();
        } 
    }
}