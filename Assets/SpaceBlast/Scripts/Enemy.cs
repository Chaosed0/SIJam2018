using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[RequireComponent(typeof(PlayerFollower))]
[RequireComponent(typeof(Flee))]
[RequireComponent(typeof(Attack))]
public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private float hitstunTime = 0.5f;

    private PlayerFollower playerFollower;
    private Attack attack;
    private GameObject player;
    private float attackingDistance = 2.0f;
    private Flee flee;
    private float onHitstunTime;
    private Quaternion onHitstunFacing;

    [System.Serializable]
    public class StateChangedEvent : UnityEvent<State> { };
    public StateChangedEvent OnStateChanged = new StateChangedEvent();

    public enum State
    {
        Following,
        Attacking,
        Hitstun,
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
                animator.SetBool("IsAttacking", false);
            }

            if (newState == State.Following)
            {
                playerFollower.SetTarget(player);
            }
            else if (newState == State.Attacking)
            {
                attack.SetTarget(player.GetComponent<Health>());
                animator.SetBool("IsAttacking", true);
            }
            else if (newState == State.Hitstun)
            {
                onHitstunTime = Time.time;
                onHitstunFacing = transform.rotation;
                flee.PrepareFlee(player);
                animator.SetBool("IsFleeing", true);
            }
            else if (newState == State.Fleeing)
            {
                animator.SetBool("IsFleeing", false);
                flee.StartFleeing();
            }

            OnStateChanged.Invoke(newState);
        }
    }

    public void OnHit(GameObject player)
    {
        if (state != State.Hitstun && state != State.Fleeing)
        {
            ChangeState(State.Hitstun);
        }
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

        if (state == State.Hitstun)
        {
            float lerp = (Time.time - onHitstunTime) / hitstunTime;
            transform.rotation = Quaternion.Slerp(onHitstunFacing, Quaternion.LookRotation(flee.currentNode.transform.position - transform.position), lerp);
            if (lerp > 1.0f)
            {
                ChangeState(State.Fleeing);
            }
        }
    }

    private void FixedUpdate()
    {
        animator.SetFloat("Speed", rb.velocity.magnitude / 4.0f);
    }
}
