using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class PortfolioTest
{

    [UnityTest]
    public IEnumerator AddCashTest()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        GameController.instance.SetupBoard();
        GameController.instance.SetupCounters();
        CounterController counter = GameController.instance.turnCounter;
        counter.portfolio.AddAsset(new Cash(500));
        Assert.AreEqual(counter.portfolio.GetCashBalance(), 500 + Portfolio.STARTING_CASH);
    }

    [UnityTest]
    public IEnumerator AddAssetTest()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        GameController.instance.SetupBoard();
        GameController.instance.SetupCounters();
        CounterController counter = GameController.instance.turnCounter;
        Space[] spaces = GameController.instance.spaces;
        PropertyGroup thisgroup = new PropertyGroup("testgroup");
        Action action = new Action("PropertyRent 10 50 150 450 625 750 ;");
        Property property = new Property(spaces[1].position, spaces[1].name, thisgroup, action, 100, 100);
        property.Purchase(counter);
        Assert.AreNotEqual(counter.portfolio.GetProperties(), null);
    }
}
