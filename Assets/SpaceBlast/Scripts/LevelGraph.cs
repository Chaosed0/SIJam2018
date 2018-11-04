using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGraph : MonoBehaviour
{
    [System.Serializable]
    public struct Edge
    {
        public Node node1;
        public Node node2;

        public Node GetOtherNode(Node node)
        {
            if (node1 == node)
            {
                return node2;
            }
            else if (node2 == node)
            {
                return node1;
            }
            else
            {
                return null;
            }
        }
    }

    [SerializeField]
    private List<Edge> edges = new List<Edge>();

    public LayerMask bakeLayerMask;

    private HashSet<Node> nodes = new HashSet<Node>();
    private Dictionary<Node, HashSet<Node>> neighborsMap = new Dictionary<Node, HashSet<Node>>();

    private void Awake()
    {
        foreach (Edge edge in edges)
        {
            nodes.Add(edge.node1);
            nodes.Add(edge.node2);

            HashSet<Node> neighbors1 = null;
            if (!neighborsMap.TryGetValue(edge.node1, out neighbors1))
            {
                neighbors1 = new HashSet<Node>();
                neighborsMap[edge.node1] = neighbors1;
            }
            neighbors1.Add(edge.node2);

            HashSet<Node> neighbors2 = null;
            if (!neighborsMap.TryGetValue(edge.node2, out neighbors2))
            {
                neighbors2 = new HashSet<Node>();
                neighborsMap[edge.node2] = neighbors2;
            }
            neighbors2.Add(edge.node1);
        }
    }

    private void OnDrawGizmosSelected()
    {
        foreach (Edge edge in edges)
        {
            Gizmos.DrawLine(edge.node1.transform.position, edge.node2.transform.position);
        }
    }

    public HashSet<Node> GetNodes()
    {
        return nodes;
    }

    public Dictionary<Node, HashSet<Node>> GetNeighborsMap()
    {
        return neighborsMap;
    }
}
