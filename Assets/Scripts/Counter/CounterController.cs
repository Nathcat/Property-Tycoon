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

    /// <summary> The space that the counter is currently on </summary>
    public Space space { get { return GameController.instance.spaces[position]; } }

    /// <summary> The space controller that the counter is currently on </summary>
    public SpaceController spaceController { get { return GameController.instance.spaceControllers[position]; } }

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
    ///     When called, the method rolls two 6 sided dice, outputting both results, along with the sum of the values.
    /// </summary>
    public void PlayTurn()
    {
        // Gets the first dice's value
        int dice1 = Random.Range(1, 7);
        // Gets the second dice's value
        int dice2 = Random.Range(1, 7);
        // Gets the total of both dice
        int output = dice1 + dice2;
        Debug.Log("for a total of " + output);

        // checks that the counter's new position is within the board limits
        position = position + output;
        if (position > (GameController.instance.spaces.Length)-1)
            position = position%GameController.instance.spaces.Length;

        Move();

        Utils.RunAfter(1, GameController.instance.NextTurn);
    }

    /// <summary> Move this counter to the space specified in <see cref="position"/> </summary>
    private void Move()
    {
        transform.position = spaceController.waypoints[order].position;
    }
}
