using UnityEngine;
using System.Collections;

public class CollisionSounder : MonoBehaviour
{
    [SerializeField]
    private SoundLibrary library;

    [SerializeField]
    private AudioSource audioSourcePrefab;

    [SerializeField]
    private float lowThreshold = 2.0f;

    [SerializeField]
    private float mediumThreshold = 4.0f;

    [SerializeField]
    private float highThreshold = 8.0f;

    [SerializeField]
    private LayerMask layerMask;

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << layerMask.value) & collision.collider.gameObject.layer) == 0)
        {
            return;
        }

        AudioClip audioClip = null;
        float magnitude = collision.relativeVelocity.magnitude;
        if (magnitude > highThreshold)
        {
            audioClip = library.GetSound("HighImpact");
        }
        else if (magnitude > mediumThreshold)
        {
            audioClip = library.GetSound("MediumImpact");
        }
        else if (magnitude > lowThreshold)
        {
            audioClip = library.GetSound("LowImpact");
        }

        if (audioClip != null)
        {
            AudioSource source = Instantiate(audioSourcePrefab, collision.contacts[0].point, Quaternion.identity);
            source.clip = audioClip;
            source.Play();
        }
    }
}
