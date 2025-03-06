using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// CounterController: A class used to control a counter, either controlled by a player or an AI.
/// </summary>
public class CounterController : MonoBehaviour
{
    /// <param name="portfolio"> The portfolio of the counter, containing any owned money and properties.</param>
    private Portfolio portfolio = new Portfolio();
    private int position;
    

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    ///     When called, the method rolls two 6 sided dice, outputting both results, along with the sum of the values.
    /// </summary>
    public void PlayTurn(Space[] board)
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
        if (position > (board.Length)-1)
        {
            position = position%board.Length;
        }


        // finds the new position of the counter
        GameObject finalSpace = GameObject.Find("Board").transform.GetChild(position).gameObject;
        Vector3 finalPos = finalSpace.transform.position;

        //moves the counter to the new position
        transform.position = finalPos;

    }

    
}
