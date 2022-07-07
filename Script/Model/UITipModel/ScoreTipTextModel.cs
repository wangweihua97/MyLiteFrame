namespace Script.Model
{
    public class ScoreTipTextModel
    {
        public GradeType grade;
        public float time;
        public bool isleft;
        public ScoreTipTextModel(GradeType grade,float time, bool isleft)
        {
            this.grade = grade;
            this.time = time;
            this.isleft = isleft;
        }
    }
}