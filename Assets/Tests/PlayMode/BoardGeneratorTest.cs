using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class BoardGeneratorTest
{
    public GameObject normalSpace;
    public GameObject cornerSpace;
    public CameraController cameraController;

    [UnityTest]
    public IEnumerator TestBoardGeneration()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        try
        {
            GameController.instance.SetupBoard();
            Assert.AreEqual(1, 1);
        }
        catch
        {
            Assert.AreEqual(1, 2);
        }
    }
}
