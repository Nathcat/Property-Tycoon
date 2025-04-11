using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class AuctionTest
{

    [UnityTest]
    public IEnumerator TestBid()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        GameController.instance.SetupBoard();
        GameController.instance.SetupCounters();

        PropertyGroup thisgroup = GameController.instance.groups[0];
        Property property = (Property)thisgroup.GetProperties()[0];
        CounterController counter1 = GameController.instance.turnCounter;
        counter1.portfolio.AddAsset(new Cash(20));
        CounterController counter2 = GameController.instance.counters[1];
        CounterController counter3 = GameController.instance.counters[2];
        CounterController counter4 = GameController.instance.counters[3];
        CounterController counter5 = GameController.instance.counters[4];
        CounterController counter6 = GameController.instance.counters[5];

        AuctionManager auction = GameUIManager.instance.auctionManager;
        counter1.MoveAbsolute(property.position);
        GameUIManager.instance.StartAuction();
        foreach (var item in GameController.instance.counters)
        {
            if (item == counter1)
            {
                Debug.Log(counter1.portfolio.GetCashBalance());
                auction.Bid1();
            }
            else
            {
                auction.Withdraw();
            }
        }
        Assert.AreEqual(1, counter1.portfolio.GetProperties().Count);

    }

    [UnityTest]
    public IEnumerator TestCompetingBids()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        GameController.instance.SetupBoard();
        GameController.instance.SetupCounters();

        PropertyGroup thisgroup = GameController.instance.groups[0];
        Property property = (Property)thisgroup.GetProperties()[0];
        CounterController counter1 = GameController.instance.turnCounter;
        counter1.portfolio.AddAsset(new Cash(20));
        CounterController counter2 = GameController.instance.counters[1];
        counter2.portfolio.AddAsset(new Cash(20));
        CounterController counter3 = GameController.instance.counters[2];
        CounterController counter4 = GameController.instance.counters[3];
        CounterController counter5 = GameController.instance.counters[4];
        CounterController counter6 = GameController.instance.counters[5];

        AuctionManager auction = GameUIManager.instance.auctionManager;
        counter1.MoveAbsolute(property.position);
        GameUIManager.instance.StartAuction();
        foreach (var item in GameController.instance.counters)
        {
            if (item == counter1 || item == counter2)
            {
                if (item == counter1)
                {
                    auction.Bid1();
                }
                else
                {
                    auction.Bid10();
                }
            }
            else
            {
                auction.Withdraw();
            }

        }
        auction.Withdraw();

        Assert.AreEqual(1, counter2.portfolio.GetProperties().Count);

    }
}
