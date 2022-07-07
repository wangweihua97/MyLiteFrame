﻿using System;
using System.Collections.Generic;
 using DG.Tweening;
 using UnityEngine;

namespace OtherMgr
{
    public class AudioManager : MonoBehaviour
    {
        public AudioClip[] AudioClipArray;                               //剪辑数组

        public static float AudioBackgroundVolumns
        {
            get { return _AudioBackgroundVolumns; }
            set
            {
                _AudioBackgroundVolumns = value;
                _AudioSource_BackgroundAudio.volume = _AudioBackgroundVolumns;
            }
        }//背景音量
        public static float AudioEffectAVolumns
        {
            get { return _AudioEffectAVolumns; }
            set
            {
                _AudioEffectAVolumns = value;
                _AudioSource_AudioEffectA.volume = value;
            }
        }
        public static float AudioEffectBVolumns
        {
            get { return _AudioEffectBVolumns; }
            set
            {
                _AudioEffectBVolumns = value;
                _AudioSource_AudioEffectB.volume = value;
            }
        }
        public static float AudioBackgroundPitch = 1f;                   //背景音乐的音调
        public static float AudioEffectPitch = 1.0f;                     //音效的音调


        private static Dictionary<string, AudioClip> _DicAudioClipLib;   //音频库，将声音名字和声音资源进行关联

        private static AudioSource[] _AudioSourceArray;                  //音频源数组

        private static float _AudioBackgroundVolumns = 0.2F;                 //背景音量
        private static float _AudioEffectAVolumns = 1F; 
        private static float _AudioEffectBVolumns = 1F;
        
        private static AudioSource _AudioSource_BackgroundAudio;         //背景音乐
        private static AudioSource _AudioSource_AudioEffectA;            //音效源A
        private static AudioSource _AudioSource_AudioEffectB;            //音效源B

        private static Tweener _tweener;
        //......可以按需求进行添加

        /// <summary>
        /// 音效库资源加载
        /// </summary>
        void Awake()
        {
            AudioClipArray = Resources.LoadAll<AudioClip>("Sound");
            //音频库加载[初始化，将音乐剪辑和名字联系起来]
            _DicAudioClipLib = new Dictionary<string, AudioClip>();

            foreach (AudioClip audioClip in AudioClipArray)
            {
                _DicAudioClipLib.Add(audioClip.name, audioClip);
            }

            gameObject.AddComponent<AudioSource>();
            gameObject.AddComponent<AudioSource>();
            gameObject.AddComponent<AudioSource>();

            //处理音频源，也就是得到用来播放声音的音乐播放器
            _AudioSourceArray = this.GetComponents<AudioSource>();
            _AudioSource_BackgroundAudio = _AudioSourceArray[0];               //其中一个用来播放背景音乐
            _AudioSource_AudioEffectA = _AudioSourceArray[1];                  //其中一个用来播放音乐1
            _AudioSource_AudioEffectB = _AudioSourceArray[2];


            //从数据持久化中得到音量数值
            /*if (PlayerPrefs.GetFloat("AudioBackgroundVolumns") >= 0)
            {
                AudioBackgroundVolumns = PlayerPrefs.GetFloat("AudioBackgroundVolumns");
                _AudioSource_BackgroundAudio.volume = AudioBackgroundVolumns;
            }

            if (PlayerPrefs.GetFloat("AudioEffectVolumns") >= 0)
            {
                AudioEffectVolumns = PlayerPrefs.GetFloat("AudioEffectVolumns");
                _AudioSource_AudioEffectA.volume = AudioEffectVolumns;
                _AudioSource_AudioEffectB.volume = AudioEffectVolumns;
            }

            //设置音乐的音效
            if (PlayerPrefs.HasKey("AudioBackgroundPitch"))
            {
                AudioBackgroundPitch = PlayerPrefs.GetFloat("AudioBackgroundPitch");
            }
            if (PlayerPrefs.HasKey("AudioEffectPitch"))
            {
                AudioEffectPitch = PlayerPrefs.GetFloat("AudioEffectPitch");
            }

            _AudioSource_BackgroundAudio.pitch = AudioBackgroundPitch;
            _AudioSource_AudioEffectA.pitch = AudioEffectPitch;
            _AudioSource_AudioEffectB.pitch = AudioEffectPitch;*/
        }

