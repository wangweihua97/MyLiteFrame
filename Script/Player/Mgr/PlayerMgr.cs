using UnityEngine;

namespace Player
{
    public class PlayerMgr : MonoBehaviour
    {
        /*public static PlayerMgr instance;
        public static PlayerMgr CoachInstance;
        public PlayerRole PlayerRole = PlayerRole.Player;
        [HideInInspector]
        public PlayerAnimationMgr playerAnimationMgr;
        [HideInInspector]
        public PlayerClothingMgr PlayerClothingMgr;
        [HideInInspector]
        public PlayerDressModel PlayerDressModel;

        public bool IsAlive
        {
            get;
            private set;
        }
        public Transform Dummy_Hit;
        public GameObject attackEffect;
        public GameObject hurtEffect;
        public GameObject defenseEffect;
        public GameObject defenseBuffer;

        public bool IsMove
        {
            get { return _playerMove.IsMove; }
        }

        public PlayerEnergy PlayerEnergy;
        public PlayerHp PlayerHp;

        private PlayerMove _playerMove;
        private int _defenseBuffTime;

        private void Awake()
        {
            if(instance == null)
                instance = this;
            _playerMove = new PlayerMove(this);
            _defenseBuffTime = 0;
            playerAnimationMgr = gameObject.GetComponent<PlayerAnimationMgr>();
            PlayerClothingMgr = gameObject.GetComponent<PlayerClothingMgr>();
        }

        private void Start()
        {
            IsAlive = true;
            if (!PlayerClothingMgr.IsInit)
            {
                switch (PlayerRole)
                {
                    case PlayerRole.Player:
                        PlayerDressModel = NativeStoreTool.Get<PlayerDressModel>("PlayerDressModel");
                        PlayerClothingMgr.PlayerRole = PlayerRole.Player;
                        break;
                    case PlayerRole.Coach:
                        PlayerDressModel = null;
                        PlayerClothingMgr.PlayerRole = PlayerRole.Coach;
                        break;
                }
                PlayerClothingMgr.Init();
            }
        }

        public void SetMoveTransform(Transform moveTransform)
        {
            _playerMove.MoveTransform = moveTransform;
        }
        
        public void SetPosition(Vector3 position)
        {
            _playerMove.SetPosition(position);
        }
        
        public Transform GetTransform()
        {
            return _playerMove.MoveTransform;
        }
        
        public void SetRotation(Quaternion rotation)
        {
            _playerMove.SetRotation(rotation);
        }

        public async void DoMove(Vector3 position, float time ,Action callBack)
        {
            await _playerMove.DoMove(position ,time ,callBack);
        }

        public void InitPlayer()
        {
            PlayerEnergy = new PlayerEnergy();
            PlayerHp = new PlayerHp();
            PlayerDressModel = NativeStoreTool.Get<PlayerDressModel>("PlayerDressModel");
            PlayerClothingMgr.PlayerRole = PlayerRole.Player;
            PlayerClothingMgr.Init();
        }

        public void InitCoach()
        {
            PlayerDressModel = null;
            PlayerClothingMgr.PlayerRole = PlayerRole.Coach;
            PlayerClothingMgr.Init();
            //transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);
        }

        public void PlayAttackEffect()
        {
            attackEffect.SetActive(false);
            attackEffect.SetActive(true);
        }
        
        public void PlayHurtEffect()
        {
            hurtEffect.SetActive(false);
            hurtEffect.SetActive(true);
        }

        public void GetHurt()
        {
            EnemySkillInfo enemySkill = EnemyMgr.instance.GetAttackSkillInfo();
            if (_defenseBuffTime <= 0)
            {
                PlayerHp.AddHp(-1 * enemySkill.Prop1);
                PlayHurtEffect();
            }
            else
            {
                _defenseBuffTime--;
                if(_defenseBuffTime <= 0)
                    defenseBuffer.SetActive(false);
                PlayDefenseEffect();
            }
        }

        public void GetDefenseBuff()
        {
            if(!EnemyMgr.instance.IsLived)
                return;
            _defenseBuffTime++;
            defenseBuffer.SetActive(true);
        }
        public void PlayDefenseEffect()
        {
            defenseEffect.SetActive(false);
            defenseEffect.SetActive(true);
        }

        public void HpZero()
        {
            IsAlive = false;
        }
        

        private void OnDestroy()
        {
            if (instance == this)
                instance = null;
            if (CoachInstance == this)
                CoachInstance = null;
            if(PlayerEnergy != null)
                PlayerEnergy.Clear();
        }

        private void OnDisable()
        {
            /*if(CoachInstance == this)
                transform.localScale = new Vector3(-transform.localScale.x,transform.localScale.y,transform.localScale.z);*/
        /*}*/
    }
}