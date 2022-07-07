using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using Script.Main;
using UnityEngine;

public class BaseCharacterFMgr : MonoBehaviour
{
    public Animator sceneAni;
    //------------------------创建角色镜头组件----------------------------
    public Transform Plane_tf;
    //
    public MeshRenderer Plane_mr;
    public MeshRenderer Plane1_mr;
    //
    public Transform player_tf;
    //
    public Transform starMap_tf;
    public Light Directional_Light_light;
    public MeshRenderer OutLightMask_mr;
    public MeshRenderer OutLightMask_Back_mr;
    public Light Spot_Light_light;
    public MeshRenderer pPlane1_mr;
    public MeshRenderer pPlane1_emi_mr;
    //lights
    public Light lights_Directional_Light_light;
    public Light lights_Spot_Light_light;
    public Light lights_Spot_Light1_light;
    public Light lights_Spot_Light2_light;
    public Light lights_Spot_Light3_light;
    //------------------------------创建角色镜头设置--------------------------------
    private Vector3 Plane_scale = new Vector3(11.4f,1f,7.1f);
    private Color plane_mr_color = new Color(0.42f,0.7f,0.99f,0.47f);
    private Color plane1_mr_color = new Color(0.42f,0.9f,0.99f,0.27f);
    private Vector3 player_pos = new Vector3(0f,0f,0f);
    private Vector3 starMapAll_pos = new Vector3(14.8f,0.07f,5.76f);
    private float starMap_dl_intensity = 0;
    private Color olm_mr_color = new Color(0.03f,0.08f,0.42f,0f);
    private Color olmb_mr_color = new Color(0.12f,0.14f,0.29f,0f);
    private float starMap_sl_intensity = 2.8f;
    private Color pPlane1_mr_color = new Color(0.41f,0.91f,0.73f,1f);
    private Color pPlane1_mrEmi_color = new Color(0.44f,0.78f,0.91f,1f);
    private float lights_dl_intensity = 0.97f;
    private float lights_sl_intensity = 5f;
    private float lights_sl1_intensity = 1f;
    private float lights_sl2_intensity = 2.5f;
    private float lights_sl3_intensity = 5f;
    //---------------------------------------------------------------------------
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.ins.AddEventListener("bcfMgr_createPlayer", OnSetScene);
        EventCenter.ins.AddEventListener("bcfMgr_playAni", OnPlayAni);
        EventCenter.ins.AddEventListener("bcfMgr_playAni_back", OnPlayAni_back);
        sceneAni.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /**
     * 设置创建角色灯光,着色器,位置......
     * 0=整体,1=脸部
     */
    public void SetCreatePlayer(int type)
    {
        if (0==type)
        {
            Plane_tf.localScale = Plane_scale;
            Plane_mr.material.SetColor("_TintColor", plane_mr_color);
            Plane1_mr.material.SetColor("_TintColor", plane1_mr_color);
            player_tf.localPosition = player_pos;
            starMap_tf.localPosition = starMapAll_pos;
            Directional_Light_light.intensity = starMap_dl_intensity;
            OutLightMask_mr.material.SetColor("_TintColor", olm_mr_color);
            OutLightMask_Back_mr.material.SetColor("_TintColor", olmb_mr_color);
            Spot_Light_light.intensity = starMap_sl_intensity;
            pPlane1_mr.material.SetColor("_TintColor", pPlane1_mr_color);
            pPlane1_emi_mr.material.SetColor("_Emission", pPlane1_mrEmi_color);
            lights_Directional_Light_light.intensity = lights_dl_intensity;
            lights_Spot_Light_light.intensity = lights_sl_intensity;
            lights_Spot_Light1_light.intensity = lights_sl1_intensity;
            lights_Spot_Light2_light.intensity = lights_sl2_intensity;
            lights_Spot_Light3_light.intensity = lights_sl3_intensity;
            // Plane_mr.material;

        }
        else
        {
            
        }
        
    }

    void OnDestroy()
    {
        EventCenter.ins.RemoveEventListener("bcfMgr_createPlayer", OnSetScene);
        EventCenter.ins.RemoveEventListener("bcfMgr_playAni", OnPlayAni);
        EventCenter.ins.RemoveEventListener("bcfMgr_playAni_back", OnPlayAni_back);
    }

    void OnSetScene()
    {
        SetCreatePlayer(0);
    }

    void OnPlayAni()
    {
        sceneAni.enabled = true;
        sceneAni.Rebind();
        sceneAni.Play("Goto_Starmap");
        Global.instance.StartCoroutine(aniPlayComp(GetAniLength("Goto_Starmap")));
    }
    
    private IEnumerator aniPlayComp(float time)
    {
        yield return new WaitForSeconds(time);
        sceneAni.enabled = false;
        EventCenter.ins.EventTrigger("bcfMgr_playAni_complete");
    }
    
    void OnPlayAni_back()
    {
        sceneAni.enabled = true;
        sceneAni.Rebind();
        sceneAni.Play("Backto_Starmap");
        Global.instance.StartCoroutine(aniPlayComp(GetAniLength("Backto_Starmap")));
    }
    
    protected float GetAniLength(string name)
    {
        float re = 0;
        AnimationClip[] clips = sceneAni.runtimeAnimatorController.animationClips;
        int len = clips.Length;
        for (int i = 0; i < len; i++)
        {
            if (clips[i].name.Equals(name))
            {
                re = clips[i].length;
                break;
            }
        }
        return re;
    }
}
