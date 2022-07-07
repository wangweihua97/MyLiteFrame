using Script.DB;
using UnityEngine;

//GM指令
public class GMOrder : MonoBehaviour
{
    private static bool isShowWindow = false;

    private string inputOrder = "";

    void Start()
    {
        //Events.EventCenter.ins.AddEventListener<KeyCode>("KeyDown", KeyDown);
    }
    private void OnDestroy()
    {
        //Events.EventCenter.ins.RemoveEventListener<KeyCode>("KeyDown", KeyDown);
    }

    void KeyDown(KeyCode keyCode)
    {
        switch (keyCode)
        {
            case KeyCode.F12:
                ShowGMOrderWindow();
                break;
        }
    }

    public static void ShowGMOrderWindow()
    {
        GMOrder.isShowWindow = !GMOrder.isShowWindow;
    }

    void OnGUI()
    {
        if (GMOrder.isShowWindow)
        {
            GUI.Window(0, new Rect(10, 10, 600, 300), ControlWindow, "GM指令");
        }
    }

    private void ControlWindow(int id)
    {
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        GUILayout.Label("通用指令输入", GUILayout.Width(120));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("输入GM指令: ", GUILayout.Width(130));
        GUI.SetNextControlName("inputOrder");
        inputOrder = GUILayout.TextField(inputOrder, 128, GUILayout.Width(300));
        if (GUILayout.Button("发送指令", GUILayout.Width(100)))
        {
            SendGMOrder(inputOrder);
        }
        GUILayout.EndHorizontal();
        ////////////////////////////////////////////////////////////////////////////////////////////

        GUILayout.BeginHorizontal();
        GUILayout.Label("------------------------------------------------------------------------------------------", GUILayout.Width(665));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("常用快捷指令", GUILayout.Width(120));
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("清除本地缓存数据", GUILayout.Width(120))) { ClearLocalData(); }
        GUILayout.Space(6);
        if (GUILayout.Button("", GUILayout.Width(120))) { }
        GUILayout.Space(6);
        if (GUILayout.Button("", GUILayout.Width(120))) { }
        GUILayout.EndHorizontal();

        GUILayout.EndHorizontal();
        ////////////////////////////////////////////////////////////////////////////////////////////
    }

    #region _各种GM指令功能的逻辑实现
    void SendGMOrder(string order)
    {
        //向服务器发送GM指令
        //...

        //或解析字符串本地处理指令
        //...
    }
    /// <summary> 清除本地缓存数据 </summary>
    void ClearLocalData()
    {
        Debug.Log("清除本地缓存数据");

        PlayerPrefs.DeleteAll();
    }
    #endregion
}


