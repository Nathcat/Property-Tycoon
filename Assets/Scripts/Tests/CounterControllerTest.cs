using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CounterControllerTest : MonoBehaviour
{
    public GameObject normalSpace;
    public GameObject cornerSpace;
    //public CounterController controller;
    public CounterController controller;
    private List<Space> spaces = new List<Space>();

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.Find("TestCounter").gameObject.GetComponent<CounterController>();
        List<PropertyGroup> groups = new List<PropertyGroup>();
        FileManager.ReadBoardCSV(Path.Combine(Application.dataPath, "board.csv"), spaces, groups);
        BoardGenerator.GenerateBoard(transform, 2, 1, normalSpace, cornerSpace, spaces.ToArray());
        Debug.Log("The board has " + spaces.Count + " spaces");

        int i = 0;

        StartCoroutine(Delay());
            
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput != 0) 
        {
            controller.PlayTurn(spaces);
        }
        */
        
    }

    public void DelayedPlayTurn()
    {
        
    }
    
    public IEnumerator Delay()
    {
        controller.PlayTurn(spaces);
        yield return new WaitForSeconds(1);
        StartCoroutine(Delay());
    }
}
