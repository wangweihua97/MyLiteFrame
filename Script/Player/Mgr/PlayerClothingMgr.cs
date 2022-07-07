using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using OtherMgr;
using Script.Excel.Table;
using Script.Main;
using Script.Mgr;
using Script.Model;
using Script.Tool;
using Tool.Others;
using UnityEngine;

namespace Player
{
    public class PlayerClothingMgr : MonoBehaviour
    {
        [Header("角色眼睛")] public GameObject Eyes;
        [Header("角色身体")] public GameObject Body;
        [Header("角色头部")] public GameObject Head;
        [Header("角色发部")] public GameObject Hair;
        [Header("角色手部")] public GameObject Hand;
        [Header("角色腿部")] public GameObject Leg;
        [Header("角色鞋子")] public GameObject Shoes;
        private Dictionary<string, GameObject> _gos;
        private Dictionary<CharacterPart, string> _clothingIDs;
        private Dictionary<CharacterPart, GameObject> _parts;

        private PlayerDressModel _default;

        [HideInInspector] public bool IsInit = false;
        public PlayerRole PlayerRole;
        private PlayerMgr _playerMgr;

        #region 生命周期

        private void Awake ()
        {
            _gos = new Dictionary<string, GameObject>();
            _clothingIDs = new Dictionary<CharacterPart, string>();
            _parts = new Dictionary<CharacterPart, GameObject>();
            InitParts();
        }

        private void OnEnable()
        {
            IsInit = false;
        }

        public void Init()
        {
            IsInit = true;
            _default = new PlayerDressModel();
            CreatDefaultDressUp(_default);
            switch (PlayerRole)
            {
                case PlayerRole.Player:
                    _playerMgr = PlayerMgr.instance;
                    if (_playerMgr.PlayerDressModel == null)
                    {
                        _playerMgr.PlayerDressModel = new PlayerDressModel();
                        CreatDefaultDressUp(_playerMgr.PlayerDressModel);
                    }
                    if (!CheckDressModel())
                    {
                        _playerMgr.PlayerDressModel = new PlayerDressModel();
                        CreatDefaultDressUp(_playerMgr.PlayerDressModel);
                    }
                    break;
                case PlayerRole.Coach:
                    _playerMgr = PlayerMgr.CoachInstance;
                    if (_playerMgr.PlayerDressModel == null)
                    {
                        _playerMgr.PlayerDressModel = new PlayerDressModel();
                        CreatDefaultCoachDressUp(_playerMgr.PlayerDressModel);
                    }
                    if (!CheckDressModel())
                    {
                        _playerMgr.PlayerDressModel = new PlayerDressModel();
                        CreatDefaultCoachDressUp(_playerMgr.PlayerDressModel);
                    }
                    break;
            }
            RefreshClothing(_playerMgr.PlayerDressModel);
        }
        
        private void OnDestroy()
        {
        }

        #endregion
        
        #region 开放方法
        /// <summary>
        /// 尝试保存当前穿着
        /// </summary>
        /// <returns>保存是否成功</returns>
        public bool TrySave()
        {
            if (!CheckDressModel())
                return false;
            switch (PlayerRole)
            {
                case PlayerRole.Player:
                    NativeStoreTool.Set("PlayerDressModel" ,_playerMgr.PlayerDressModel);
                    break;
                case PlayerRole.Coach:
                    NativeStoreTool.Set("CoachDressModel" ,_playerMgr.PlayerDressModel);
                    break;
            }
            return true;
        }

        /// <summary>
        /// 换装
        /// </summary>
        /// <param name="tdCharacter"> 要换装的数据</param>
        public void ChangeDressUp(TDCharacter tdCharacter)
        {
            Change(_playerMgr.PlayerDressModel, tdCharacter);
        }
        
        /// <summary>
        /// 卸载
        /// </summary>
        /// <param name="tdCharacter"> 要换装的数据</param>
        public void Unload(TDCharacter tdCharacter)
        {
            Change(_playerMgr.PlayerDressModel ,ExcelMgr.TDCharacter.Get(GetPartClothingId(_default ,GetCharacterPart(tdCharacter))));
        }

