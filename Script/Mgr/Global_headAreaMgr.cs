using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global_headAreaMgr : MonoBehaviour
{
    public static Global_headAreaMgr instance;

    private RenderTexture rt;
    
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);

        rt = GetComponent<Camera>().targetTexture;
    }

    public RenderTexture GetRT()
    {
        return rt;
    }
    // Start is called before the first frame update
    void Start()
    {
        // RenderTexture = new RenderTexture(500 ,400 ,1);
        // Camera.targetTexture = RenderTexture;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
