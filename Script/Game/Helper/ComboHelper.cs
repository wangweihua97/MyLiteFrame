using Events;
using Ghost;
using Script.Main;
using Script.Model;

namespace Script.Game
{
    public class ComboHelper
    {
        private int _comboTime;
        private int _curComboCount;
        public int MaxComboCount;
        public void Init()
        {
            MainGameCtr.instance.IsComBo = false;
            MaxComboCount = 0;
            EventCenter.ins.AddEventListener("EnterComBo",EnterComBo);
        }

        public void DoUpdate()
        {
            if(!MainGameCtr.instance.IsComBo)
                return;
        }
        
        void EnterComBo()
        {
            MainGameCtr.instance.IsComBo = true;
            _comboTime = GameVariable.ComboTime;
            _curComboCount = 0;
        }

        public void PunchInComboState(ActionModel actionModel, GradeType grade)
        {
            if(!MainGameCtr.instance.IsComBo)
                return;
            if(actionModel.ActionClass != ActionClass.Attack)
                return;

            _comboTime--;
            EventCenter.ins.EventTrigger("DeductEnergy");

            if (grade != GradeType.Miss)
            {
                MaxComboCount++;
                _curComboCount++;
                GameUIMgr.GameMainView.ComboCom.DoComboEffect(_curComboCount);
            }
            if (_comboTime <= 0 )
            {
                MainGameCtr.instance.IsComBo = false;
                EventCenter.ins.EventTrigger("ExitComBo");
            }
        }

        public void Clear()
        {
            EventCenter.ins.RemoveEventListener("EnterComBo",EnterComBo);
        }
    }
}