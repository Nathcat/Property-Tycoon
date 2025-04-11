using UnityEngine;

public class PoliceCarLights : MonoBehaviour
{
    public Light light;
    private bool isBlueLightNext = false;

    void Start()
    {
        InvokeRepeating("kmn", 0, 1f);
    }

    public void kmn()
    {
        if (isBlueLightNext) light.color = Color.blue;
        else light.color = Color.red;

        isBlueLightNext = !isBlueLightNext;
    }
}
