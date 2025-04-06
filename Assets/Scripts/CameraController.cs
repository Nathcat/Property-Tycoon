using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    public static UnityEvent<CameraController> onUpdateCamera = new UnityEvent<CameraController>();
    //for testing purposes
    [SerializeField] public GameObject target = null;
    ///used to set the board radius
    [SerializeField] private float boardRadius = 4.5f;
    ///used to contol the camers offset
    [SerializeField] private int sideOffset = 1;
    [SerializeField] private int heightOffset = 2;
    [SerializeField] private int lengthOffset = 5;
    [SerializeField] private GameObject propertyUIConroller;
    [SerializeField] public Property property;
    [SerializeField] public int lastPropertyIndex;
    [SerializeField] private GameObject events;


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
        lastPropertyIndex = property.position;

        if (target.TryGetComponent<CounterController>(out CounterController counter))
        {

            if ((counter.space) is Property)
            {
                
                property = (counter.space as Property);
                onUpdateCamera.Invoke(this);
               
                propertyUIConroller.GetComponent<CanvasGroup>().alpha = 1;
            }
            else
            {
                propertyUIConroller.GetComponent<CanvasGroup>().alpha = 0;
            }

            //sets the position to that of the target
            this.transform.position = target.transform.position;

            //checks where the target is and then moves it based on if its in the N,S,E or W
            if (this.transform.position.x > boardRadius)
            {
                this.transform.position = new Vector3(target.transform.position.x + sideOffset, target.transform.position.y + heightOffset, target.transform.position.z + lengthOffset);
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

    public void nextProperty() {
        if (lastPropertyIndex >= GameController.instance.spaceControllers.Length-1)
        {
            lastPropertyIndex = 0;
        }
        else
        {
            lastPropertyIndex = lastPropertyIndex + 1;
        }
        target = GameController.instance.spaceControllers[lastPropertyIndex].gameObject;
        if (target.GetComponent<SpaceController>().space is Property)
        {
            property = target.GetComponent<SpaceController>().space as Property;
            events.GetComponent<PropertyUIController>().GetPropertyDetails(this);
            onUpdateCamera.Invoke(this);
        }
        else 
        {
            nextProperty();       
        }

    }

    public void lastProperty()
    {
        if (lastPropertyIndex <= 0)
        {
            lastPropertyIndex = GameController.instance.spaceControllers.Length-1;
        }
        else
        {
            lastPropertyIndex = lastPropertyIndex - 1;
        }
        target = GameController.instance.spaceControllers[lastPropertyIndex].gameObject;
        if (target.GetComponent<SpaceController>().space is Property)
        {
            property = target.GetComponent<SpaceController>().space as Property;
            events.GetComponent<PropertyUIController>().GetPropertyDetails(this);
            onUpdateCamera.Invoke(this);
        }
        else 
        {
            lastProperty();
        }

    }
}
