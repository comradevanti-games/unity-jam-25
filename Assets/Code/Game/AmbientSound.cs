using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AmbientSound : MonoBehaviour
{
    [Header("Ambient Sound Settings")]
    [Tooltip("Ambient audio clip to play on loop")]
    [SerializeField] private AudioClip ambientClip;

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        ConfigureAudioSource();
    }

    void Start()
    {
        PlayAmbientSound();
    }

    private void ConfigureAudioSource()
    {
        audioSource.clip = ambientClip;
        audioSource.loop = true;
        audioSource.playOnAwake = false; // Let Start() handle playback
        audioSource.spatialBlend = 0f;   // 2D sound (set to 1 for 3D if needed)
        audioSource.volume = 1f;
    }

    private void PlayAmbientSound()
    {
        if (ambientClip != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
        else if (ambientClip == null)
        {
            Debug.LogWarning($"{nameof(AmbientSound)} on {gameObject.name} has no AudioClip assigned.");
        }
    }
}
