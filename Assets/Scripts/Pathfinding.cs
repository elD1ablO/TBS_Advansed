using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance { get; private set; }

    [SerializeField] LayerMask obstaclesLayerMask;
    [SerializeField] Transform gridDebugPrefab;

    const int MOVE_STRAINGT_COST = 10;
    const int MOVE_DIAGONAL_COST = 14;

    int width;
    int height;
    float cellSize;
    GridSystem<PathNode> gridSystem;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    public void Setup(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;

        gridSystem = new GridSystem<PathNode>(width, height, cellSize, (GridSystem<PathNode> g, GridPosition gridPosition) => new PathNode(gridPosition));
        //gridSystem.CreateDebugObjects(gridDebugPrefab);

        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                float raycastOffsetDistance = 5f;
                if (Physics.Raycast(worldPosition + Vector3.down * raycastOffsetDistance, Vector3.up, raycastOffsetDistance * 2, obstaclesLayerMask))
                {
                    GetNode(x, z).SetIsWalkable(false);
                }
            }
        } 
    }

    public List<GridPosition> FindPath(GridPosition startPosition, GridPosition endPosition, out int pathLength)
    {
        List<PathNode> openList = new();
        List<PathNode> closedList = new();

        PathNode startNode = gridSystem.GetGridObject(startPosition);
        PathNode endNode = gridSystem.GetGridObject(endPosition);
        
        openList.Add(startNode);        

        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < gridSystem.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);

                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetPreviousPathNode();
            }
        }

        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startPosition, endPosition));
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);

            if (currentNode == endNode)
            {
                pathLength = endNode.GetGCost();
                return CalculatePath(endNode);
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;

                if (!neighbourNode.IsWalkable())
                {
                    closedList.Add(neighbourNode);
                    continue;
                }
                
                int tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());

                if (tentativeGCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetPreviousNode(currentNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endPosition));
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode)) 
                    { 
                        openList.Add(neighbourNode); 
                    }
                }

            }
        }
        // No path found
        Debug.Log("You shall not pass!");
        pathLength = 0;
        return null;
    }

    
    
    public int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        GridPosition gridPositionDistance = gridPositionA - gridPositionB;

        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + MOVE_STRAINGT_COST * remaining;
    }
        
    
    PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostPathNode = pathNodeList[0];
        for (int i = 0; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost())
            {
                lowestFCostPathNode = pathNodeList[i];
            }
        }
        
        return lowestFCostPathNode;
    }

    PathNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }

    List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new();

        GridPosition gridPosition = currentNode.GetGridPosition();
                
        if (gridPosition.x - 1 >= 0)
        {
            //Left
            neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z));

            if (gridPosition.z - 1 >= 0)
            {
                //LeftDown
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1));
            }
            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                //LeftUp
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1));
            }
        }
        if (gridPosition.x + 1 < gridSystem.GetWidth())
        {
            //Right
            neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z));

            if (gridPosition.z - 1 >= 0)
            {
                //RightDown
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1));
            }
            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                //RightUp
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1));
            }
        }
        if (gridPosition.z - 1 >= 0)
        {
            //Down
            neighbourList.Add(GetNode(gridPosition.x, gridPosition.z - 1));
        }
        if (gridPosition.z + 1 < gridSystem.GetHeight())
        {
            //Up
            neighbourList.Add(GetNode(gridPosition.x, gridPosition.z + 1));
        }
        
        return neighbourList;
       
    }

    List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new();
        pathNodeList.Add(endNode);
        PathNode currentNode = endNode;

        while (currentNode.GetPreviousNode() != null)
        {
            pathNodeList.Add(currentNode.GetPreviousNode());
            currentNode = currentNode.GetPreviousNode();
        }

        pathNodeList.Reverse();

        List<GridPosition> gridPositionList = new();
        foreach (PathNode pathNode in pathNodeList)
        {
            gridPositionList.Add(pathNode.GetGridPosition());
        }
        
        return gridPositionList;
    }

    public bool IsWalkableGridPosition (GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).IsWalkable();
    }

    public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        return FindPath(startGridPosition, endGridPosition, out int pathLength) != null;
    }

    public int GetPathLength(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        FindPath(startGridPosition, endGridPosition, out int pathLength);
        return pathLength;
    }
}