        /// <summary>
        /// 重置玩家的装扮，重置到上次保存的装扮
        /// </summary>
        public void Reset()
        {
            switch (PlayerRole)
            {
                case PlayerRole.Player:
                    _playerMgr.PlayerDressModel = NativeStoreTool.Get<PlayerDressModel>("PlayerDressModel");
                    break;
                case PlayerRole.Coach:
                    _playerMgr.PlayerDressModel = NativeStoreTool.Get<PlayerDressModel>("CoachDressModel");
                    break;
            }
            RefreshClothing(_playerMgr.PlayerDressModel);
        }

        /// <summary>
        /// 刷新装扮
        /// </summary>
        /// <param name="playerDressModel"> 装扮数据</param>
        public void RefreshClothing(PlayerDressModel playerDressModel)
        {
            Clothing(playerDressModel);
        }
        
        
        /// <summary>
        /// 是否穿过服装
        /// </summary>
        /// <param name="id">服装Id</param>
        /// <returns></returns>
        public bool IsWore(string id)
        {
            foreach (var kvp in _clothingIDs)
            {
                if (kvp.Value.Equals(id))
                    return true;
            }
            return false;
        }
        
        /// <summary>
        /// 得到身体部位上面的服装
        /// </summary>
        /// <param name="characterPart"></param>
        /// <returns></returns>
        public string GetPartClothingId(CharacterPart characterPart)
        {
            return _clothingIDs[characterPart];
        }
        
        public string GetPartClothingId(PlayerDressModel playerDressModel,CharacterPart characterPart)
        {
            switch (characterPart)
            {
                case CharacterPart.Body:
                    return playerDressModel.Body.Id;
                case CharacterPart.Eyes:
                    return playerDressModel.Eyes.Id;
                case CharacterPart.Hair:
                    return playerDressModel.Hair.Id;
                case CharacterPart.Hand:
                    return playerDressModel.Hand.Id;
                case CharacterPart.Head:
                    return playerDressModel.Head.Id;
                case CharacterPart.Leg:
                    return playerDressModel.Leg.Id;
                case CharacterPart.Shoes:
                    return playerDressModel.Shoes.Id;
            }
            return "";
        }

        public string GetCorrespondingId(TDCharacter tdCharacter)
        {
            return GetPartClothingId(GetCharacterPart(tdCharacter));
        }

        public CharacterPart GetCharacterPart(TDCharacter tdCharacter)
        {
            CharacterPart characterPart = default;
            switch (GetDressUpType(tdCharacter))
            {
                case    DressUpType.EyesColor:
                    characterPart = CharacterPart.Eyes;
                    break;
                case    DressUpType.FaceShape:
                    characterPart = CharacterPart.Head;
                    break;
                case    DressUpType.HairShape:
                    characterPart = CharacterPart.Hair;
                    break;
                case    DressUpType.Jacket:
                    characterPart = CharacterPart.Body;
                    break;
                case    DressUpType.Trousers:
                    characterPart = CharacterPart.Leg;
                    break;
                case    DressUpType.Shoe:
                    characterPart = CharacterPart.Shoes;
                    break;
                case   DressUpType.Hand:
                    characterPart = CharacterPart.Hand;
                    break;
            }
            return characterPart;
        }
        
        #endregion

        #region 私有方法
        void InitParts()
        {
            foreach (var t in GetComponentsInChildren<Transform>(true))
            {
                if(t.GetComponent<SkinnedMeshRenderer>())
                    _gos.Add( t.gameObject.name , t.gameObject);
            }
            _parts.Add(CharacterPart.Body ,Body);
            _parts.Add(CharacterPart.Eyes ,Eyes);
            _parts.Add(CharacterPart.Head ,Head);
            _parts.Add(CharacterPart.Hair ,Hair);
            _parts.Add(CharacterPart.Leg ,Leg);
            _parts.Add(CharacterPart.Hand ,Hand);
            _parts.Add(CharacterPart.Shoes ,Shoes);
        }

        void CreatDefaultDressUp(PlayerDressModel playerDressModel)
        {
            foreach (var kvp in ExcelMgr.TDCharacter.GetDictionary())
            {
                if (kvp.Value.defaultch)
                    AddDress(playerDressModel ,GetDressUpType(kvp.Value) ,kvp.Value);
            }
        }
        
