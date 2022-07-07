using System.Collections;
using System.Collections.Generic;
using Script.Main;
using UnityEngine;
using UnityEngine.UI;

public class TipsCtr : MonoBehaviour
{
    public RectTransform bg;

    public Text txt;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(bool b)
    {
        gameObject.SetActive(b);
    }

    public void ShowTxt(string str, bool autoShow=true)
    {
        Show(autoShow);
        txt.text = str;
        bg.sizeDelta = new Vector2(200+str.Length*50, 72);
        Global.instance.StartCoroutine(OnClose(1));
    }
    
    private IEnumerator OnClose(float time)
    {
        yield return new WaitForSeconds(time);
        //
        Show(false);
    }
}
