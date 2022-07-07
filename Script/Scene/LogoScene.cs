using Script.Main;
using Script.Mgr;
using Script.Scene.Base;

namespace Script.Scene
{
    public class LogoScene : IScene
    {
        public string Name => "LogoScene";
        public void OnUpdate()
        {
        }

        public void OnBattleUnLoad()
        {
        }

        public void OnBattleUnLoaded()
        {
        }

        public void OnBattleLoad()
        {
        }

        public void LoadPrepareData70Per()
        {
        }

        public void LoadPrepareData80Per()
        {
            RootUIMgr.instance.CreatUIMgr<ShowLogoViewMgr>(true);
            // RootUIMgr.instance.CreatUIMgr<CreatePlayerViewMgr>();
        }

        public void LoadPrepareData90Per()
        {
        }

        public void OnBattleLoaded()
        {
        }

        public void OnUnLoadData()
        {
            RootUIMgr.instance.DestroyUIMgr<ShowLogoViewMgr>();
            // RootUIMgr.instance.DestroyUIMgr<CreatePlayerViewMgr>();
            
        }
        public void EnterNewScene()
        {
        }
    }
}