using System.Collections;
using System.Collections.Generic;
using UI.Base;
using UnityEngine;

namespace UI.LevelSelect
{
    public class LvItemCtr : UComponent
    {
        public GameObject select;

        public void SetSelect(bool b)
        {
            select.SetActive(b);
        }
    }
}
