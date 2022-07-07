using System.Collections.Generic;
using System.Linq;
using OtherMgr;
using Player;
using Script.Model;
using Tool.Others;
using UnityEngine;

namespace Script.Scene.Game
{
    public class SceneBlockHelper
    {
        public int BlockCount;
        public float[] BlockLocations;
        public int CurStarBlockIndex;

        private DoubleLinkedList<SceneBlock> _list;
        private Dictionary<string, List<SceneBlock>> _blocks;
        private Dictionary<string, int> _blockMaxNumber;
        private Vector2 _nextBlockXY;

        public SceneBlockHelper()
        {
            _blocks = new Dictionary<string, List<SceneBlock>>();
            _blockMaxNumber = new Dictionary<string, int>();
            BlockCount = GameVariable.SceneBlock.Length;
            BlockLocations = new float[BlockCount];
            _list = new DoubleLinkedList<SceneBlock>();
        }
        
        public void InitBlocks()
        {
            Queue<string> queue = new Queue<string>();
            Dictionary<string, int> temp = new Dictionary<string, int>();
            foreach (var sceneBlock in BattleMgr.instance.ListSceneBlocks)
            {
                _blocks[sceneBlock.Name] = new List<SceneBlock>() {sceneBlock};
                _blockMaxNumber[sceneBlock.Name] = 1;
            }
            
            int i = 0;
            float extent = 0;
            foreach (var block in GameVariable.SceneBlock)
            {
                InsetQueue(queue, block, temp);
                SetBlockLocations(i, extent);
                extent = _blocks[block].First().Extent;
                i++;
            }
            InitList();
        }

        public void DoUpdata()
        {
            if(!PlayerMgr.instance.IsMove)
                return;

            bool isNotMoreBlock = CurStarBlockIndex + GameVariable.MaxBlockNumber >= BlockCount;
            if(isNotMoreBlock)
                return;
            
            //当玩家跑到第二个地块的1/3距离时，把第一个地块隐藏掉，同时最最后添加一个地块
            float checkPosition = GetBlock(CurStarBlockIndex + 1).Extent * 0.33f + BlockLocations[CurStarBlockIndex + 1];
            if (PlayerMgr.instance.GetTransform().position.x > checkPosition)
            {
                AddBlock();
            }
        }
        
        public SceneBlock GetFirstBlock()
        {
            return _list.Head.Value;
        }
        
        public Vector3 GetBattleBlockPlayerFightPosition(int battleIndex)
        {
            if (GameVariable.GameMode == GameMode.TrainMode)
                return GetFirstBlock().BattleShift.PlayerPoint.position;
            return GetBlockPlayerPosition(GetBattleBlockIndex(battleIndex));
        }

        public Vector3 GetBattleBlockMonsterPosition(int battleIndex)
        {
            if (GameVariable.GameMode == GameMode.TrainMode)
                return GetFirstBlock().BattleShift.MonsterBrithPoint.position;
            return GetBlockMonsterPosition(GetBattleBlockIndex(battleIndex));
        }
        
        public Quaternion GetBattleBlockMonsterRotation(int battleIndex)
        {
            if (GameVariable.GameMode == GameMode.TrainMode)
                return GetFirstBlock().BattleShift.MonsterBrithPoint.rotation;
            return GetBlockMonsterRotation(GetBattleBlockIndex(battleIndex));
        }
        
        public SceneBlock GetBlock(int index)
        {
            if (index < CurStarBlockIndex || index >= CurStarBlockIndex + BlockCount)
            {
                Debug.LogError("地块已经消失或者没有刷出");
                return default;
            }
            return _list[index - CurStarBlockIndex];
        }

        SceneBlock GetNotUsedBlock(string name)
        {
            List<SceneBlock> blocks = _blocks[name];
            foreach (var block in blocks)
            {
                if (!block.IsUsed)
                    return block;
            }

            Debug.LogError("没有可以使用的地块，地块名" + name);
            return default;
        }

        void AddBlock()
        {
             int addIndex = CurStarBlockIndex + _list.Size;
             string blockName = GameVariable.SceneBlock[addIndex];
             if (_list.Size >= GameVariable.MaxBlockNumber)
             {
                 SceneBlock head = _list.PopHead();
                 head.Shelve();
                 CurStarBlockIndex++;
             }
             SceneBlock block = GetNotUsedBlock(blockName);
             block.SetIndex(addIndex);
             block.SetPosition(BlockLocations[addIndex]);
            _list.PushBack(block);
        }

        int GetBattleBlockIndex(int battleIndex)
        {
            return GameVariable.Born[battleIndex] - 1;
        }


        Vector3 GetBlockPlayerPosition(int blockIndex)
        {
            string blockName = GameVariable.SceneBlock[blockIndex];
            Vector3 position = _blocks[blockName].First().GetPlayerLocalPosition();
            float x = BlockLocations[blockIndex];
            
            return position + new Vector3(x ,0 ,0);
        }
        
        Vector3 GetBlockMonsterPosition(int blockIndex)
        {
            string blockName = GameVariable.SceneBlock[blockIndex];
            Vector3 position = _blocks[blockName].First().GetMonsterLocalPosition();
            float x = BlockLocations[blockIndex];
            
            return position + new Vector3(x ,0 ,0);
        }
        
        Quaternion GetBlockMonsterRotation(int blockIndex)
        {
            string blockName = GameVariable.SceneBlock[blockIndex];
            return _blocks[blockName].First().GetMonsterRotation();
        }
        
        void InitList()
        {
            CurStarBlockIndex = 0;
            for (int i = 0; i < BlockCount && i < GameVariable.MaxBlockNumber; i++)
            {
                AddBlock();
            }
        }

        void SetBlockLocations(int index , float extent)
        {
            if (index == 0)
                BlockLocations[index] = 0;
            else
                BlockLocations[index] = BlockLocations[index - 1] + extent;
        }

        void InsetQueue(Queue<string> queue ,string value ,Dictionary<string, int> curBlockNumber)
        {
            if (!curBlockNumber.ContainsKey(value))
                curBlockNumber[value] = 1;
            else
                curBlockNumber[value]++;
            if (queue.Count >= GameVariable.MaxBlockNumber)
            {
                string dequeueString = queue.Dequeue();
                curBlockNumber[dequeueString]--;
            }
            queue.Enqueue(value);

            if (curBlockNumber[value] > _blockMaxNumber[value])
            {
                _blockMaxNumber[value]++;
                GameObject newGo = GameObject.Instantiate(_blocks[value].First().gameObject);
                SceneBlock sceneBlock = newGo.GetComponent<SceneBlock>();
                sceneBlock.Shelve();
                _blocks[value].Add(sceneBlock);
            }
        }
        
    }
}