        void CreatDefaultCoachDressUp(PlayerDressModel playerDressModel)
        {
            foreach (var kvp in ExcelMgr.TDCharacter.GetDictionary())
            {
                if (kvp.Value.coach)
                    AddDress(playerDressModel ,GetDressUpType(kvp.Value) ,kvp.Value);
            }
        }

        bool CheckDressModel()
        {
            if (CheckDressPart(_playerMgr.PlayerDressModel.Body) ||
                CheckDressPart(_playerMgr.PlayerDressModel.Head) ||
                CheckDressPart(_playerMgr.PlayerDressModel.Hair) ||
                CheckDressPart(_playerMgr.PlayerDressModel.Hand) ||
                CheckDressPart(_playerMgr.PlayerDressModel.Leg) ||
                CheckDressPart(_playerMgr.PlayerDressModel.Eyes) ||
                CheckDressPart(_playerMgr.PlayerDressModel.Shoes))
                return false;
            return true;
        }

        bool CheckDressPart(DressUpData part)
        {
            if (part.Model == null)
                return true;
            if (part.Texture == null || part.Texture.Count <= 0)
                return true;
            if (part.Id == null)
                return true;
            return false;
        }

        DressUpType GetDressUpType(TDCharacter character)
        {
            return (DressUpType)character.type - 1;
        }

        //根据TDCharacter配置表配置任务装扮
        void AddDress(PlayerDressModel playerDressModel, DressUpType dressUpType ,TDCharacter character)
        {
            switch (dressUpType)
            {
                case    DressUpType.SkinColor:
                    playerDressModel.SkinColor.Color = StringTool.ToColor(character.color);
                    playerDressModel.SkinColor.Id  = character.Id;
                    break;
                case    DressUpType.HairColor:
                    playerDressModel.HairColor.Color = StringTool.ToColor(character.color);
                    playerDressModel.HairColor.Metallic = character.metallic;
                    playerDressModel.HairColor.Id  = character.Id;
                    break;
                case    DressUpType.EyesColor:
                    playerDressModel.Eyes.Model = character.model;
                    playerDressModel.Eyes.Texture = character.texturing;
                    playerDressModel.Eyes.Id  = character.Id;
                    break;
                case    DressUpType.FaceShape:
                    playerDressModel.Head.Model = character.model;
                    playerDressModel.Head.Texture = character.texturing;
                    playerDressModel.Head.Id  = character.Id;
                    break;
                case    DressUpType.HairShape:
                    playerDressModel.Hair.Model = character.model;
                    playerDressModel.Hair.Texture = character.texturing;
                    playerDressModel.Hair.Id  = character.Id;
                    break;
                case    DressUpType.Jacket:
                    playerDressModel.Body.Model = character.model;
                    playerDressModel.Body.Texture = character.texturing;
                    playerDressModel.Body.Id  = character.Id;
                    break;
                case    DressUpType.Trousers:
                    playerDressModel.Leg.Model = character.model;
                    playerDressModel.Leg.Texture = character.texturing;
                    playerDressModel.Leg.Id  = character.Id;
                    break;
                case    DressUpType.Shoe:
                    playerDressModel.Shoes.Model = character.model;
                    playerDressModel.Shoes.Texture = character.texturing;
                    playerDressModel.Shoes.Id  = character.Id;
                    break;
                case   DressUpType.Hand:
                    playerDressModel.Hand.Model = character.model;
                    playerDressModel.Hand.Texture = character.texturing;
                    playerDressModel.Hand.Id  = character.Id;
                    break;
            }
        }

