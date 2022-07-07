namespace UIModel.TrainMode
{
    public class TrainScrollItemData
    {
        public string ExcelId;
        public int Degree;
        public bool IsLocked;
        public string Describe;
        public int GetStarNumber;
        public int Score;

        public TrainScrollItemData(string excelId, bool isLocked ,string describe ,int getStarNumber ,int score)
        {
            ExcelId = excelId;
            IsLocked = isLocked;
            Describe = describe;
            GetStarNumber = getStarNumber;
            Score = score;
        }
    }
}