using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PathfindingGridDebugObject : GridDebugObject
{
    [SerializeField] TextMeshPro gCostText;
    [SerializeField] TextMeshPro hCostText;
    [SerializeField] TextMeshPro fCostText;
    [SerializeField] SpriteRenderer isWalkableSprite;

    PathNode pathNode;

    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);

        pathNode = (PathNode)gridObject;        
    }

    protected override void Update()
    {
        base.Update();
        gCostText.text = pathNode.GetGCost().ToString();
        hCostText.text = pathNode.GetHCost().ToString();
        fCostText.text = pathNode.GetFCost().ToString();
        isWalkableSprite.color = pathNode.IsWalkable() ? Color.green : Color.red;
    }
}
