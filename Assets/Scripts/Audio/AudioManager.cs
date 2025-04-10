using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }
    public AudioSource audioSource { get; private set; }
    public AudioLibrary library;

    private void Awake()
    {
        if (instance != null && !instance.IsDestroyed()) Destroy(gameObject);
        else instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    public void Play(AudioClip clip)
    {
        audioSource.PlayOneShot(clip, 1);
    }
}
