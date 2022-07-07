namespace Script.Scene.Base
{
    public interface IScene
    {
        string Name { get; }
        void OnUpdate();
        
        void LoadPrepareData70Per();
        void LoadPrepareData80Per();
        void LoadPrepareData90Per();
        void OnBattleLoaded();
        void EnterNewScene();
        
        void OnBattleUnLoad();
        
        void OnUnLoadData();
        void OnBattleUnLoaded();
        
        
        
    }
}