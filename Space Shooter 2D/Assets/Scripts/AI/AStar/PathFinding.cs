using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinding : MonoBehaviour
{
    public Transform target;
    public Node targetNode;
    public Node overrideTarget;
    public List<Node> inspectedPoints;
    public List<Node> postUnwalkableNodes;
    public List<Node> rememberNode;
    bool pathExist;
    Grid grid;

    void Awake()
    {
        rememberNode = new List<Node>();
        inspectedPoints = new List<Node>();
        postUnwalkableNodes = new List<Node>();
        grid = GetComponent<Grid>();
    }
    public void FindPath(Vector3 startPos, Vector3 targetPos)
    {
        pathExist = false;
        Node startNode = grid.NodeFromWorldPoint(startPos);
        targetNode = grid.NodeFromWorldPoint(targetPos);

        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);

        while (openSet.Count > 0)
        {
            Node currentNode = openSet[0];

            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if (currentNode == targetNode)
            {
                pathExist = true;
                RetracePath(startNode, targetNode);

                // dynamic retrace off
                // overrideTarget = targetNode;
                return;
            }

            foreach (Node neighbour in grid.GetNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedSet.Contains(neighbour))
                    continue;

                if (overrideTarget == null)
                {
                    bool contains = false;
                    foreach (Node n in postUnwalkableNodes)
                        if (n.worldPosition == neighbour.worldPosition)
                        {
                            contains = true;
                            break;
                        }
                    if (contains) continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        // если провалились, то ближайшую к финишу
        if (!pathExist)
        {
            inspectedPoints.Add(targetNode);
            List<Node> neighbours = grid.GetNeighbours(targetNode);
            for (int i = 0; i < neighbours.Count; i++)
            {
                bool contains = false;

                // inspectedPoints нужны для недопущения рекурсии
                foreach (Node n in inspectedPoints)
                {
                    if (n.worldPosition == neighbours[i].worldPosition)
                    {
                        contains = true;
                        break;
                    }
                }

                if (!contains)
                {
                    FindPath(this.transform.position, neighbours[i].worldPosition);
                }
                if (pathExist) break;
            }

            //--30.04 Если путь так и не нашли 
            if (!pathExist)
            {
                GetPostUnwalkableNodes(grid.NodeFromWorldPoint(startPos));

                for (int i = 0; i < postUnwalkableNodes.Count; i++)
                {
                    // Если точка находится на краю -> потенциальный путь
                    if (grid.NodeOnBorder(postUnwalkableNodes[i]))
                    {
                        rememberNode.Add(postUnwalkableNodes[i]);
                        postUnwalkableNodes.RemoveAt(i);
                        i--;
                    }
                }

                if (rememberNode.Count != 0)
                {
                    inspectedPoints.Clear();
                    //--------------
                    for (int i = 0; i < rememberNode.Count; i++)
                        foreach (Node n in postUnwalkableNodes)
                            if (n.worldPosition == rememberNode[i].worldPosition)
                            {
                                rememberNode.RemoveAt(i);
                                i--;
                                break;
                            }
                    //-------------
                    overrideTarget = FindNearestTo(target.transform.position, rememberNode);
                    if (overrideTarget != null)
                        FindPath(this.transform.position, overrideTarget.worldPosition);
                    rememberNode.Remove(overrideTarget);
                }
            }

        }
        // --------
    }
    // Пункт 1 work
    Node FindNearestTo(Vector3 target, List<Node> okNode)
    {
        float length = int.MaxValue;
        Node tempNode = null;
        for (int i = 0; i < okNode.Count; i++)
        {
            float tempLength = (target - okNode[i].worldPosition).sqrMagnitude;
            if (length > tempLength)
            {
                length = tempLength;
                tempNode = okNode[i];
            }
        }
        return tempNode;
    }
    void GetPostUnwalkableNodes(Node startNode)
    {
        postUnwalkableNodes.Add(startNode);
        List<Node> neighbours = grid.GetNeighbours(startNode);
        for (int i = 0; i < neighbours.Count; i++)
        {
            if (neighbours[i].walkable)
            {
                bool contain = false;
                //Закрасить внутренние
                foreach (Node n in postUnwalkableNodes)
                {
                    if (n.worldPosition == neighbours[i].worldPosition)
                    {
                        // Такая есть
                        contain = true;
                        break;
                    }
                }
                if (!contain)
                    GetPostUnwalkableNodes(neighbours[i]);
            }
        }
    }
    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        grid.path = path;
    }
    int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY) return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }
}
