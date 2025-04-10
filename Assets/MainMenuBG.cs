using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBG : MonoBehaviour
{
    // Start is called before the first frame update

    float speed;
    void Start()
    {
        speed = Random.Range(5f, 15f);
    }

    void Update()
    {
        this.transform.position -= new Vector3(0,speed,0)*Time.deltaTime;


        if (this.transform.position.y < -15)
        {
            int x = Random.Range(0, 360);

            int y = Random.Range(0, 360);
            int z = Random.Range(0, 360);
            this.transform.rotation = new Quaternion(x, y, z, 0);
            speed = Random.Range(5f, 15f);
            float randx = Random.Range(-20f, 20f);
            float randz = Random.Range(5f, 15f);
            this.transform.position = new Vector3(randx, 15, randz);
        }
    }
}
