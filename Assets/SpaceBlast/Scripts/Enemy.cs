using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(PlayerFollower))]
[RequireComponent(typeof(Flee))]
[RequireComponent(typeof(Attack))]
public class Enemy : MonoBehaviour
{
    private PlayerFollower playerFollower;
    private Attack attack;
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
        attack = GetComponent<Attack>();
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
            else if (oldState == State.Attacking)
            {
                attack.SetTarget(null);
            }

            if (newState == State.Following)
            {
                playerFollower.SetTarget(player);
            }
            else if (newState == State.Attacking)
            {
                attack.SetTarget(player.GetComponent<Health>());
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
        bool withinAttackRange = relative.sqrMagnitude < attackingDistance * attackingDistance;
        if (withinAttackRange && state == State.Following)
        {
            ChangeState(State.Attacking);
        }
        else if (!withinAttackRange && state == State.Attacking)
        {
            ChangeState(State.Following);
        }
    }
}
