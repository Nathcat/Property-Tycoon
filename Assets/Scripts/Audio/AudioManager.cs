using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    public AudioSource audioSource { get; private set; }
    public AudioLibrary library;
    public AudioMixer mixer;

    private void Awake()
    {
        if (instance != null && !instance.IsDestroyed()) Destroy(gameObject);
        else instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);

        refresh("Master");
        refresh("Music");
        refresh("SFX");
    }

    public void play(AudioClip clip)
    {
        audioSource.PlayOneShot(clip, 1);
    }

    public void set(string group, float value)
    {
        float x = Mathf.Log10(value) * 20;
        mixer.SetFloat(group, x);
        PlayerPrefs.SetFloat(group, value);
    }

    public float get(string group)
    {
        if (!PlayerPrefs.HasKey(group))
            return group == "Music" ? 0.25f : 1;

        return PlayerPrefs.GetFloat(group);
    }

    private void refresh(string group)
    {
        set(group, get(group));
    }
}
