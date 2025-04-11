using System.Collections;
using UnityEngine;

public abstract class CounterController : MonoBehaviour
{
    /// <summary> The portfolio of the counter, containing any owned money and properties. </summary>
    [HideInInspector] public readonly Portfolio portfolio = new Portfolio();

    /// <summary> The index in <see cref="GameController.spaces"/> that this counter is currently on. </summary>
    [HideInInspector] public int position { get; protected set; }

    /// <summary> The index of this counter in <see cref="GameController.counters"/>. </summary>
    public int order { get { return System.Array.IndexOf(GameController.instance.counters, this); } }

    /// <summary> The space that the counter is currently on </summary>
    public Space space { get { return GameController.instance.spaces[position]; } }

    /// <summary> The icon used to represent this counter </summary>
    public Sprite icon { get; private set; }

    /// <summary> The space controller that the counter is currently on </summary>
    public SpaceController spaceController { get { return GameController.instance.spaceControllers[position]; } }
    /// <summary>
    /// The name of the game object this controller is attached to
    /// </summary>
    public string name { get { return gameObject.name; } }

    /// <summary>
    /// Stores the current counter's model
    /// </summary>
    protected GameObject currentModel;

    /// <summary>
    /// Stores the last roll performed by this counter
    /// </summary>
    public GameUIManager.RollData lastRoll { get { return GameUIManager.instance.lastDiceRoll; } }

    /// <summary>
    /// Stores if the player is in jail
    /// </summary>
    public bool isInJail { get; protected set; }

    /// <summary>
    /// The number of turns the player has been in jail
    /// </summary>
    protected int turnsInJail = 0;
    /// <summary>
    /// True if the player is able to get out of jail free
    /// </summary>
    public bool getOutOfJailFree = false;
    /// <summary>
    /// Determines whether or not the player can currently buy properties
    /// </summary>
    public bool canPurchaseProperties { get; protected set; } = false;

    /// <summary>
    /// Determines whether or not this counter is controllable by the user.
    /// i.e. for a Human counter this should always be true, otherwise this should be false
    /// </summary>
    virtual public bool isControllable { get { return false; } }

    /// <summary>
    /// This makes the counter's model the model of its index
    /// </summary>
    public void PickModel(int modelNum)
    {
        if (currentModel != null)
        {
            Destroy(currentModel);
        }
        currentModel = GameController.instance.counterModels[modelNum];
        icon = GameController.instance.counterIcons[modelNum];
        Instantiate(currentModel, transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Move());
    }

    /// <summary>
    /// Should be called when this counter should be sent to jail.
    /// Whether or not it actually goes to jail will be determined in this method.
    /// </summary>
    abstract public IEnumerator GoToJail();

    /// <summary>
    /// Have this counter leave jail
    /// </summary>
    public void LeaveJail()
    {
        isInJail = false;
        turnsInJail = 0;
        Debug.Log(name + " has left jail!");
        //Utils.RunAfter(1, GameController.instance.NextTurn);
    }

    /// <summary>
    /// Play this counter's turn
    /// </summary>
    abstract public IEnumerator PlayTurn();

    /// <summary>
    /// Move this counter to the space specified by <paramref name="space"/>
    /// </summary>
    /// <param name="space">The index of the space to move to</param>
    public IEnumerator MoveAbsolute(int space)
    {
        position = space % GameController.instance.spaces.Length;
        return Move();
    }

    /// <summary>
    /// Move this counter by the sum of <paramref name="dice1"/> and <paramref name="dice2"/>
    /// </summary>
    /// <param name="dice1">The first value</param>
    /// <param name="dice2">The second value</param>
    public IEnumerator MoveCounter(int dice1, int dice2)
    {
        int move = dice1 + dice2;
        Debug.Log("for a total of " + move);

        // checks that the counter's new position is within the board limits
        position = position + move;
        if (position > (GameController.instance.spaces.Length) - 1)
        {
            position = position % GameController.instance.spaces.Length;

            // Add 200 cash when the player has moved fully around the board
            portfolio.AddAsset(new Cash(200));
            if (!canPurchaseProperties) Debug.Log(name + " can now buy properties");
            canPurchaseProperties = true;
            Debug.Log(name + " receives 200 for completing a lap of the board.");
        }

        return Move();
    }

    /// <summary>
    /// Perform a physical movement to the space given by <paramref name="position"/>
    /// </summary>
    protected IEnumerator Move()
    {
        float boardPosition = position / GameController.instance.spaces.Length;
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = GameController.instance.spaceControllers[position].waypoints[order].position;
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation;

        if (boardPosition < 0.25)
        {
            targetRotation = Quaternion.Euler(transform.rotation.x, 90, transform.rotation.z);
        }
        else if (boardPosition < 0.50)
        {
            targetRotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        }
        else if (boardPosition < 0.75)
        {
            targetRotation = Quaternion.Euler(transform.rotation.x, 270, transform.rotation.z);
        }
        else
        {
            targetRotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
        }

        float interval = 0.01f;
        float timeToMove = 0.5f;
        for (float t = 0; t <= timeToMove; t += interval)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, t / timeToMove);
            transform.rotation = Quaternion.Euler(Vector3.Lerp(startRotation.eulerAngles, targetRotation.eulerAngles, t / timeToMove));

            yield return new WaitForSeconds(interval);
        }

        GameController.instance.onCounterMove.Invoke(this);
    }

    /// <summary>
    /// Play an auction turn for this counter
    /// </summary>
    virtual public IEnumerator DoAuctionTurn() { yield return null; }
}
