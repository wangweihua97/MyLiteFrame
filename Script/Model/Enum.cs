namespace Script.Model
{
    public enum CheckStoryStep
    {
        /**选择关卡确定时*/
        SelectLevel
        /**战斗开始前*/
        ,BeforeBattle
        /**战斗跑步中*/
        ,MoveBattle
        /**战斗结束*/
        ,AfterBattle
    }
    public enum GameMode
    {
        LevelMode,
        TrainMode
    }
    public enum CsvReadState
    {
       NoReaded,
       Reaing,
       Readed
    }
    
    public enum ActionType
    {
        StraightPunch,//攻击
    }

    public enum ActionClass
    {
        Common,//一般动作
        Attack,//攻击
        Defense,//防御
        Dodge,//防御
    }

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right,
    }

    public enum BeltType
    {
        Left,
        Right,
    }
    
    public enum KeyType
    {
        Up,
        Down,
        Left,
        Right,
        Home,
        Back,
        S1,
        S2,
    }
    
    public enum GameState
    {
        Gaming,
        Pause,
        Story,
        Succeed,
        Failure,
        CompletelyEnd,
    }
    
    public enum AcitonLocat
    {
        Left,
        Right,
        All,
    }
    
    public enum GradeType
    {
        Perfect,//S
        VeryGood,//A
        Good,//B
        Correct,//C
        Miss,//D
    }
    
    public enum SType
    {
        Run,
        MonsterBirth,
        MonsterDie,
    }
    
    public enum Story
    {
        S,//S
        A,//A
        B,//B
        C,//C
        D,//D
    }

    public enum RewardType
    {
        Energy,
    }
    
    public enum EnemyAnimation
    {
        Attack,
        Hurt,
        Dead,
    }

    public enum BulletFlyType
    {
        StraightLine,
        Parabola,
    }
    
    public enum Sex
    {
        Male,
        Famale,
    }
    
    public enum DressUpType
    {
        SkinColor,
        HairColor,
        EyesColor,
        FaceShape,
        HairShape,
        Jacket,
        Trousers,
        Shoe,
        Hand
    }
    
    public enum CharacterPart
    {
        Body,
        Head,
        Hair,
        Hand,
        Leg,
        Eyes,
        Shoes,
    }
    
    public enum PlayerRole
    {
        Player,
        Coach,
    }
}