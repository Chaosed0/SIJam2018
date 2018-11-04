using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(PlayerFollower))]
[RequireComponent(typeof(Flee))]
public class Enemy : MonoBehaviour
{
    private PlayerFollower playerFollower;
    private GameObject player;
    private float attackingDistance = 2.0f;
    private Flee flee;

    [System.Serializable]
    public class StateChangedEvent : UnityEvent<State> { };
    public StateChangedEvent OnStateChanged = new StateChangedEvent();

    public enum State
    {
        Following,
        Attacking,
        Fleeing
    }

    State state;

    private void Awake()
    {
        playerFollower = GetComponent<PlayerFollower>();
        flee = GetComponent<Flee>();
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
            State oldState = state;
            this.state = newState;

            if (oldState == State.Following)
            {
                playerFollower.SetTarget(null);
            }

            if (newState == State.Following)
            {
                playerFollower.SetTarget(player);
            }
            else if (newState == State.Fleeing)
            {
                flee.StartFleeing(player);
            }

            OnStateChanged.Invoke(newState);
        }
    }

    public void OnHit(GameObject player)
    {
        ChangeState(State.Fleeing);
    }

    void Update()
    {
        Vector3 relative = player.transform.position - transform.position;
        if (relative.sqrMagnitude < attackingDistance * attackingDistance && state == State.Following)
        {
            ChangeState(State.Attacking);
        }
    }
}
