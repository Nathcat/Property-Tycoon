using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


/// <summary>
/// CounterController: A class used to control a counter, either controlled by a player or an AI.
/// </summary>
public class CounterController : MonoBehaviour
{
    /// <param name="portfolio"> The portfolio of the counter, containing any owned money and properties.</param>
    private Portfolio portfolio = new Portfolio();
    private int position;
    private System.Random rnd = new System.Random();
    private GameObject token;

    
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
    public void PlayTurn(List<Space> board)
    {
        int dice1 = rnd.Next(1, 7);
        Debug.Log("Dice 1 rolled a " + dice1);
        int dice2 = rnd.Next(1, 7);
        Debug.Log("Dice 2 rolled a " + dice1);
        int output = dice1 + dice2;
        Debug.Log("for a total of " + output);

        position = position + output;
        if (position > board.Count)
        {
            position = position - board.Count;
        }

        Vector3 finalPos = GameObject.Find("Board").transform.GetChild(position).gameObject.transform.position;

        token.gameObject.transform.position = finalPos;

    }

    
}
