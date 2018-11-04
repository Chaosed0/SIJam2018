using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Pathfinder))]
public class Flee : MonoBehaviour
{
    [SerializeField]
    private float speed = 6.0f;
    
    [SerializeField]
    private float closeThreshold = 0.25f;

    [SerializeField]
    private float disappearThreshold = 8.0f;

    private Rigidbody rb;
    private Pathfinder pathfinder;

    private bool isFleeing = false;
    GameObject thingToFlee = null;

    LevelGraph levelGraph;
    public Node currentNode { get; private set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        pathfinder = GetComponent<Pathfinder>();

        currentNode = null;
    }

    private void Start()
    {
        levelGraph = FindObjectOfType<LevelGraph>();
        if (levelGraph == null)
        {
            Debug.LogError("Pathfinder couldn't find level graph, there's gonna be trouble");
        }
    }

    void Update()
    {
        if (isFleeing)
        {
            Debug.Assert(currentNode != null);
            Vector3 relativeToPlayer = thingToFlee.transform.position - transform.position;
            if (relativeToPlayer.sqrMagnitude > disappearThreshold * disappearThreshold)
            {
                Destroy(this.gameObject);
            }

            Vector3 relativeToNode = currentNode.transform.position - transform.position;
            transform.rotation = Quaternion.LookRotation(relativeToNode);
            if (relativeToNode.sqrMagnitude < closeThreshold * closeThreshold)
            {
                FindNewNode();
            }
        }
    }

    private void FixedUpdate()
    {
        if (isFleeing)
        {
            Debug.Assert(currentNode != null);
            Vector3 relativeToNode = currentNode.transform.position - transform.position;
            rb.velocity = relativeToNode.normalized * speed;
        }
    }

    void FindNewNode()
    {
        HashSet<Node> candidates = null;
        if (currentNode != null)
        {
            candidates = levelGraph.GetNeighborsMap()[currentNode];
        }
        else
        {
            candidates = new HashSet<Node>();
            foreach (Node node in levelGraph.GetNodes())
            {
                if (pathfinder.CanTraverse(transform.position, node.transform.position))
                {
                    candidates.Add(node);
                }
            }
        }

        // Which one of these nodes gets us away from the player?
        currentNode = null;
        foreach (Node node in candidates)
        {
            if (node == currentNode)
            {
                continue;
            }

            if (currentNode == null)
            {
                currentNode = node;
                continue;
            }

            Vector3 relative = thingToFlee.transform.position - transform.position;
            Vector3 relativeToCurrentNode = currentNode.transform.position - transform.position;
            Vector3 relativeToNewNode = node.transform.position - transform.position;
            float oldFacing = Vector3.Dot(relative.normalized, relativeToCurrentNode.normalized);
            float newFacing = Vector3.Dot(relative.normalized, relativeToNewNode.normalized);

            if (newFacing < oldFacing)
            {
                currentNode = node;
            }
        }

        // If we are gonna go towards the player, rather just disappear
        if (Vector3.Angle((currentNode.transform.position - transform.position).normalized, (thingToFlee.transform.position - transform.position).normalized) < 30.0f)
        {
            Destroy(this.gameObject);
        }
    }

    public void PrepareFlee(GameObject player)
    {
        thingToFlee = player;
        FindNewNode();
    }

    public void StartFleeing()
    {
        Debug.Assert(thingToFlee != null);
        Debug.Assert(currentNode != null);
        isFleeing = true;
    }
}
