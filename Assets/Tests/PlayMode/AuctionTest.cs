using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class AuctionTest
{

    //work in progress atm, currently throws an error on line 28 of AuctionManager.cs
    //[UnityTest]
    public IEnumerator TestBid()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        GameController.instance.SetupBoard();
        GameController.instance.SetupCounters();

        PropertyGroup thisgroup = GameController.instance.groups[0];
        Property property = (Property)thisgroup.GetProperties()[0];
        CounterController counter1 = GameController.instance.turnCounter;
        counter1.MoveAbsolute(12);
        GameUIManager.instance.StartAuction();

    }
}
