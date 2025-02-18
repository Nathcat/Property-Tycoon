using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BoardGeneratorTest : MonoBehaviour
{
    public GameObject normalSpace;
    public GameObject cornerSpace;

    // Start is called before the first frame update
    void Start()
    {
        List<Space> spaces = new List<Space>();
        List<PropertyGroup> groups = new List<PropertyGroup>();
        FileManager.ReadBoardCSV(Path.Combine(Application.dataPath, "board.csv"), spaces, groups);
        BoardGenerator.GenerateBoard(transform, 2, 1, normalSpace, cornerSpace, spaces.ToArray());
    }
}
