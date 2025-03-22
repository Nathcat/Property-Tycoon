using System.Collections.Generic;
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
    
    // Start is called before the first frame update
    void Start()
    {
        Move();
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    /// <summary>
    /// Rolls both dice, and moves the counter. If the roll is a double roll, the dice are rolled again and counter moved again.
    /// </summary>
    public void PlayTurn()
    {
        rollData roll = RollDice();
        MoveCounter(roll.dice1, roll.dice2);
        if (roll.doubleRoll)
        {
            Debug.Log("Double roll");
            roll = RollDice();
            MoveCounter(roll.dice1, roll.dice2);
            // add in triple roll for prison
        }
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
        if (position > (GameController.instance.spaces.Length)-1)
            position = position%GameController.instance.spaces.Length;

        Move();

        Utils.RunAfter(1, GameController.instance.NextTurn);
    }

    /// <summary>
    /// Rolls two dice, and returns them, along with whether the dice rolls are the same.
    /// </summary>
    /// <returns> a record containing the two dice rolls as integers, as well as a boolean to denote whether the dice rolls were the same. </returns>
    public rollData RollDice()
    {
        // Gets the first dice's value
        int dice1 = Random.Range(1, 7);
        // Gets the second dice's value
        int dice2 = Random.Range(1, 7);
        
        if(dice1 == dice2)
        {
            return new rollData(dice1, dice2, true);
        }
        else
        {
            return new rollData(dice1, dice2, false);
        }
    }



    /// <summary> Move this counter to the space specified in <see cref="position"/> </summary>
    private void Move()
    {
        transform.position = GameController.instance.spaceControllers[position].waypoints[order].position;
    }
    /// <summary>
    /// A record used to return dice roll data.
    /// </summary>
    /// <param name="dice1"> First dice value. </param>
    /// <param name="dice2"> Second dice value. </param>
    /// <param name="doubleRoll"> Whether the dice rolls are the same. </param>
    public record RollData(int dice1, int dice2, bool doubleRoll);
}
