using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject token1, token2, token3;
    public List<Node> openList = new();
    public List<Node> closedList = new();
    public float totalCost = 0;

    // "O" = Unset
    // "X" = Objective
    // "+" = Start
    public string[,] GameMatrix;
    public readonly int[] startPos = new int[2];
    public readonly int[] objectivePos = new int[2];

    private void Awake()
    {
        GameMatrix = new string[Calculator.length, Calculator.length];
        Instance = this;
        openList = new();
        closedList = new();
        totalCost = 0;

        // Populate matrix
        for (int i = 0; i < Calculator.length; i++) // Row
            for (int j = 0; j < Calculator.length; j++) // Column
                GameMatrix[i, j] = "O";

        // Randomize start/exit
        startPos[0] = Random.Range(0, Calculator.length);
        startPos[1] = Random.Range(0, Calculator.length);
        SetObjectivePoint(startPos);

        // Add to the main matrix
        GameMatrix[startPos[0], startPos[1]] = "+";
        GameMatrix[objectivePos[0], objectivePos[1]] = "X";
        
        // Create the tokens in scene and show the matrix
        InstantiateToken(token1, startPos);
        InstantiateToken(token2, objectivePos);
        PrintMatrix();
    }

    // Where the magic happens
    private void Start()
    {
        // :boom:
        new Node(startPos, Calculator.CheckDistanceToObj(startPos, objectivePos)).Search();

        // Show open and closed list
        Debug.LogWarning($"Open list:");
        openList.ForEach(node => Debug.Log($"{node.position[0]}, {node.position[1]}"));

        Debug.LogWarning($"Closed list:");
        closedList.ForEach(node => Debug.Log($"{node.position[0]}, {node.position[1]}"));

        Debug.LogWarning($"Total cost: {totalCost}");
    }

    // Sets the objective
    private void SetObjectivePoint(int[] startPos) 
    {
        var rand1 = Random.Range(0, Calculator.length);
        var rand2 = Random.Range(0, Calculator.length);

        if (rand1 != startPos[0] || rand2 != startPos[1])
        {
            objectivePos[0] = rand1;
            objectivePos[1] = rand2;
        }
    }

    // Creates a token at a position
    public void InstantiateToken(GameObject token, int[] position)
    {
        if (!token) return;
        Instantiate(token, Calculator.GetPositionFromMatrix(position), Quaternion.identity);
    }

    // Shows the entire node matrix
    public void PrintMatrix()
    {
        string matrix = "";
        for (int i = 0; i < Calculator.length; i++)
        {
            for (int j = 0; j < Calculator.length; j++)
            {
                matrix += GameMatrix[i, j] + "  ";
            }
            matrix += "\n";
        }
        Debug.Log(matrix);
    }

    // Checks if you've won
    public bool CheckWin(int[] position) { return GameMatrix[position[0], position[1]] == "X"; }

    // If already in open list, ignore.
    public bool IsInOpenList(int[] pos) { return openList.Any(node => node.position == pos); }

    // Returns if a position is outside the board bounds.
    public bool IsOutOfBounds(int[] pos)
    {
        return pos[0] >= Calculator.length || pos[1] >= Calculator.length || pos[0] <= -1 || pos[1] <= -1;
    }
}
