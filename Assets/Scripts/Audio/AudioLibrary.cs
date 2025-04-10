using UnityEngine;

[CreateAssetMenu(fileName = "Sound Manager")]
public class AudioLibrary : ScriptableObject
{
    [SerializeField] private AudioClip clickSfx;
    [SerializeField] private AudioClip moneySfx;

    public void PlayCLick()
    {
        AudioManager.instance.Play(clickSfx);
    }

    public void PlayMoney()
    {
        AudioManager.instance.Play(moneySfx);
    }
}
