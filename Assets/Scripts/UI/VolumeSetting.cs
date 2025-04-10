using UnityEngine;
using UnityEngine.UI;

public class VolumeSetting : MonoBehaviour
{
    [SerializeField] private Slider master;
    [SerializeField] private Slider music;
    [SerializeField] private Slider sfx;

    private void Start()
    {
        master.value = AudioManager.instance.get("Master");
        music.value = AudioManager.instance.get("Music");
        sfx.value = AudioManager.instance.get("SFX");

        master.onValueChanged.AddListener(f => AudioManager.instance.set("Master", f));
        music.onValueChanged.AddListener(f => AudioManager.instance.set("Music", f));
        sfx.onValueChanged.AddListener(f => AudioManager.instance.set("SFX", f));
    }
}