        public static void RefreshBackgroundAudio()
        {
            _AudioSource_BackgroundAudio.DOKill(true);
            _AudioSource_BackgroundAudio.volume = AudioBackgroundVolumns;
        }
        
        public static void RefreshAudioEffect()
        {
            _AudioSource_BackgroundAudio.volume = AudioBackgroundVolumns;
            _AudioSource_AudioEffectA.volume = AudioEffectAVolumns;
            _AudioSource_AudioEffectB.volume = AudioEffectBVolumns;
        }

        /// <summary>
        /// 播放背景音乐
        /// 传入的参数是背景音乐的AudioClip
        /// </summary>
        /// <param name="audioClip">音频剪辑</param>
        public static void PlayBackground(AudioClip audioClip)
        {
            //防止背景音乐的重复播放。
            if (_AudioSource_BackgroundAudio.clip == audioClip)
            {
                return;
            }

            _AudioSource_BackgroundAudio.DOKill(true);

            //处理全局背景音乐音量
            _AudioSource_BackgroundAudio.volume = AudioBackgroundVolumns;
            _AudioSource_BackgroundAudio.pitch = AudioBackgroundPitch;
            if (audioClip)
            {
                _AudioSource_BackgroundAudio.loop = true;                      //背景音乐是循环播放的
                _AudioSource_BackgroundAudio.clip = audioClip;
                _AudioSource_BackgroundAudio.Play();
                   
            }
            else
            {
                Debug.LogWarning("[AudioManager.cs/PlayBackground()] audioClip==null !");
            }
        }
        
        public static void BackgroundVolumeDecline(float time ,Action callBack)
        {
            _AudioSource_BackgroundAudio.DOKill(true);
            _tweener = DOTween.To(setter: value =>
                {
                    _AudioSource_BackgroundAudio.volume = value;
                }, startValue: AudioBackgroundVolumns, endValue: 0, duration: time)
                .SetEase(Ease.Linear);
            _tweener.OnComplete(() =>
            {
                callBack?.Invoke();
            });
        }
        
        public static void BackgroundVolumeAscending(float time ,Action callBack)
        {
            _AudioSource_BackgroundAudio.DOKill(true);
            _tweener = DOTween.To(setter: value =>
                {
                    _AudioSource_BackgroundAudio.volume = value;
                }, startValue: 0, endValue: AudioBackgroundVolumns, duration: time)
                .SetEase(Ease.Linear);
            _tweener.OnComplete(() =>
            {
                callBack?.Invoke();
            });
        }

        /// <summary>
        /// 播放背景音乐
        /// 传入的参数是声音片段的名字，要注意，其声音片段要加入声音数组中
        /// </summary>
        /// <param name="strAudioName"></param>
        public static void PlayBackground(string strAudioName)
        {
            RefreshBackgroundAudio();
            if (!string.IsNullOrEmpty(strAudioName) && _DicAudioClipLib.ContainsKey(strAudioName))
            {
                PlayBackground(_DicAudioClipLib[strAudioName]);
            }
            else
            {
                Debug.LogWarning("[AudioManager.cs/PlayBackground()] strAudioName==null !");
            }
        }

