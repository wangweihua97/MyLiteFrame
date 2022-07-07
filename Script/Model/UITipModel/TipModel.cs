namespace Script.Model
{
    public class TipModel
    {
        public string text;
        public string textSoundRes;
        public bool IsContinuous;

        public TipModel(string text, string textSoundRes ,bool IsContinuous = false)
        {
            this.text = text;
            this.textSoundRes = textSoundRes;
            this.IsContinuous = IsContinuous;
        }
    }
}