using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    // public
    public Transform startPosition;
    public Transform targetPosition;
    //private
    private Grid grid;
    private void Awake()
    {
        grid = GetComponent<Grid>();
    }
    void Update()
    {
        FindPath(startPosition.position, targetPosition.position);
    }
    public void FindPath(Vector3 a_StartPosition,Vector3 a_TargetPosition)
    {
        Node startNode = grid.NodeFromWorldPosition(a_StartPosition);
        Node targetNode = grid.NodeFromWorldPosition(a_TargetPosition);

        List<Node> openlist = new List<Node>();
        HashSet<Node> closedList = new HashSet<Node>();

        openlist.Add(startNode);

        while(openlist.Count > 0)
        {
            Node currentNode = openlist[0];
            for(int i = 1; i < openlist.Count;i++)
            {
                if(openlist[i].FCost<currentNode.FCost || openlist[i].FCost == currentNode.FCost && openlist[i].hCost < currentNode.hCost)
                {
                    currentNode = openlist[i];
                }
            }
            openlist.Remove(currentNode);
            closedList.Add(currentNode);

            if(currentNode == targetNode)
            {
                GetFinalPath(startNode,targetNode);
            }

            foreach (Node neighBorNode in grid.GetNeightBorNodes(currentNode))
            {
                if (!neighBorNode.isWall || closedList.Contains(neighBorNode))
                {
                    continue;
                }
                int moveCost = currentNode.gCost + GetManHattenDistance(currentNode, neighBorNode);

                if(moveCost < neighBorNode.gCost || !openlist.Contains(neighBorNode))
                {
                    neighBorNode.gCost = moveCost;
                    neighBorNode.hCost = GetManHattenDistance(neighBorNode, targetNode);
                    neighBorNode.parent = currentNode;

                    if (!openlist.Contains(neighBorNode))
                    {
                        openlist.Add(neighBorNode);
                    }
                }
            }
        }
    }
    public void GetFinalPath(Node a_StartingNode,Node a_EndNode)
    {
        List<Node> finalPath = new List<Node>();
        Node currentNode = a_EndNode;
        
        while(currentNode != a_StartingNode)
        {
            finalPath.Add(currentNode);
            currentNode = currentNode.parent;
        }

        finalPath.Reverse();

        grid.finalPath = finalPath;
    }

    int GetManHattenDistance(Node a_NodeA, Node a_NodeB)
    {
        int ix = Mathf.Abs(a_NodeA.gridX - a_NodeB.gridX);
        int iy = Mathf.Abs(a_NodeA.gridY - a_NodeB.gridY);

        return ix + iy;
    }
}