        /// <summary>
        /// 播放音效_音频源A
        /// </summary>
        /// <param name="audioClip">音频剪辑</param>
        public static void PlayAudioEffectA(AudioClip audioClip)
        {
            //处理全局音效音量
            _AudioSource_AudioEffectA.volume = AudioEffectAVolumns;
            _AudioSource_AudioEffectA.pitch = AudioEffectPitch;

            if (audioClip)
            {
                _AudioSource_AudioEffectA.clip = audioClip;
                _AudioSource_AudioEffectA.Play();
            }
            else
            {
                Debug.LogWarning("[AudioManager.cs/PlayAudioEffectA()] audioClip==null ! Please Check! ");
            }
        }
        /// <summary>
        /// 播放音效_音频源A
        /// </summary>
        /// <param name="strAudioEffctName">音效名称</param>
        public static void PlayAudioEffectA(string strAudioEffctName)
        {
            if (!string.IsNullOrEmpty(strAudioEffctName) && _DicAudioClipLib.ContainsKey(strAudioEffctName))
            {
                PlayAudioEffectA(_DicAudioClipLib[strAudioEffctName]);
            }
            else
            {
                Debug.LogWarning("[AudioManager.cs/PlayAudioEffectA()] strAudioEffctName==null ! Please Check! ");
            }
        }
        /// <summary>
        /// 播放音效_音频源B
        /// </summary>
        /// <param name="audioClip">音频剪辑</param>
        public static void PlayAudioEffectB(AudioClip audioClip)
        {
            //处理全局音效音量
            _AudioSource_AudioEffectB.volume = AudioEffectBVolumns;
            _AudioSource_AudioEffectB.pitch = AudioEffectPitch;
            if (audioClip)
            {
                _AudioSource_AudioEffectB.clip = audioClip;
                _AudioSource_AudioEffectB.Play();
            }
            else
            {
                Debug.LogWarning("[AudioManager.cs/PlayAudioEffectB()] audioClip==null ! Please Check! ");
            }
        }

        /// <summary>
        /// 播放音效_音频源B
        /// </summary>
        /// <param name="strAudioEffctName">音效名称</param>
        public static void PlayAudioEffectB(string strAudioEffctName)
        {
            if (!string.IsNullOrEmpty(strAudioEffctName) && _DicAudioClipLib.ContainsKey(strAudioEffctName))
            {
                PlayAudioEffectB(_DicAudioClipLib[strAudioEffctName]);
            }
            else
            {
                Debug.LogWarning("[AudioManager.cs/PlayAudioEffectB()] strAudioEffctName==null ! Please Check! ");
            }
        }


        /// <summary>
        /// 停止播放音效A
        /// </summary>
        public static void StopPlayAudioEffectA()
        {
            _AudioSource_AudioEffectA.Stop();
        }

        /// <summary>
        /// 停止播放音效B
        /// </summary>
        public static void StopPlayAudioEffectB()
        {
            _AudioSource_AudioEffectB.Stop();
        }

        /// <summary>
        /// 停止播放背景音乐
        /// </summary>
        public static void StopPlayAudioBackGround()
        {
            _AudioSource_BackgroundAudio.Stop();
        }
        
        /// <summary>
        /// 停止播放音效A
        /// </summary>
        public static void PausePlayAudioEffectA()
        {
            _AudioSource_AudioEffectA.Pause();
        }

        /// <summary>
        /// 停止播放音效B
        /// </summary>
        public static void PausePlayAudioEffectB()
        {
            _AudioSource_AudioEffectB.Pause();
        }

        /// <summary>
        /// 停止播放背景音乐
        /// </summary>
        public static void PausePlayAudioBackGround()
        {
            _AudioSource_BackgroundAudio.Pause();
        }
        
        /// <summary>
        /// 停止播放音效A
        /// </summary>
        public static void ContinuePlayAudioEffectA(float volume)
        {
            _AudioSource_BackgroundAudio.volume = volume;
            _AudioSource_AudioEffectA.Play();
        }

        /// <summary>
        /// 停止播放音效B
        /// </summary>
        public static void ContinuePlayAudioEffectB(float volume)
        {
            _AudioSource_BackgroundAudio.volume = volume;
            _AudioSource_AudioEffectB.Play();
        }

        /// <summary>
        /// 停止播放背景音乐
        /// </summary>
        public static void ContinuePlayAudioBackGround(float volume)
        {
            _AudioSource_BackgroundAudio.volume = volume;
            _AudioSource_BackgroundAudio.Play();
        }
    }
}