        void Change(PlayerDressModel playerDressModel,TDCharacter character)
        {
            DressUpType dressUpType = GetDressUpType(character);
            switch (dressUpType)
            {
                case    DressUpType.SkinColor:
                    playerDressModel.SkinColor.Color = StringTool.ToColor(character.color);
                    playerDressModel.SkinColor.Id  = character.Id;
                    RefreshClothing(playerDressModel);
                    break;
                case    DressUpType.HairColor:
                    playerDressModel.HairColor.Color = StringTool.ToColor(character.color);
                    playerDressModel.HairColor.Metallic = character.metallic;
                    playerDressModel.HairColor.Id  = character.Id;
                    ClothingPart(playerDressModel, playerDressModel.Hair, CharacterPart.Hair);
                    ClothingPart(playerDressModel, playerDressModel.Head, CharacterPart.Head);
                    break;
                case    DressUpType.EyesColor:
                    playerDressModel.Eyes.Model = character.model;
                    playerDressModel.Eyes.Texture = character.texturing;
                    playerDressModel.Eyes.Id  = character.Id;
                    ClothingPart(playerDressModel, playerDressModel.Eyes, CharacterPart.Eyes);
                    break;
                case    DressUpType.FaceShape:
                    playerDressModel.Head.Model = character.model;
                    playerDressModel.Head.Texture = character.texturing;
                    playerDressModel.Head.Id  = character.Id;
                    ClothingPart(playerDressModel, playerDressModel.Head, CharacterPart.Head);
                    break;
                case    DressUpType.HairShape:
                    playerDressModel.Hair.Model = character.model;
                    playerDressModel.Hair.Texture = character.texturing;
                    playerDressModel.Hair.Id = character.Id;
                    ClothingPart(playerDressModel, playerDressModel.Hair, CharacterPart.Hair);
                    break;
                case    DressUpType.Jacket:
                    playerDressModel.Body.Model = character.model;
                    playerDressModel.Body.Texture = character.texturing;
                    playerDressModel.Body.Id = character.Id;
                    ClothingPart(playerDressModel, playerDressModel.Body, CharacterPart.Body);
                    break;
                case    DressUpType.Trousers:
                    playerDressModel.Leg.Model = character.model;
                    playerDressModel.Leg.Texture = character.texturing;
                    playerDressModel.Leg.Id = character.Id;
                    ClothingPart(playerDressModel, playerDressModel.Leg, CharacterPart.Leg);
                    break;
                case    DressUpType.Shoe:
                    playerDressModel.Shoes.Model = character.model;
                    playerDressModel.Shoes.Texture = character.texturing;
                    playerDressModel.Shoes.Id  = character.Id;
                    ClothingPart(playerDressModel, playerDressModel.Shoes, CharacterPart.Shoes);
                    break;
                case   DressUpType.Hand:
                    playerDressModel.Hand.Model = character.model;
                    playerDressModel.Hand.Texture = character.texturing;
                    playerDressModel.Hand.Id = character.Id;
                    ClothingPart(playerDressModel, playerDressModel.Hand, CharacterPart.Hand);
                    break;
            }
        }

        void Clothing(PlayerDressModel playerDressModel)
        {
            ClothingPart(playerDressModel, playerDressModel.Hair, CharacterPart.Hair);
            ClothingPart(playerDressModel, playerDressModel.Eyes, CharacterPart.Eyes);
            ClothingPart(playerDressModel, playerDressModel.Body, CharacterPart.Body);
            ClothingPart(playerDressModel, playerDressModel.Shoes, CharacterPart.Shoes);
            ClothingPart(playerDressModel, playerDressModel.Hand, CharacterPart.Hand);
            ClothingPart(playerDressModel, playerDressModel.Head, CharacterPart.Head);
            ClothingPart(playerDressModel, playerDressModel.Leg, CharacterPart.Leg);
        }

        void ClothingPart(PlayerDressModel playerDressModel ,DressUpData dressUpData, CharacterPart part)
        {
            if (dressUpData.Model == Const.NULL_STRING)
                return;
            GameObject go = _parts[part];
            _clothingIDs[part] = dressUpData.Id;
            if(go!=null)
                go.SetActive(false);
            go = _gos[dressUpData.Model];
            _parts[part] = go;
            go.SetActive(true);

            SetMaterials(playerDressModel, go, dressUpData);
        }

        void SetMaterials(PlayerDressModel playerDressModel ,GameObject go ,DressUpData part)
        {
            Material[] materials = new Material[part.Texture.Count];
            int i = 0;
            foreach (var str in part.Texture)
            {
                string[] strings = str.Split(':');
                Material sourceMat = Resources
                    .Load<Material>("Art_Resources/Caracter/Player_Formal/Material/Player_F/" + strings[0]);
                Shader shader = sourceMat.shader;
                Material material = new Material(shader);
                material.CopyPropertiesFromMaterial(sourceMat);
                if (strings.Length >= 2)
                {
                    DressColor dressColor = GetDressColor(playerDressModel, Int32.Parse(strings[1]));
                    material.color = dressColor.Color;
                    if(dressColor.Metallic != Const.NULL_FLOAT)
                        material.SetFloat("_Metallic" ,dressColor.Metallic);
                }
                materials[i] = material;
                i++;
            }
            SkinnedMeshRenderer skinnedMeshRenderer = go.GetComponent<SkinnedMeshRenderer>();
            skinnedMeshRenderer.materials = materials;
        }

