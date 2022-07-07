using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HallSceneHelper : MonoBehaviour
{
    public GameObject camera2;
    public Animator cameraAni;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCameraAct(bool b)
    {
        /*cameraAni.speed = 0;
        camera1.SetActive(b);
        camera2.SetActive(b);*/
    }

    public void Play()
    {
        //camera1.SetActive(true);
        cameraAni.enabled = true;
        //cameraAni.Play("Camera_1");

    }

    public void ToComplete()
    {
        cameraAni.Play("Camera_1", 0, 2.5f);
    }
}
