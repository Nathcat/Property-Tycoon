using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //for testing purposes
    [SerializeField] private GameObject target;
    ///used to set the board radius
    [SerializeField] private float boardRadius = 5;
    ///used to contol the camers offset
    [SerializeField] private int sideOffset = 1;
    [SerializeField] private int heightOffset = 2;
    [SerializeField] private int lengthOffset = 5;

    [SerializeField] public Space space;

    // Start is called before the first frame update
    void Start()
    {
        GameController.instance.onNextTurn.AddListener(c => UpdateCamera(c.gameObject));
    }

    //a method to move the camera to a target gameobject
    /// <summary>
    /// The UpdateCamera fuction is called with a target to make it face the specified target with an angle that coressponds to its place on the board
    /// </summary>
    /// <param name="target">what you want the camera to focus on</param>
    void UpdateCamera(GameObject target) 
    {
        //sets the position to that of the target
        this.transform.position = target.transform.position;

        //checks where the target is and then moves it based on if its in the N,S,E or W
        if (this.transform.position.x > boardRadius) 
        {
            this.transform.position = new Vector3(target.transform.position.x+ sideOffset, target.transform.position.y + heightOffset, target.transform.position.z+ lengthOffset);
            this.transform.LookAt(target.transform.position);
        }
        else if (this.transform.position.z < -boardRadius)
        {
            this.transform.position = new Vector3(target.transform.position.x + lengthOffset, target.transform.position.y + heightOffset, target.transform.position.z - sideOffset);
            this.transform.LookAt(target.transform.position);
        }
        else if (this.transform.position.x < -boardRadius)
        {
            this.transform.position = new Vector3(target.transform.position.x - sideOffset, target.transform.position.y + heightOffset, target.transform.position.z - lengthOffset);
            this.transform.LookAt(target.transform.position);
        }
        else if (this.transform.position.z > boardRadius)
        {
            this.transform.position = new Vector3(target.transform.position.x - lengthOffset, target.transform.position.y + heightOffset, target.transform.position.z + sideOffset);
            this.transform.LookAt(target.transform.position);
        }
    }
}
