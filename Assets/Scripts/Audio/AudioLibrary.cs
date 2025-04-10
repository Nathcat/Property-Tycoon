using UnityEngine;

[CreateAssetMenu(fileName = "Sound Manager")]
public class AudioLibrary : ScriptableObject
{
    [SerializeField] private AudioClip clickSfx;

    public void PlayCLick()
    {
        AudioManager.instance.Play(clickSfx);
    }
}
