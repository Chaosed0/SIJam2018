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

    private float lastRepathTime = 0f;

    private Pathfinder pathfinder;

    private List<Node> currentPath = null;
    private int currentPathIndex = -1;

    private GameObject target = null;

    void Awake()
    {
        pathfinder = GetComponent<Pathfinder>();
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

        Vector3 facing = Vector3.zero;
        if (goStraight)
        {
            facing = target.transform.position - transform.position;
        }
        else if (currentPath != null)
        {
            facing = currentPath[currentPathIndex + 1].transform.position - transform.position;
        }

        if (facing != Vector3.zero)
        {
            transform.position += facing.normalized * speed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(facing);
        }

        if (currentPath != null)
        {
            Vector3 relative = currentPath[currentPathIndex + 1].transform.position - transform.position;
            Debug.Log(relative);
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

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }
}
