using Events;
using Script.Main;
using Script.Model;

namespace Enemy
{
    public class EnemyHp
    {
        public int _hp = 0; //当前生命值
        public void Init()
        {
            _hp = GameVariable.EnemyHp[GameVariable.CurBattleIndex];
            RefreshEnemyHp();
        }

        public void SetHp(int hp)
        {
            if (!EnemyMgr.instance.IsLived)
                return;
            _hp = hp;
            if (_hp <= 0)
            {
                _hp = 0;
                EventCenter.ins.EventTrigger("EnemyDie");
            }
            RefreshEnemyHp();
        }
        
        public int GetHp()
        {
            return _hp;
        }
        
        public void AddHp(int hp)
        {
            _hp += hp;
            RefreshEnemyHp();
        }

        public void RefreshEnemyHp()
        {
            GameUIMgr.GameMainView.SetEnemyHP(GetHp());
        }
    }
}