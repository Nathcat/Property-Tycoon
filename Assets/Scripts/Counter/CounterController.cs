using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;


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

    /// <summary>
    /// The name of the game object this controller is attached to
    /// </summary>
    new public string name { get { return gameObject.name; } }

    /// <summary>
    /// Stores the last roll performed by this counter
    /// </summary>
    public GameUIManager.RollData lastRoll { get { return GameUIManager.instance.lastDiceRoll; } }

    public bool isInJail { get; private set; }

    /// <summary>
    /// The number of turns the player has been in jail
    /// </summary>
    private int turnsInJail = 0;
    /// <summary>
    /// True if the player is able to get out of jail free
    /// </summary>
    public bool getOutOfJailFree = false;
    /// <summary>
    /// Determines whether or not the player can currently buy properties
    /// </summary>
    public bool canPurchaseProperties { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        Move();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator GoToJail()
    {
        MoveAbsolute(GameController.instance.jailSpace.position);

        Debug.Log(name + " has gone to jail, can they pay?");

        if (getOutOfJailFree)
        {
            Debug.Log("... they have a get out of jail free card!");
            //Utils.RunAfter(1, GameController.instance.NextTurn);
            yield return new WaitForSeconds(1f);
        }

        if (portfolio.GetCashBalance() >= 50)
        {
            Debug.Log("... they can pay, asking if they want to...");

            yield return GameUIManager.instance.YesNoPrompt("Pay £50 to get out of jail?");
            bool reply = GameUIManager.instance.promptState.response;

            if (reply)
            {
                Cash fine = new Cash(50);
                portfolio.RemoveCash(fine);
                GameController.instance.freeParking.AddCash(fine);
                Debug.Log(name + " pays to leave jail!");

                //Utils.RunAfter(1, GameController.instance.NextTurn);
            }
            else
            {
                isInJail = true;
                Debug.Log(name + " is now in jail! Jail space is at position " + GameController.instance.jailSpace.position);

                //Utils.RunAfter(1, GameController.instance.NextTurn);
            }
        }
        else
        {
            Debug.Log("... they cannot pay.");
            isInJail = true;
            Debug.Log(name + " is now in jail! Jail space is at position " + GameController.instance.jailSpace.position);
            //Utils.RunAfter(1, GameController.instance.NextTurn);
            yield return new WaitForSeconds(1f);
        }
    }

    public void LeaveJail()
    {
        isInJail = false;
        turnsInJail = 0;
        Debug.Log(name + " has left jail!");
        //Utils.RunAfter(1, GameController.instance.NextTurn);
    }

    /// <summary>
    /// Rolls both dice, and moves the counter. If the roll is a double roll, the dice are rolled again and counter moved again.
    /// </summary>
    public IEnumerator PlayTurn()
    {
        if (!isInJail)
        {
            // Roll three times
            yield return GameUIManager.instance.RollDice();

            MoveCounter(lastRoll.dice1, lastRoll.dice2);
            int doubles = 0;
            while (lastRoll.doubleRoll)
            {
                doubles++;

                if (doubles == 3)
                {
                    break;
                }

                yield return GameUIManager.instance.RollDice();

                MoveCounter(lastRoll.dice1, lastRoll.dice2);
            }

            // If 3 doubles have been rolled, go to jail
            if (doubles == 3)
            {
                Debug.Log(name + " has rolled 3 doubles, going to jail!");
                yield return GoToJail();
                yield break;
            }

            int oldPos = position;
            do
            {
                oldPos = position;
                yield return GameController.instance.spaces[position].action.Run(this);
            } while (oldPos != position);


            Space space = GameController.instance.spaces[position];

            if (space is Property)
            {
                Property p = space as Property;

                if (!p.isOwned && canPurchaseProperties)
                {
                    if (p.CanPurchase(this))
                    {
                        yield return GameUIManager.instance.YesNoPrompt("Would you like to buy " + p.name + " for £" + p.GetValue() + "?");

                        bool reply = GameUIManager.instance.promptState.response;
                        if (reply)
                        {
                            p.Purchase(this);
                            Debug.Log(name + " has purchased " + p.name);
                        }
                        else
                        {
                            GameUIManager.instance.StartAuction();
                        }
                    }
                    else
                    {
                        GameUIManager.instance.StartAuction();
                    }
                }
            }
        }
        else
        {
            turnsInJail++;

            Debug.Log(name + " has been in jail for " + turnsInJail + " turns");
            if (turnsInJail == 2)
            {
                Debug.Log(name + " has done their time in jail!");
                LeaveJail();
            }
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
            if (!canPurchaseProperties) Debug.Log(name + " can now buy properties");
            canPurchaseProperties = true;
            Debug.Log(name + " receives 200 for completing a lap of the board.");
        }

        Move();
    }

    /// <summary> Move this counter to the space specified in <see cref="position"/> </summary>
    private void Move()
    {
        Debug.Log("for player " + name + ", the position is " + position + " and the order is " + order);
        transform.position = GameController.instance.spaceControllers[position].waypoints[order].position;

        GameController.instance.onCounterMove.Invoke(this);
    }
}
