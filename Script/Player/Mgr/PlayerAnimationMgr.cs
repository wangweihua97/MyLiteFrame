using UnityEngine;

namespace Player
{
    public class PlayerAnimationMgr : MonoBehaviour
    {
        //private ITDLuaMgr AnimationLuaMgr;
        
        /*[HideInInspector]
        public Dictionary<string,AnimationModel> AniMap = new Dictionary<string, AnimationModel>();
        
        
        private AnimationMixerPlayable mixedPlayable;
        private AnimationMixerPlayable idleMixedPlayable;
        PlayableGraph mPlayableGraph;

        public bool BodyIsLeft
        {
            get
            {
                return CurIdelAnimation.aniName == "L_FIdle";
            }
        }

        private bool _bodyIsLeft = true;

        //public AnimationClip[] act;
        private Animator Animator;

        private float time1;
        private bool isPlay1;
        
        private float time2;
        private bool isPlay2;
        
        private float idelW;
        private float act1W;
        private float act2W;

        private int isPlayCount;
        private PlayState _playState = PlayState.Idel;

        private bool _isIdleChangeState = false;
        private float _idleChangeSpeed;
        private float _idleChangeWeight1;
        private float _idleChangeWeight2;
        [HideInInspector]
        public AnimationModel CurAnimation;
        [HideInInspector]
        public AnimationModel ToChangeAnimation;
        [HideInInspector]
        public AnimationModel CurIdelAnimation;
        [HideInInspector]
        public AnimationModel ToChangeIdleAnimation;
        
        public enum PlayState
        {
            Idel,
            Idel2Act,
            Act,
            Act2Act,
            Act2Idel,
        }

        void Start()
        {
            isPlayCount = 0;

            //AnimationLuaMgr = LuaMgr.GetTDLuaMgr(typeof(TDActAnimationData));
            Animator = GetComponent<Animator>();
            Animator.Rebind();
            mPlayableGraph = PlayableGraph.Create();
            var OutPlayable = AnimationPlayableOutput.Create(mPlayableGraph, "Output", Animator);
            //AnimationPlayableUtilities.PlayClip(Animator ,animationOutputPlayable ,out mPlayableGraph);
            mixedPlayable = AnimationMixerPlayable.Create(mPlayableGraph, 4);
            OutPlayable.SetSourcePlayable(mixedPlayable, 0);
            // = AnimationPlayableUtilities.PlayMixer(Animator, 4, out mPlayableGraph);
            idleMixedPlayable = AnimationMixerPlayable.Create(mPlayableGraph ,2);
            mixedPlayable.ConnectInput(0 ,idleMixedPlayable ,0);
            
            mPlayableGraph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
            InitActionAnimation();
            InitIdel();
            StartIdle();
        }

        void InitIdel()
        {
            if(Global.SceneMgr.GetCurSceneName().Equals("GameScene"))
                CurIdelAnimation = AniMap["L_FIdle"];
            else
                CurIdelAnimation = AniMap["Idle"];
            idleMixedPlayable.ConnectInput(0 ,CurIdelAnimation.animationClip ,0);
            idleMixedPlayable.SetInputWeight(0 ,1f);
            //mPlayableGraph.Connect(CurIdelAnimation.animationClip, 0, idleMixedPlayable, 0);
            mPlayableGraph.Play();
        }

        void InitActionAnimation()
        {
            AnimationClip[] clips = Animator.runtimeAnimatorController.animationClips;
            for (int i = 0; i < clips.Length; i++)
            {
                AnimationClipPlayable acp = AnimationClipPlayable.Create(mPlayableGraph ,clips[i]);
                AnimationModel animationModel = new AnimationModel(clips[i].name ,acp);
                foreach (var kvp in ExcelMgr.TDAction.GetDictionary())
                {
                    if (kvp.Value.actAnimation.Equals(clips[i].name))
                    {
                        animationModel.SetData(kvp.Value.fTime,kvp.Value.bTime,kvp.Value.aTime,kvp.Value.time);
                        break;
                    }
                }
                AniMap.Add(clips[i].name ,animationModel);
            }
        }

        public void PlayAnimation(string name)
        {
            if(!CheckAnimation(name))
                return;
            if (isPlayCount == 0)
            {
                isPlayCount = 1;
                isPlay1 = true;
                PlayAnimation(name, PlayState.Idel2Act);
            }
            else
            {
                isPlayCount = 2;
                isPlay2 = true;
                PlayAnimation(name, PlayState.Act2Act);
            }
        }

        void PlayAnimation(string name, PlayState playState)
        {
            _playState = playState; 
            AnimationModel animationModel = GetAnimationModel(name);
            TryDisconnect(animationModel);
            switch (playState)
            {
                case PlayState.Idel2Act:
                    mPlayableGraph.Connect(animationModel.animationClip, 0, mixedPlayable, 1);
                    mixedPlayable.SetInputWeight(1 ,0);
                    animationModel.animationClip.SetTime(0);
                    CurAnimation = animationModel;
                    time1 = 0;
                    act1W = 0;
                    idelW = 1;
                    break;
                case PlayState.Act2Act:
                    mixedPlayable.DisconnectInput(2);
                    mPlayableGraph.Connect(animationModel.animationClip, 0, mixedPlayable, 2);
                    mixedPlayable.SetInputWeight(2 ,0);
                    animationModel.animationClip.SetTime(0);
                    ToChangeAnimation = animationModel;
                    time2 = 0;
                    act2W = idelW;
                    mixedPlayable.SetInputWeight(0 ,0);
                    break;
            }
        }

        void TryDisconnect(AnimationModel model)
        {
            if (mixedPlayable.GetInput(1).Equals(model.animationClip))
            {
                mixedPlayable.DisconnectInput(1);
            }
            else if (mixedPlayable.GetInput(2).Equals(model.animationClip))
            {
                mixedPlayable.DisconnectInput(2);
            }
        }

        private void Update()
        {
            UpdateIdle();
            if (_playState == PlayState.Idel)
                return;
            if (_playState == PlayState.Idel2Act)
                DoIdel2Act();
            else if (_playState == PlayState.Act)
                DoAct();
            else if (_playState == PlayState.Act2Idel)
                DoAct2Idel();
            else if(_playState == PlayState.Act2Act)
                DoAct2Act();
        }

        void DoIdel2Act()
        {
            time1 += Time.deltaTime;
            float decay = 1 / CurAnimation.attackTime * Time.deltaTime;
            act1W += decay;
            idelW -= decay;
            if (act1W >= 1)
            {
                act1W = 1;
                idelW = 0;
                _playState = PlayState.Act;
            }
            mixedPlayable.SetInputWeight(0 ,idelW);
            mixedPlayable.SetInputWeight(1 ,act1W);
        }

        void DoAct()
        {
            time1 += Time.deltaTime;
            if (time1 > CurAnimation.backTime)
            {
                _playState = PlayState.Act2Idel;
                float decay = 1 / (CurAnimation.totalTime - CurAnimation.backTime) * (time1 - CurAnimation.backTime);
                act1W -= decay;
                idelW += decay;
                mixedPlayable.SetInputWeight(0 ,idelW);
                mixedPlayable.SetInputWeight(1 ,act1W);
            }
        }

        bool CheckAnimation(string name)
        {
            if (CurIdelAnimation.aniName == name)
                return false;
            return true;
        }

        void DoAct2Act()
        {
            time1 += Time.deltaTime;
            time2 += Time.deltaTime;
            float decay = 1 / (CurAnimation.totalTime - CurAnimation.backTime) * Time.deltaTime;
            act1W -= decay;
            act2W += decay;
            if (act1W <= 0)
            {
                act1W = 1;
                idelW = 0;
                isPlay2 = false;
                _playState = PlayState.Act;
                isPlayCount--;
                mPlayableGraph.Disconnect(mixedPlayable,1);
                mPlayableGraph.Disconnect(mixedPlayable,2);
                mPlayableGraph.Connect(ToChangeAnimation.animationClip,0,mixedPlayable,1);
                CurAnimation = ToChangeAnimation;
                time1 = time2;
                mixedPlayable.SetInputWeight(1 ,1);
                return;
            }
            mixedPlayable.SetInputWeight(1 ,act1W);
            mixedPlayable.SetInputWeight(2 ,act2W);
        }

        void DoAct2Idel()
        {
            time1 += Time.deltaTime;
            float decay = 1 / (CurAnimation.totalTime - CurAnimation.backTime) * Time.deltaTime;
            act1W -= decay;
            idelW += decay;
            if (act1W <= 0)
            {
                act1W = 0;
                idelW = 1;
                isPlay1 = false;
                _playState = PlayState.Idel;
                isPlayCount--;
                mPlayableGraph.Disconnect(mixedPlayable,1);
                mixedPlayable.SetInputWeight(0 ,1);
                ToChangeAnimation = null;
                return;
            }
            mixedPlayable.SetInputWeight(0 ,idelW);
            mixedPlayable.SetInputWeight(1 ,act1W);
        }

        void UpdateIdle()
        {
            if (_isIdleChangeState)
            {
                _idleChangeWeight1 -= _idleChangeSpeed * Time.deltaTime;
                _idleChangeWeight2 += _idleChangeSpeed * Time.deltaTime;
                if (_idleChangeWeight1 <= 0)
                {
                    _idleChangeWeight1 = 1;
                    _idleChangeWeight2 = 0;
                    mPlayableGraph.Disconnect(idleMixedPlayable,0);
                    mPlayableGraph.Disconnect(idleMixedPlayable,1);
                    mPlayableGraph.Connect(ToChangeIdleAnimation.animationClip, 0 ,idleMixedPlayable, 0);
                    CurIdelAnimation = ToChangeIdleAnimation;
                    ToChangeIdleAnimation = null;
                    _isIdleChangeState = false;
                }
                idleMixedPlayable.SetInputWeight(0 ,_idleChangeWeight1);
                idleMixedPlayable.SetInputWeight(1 ,_idleChangeWeight2);
            }
        }
        
        AnimationModel GetAnimationModel(string name)
        {
            return AniMap[name];
        }

        void StartIdle()
        {
            mixedPlayable.SetInputWeight(0 ,1f);
        }

        public void PlayVictory()
        {
            PlayAnimation("Victory");
            StartCoroutine(Change_Idle("Victory_Idle", 0.3f, 0.3f));
        }
        
        public void PlayLose()
        {
            PlayAnimation("Lose");
            StartCoroutine(Change_Idle("Lose_Idle", 0.3f, 0.3f));
        }

        public void ChangeBodyPosture(bool isLeft)
        {
            if(this.BodyIsLeft == isLeft)
                return;
            if (isLeft)
            {
                PlayAnimation("FIdle_R_to_L");
                ChangeIdle("L_FIdle" ,0 ,0.3f);
            }
            else
            {
                PlayAnimation("FIdle_L_to_R");
                ChangeIdle("R_FIdle" ,0 ,0.3f);
            }
        }

        public void ChangeToFIdle(bool isLeft)
        {
            if (isLeft)
            {
                ChangeIdle("L_FIdle" ,0 ,0.5f);
            }
            else
            {
                ChangeIdle("R_FIdle" ,0 ,0.5f);
            }
        }
        
        public void ChangeToIdle()
        {
            ChangeIdle("Idle" ,0 ,0.5f);
        }
        
        public void ChangeDefaultIdle(float time = 0.5f)
        {
            if (MainGameCtr.instance.bodyIsLeft)
            {
                ChangeIdle("L_FIdle" ,0 ,time);
            }
            else
            {
                ChangeIdle("R_FIdle" ,0 ,time);
            }
        }

        public void ChangeIdle(string name ,float delay ,float blendTIme)
        {
            if (delay <= 0)
            {
                ChangeIdle(name ,blendTIme);
            }
            else
            {
                StartCoroutine(Change_Idle(name ,delay , blendTIme));
            }
        }

        IEnumerator Change_Idle(string name, float delay ,float blendTIme)
        {
            yield return new WaitForSeconds(delay);
            ChangeIdle(name ,blendTIme);
        }

        void ChangeIdle(string name ,float blendTIme)
        {
            AnimationModel animationModel = AniMap[name];
            if(!CheckIdleChange(animationModel))
                return;
            if (blendTIme <= 0)
            {
                idleMixedPlayable.DisconnectInput(0);
                CurIdelAnimation = animationModel;
                try
                {
                    mPlayableGraph.Connect(CurIdelAnimation.animationClip, 0, idleMixedPlayable, 0);
                }
                catch (Exception e)
                {
                    Debug.Log(e);
                    throw;
                }
            }
            else
            {
                _isIdleChangeState = true;
                _idleChangeSpeed = 1 / blendTIme;
                _idleChangeWeight1 = 1;
                _idleChangeWeight2 = 0;
                ToChangeIdleAnimation = animationModel;
                mPlayableGraph.Connect(ToChangeIdleAnimation.animationClip, 0 ,idleMixedPlayable, 1);
            }
            
        }

        bool CheckIdleChange(AnimationModel animationModel)
        {
            if (_isIdleChangeState)
            {
                if(animationModel == ToChangeIdleAnimation)
                    return false;
                idleMixedPlayable.DisconnectInput(0);
                idleMixedPlayable.DisconnectInput(1);
                idleMixedPlayable.ConnectInput(0 ,ToChangeIdleAnimation.animationClip ,0);
                idleMixedPlayable.ConnectInput(0 ,animationModel.animationClip ,1);
                float temp = _idleChangeWeight1;
                _idleChangeWeight1 = _idleChangeWeight2;
                _idleChangeWeight2 = temp;
                return false;
            }
            return true;
        }
        
        void OnDestroy()
        {
            if(mPlayableGraph.IsValid())
                mPlayableGraph.Destroy();
        }*/
    }
}