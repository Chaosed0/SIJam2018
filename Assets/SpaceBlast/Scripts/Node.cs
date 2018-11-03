using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, "stairs-goal.png");
    }
}
