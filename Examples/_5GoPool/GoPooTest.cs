using System.Collections;
using System.Collections.Generic;
using Examples.GoPool;
using UnityEngine;

public class GoPooTest : MonoBehaviour
{
    public Transform BornTf;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreatCube()
    {
        GoPoolMgr.GoPool.Spawn("CubeGo",BornTf,Vector3.zero,Vector3.zero);
    }

    public void CreatSphere()
    {
        GoPoolMgr.GoPool.Spawn("SphereGo",BornTf,Vector3.zero,Vector3.zero);
    }

    public void ReleaseCube()
    {
        GoPoolMgr.GoPool.RemovePool("CubeGo");
    }
    
    public void ReleaseSphere()
    {
        GoPoolMgr.GoPool.RemovePool("SphereGo");
    }
    
    
}
