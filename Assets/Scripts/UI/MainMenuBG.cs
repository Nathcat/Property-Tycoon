using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBG : MonoBehaviour
{
    // Start is called before the first frame update

    float speed;
    void Start()
    {
        //sets the ititial speed of the object
        speed = Random.Range(5f, 15f);
    }

    void Update()
    {
        //moves the object down at the set speed
        this.transform.position -= new Vector3(0,speed,0)*Time.deltaTime;

        //called when the object is below y-15
        if (this.transform.position.y < -15)
        {
            //gets a random value for the x,y and z values and then sets them as the roataion
            int x = Random.Range(0, 360);
            int y = Random.Range(0, 360);
            int z = Random.Range(0, 360);
            this.transform.rotation = new Quaternion(x, y, z, 0);
            //sets the speed and x and z as random floats and then telports the object to a the z and x at y 15
            speed = Random.Range(5f, 15f);
            float randx = Random.Range(-20f, 20f);
            float randz = Random.Range(5f, 15f);
            this.transform.position = new Vector3(randx, 15, randz);
        }
    }
}
