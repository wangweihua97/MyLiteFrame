namespace UIModel.TrainMode
{
    public class MusicItemData
    {
        public string MusicId;
        public bool IsRandom;
        public bool IsLocked;
        public string Name;
        public int PlayCount;
        public int Time;

        public MusicItemData(string musicId,bool isLocked, bool isRandom ,string name ,int playCount, int time)
        {
            MusicId = musicId;
            IsLocked = isLocked;
            IsRandom = isRandom;
            Name = name;
            PlayCount = playCount;
            Time = time;
        }
    }
}