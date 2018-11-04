using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class Health : MonoBehaviour
{
    [SerializeField]
    private float maximumHealth = 3;

    private float health = 3;
    public bool isDead { get; private set; }

    [SerializeField]
    public class HealthChangedEvent : UnityEvent<float, float> { }
    public HealthChangedEvent OnHealthChanged = new HealthChangedEvent();

    [SerializeField]
    public class DeadEvent : UnityEvent { }
    public DeadEvent OnDead = new DeadEvent();

    private void Awake()
    {
        this.isDead = false;
        this.health = maximumHealth;
    }

    public void AddHealth(float health)
    {
        float oldHealth = this.health;
        this.health = Mathf.Clamp(this.health + health, 0f, maximumHealth);

        if (oldHealth != this.health)
        {
            Debug.Log("Health change " + oldHealth + " -> " + this.health);
            OnHealthChanged.Invoke(this.health, oldHealth);

            if (!isDead && this.health <= 0f)
            {
                isDead = true;
                OnDead.Invoke();
            }
        }
    }

    public float GetMaximumHealth()
    {
        return maximumHealth;
    }

    public float GetHealth()
    {
        return health;
    }
}
