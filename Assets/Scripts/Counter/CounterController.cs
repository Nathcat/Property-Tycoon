using UnityEngine;


/// <summary>
/// CounterController: A class used to control a counter, either controlled by a player or an AI.
/// </summary>
public class CounterController : MonoBehaviour
{
    /// <summary> The portfolio of the counter, containing any owned money and properties. </summary>
    [HideInInspector] public readonly Portfolio portfolio = new Portfolio();

    /// <summary> The index in <see cref="GameController.spaces"/> that this counter is currently on. </summary>
    [HideInInspector] public int position { get; private set; }

    /// <summary> The index of this counter in <see cref="GameController.counters"/>. </summary>
    public int order { get { return System.Array.IndexOf(GameController.instance.counters, this); } }

    /// <summary> The space that the counter is currently on </summary>
    public Space space { get { return GameController.instance.spaces[position]; } }

    /// <summary> The space controller that the counter is currently on </summary>
    public SpaceController spaceController { get { return GameController.instance.spaceControllers[position]; } }
    /// <summary>
    /// The name of the game object this controller is attached to
    /// </summary>
    public string name { get { return gameObject.name; } }

    /// <summary>
    /// Stores the last roll performed by this counter
    /// </summary>
    public RollData lastRoll { get; private set; }

    public bool isInJail { get; private set; }

    /// <summary>
    /// The number of turns the player has been in jail
    /// </summary>
    private int turnsInJail = 0;

    // Start is called before the first frame update
    void Start()
    {
        Move();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GoToJail()
    {
        MoveAbsolute(GameController.instance.jailSpace.position);

        if (portfolio.GetCashBalance() >= 50 && AskIfPayForJail())
        {
            Cash fine = new Cash(50);
            portfolio.RemoveCash(fine);
            GameController.instance.freeParking.AddCash(fine);
            Debug.Log(name + " pays to leave jail!");

            Utils.RunAfter(1, GameController.instance.NextTurn);
        }
        else
        {
            isInJail = true;
            Debug.Log(name + " is now in jail! Jail space is at position " + GameController.instance.jailSpace.position);
            Utils.RunAfter(1, GameController.instance.NextTurn);
        }
    }

    public void LeaveJail()
    {
        isInJail = false;
        Debug.Log(name + " has left jail!");
        Utils.RunAfter(1, GameController.instance.NextTurn);
    }

    /// <summary>
    /// Ask the player if they want to pay to leave jail
    /// </summary>
    /// <returns></returns>
    public bool AskIfPayForJail()
    {
        // TODO Implement this with UI
        Debug.LogWarning("The player would be asked to pay to leave jail here!");
        return false;
    }

    /// <summary>
    /// Rolls both dice, and moves the counter. If the roll is a double roll, the dice are rolled again and counter moved again.
    /// </summary>
    public void PlayTurn()
    {
        if (!isInJail)
        {
            RollData roll = RollDice();
            lastRoll = roll;
            MoveCounter(roll.dice1, roll.dice2);
            if (roll.doubleRoll)
            {
                Debug.Log("Double roll");
                roll = RollDice();
                lastRoll = roll;
                MoveCounter(roll.dice1, roll.dice2);

                if (roll.doubleRoll)
                {
                    Debug.Log("Another double roll!");
                    roll = RollDice();
                    lastRoll = roll;

                    if (roll.doubleRoll)
                    {
                        Debug.Log("Triple double roll, player goes to jail!");
                        GoToJail();
                        return;
                    }
                    else
                    {
                        MoveCounter(roll.dice1, roll.dice2);
                    }
                }
            }

            // Call the action when landing on a new space, if the counter is moved to a new space as a result of the action
            int oldPos = position;
            do
            {
                oldPos = position;
                GameController.instance.spaces[position].action.Run(this);
            } while (oldPos != position);

            Utils.RunAfter(1, GameController.instance.NextTurn);
        }
        else
        {
            // Wait for 2 turns to leave jail
            turnsInJail++;

            if (turnsInJail == 2)
            {
                LeaveJail();
            }

            Utils.RunAfter(1, GameController.instance.NextTurn);
        }
    }

    /// <summary>
    /// Move the counter to a specific space
    /// </summary>
    /// <param name="space">The index of the space to  move to</param>
    public void MoveAbsolute(int space)
    {
        position = space % GameController.instance.spaces.Length;
        Move();
    }

    /// <summary>
    /// Takes in two integers for the dice rolled, and moves the counter the required number of spaces.
    /// </summary>
    /// <param name="dice1"> First dice value. </param>
    /// <param name="dice2"> Second dice value. </param>
    public void MoveCounter(int dice1, int dice2)
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
            Debug.Log(name + " receives 200 for completing a lap of the board.");
        }

        Move();
    }

    /// <summary>
    /// Rolls two dice, and returns them, along with whether the dice rolls are the same.
    /// </summary>
    /// <returns> a record containing the two dice rolls as integers, as well as a boolean to denote whether the dice rolls were the same. </returns>
    public RollData RollDice()
    {
        // Gets the first dice's value
        int dice1 = Random.Range(1, 7);
        // Gets the second dice's value
        int dice2 = Random.Range(1, 7);

        return new RollData(dice1, dice2, dice1 == dice2);
    }



    /// <summary> Move this counter to the space specified in <see cref="position"/> </summary>
    private void Move()
    {
        transform.position = spaceController.waypoints[order].position;
    }
    /// <summary>
    /// A record used to return dice roll data.
    /// </summary>
    /// <param name="dice1"> First dice value. </param>
    /// <param name="dice2"> Second dice value. </param>
    /// <param name="doubleRoll"> Whether the dice rolls are the same. </param>
    public record RollData(int dice1, int dice2, bool doubleRoll);
}
