using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerFollower))]
public class Enemy : MonoBehaviour
{
    private PlayerFollower playerFollower;
    private GameObject player;
    private float attackingDistance = 1.0f;

    private enum State
    {
        Following,
        Attacking,
        Fleeing
    }

    State state;

    private void Awake()
    {
        playerFollower = GetComponent<PlayerFollower>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Enemy couldn't locate player, bad things might happen", this);
        }

        ChangeState(State.Following, true);
    }

    void ChangeState(State newState, bool force = false)
    {
        if (force || state != newState)
        {
            State oldState = newState;
            this.state = newState;

            if (oldState == State.Following || oldState == State.Fleeing)
            {
                playerFollower.SetTarget(null);
            }

            if (newState == State.Following)
            {
                playerFollower.SetTarget(player);
            }
        }
    }

    void Update()
    {
    }
}
