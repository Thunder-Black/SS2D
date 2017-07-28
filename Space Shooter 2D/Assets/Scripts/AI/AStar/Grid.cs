using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour
{

    public List<Node> path;
    public LayerMask[] unwalkableMasks;
    public Vector2 gridSize;
    public float nodeRadius;
    public float refreshTime;
    public Node[,] grid;

    float timer = 0.0f;
    float nodeDiameter;
    int gridSizeX, gridSizeY;
    Vector3 cubedTransformPos;
    PathFinding aStar;

    void Start()
    {
        aStar = GetComponent<PathFinding>();
        nodeDiameter = nodeRadius * 2;
        gridSizeX = Mathf.RoundToInt(gridSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridSize.y / nodeDiameter);

        grid = new Node[gridSizeX, gridSizeY];
        FillGrid();
    }
    void FillGrid()
    {
        cubedTransformPos = new Vector3(Mathf.CeilToInt(transform.position.x / nodeDiameter) * nodeDiameter - nodeRadius, Mathf.CeilToInt(transform.position.y / nodeDiameter) * nodeDiameter - nodeRadius, transform.position.z);
        Vector3 worldBottomLeft = cubedTransformPos - Vector3.right * gridSize.x / 2 - Vector3.up * gridSize.y / 2; // right-up
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.up * (y * nodeDiameter + nodeRadius);
                bool walkable = true;
                for (int i = 0; i < unwalkableMasks.Length; i++)
                {
                    if (Physics2D.OverlapCircle(worldPoint, nodeRadius, unwalkableMasks[i]) != null)
                        walkable = false;
                    if (!walkable) break;
                }
                grid[x, y] = new Node(walkable, worldPoint, x, y);
            }
        }
    }
    public bool NodeOnBorder(Node node)
    {
        if (node.gridX == 0 || node.gridX == gridSizeX -1 || node.gridY == 0 || node.gridY == gridSizeY -1)
            return true;
        return false;
    }
    public List<Node> GetNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();
        for (int x = -1; x <= 1; x++)
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;


                int checkX = node.gridX + x;
                int checkY = node.gridY + y;

                if (checkX >= 0 && checkX < gridSizeX && checkY >= 0 && checkY < gridSizeY)
                {
                    neighbours.Add(grid[checkX, checkY]);
                }
            }
        return neighbours;
    }
    public Node NodeFromWorldPoint(Vector3 point)
    {
        if (grid != null)
        {
            Node myNode = grid[grid.GetLength(0) / 2, grid.GetLength(1) / 2];
            // Если точка за пределами досягаемости
            Vector3 tempNodePos = point - myNode.worldPosition;
            int dx = 0, dy = 0;
            if (Mathf.Abs(tempNodePos.x) >= gridSizeX * nodeRadius || Mathf.Abs(tempNodePos.y) >= gridSizeY * nodeRadius)
            {
                float angle = Vector2.Angle(Vector2.left, point - myNode.worldPosition) * -Mathf.Sign(tempNodePos.y);
                angle = angle * Mathf.PI / 180;
                tempNodePos.x = -gridSizeX * nodeRadius * Mathf.Cos(angle) * 0.99f;
                tempNodePos.y = -gridSizeY * nodeRadius * Mathf.Sin(angle) * 0.99f;
            }
            // Смещение
            dx = Mathf.CeilToInt((tempNodePos.x - nodeRadius) / nodeDiameter);
            dy = Mathf.CeilToInt((tempNodePos.y - nodeRadius) / nodeDiameter);

            return grid[(grid.GetLength(0) / 2) + dx, (grid.GetLength(1) / 2) + dy];
        }
        return null;
    }
    public void DrawPoint(Vector3 point, Color color)
    {
        if (grid != null)
        {
            Node drawNode = NodeFromWorldPoint(point);
            Gizmos.color = color;
            Gizmos.DrawCube(new Vector3(drawNode.worldPosition.x, drawNode.worldPosition.y, 5), Vector3.one * (nodeDiameter - .1f));
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(cubedTransformPos, new Vector3(gridSize.x, gridSize.y, 1));
        if (grid != null)
        {
            foreach (Node n in grid)
            {
                Gizmos.color = (n.walkable) ? Color.white : Color.red;
                if (path != null)
                {
                    for (int j = 0; j < aStar.inspectedPoints.Count; j++)
                        if (aStar.inspectedPoints[j].worldPosition == n.worldPosition)
                            Gizmos.color = Color.yellow;

                    for (int j = 0; j < aStar.postUnwalkableNodes.Count; j++)
                        if (aStar.postUnwalkableNodes[j].worldPosition == n.worldPosition)
                            Gizmos.color = Color.gray;

                    for (int j = 0; j < aStar.rememberNode.Count; j++)
                        if (aStar.rememberNode[j].worldPosition == n.worldPosition)
                            Gizmos.color = Color.blue;

                    for (int j = 0; j < path.Count; j++)
                        if (path[j].worldPosition == n.worldPosition)
                            Gizmos.color = Color.black;
                }
                Gizmos.DrawCube(new Vector3(n.worldPosition.x, n.worldPosition.y, 6), Vector3.one * (nodeDiameter - .1f));
            }
            DrawPoint(cubedTransformPos, Color.green);
            if (aStar.targetNode != null)
                DrawPoint(aStar.targetNode.worldPosition, Color.magenta);
        }
    }
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= refreshTime)
        {
            FillGrid();
            //if (aStar.target != null && (path == null || (path.Count <= 2)))
            if (aStar.target != null && aStar.overrideTarget == null)
                aStar.FindPath(cubedTransformPos, aStar.target.position);
            else if (aStar.overrideTarget != null && path.Count <= 2)
            {
                aStar.overrideTarget = null;
            }
            timer = 0.0f;
        }
    }

}
