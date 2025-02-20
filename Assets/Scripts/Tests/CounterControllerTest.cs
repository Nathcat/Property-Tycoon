using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CounterControllerTest : MonoBehaviour
{
    public GameObject normalSpace;
    public GameObject cornerSpace;
    private CounterController controller = new CounterController();
    List<Space> spaces = new List<Space>();

    // Start is called before the first frame update
    void Start()
    {
        
        List<PropertyGroup> groups = new List<PropertyGroup>();
        FileManager.ReadBoardCSV(Path.Combine(Application.dataPath, "board.csv"), spaces, groups);
        BoardGenerator.GenerateBoard(transform, 2, 1, normalSpace, cornerSpace, spaces.ToArray());
    }

    // Update is called once per frame
    void Update()
    {
        controller.PlayTurn(spaces);
    }
}
