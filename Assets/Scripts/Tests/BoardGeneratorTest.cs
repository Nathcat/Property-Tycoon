using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BoardGeneratorTest : MonoBehaviour
{
    public GameObject normalSpace;
    public GameObject cornerSpace;
    public CameraController cameraController;

    // Start is called before the first frame update
    void Start()
    {
        List<Space> spaces = new List<Space>();
        List<PropertyGroup> groups = new List<PropertyGroup>();
        FileManager.ReadBoardCSV(Path.Combine(Application.dataPath, "board.csv"), spaces, groups);
        BoardGenerator.GenerateBoard(transform, 2, 1, normalSpace, cornerSpace, spaces.ToArray());

        // CameraController requires radius to the inside of the board, this is the board dimension / 2, and subtract the length of the spaces
        float r = (BoardGenerator.GetBoardDimensions(spaces.Count) / 2) - 2;
        Debug.Log("Board radius is " + r);
        cameraController.SetBoardRadius(r);
    }
}
