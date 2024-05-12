using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public List<int[]> positionsAround = new();
    public int[] position;
    public float cost;

    // Constructor
    public Node(int[] position, float cost, bool token = true)
    {
        // Token to instantiate and add to closed/open list
        if (token) {
            GameManager.Instance.InstantiateToken(GameManager.Instance.token3, position);
            GameManager.Instance.closedList.Add(this);
            GameManager.Instance.totalCost += cost;
        }
        else {
            GameManager.Instance.InstantiateToken(GameManager.Instance.token1, position);
            GameManager.Instance.openList.Add(this);
        }

        // Set position and cost
        this.position = position;
        this.cost = cost;

        // Is this node at the exit?
        if (GameManager.Instance.CheckWin(position)) return;

        // Set the positions around the object
        positionsAround.Add(new int[] { position[0] + 1, position[1] });
        positionsAround.Add(new int[] { position[0] - 1, position[1] });
        positionsAround.Add(new int[] { position[0], position[1] + 1 });
        positionsAround.Add(new int[] { position[0], position[1] - 1 });
    }

    // Searches around itself for the new closest point
    public void Search()
    {
        float nearestCost = cost;
        int[] nearestAsMatrix = position;
        
        // Find nearest children around object
        foreach (int[] pos in positionsAround)
        {
            if (GameManager.Instance.IsInOpenList(pos) || GameManager.Instance.IsOutOfBounds(pos)) continue; 

            float newCost = Calculator.CheckDistanceToObj(pos, GameManager.Instance.objectivePos);
            new Node(pos, newCost, false); // Adds a node to the board and the open list

            // Set a new nearest?
            if (GameManager.Instance.closedList[^1].cost + newCost < GameManager.Instance.closedList[^1].cost + nearestCost)
            {
                nearestCost = newCost;
                nearestAsMatrix = pos;
            }
        }

        // Create a new node and search again! (if you didnt win already)
        if (GameManager.Instance.CheckWin(position)) return;
        new Node(nearestAsMatrix, nearestCost).Search();
    }
}
