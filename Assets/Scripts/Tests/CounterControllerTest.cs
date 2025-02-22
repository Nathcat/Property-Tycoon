using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
/// <summary>
/// A class to test the ConterController in its most basic form
/// </summary>
public class CounterControllerTest : MonoBehaviour
{
    /// <param name="normalSpace"> Standard board space</param>
    public GameObject normalSpace;
    /// <param name="cornerSpace"> Corner board space</param>
    public GameObject cornerSpace;
    /// <param name="controller"> CounterController used for testing</param>
    public CounterController controller;
    /// <param name="space"> List of spaces that make up the board</param>
    private List<Space> spaces = new List<Space>();

    /// <summary>
    /// Start sets up the board, sets controller as the test counter's controller, and starts calling counter's PlayTurn method.
    /// </summary>
    void Start()
    {
        controller = GameObject.Find("TestCounter").gameObject.GetComponent<CounterController>();
        List<PropertyGroup> groups = new List<PropertyGroup>();
        FileManager.ReadBoardCSV(Path.Combine(Application.dataPath, "board.csv"), spaces, groups);
        BoardGenerator.GenerateBoard(transform, 2, 1, normalSpace, cornerSpace, spaces.ToArray());
        Debug.Log("The board has " + spaces.Count + " spaces");

        StartCoroutine(Delay());
            
        
    }

    /// <summary>
    /// Allows PlayTurn to be constantly called while the L/R arrow keys are held down - if for some reason you want to test that!
    /// </summary>
    void Update()
    {
        
        
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput != 0) 
        {
            controller.PlayTurn(spaces);
        }
        
        
    }

    /// <summary>
    /// Calls controller's PlayTurn method, with a delay of 1s between each call.
    /// </summary>
    /// <returns>Returns the delay between turns</returns>
    public IEnumerator Delay()
    {
        controller.PlayTurn(spaces);
        yield return new WaitForSeconds(1);
        StartCoroutine(Delay());
    }
}
