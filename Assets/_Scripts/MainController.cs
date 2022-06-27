using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class MainController : MonoBehaviour
{
    [SerializeField] private int _width = 4;
    [SerializeField] private int _height = 4;
    [SerializeField] private float _travelTime;
    [SerializeField] private int _winCondition = 2048;
    public GameObject toInstantiate;
    
    [Space(25)]
    [SerializeField] private Node _nodePrefab;
    [SerializeField] private SpriteRenderer _boardPrefab;
    [SerializeField] private Block _blockPrefab;
    [SerializeField] private List<BlockType> _blockTypes;

    private List<Node> _nodes; 
    private List<Block> _blocks;
    private GameState _state;
    private int _round;

    private Transform Board;
    private Transform cameraTransform;

    BlockType GetBlockTypeByValue(int value) => _blockTypes.First(t => t.value == value);
    
    void Start()
    {
        cameraTransform = Camera.main.transform;
        ChangeState(GameState.GenerateLevel); 
        /*InputEventsManager.instance.SwipeRightEvent += ShiftRight();
        InputEventsManager.instance.SwipeLeftEvent += MoveLeft;
        InputEventsManager.instance.SwipeUpEvent += MoveForward;
        InputEventsManager.instance.SwipeDownEvent += MoveBackward;*/
    }

    public void ChangeState(GameState newState)
    {
        _state = newState;

        switch (newState)
        {
            case GameState.GenerateLevel:
                GenerateGrid();
                break;
            case GameState.SpawningBlock:
                SpawnBlocks(_round++ == 0 ? 2 : 1);
                break;
            case GameState.WaitingInput:
                break;
            case GameState.Moving:
                break;
            case GameState.Win:
                break;
            case GameState.Lose:
                break;
        }
    }

    private void Update()
    {
        if (_state != GameState.WaitingInput) return;
        if (Input.GetMouseButtonDown(1)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        if(Input.GetKey(KeyCode.LeftArrow)) Shift(Vector2.left);
        if(Input.GetKey(KeyCode.RightArrow)) Shift(Vector2.right);
        if(Input.GetKey(KeyCode.DownArrow)) Shift(Vector2.down);
        if(Input.GetKey(KeyCode.UpArrow)) Shift(Vector2.up);
    }

    void GenerateGrid()
    {
        _round = 0;
        _nodes = new List<Node>();
        _blocks = new List<Block>();
        var center = new Vector2((float) _width / 2 - 0.5f, (float) _height / 2 - 0.5f);
        var board = Instantiate(_boardPrefab, center, Quaternion.identity);
        //board.transform.eulerAngles = cameraTransform.eulerAngles;
        board.size = new Vector2(_width, _height);
        Board = board.transform;
        
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                var node = Instantiate(_nodePrefab, new Vector2(x, y), Quaternion.identity);
                _nodes.Add(node);
                //node.transform.eulerAngles = cameraTransform.eulerAngles;
            }
        }
        
        
        cameraTransform.position = new Vector3(center.x, center.y - 2.5f, -11.5f);
        
        ChangeState(GameState.SpawningBlock);
    }

    void SpawnBlocks(int amount)
    {
        var freeNodes = _nodes.Where(n => n.OccupiedBlock == null).OrderBy(b => Random.value).ToList();

        foreach (var node in freeNodes.Take(amount))
        {
            SpawnBlock(node, Random.value > 0.8f ? 1 : 0);
        }
        if (freeNodes.Count() == 1)
        {
            ChangeState(GameState.Lose);
            return;
        }
        
        ChangeState(_blocks.Any(b=>b.Value == _winCondition) ? GameState.Win : GameState.WaitingInput);
    }

    void SpawnBlock(Node node, int value)
    {
        var block = Instantiate(_blockPrefab, node.pos, Quaternion.identity);
        //block.transform.parent = Board;
        //block.transform.eulerAngles = cameraTransform.eulerAngles;
        block.Init(GetBlockTypeByValue(value));
        block.SetBlock(node);
        _blocks.Add(block);
    }
    void Shift(Vector2 dir)
    {
        ChangeState(GameState.Moving);
        var orderedBlocks = _blocks.OrderBy(b => b.pos.x).ThenBy(b => b.pos.y);
        if (dir == Vector2.right || dir == Vector2.up) orderedBlocks.Reverse();

        foreach (var block in orderedBlocks)
        {
            var next = block.Node;
            do
            {
                block.SetBlock(next);
                var possibleNode = GetNodeAtPositin(next.pos + dir);
                if (possibleNode != null)
                {
                    if (possibleNode.OccupiedBlock != null && possibleNode.OccupiedBlock.CanMerge(block.Value))
                    {
                        /*possibleNode.OccupiedBlock.gameObject.SetActive(false);
                        block.gameObject.SetActive(false);*/
                        block.MergeBlock(possibleNode.OccupiedBlock);
                    }
                    else if (possibleNode.OccupiedBlock == null) next = possibleNode;
                }

            } while (next != block.Node);
        }

        var sequence = DOTween.Sequence();
        foreach (var block in orderedBlocks)
        {
            var movePoint = block.MergingBlock != null ? block.MergingBlock.Node.pos : block.Node.pos;
            sequence.Insert(0, block.transform.DOMove(movePoint, _travelTime));
        }
        sequence.OnComplete(()=>{
            foreach (var block in orderedBlocks.Where(b =>b.MergingBlock != null))
            {
                MergeBlocks(block.MergingBlock, block);
            }
            
            ChangeState(GameState.SpawningBlock);
        });
    }

    void MergeBlocks(Block baseBlock, Block mergingBlock)
    {
        //SpawnBlock(baseBlock.Node, baseBlock.Value * 2);
        Vector3 pos = new Vector3(mergingBlock.Node.pos.x, mergingBlock.Node.pos.y, -1);
        ItemHolder.instance.SpawnItems(mergingBlock.Value, pos);
        RemoveBlock(baseBlock);
        RemoveBlock(mergingBlock);
    }

    void RemoveBlock(Block block)
    {
        _blocks.Remove(block);
        Destroy(block.gameObject);
    }
    Node GetNodeAtPositin(Vector2 pos)
    {
        return _nodes.FirstOrDefault(n => n.pos == pos);
    }
    
    [Serializable]
    public struct  BlockType
    {
        public int value;
        public Color color;
        public Sprite icon;
    }
    
    public enum GameState
    {
        GenerateLevel,
        SpawningBlock,
        WaitingInput,
        Moving,
        Win,
        Lose
    }
}
