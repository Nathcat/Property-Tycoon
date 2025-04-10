using UnityEngine;

[CreateAssetMenu(fileName = "Sound Manager")]
public class SoundManager : ScriptableObject
{
    [SerializeField] private AudioClip clickSfx;

    public void PlayCLick()
    {
        AudioSource.PlayClipAtPoint(clickSfx, Camera.main.transform.position);
        Debug.Log("asdasd");
    }
}
