using System.Collections;
using System.Collections.Generic;
using Examples.GoPool;
using UnityEngine;

public class SphereGo : MonoBehaviour
{
    public float Life = 2;

    private float curLiveTime;
    // Start is called before the first frame update
    void OnEnable()
    {
        curLiveTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        curLiveTime += Time.deltaTime;
        if(curLiveTime > Life)
            GoPoolMgr.GoPool.DesSpawn(gameObject);
    }
}
