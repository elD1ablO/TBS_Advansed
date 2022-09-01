using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode 
{
    GridPosition gridPosition;

    int gCost;
    int hCost;
    int fCost;

    PathNode previousPathNode;

    public PathNode (GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return gridPosition.ToString();
    }

    public int GetGCost()
    {
        return gCost;
    }

    public int GetHCost()
    {
        return hCost;
    }
    public int GetFCost()
    {
        return fCost;
    }

    public void SetGCost(int gCost)
    {
        this.gCost = gCost;
    }
    public void SetHCost(int hCost)
    {
        this.hCost = hCost;
    }
    public void CalculateFCost()
    {
        fCost = gCost + fCost;
    }

    public void ResetPreviousPathNode()
    {
        previousPathNode = null;
    }

    public GridPosition GetGridPosition()
    {
        return gridPosition;
    }

    public void SetPreviousNode(PathNode pathNode)
    {
        previousPathNode = pathNode;
    }
    public PathNode GetPreviousNode()
    {
        return previousPathNode;
    }
}
