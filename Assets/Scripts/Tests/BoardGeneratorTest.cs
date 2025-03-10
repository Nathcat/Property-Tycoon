using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class BoardGeneratorTest : MonoBehaviour
{
    public GameObject normalSpace;
    public GameObject cornerSpace;
    public CameraController cameraController;

    void Start()
    {
        var data = FileManager.ReadBoardCSV(Path.Combine(Application.dataPath, "board.csv"));
        BoardGenerator.GenerateBoard(transform, 2, 1, normalSpace, cornerSpace, data.spaces);
    }
}
