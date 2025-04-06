using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using System.IO;
using UnityEngine;
using UnityEngine.TestTools;
using System;
using System.Diagnostics;
using JetBrains.Annotations;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;

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
        } catch 
        {
            Assert.AreEqual(1,2);
        }
    }
}
