using Events;
using Script.Main;
using Script.Model;

namespace Player
{
    public class PlayerHp
    {
        public int _hp = 0; //当前生命值
        public void Init()
        {
            _hp = GameVariable.PlayerHp;
            RefreshHP();
        }

        public void SetHp(int hp)
        {
            _hp = hp;
            RefreshHP();
        }
        
        public int GetHp()
        {
            return _hp;
        }
        
        public void AddHp(int hp)
        {
            _hp += hp;
            RefreshHP();
        }

        public void RefreshHP()
        {
            if (_hp <= 0)
            {
                _hp = 0;
                if (PlayerMgr.instance.IsAlive)
                {
                    PlayerMgr.instance.HpZero();
                    EventCenter.ins.EventTrigger("HpZero");
                }
                    
            }
            GameUIMgr.GameMainView.SetHp(GetHp());
        }
    }
}