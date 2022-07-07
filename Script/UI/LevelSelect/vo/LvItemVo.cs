using FancyScrollView;
using Script.Excel.Table;
using UnityEngine;

namespace UI.CreatePlayer.vo
{
    public class LvItemVo:ItemData
    {
        public TDLevel cfg;
        // public Color cl;
        //0=character,1=color
        /**-1=站位,0=已通过,1=已解锁,2=未通过*/
        public int type = 0;
        // public int count;
    }
}