        DressColor GetDressColor(PlayerDressModel playerDressModel ,int type)
        {
            switch (type)
            {
                case 1:
                    return playerDressModel.SkinColor;
                    break;
                default:
                    return playerDressModel.HairColor;
            }
        }
        
        /// <summary>
        /// 合并蒙皮网格，刷新骨骼,合并后的网格会使用同一个Material
        /// 提高帧率，需要设置模型和材质Read/Write enabled
        /// 合并后将不能在换装
        /// </summary>
        private void Combine()
        {
            List<CombineInstance> combineInstances = new List<CombineInstance>();
            List<Transform> boneList = new List<Transform>();
            Transform[] transforms = transform.GetComponentsInChildren<Transform>();
            List<Texture2D> textures = new List<Texture2D>();

            int width = 0;
            int height = 0;

            int uvCount = 0;

            List<Vector2[]> uvList = new List<Vector2[]>();

            // 遍历所有蒙皮网格渲染器，以计算出所有需要合并的网格、UV、骨骼的信息
            foreach (SkinnedMeshRenderer smr in transform.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                for (int sub = 0; sub < smr.sharedMesh.subMeshCount; sub++)
                {
                    CombineInstance ci = new CombineInstance();
                    ci.mesh = smr.sharedMesh;
                    ci.subMeshIndex = sub;
                    combineInstances.Add(ci);
                }

                uvList.Add(smr.sharedMesh.uv);
                uvCount += smr.sharedMesh.uv.Length;

                if (smr.material.mainTexture != null)
                {
                    textures.Add(smr.GetComponent<Renderer>().material.mainTexture as Texture2D);
                    width += smr.GetComponent<Renderer>().material.mainTexture.width;
                    height += smr.GetComponent<Renderer>().material.mainTexture.height;
                }

                foreach (Transform bone in smr.bones)
                {
                    foreach (Transform item in transforms)
                    {
                        if (item.name != bone.name) continue;
                        boneList.Add(item);
                        break;
                    }
                }
            }

            // 获取并配置角色所有的SkinnedMeshRenderer
            SkinnedMeshRenderer tempRenderer = transform.gameObject.GetComponent<SkinnedMeshRenderer>();
            if (!tempRenderer)
            {
                tempRenderer = transform.gameObject.AddComponent<SkinnedMeshRenderer>();
            }

            tempRenderer.sharedMesh = new Mesh();

            // 合并网格，刷新骨骼，附加材质
            tempRenderer.sharedMesh.CombineMeshes(combineInstances.ToArray(), true, false);
            tempRenderer.bones = boneList.ToArray();
            tempRenderer.material = null;

            Texture2D skinnedMeshAtlas = new Texture2D(get2Pow(width), get2Pow(height));
            Rect[] packingResult = skinnedMeshAtlas.PackTextures(textures.ToArray(), 0);
            Vector2[] atlasUVs = new Vector2[uvCount];

            // 因为将贴图都整合到了一张图片上，所以需要重新计算UV
            int j = 0;
            for (int i = 0; i < uvList.Count; i++)
            {
                foreach (Vector2 uv in uvList[i])
                {
                    atlasUVs[j].x = Mathf.Lerp(packingResult[i].xMin, packingResult[i].xMax, uv.x);
                    atlasUVs[j].y = Mathf.Lerp(packingResult[i].yMin, packingResult[i].yMax, uv.y);
                    j++;
                }
            }

            // 设置贴图和UV
            tempRenderer.material.mainTexture = skinnedMeshAtlas;
            tempRenderer.sharedMesh.uv = atlasUVs;

            // 销毁所有部件
            foreach (var goTemp in _gos)
            {
                if (goTemp.Value.gameObject)
                {
                    Destroy(goTemp.Value.gameObject);
                }
            }
        }
        
        private int get2Pow(int into)
        {
            int outo = 1;
            for (int i = 0; i < 10; i++)
            {
                outo *= 2;
                if (outo > into)
                {
                    break;
                }
            }

            return outo;
        }
        #endregion
    }
}