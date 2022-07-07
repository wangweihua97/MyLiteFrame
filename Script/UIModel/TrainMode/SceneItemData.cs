namespace UIModel.TrainMode
{
    public class SceneItemData
    {
        public string SceneId;
        public bool IsRandom;
        public bool IsLocked;
        public string Name;
        public string Icon;

        public SceneItemData(string sceneId,bool isLocked, bool isRandom ,string name ,string icon)
        {
            SceneId = sceneId;
            IsLocked = isLocked;
            IsRandom = isRandom;
            Name = name;
            Icon = icon;
        }
    }
}