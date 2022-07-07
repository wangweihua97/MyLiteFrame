using BehaviorDesigner.Runtime;
using Script.Main;
using UnityEngine;

namespace Script.Game
{
    public class BTMain
    {
        public BehaviorTree bt;

        public void Init()
        {
            // 动态添加行为树
            bt = Global.instance.gameObject.AddComponent<BehaviorTree>();
            // 加载行为树资源
            var extBt = Resources.Load<ExternalBehaviorTree>("ETree/Behavior");
            bt.StartWhenEnabled = false;
            bt.RestartWhenComplete = false;
            // 设置行为树
            bt.ExternalBehavior = extBt;
        }

        public void SetValue(string name, string value)
        {
            bt.SetVariableValue(name, value);
        }

        public void execute()
        {
            bt.EnableBehavior();
        }

        public void OnDestory()
        {
            bt.OnDestroy();
        }
    }
}
