using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using static GameController;

public class CameraController : MonoBehaviour
{
    public static UnityEvent<CameraController> onUpdateCamera = new UnityEvent<CameraController>();
    public static CameraController instance { get; private set; }
    public GameObject target { get; private set; } = null;
    ///used to set the board radius
    [SerializeField] private float boardRadius = 4.5f;
    ///used to contol the camers offset
    [SerializeField] private int sideOffset = 1;
    [SerializeField] private int heightOffset = 2;
    [SerializeField] private int lengthOffset = 5;
    /// <summary> The currently targeted space, null if the target is not a space. </summary>
    public SpaceController space { get; private set; }

    private void Awake()
    {
        if (instance != null && !instance.IsDestroyed())
            throw new DuplicateInstanceException();
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameController.instance.onNextTurn.AddListener(c => UpdateCamera(c.gameObject));
        GameController.instance.onCounterMove.AddListener(c => UpdateCamera(c.gameObject));
    }

    //a method to move the camera to a target gameobject
    /// <summary>
    /// The UpdateCamera fuction is called with a target to make it face the specified target with an angle that coressponds to its place on the board
    /// </summary>
    /// <param name="target">what you want the camera to focus on</param>
    public void UpdateCamera(GameObject target)
    {
        this.target = target;
        if (target.TryGetComponent(out SpaceController space)) this.space = space;
        else this.space = null;

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

        onUpdateCamera.Invoke(this);
    }

    private GameObject nextProperty(int start, int direction)
    {
        int i = start;
        SpaceController space;

        do
        {
            i = (i + direction) % GameController.instance.boardLength;
            if (i < 0) i = GameController.instance.boardLength + i;

            space = GameController.instance.spaceControllers[i];
        } while (space.space is not Property);

        return space.gameObject;
    }

    public void nextProperty() {
        UpdateCamera(nextProperty(space.position, 1));
    }

    public void lastProperty()
    {
        UpdateCamera(nextProperty(space.position, -1));
    }
}
