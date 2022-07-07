using Events;
using Script.Game;
using Script.Main;
using Script.Model;
using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerEnergy
    {
        public int _energy = 0; //当前能力条

        private bool _addInvalid = false;
        public void Init()
        {
            _energy = 0;
            RefreshEnergy();
            EventCenter.ins.AddEventListener("ExitComBo",ExitComBo);
            EventCenter.ins.AddEventListener("DeductEnergy", ReduceEnergy);
        }

        IEnumerator NextAddEnergyInvalid()
        {
            _addInvalid = true;
            yield return new WaitForEndOfFrame();
            _addInvalid = false;
        }

        public void SetEnergy(int energy)
        {
            if(MainGameCtr.instance.IsComBo)
                return;
            _energy = energy;
            RefreshEnergy();
        }
        
        public int GetEnergy()
        {
            return _energy;
        }
        
        public void AddEnergy(int energy)
        {
            if(MainGameCtr.instance.IsComBo)            
                return;
            if (_addInvalid)
            {
                _addInvalid = false;
                return;
            }
            _energy += energy;
            RefreshEnergy();
        }
        public void ReduceEnergy()
        {
            if (MainGameCtr.instance.IsComBo)
            {
                _energy -= Mathf.RoundToInt((float)GameVariable.FullEnergy / (float)GameVariable.ComboTime);
                RefreshEnergy();
            }
        }

        public void RefreshEnergy()
        {
            if (_energy >= GameVariable.FullEnergy)
            {
                _energy = GameVariable.FullEnergy;
                EnterComBoState();
            }
            if (_energy < 0)
            {
                _energy = 0;
            }
            GameUIMgr.GameMainView.SetPower(GetEnergy());
        }

        void ExitComBo()
        {
            SetEnergy(0);
            Global.instance.StartCoroutine(NextAddEnergyInvalid());
        }

        void EnterComBoState()
        {
            EventCenter.ins.EventTrigger("EnterComBo");
        }

        public void Clear()
        {
            EventCenter.ins.RemoveEventListener("ExitComBo",ExitComBo);
            EventCenter.ins.RemoveEventListener("DeductEnergy", ReduceEnergy);
        }
    }
}