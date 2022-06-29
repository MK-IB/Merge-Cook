using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int Value;
    public Node Node;
    public Block MergingBlock;
    public bool Merging;
    
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private SpriteRenderer _icon;
    [SerializeField] private TextMeshPro _text;
    
    public Vector2 pos => transform.position;

    public void Init(PlayController.BlockType type)
    {
        Value = type.value;
        _renderer.color = type.color;
        _text.text = type.value.ToString();
        _icon.sprite = type.icon;
    }

    public void SetBlock(Node node)
    {
        if (Node != null) Node.OccupiedBlock = null;
        Node = node;
        Node.OccupiedBlock = this;
    }

    public void MergeBlock(Block blockToMergeWith)
    {
        //set the block we are merging with
        MergingBlock = blockToMergeWith;
        //allow other blocks to use it
        Node.OccupiedBlock = null;
        
        //set the base block as Merging, so it wont be used twice
        blockToMergeWith.Merging = true;
    }

    public bool CanMerge(int value) => value == Value && !Merging && MergingBlock == null;
}
