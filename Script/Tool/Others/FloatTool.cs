using Unity.IO.LowLevel.Unsafe;

namespace Tool.Others
{
    public static class FloatTool
    {
        public static float Lerp(this float _this, float endValue, float lerpValue)
        {
            if (lerpValue > 1)
                return endValue;
            if (lerpValue < 0)
                return _this;
            return endValue * lerpValue + (1 - lerpValue) * _this;
        }

        public static float Min(this float _this, float another)
        {
            return _this < another ? _this : another;
        }
        
        public static float Max(this float _this, float another)
        {
            return _this > another ? _this : another;
        }

        public static float Clamp(this float _this, float min, float max)
        {
            return Min(Max(_this, min), max);
        }
        
    }
}