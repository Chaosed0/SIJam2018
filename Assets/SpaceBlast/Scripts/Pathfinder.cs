using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Pathfinder : MonoBehaviour
{
    [SerializeField]
    LayerMask visibilityLayerMask;

    LevelGraph levelGraph;

    void Start()
    {
        levelGraph = FindObjectOfType<LevelGraph>();
        if (levelGraph == null)
        {
            Debug.LogError("Pathfinder couldn't find level graph, there's gonna be trouble");
        }
    }

    public List<Node> FindPath(Vector3 endPosition, bool considerSight = true)
    {
        Node startNode = GetClosestNode(transform.position, considerSight);
        Node endNode = GetClosestNode(endPosition, considerSight);

        if (startNode == null)
        {
            Debug.LogError("Enemy is not in sight of a node! You probably need to add more nodes.", this);
            return null;
        }

        if (endNode == null)
        {
            Debug.LogError("Player is not in sight of a node! You probably need to add more nodes.", this);
            return null;
        }

        Stack<Node> stack = new Stack<Node>();
        Dictionary<Node, Node> previousNodes = new Dictionary<Node, Node>();

        stack.Push(startNode);
        previousNodes.Add(startNode, startNode);

        var neighborsMap = levelGraph.GetNeighborsMap();
        while (stack.Count > 0)
        {
            Node node = stack.Pop();

            if (node == endNode)
            {
                break;
            }

            foreach (Node neighbor in neighborsMap[node])
            {
                if (!previousNodes.ContainsKey(neighbor))
                {
                    stack.Push(neighbor);
                    previousNodes.Add(neighbor, node);
                }
            }
        }

        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = previousNodes[currentNode];
        }
        path.Add(startNode);
        path.Reverse();

        // If we can traverse to the initial nodes, then postprocess them out; this prevents
        // back and forth where the closest node is behind us
        int startNodeIndex = 0;
        while (startNodeIndex < path.Count && CanTraverse(transform.position, path[startNodeIndex].transform.position))
        {
            startNodeIndex++;
        }

        for (int i = 0; i < startNodeIndex-1; i++)
        {
            path.RemoveAt(i);
        }

        return path;
    }

    public bool CanTraverse(Vector3 from, Vector3 to)
    {
        Vector3 relative = to - from;
        RaycastHit hitInfo;
        bool hit = Physics.SphereCast(from, 0.75f, relative.normalized, out hitInfo, relative.magnitude, visibilityLayerMask, QueryTriggerInteraction.Ignore);
        return !hit;
    }

    public Node GetClosestNode(Vector3 position, bool considerSight)
    {
        Node result = null;
        foreach (Node node in levelGraph.GetNodes())
        {
            if (!considerSight || CanTraverse(position, node.transform.position))
            {
                if (result == null)
                {
                    result = node;
                }
                else if ((position - result.transform.position).sqrMagnitude > (position - node.transform.position).sqrMagnitude)
                {
                    result = node;
                }
            }
        }

        return result;
    }
}
