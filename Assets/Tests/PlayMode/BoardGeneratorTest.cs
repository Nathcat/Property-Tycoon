using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.TestTools;

public class BoardGeneratorTest
{
    public GameObject normalSpace;
    public GameObject cornerSpace;
    public CameraController cameraController;

    [UnityTest]
    public IEnumerator TestBoardGeneration()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        try
        {
            var data = FileManager.ReadBoardCSV(Path.Combine(Application.dataPath, "board.csv"));
            BoardGenerator.GenerateBoard(null, 2, 1, normalSpace, cornerSpace, data.spaces);
        }
        catch
        {

        }


        yield return null;
    }
}
