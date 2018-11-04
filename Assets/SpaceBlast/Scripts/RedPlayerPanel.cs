using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CanvasGroup))]
public class RedPlayerPanel : MonoBehaviour
{
    [SerializeField]
    private Health health;

    [SerializeField]
    private float lerpSpeed = 3.0f;

    [SerializeField]
    private float flashTime = 0.25f;

    [SerializeField]
    private CanvasGroup deathCanvasGroup;

    private CanvasGroup canvasGroup;
    private Coroutine flashCoroutine = null;

    void Start()
    {
        if (health == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            health = player.GetComponent<Health>();
        }

        canvasGroup = GetComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        deathCanvasGroup.alpha = 0f;

        health.OnHealthChanged.AddListener(OnHealthChanged);
        health.OnDead.AddListener(OnDeath);
    }

    void Update()
    {
        if (flashCoroutine == null)
        {
            float target = 1.0f - (health.GetHealth() / health.GetMaximumHealth());
            float current = canvasGroup.alpha;

            float dist = target - current;
            float newDist = Mathf.Sign(dist) * Mathf.Max(Mathf.Abs(dist) - lerpSpeed * Time.deltaTime, 0f);
            canvasGroup.alpha = target - newDist;
        }
    }

    void OnHealthChanged(float newHealth, float oldHealth)
    {
        if (newHealth < oldHealth)
        {
            Flash();
        }
    }

    void OnDeath()
    {
        StartCoroutine(DeathCoroutine());
    }

    void Flash()
    {
        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
        }

        flashCoroutine = StartCoroutine(FlashCoroutine());
    }

    IEnumerator FlashCoroutine()
    {
        canvasGroup.alpha = 1.0f;

        float startTime = Time.time;

        while (Time.time - startTime < flashTime)
        {
            float lerp = (Time.time - startTime) / flashTime;
            float target = 1.0f - (health.GetHealth() / health.GetMaximumHealth());
            canvasGroup.alpha = Mathf.Lerp(1.0f, target, lerp);
            yield return null;
        }

        flashCoroutine = null;
    }

    IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        deathCanvasGroup.alpha = 1f;
    }
}
