using System.Linq;
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
    public float boardRadius { get; private set; } = 4.5f;
    ///used to contol the camers offset
    [SerializeField] private int sideOffset = 1;
    [SerializeField] private int heightOffset = 2;
    [SerializeField] private int lengthOffset = 5;
    [SerializeField] private float moveLerp = 1f;
    [SerializeField] private float rotateLerp = 2f;
    [SerializeField] private Vector3 orbitOffset;
    [SerializeField] private float orbitSpeed = 4;
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
        GameController.instance.onNextTurn.AddListener(c => SetTarget(c.gameObject));
        GameController.instance.onCounterMove.AddListener(c => SetTarget(c.gameObject));
    }

    private void Update()
    {
        if (GameUIManager.instance.gameStarted)
        {
            boardRadius = (BoardGenerator.GetBoardDimensions(GameController.instance.spaces.Count()) - 4f) / 2f;
        }

        Vector3 moveTarget;
        Vector3 lookTarget;

        if (target == null || GameController.instance.gameOver)
        {
            moveTarget = GetOrbit();
            lookTarget = Vector3.zero;
        }
        else
        {
            moveTarget = GetDestination();
            lookTarget = target.transform.position;
        }

        transform.position = Vector3.Lerp(transform.position, moveTarget, Time.deltaTime * moveLerp);

        Quaternion rotation = Quaternion.LookRotation(lookTarget - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotateLerp);
    }

    public Vector3 GetOrbit()
    {
        return Quaternion.Euler(0, orbitSpeed * Time.time % 360, 0) * orbitOffset;
    }

    //a method to move the camera to a target gameobject
    /// <summary>
    /// The UpdateCamera fuction is called with a target to make it face the specified target with an angle that coressponds to its place on the board
    /// </summary>
    /// <param name="target">what you want the camera to focus on</param>
    public void SetTarget(GameObject target)
    {
        this.target = target;
        if (target.TryGetComponent(out SpaceController space)) this.space = space;
        else this.space = null;

        onUpdateCamera.Invoke(this);
    }

    public Vector3 GetDestination()
    {

        //checks where the target is and then moves it based on if its in the N,S,E or W

        if (target.transform.position.x > boardRadius)
        {
            return new Vector3(target.transform.position.x + sideOffset, target.transform.position.y + heightOffset, target.transform.position.z + lengthOffset);
        }
        else if (target.transform.position.z < -boardRadius)
        {
            return new Vector3(target.transform.position.x + lengthOffset, target.transform.position.y + heightOffset, target.transform.position.z - sideOffset);
        }
        else if (target.transform.position.x < -boardRadius)
        {
            return new Vector3(target.transform.position.x - sideOffset, target.transform.position.y + heightOffset, target.transform.position.z - lengthOffset);
        }
        else if (target.transform.position.z > boardRadius)
        {
            return new Vector3(target.transform.position.x - lengthOffset, target.transform.position.y + heightOffset, target.transform.position.z + sideOffset);
        }

        return Vector3.zero;
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

    public void nextProperty()
    {
        SetTarget(nextProperty(space.position, 1));
    }

    public void lastProperty()
    {
        SetTarget(nextProperty(space.position, -1));
    }
}
