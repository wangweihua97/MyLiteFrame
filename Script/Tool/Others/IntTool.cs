namespace Tool.Others
{
    public static class IntTool
    {
        public static string SecondToTime(this int _this ,string middleStr)
        {
            int second = _this % 60;
            string secondStr = second < 10 ? "0" + second : second.ToString();
            int minute = _this / 60;
            return minute + middleStr + secondStr;
        }
    }
}