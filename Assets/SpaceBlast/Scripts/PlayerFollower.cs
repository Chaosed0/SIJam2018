using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Pathfinder))]
public class PlayerFollower : MonoBehaviour
{
    [SerializeField]
    private float speed = 4.0f;

    [SerializeField]
    private float repathInterval = 3.0f;

    [SerializeField]
    private float closeDistance = 0.5f;

    private Rigidbody rb;

    private float lastRepathTime = 0f;

    private Pathfinder pathfinder;

    private List<Node> currentPath = null;
    private int currentPathIndex = -1;

    private GameObject target = null;
    private Vector3 facing;

    void Awake()
    {
        pathfinder = GetComponent<Pathfinder>();
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
    }

    void Update()
    {
        if (target == null)
        {
            return;
        }

        bool goStraight = false;
        if (pathfinder.CanTraverse(transform.position, target.transform.position))
        {
            goStraight = true;
        }

        if (!goStraight && Time.time - lastRepathTime > repathInterval)
        {
            lastRepathTime = Time.time;
            currentPath = pathfinder.FindPath(target.transform.position);
            currentPathIndex = -1;
        }

        this.facing = Vector3.zero;
        if (goStraight)
        {
            this.facing = target.transform.position - transform.position;
        }
        else if (currentPath != null)
        {
            this.facing = currentPath[currentPathIndex + 1].transform.position - transform.position;
        }

        if (facing != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(facing);
        }

        if (currentPath != null)
        {
            Vector3 relative = currentPath[currentPathIndex + 1].transform.position - transform.position;
            if (relative.sqrMagnitude < closeDistance * closeDistance)
            {
                currentPathIndex++;
                if (currentPathIndex + 1 >= currentPath.Count)
                {
                    currentPath = null;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = facing.normalized * speed;
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }
}
