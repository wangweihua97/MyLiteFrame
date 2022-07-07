using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using Script.Main;
using UnityEngine;

public class P1moveCamera : MonoBehaviour
{
    public Animator camera_ani;
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.ins.AddEventListener("move_p1_scene_camera",move_camera);
    }

    void move_camera()
    {
        camera_ani.enabled = true;
        camera_ani.Rebind();
        camera_ani.speed = 1;
        camera_ani.Play("Camera_Group_Start");
        Global.instance.StartCoroutine(delayEnter(0.6f));
    }
    
    private IEnumerator delayEnter(float time)
    {
        yield return new WaitForSeconds(time);
        EventCenter.ins.EventTrigger("move_p1_scene_camera_complete");
    } 

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDestroy()
    {
        EventCenter.ins.RemoveEventListener("move_p1_scene_camera",move_camera);
    }
}
