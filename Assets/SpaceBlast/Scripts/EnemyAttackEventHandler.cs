using UnityEngine;
using System.Collections;

public class EnemyAttackEventHandler : MonoBehaviour
{
    [SerializeField]
    private Attack attack;
    void AnimationAttack1()
    {
        attack.DoAttack();
    }

    void AnimationAttack2()
    {
        attack.DoAttack();
    }
}
