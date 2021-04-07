using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform startPosition;
    public LayerMask wallMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    public float distance;

    Node[,] grid;
    public List<Node> finalPath;

    public float nodeDiameter;
    public int gridSizeX;
    public int gridSizeY;

    private void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x/nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);

        CreateGrid();
    }
    public void CreateGrid()
    {
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 bottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 wordPoint = bottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool wall = true;

                if (Physics.CheckSphere(wordPoint, nodeRadius, wallMask))
                {
                    wall = false;
                }

                grid[x, y] = new Node(wall, wordPoint, x, y);
            }
        }
    }
    public List<Node> GetNeightBorNodes(Node a_Node)
    {
        List<Node> neighBorNodes = new List<Node>();
        int xCheck;
        int yCheck;

        xCheck = a_Node.gridX + 1;
        yCheck = a_Node.gridY;

        if(xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighBorNodes.Add(grid[xCheck,yCheck]);
            }
        }

        xCheck = a_Node.gridX - 1;
        yCheck = a_Node.gridY;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighBorNodes.Add(grid[xCheck, yCheck]);
            }
        }


        xCheck = a_Node.gridX;
        yCheck = a_Node.gridY + 1;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighBorNodes.Add(grid[xCheck, yCheck]);
            }
        }

        xCheck = a_Node.gridX;
        yCheck = a_Node.gridY - 1;

        if (xCheck >= 0 && xCheck < gridSizeX)
        {
            if (yCheck >= 0 && yCheck < gridSizeY)
            {
                neighBorNodes.Add(grid[xCheck, yCheck]);
            }
        }

        return neighBorNodes;
    }
    public Node NodeFromWorldPosition(Vector3 a_WorldPosition)
    {
        float xPoint = ((a_WorldPosition.x = gridWorldSize.x / 2) / gridWorldSize.x);
        float yPoint = ((a_WorldPosition.z = gridWorldSize.y / 2) / gridWorldSize.y);

        xPoint = Mathf.Clamp01(xPoint);
        yPoint = Mathf.Clamp01(yPoint);


        int ix = Mathf.RoundToInt((gridSizeX - 1) * xPoint);
        int iy = Mathf.RoundToInt((gridSizeY - 1) * yPoint);

        return grid[ix, iy];
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));

        if(grid != null)
        {
            foreach(Node node in grid)
            {
                if (node.isWall)
                {
                    Gizmos.color = Color.white;
                }
                else
                {
                    Gizmos.color = Color.yellow;
                }
                if (finalPath != null)
                {
                    if (finalPath.Contains(node))
                    {
                        Gizmos.color = Color.red;
                    }
                }

                Gizmos.DrawCube(node.position,Vector3.one * (nodeDiameter - distance));
            }
        }
    }
}