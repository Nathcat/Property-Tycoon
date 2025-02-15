using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //for testing purposes
    [SerializeField] private GameObject target;
    //used to contol the camers offset
    [SerializeField] private int sideOffset = 1;
    [SerializeField] private int hieghtOffset = 2;
    [SerializeField] private int lengthOffset = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //for testing purposes comment out when not in use
        UpdateCamera(target);
    }

    //a method to move the camera to a target gameobject
    void UpdateCamera(GameObject target) 
    {
        //sets the position to that of the target
        this.transform.position = target.transform.position;
        //checks where the target is and then moves it based on if its in the N,S,E or W
        if (this.transform.position.x > 4) 
        {
            this.transform.position = new Vector3(target.transform.position.x+ sideOffset, target.transform.position.y + hieghtOffset, target.transform.position.z+ lengthOffset);
            this.transform.LookAt(target.transform.position);
        }
        else if (this.transform.position.z < -4)
        {
            this.transform.position = new Vector3(target.transform.position.x + lengthOffset, target.transform.position.y + hieghtOffset, target.transform.position.z - sideOffset);
            this.transform.LookAt(target.transform.position);
        }
        else if (this.transform.position.x < - 4)
        {
            this.transform.position = new Vector3(target.transform.position.x - sideOffset, target.transform.position.y + hieghtOffset, target.transform.position.z - lengthOffset);
            this.transform.LookAt(target.transform.position);
        }
        else if (this.transform.position.z > 4)
        {
            this.transform.position = new Vector3(target.transform.position.x - lengthOffset, target.transform.position.y + hieghtOffset, target.transform.position.z + sideOffset);
            this.transform.LookAt(target.transform.position);
        }
    }
}
