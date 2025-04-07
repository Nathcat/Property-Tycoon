using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class CardTest
{

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator DrawChanceTest()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        GameController.instance.SetupBoard();
        GameController.instance.SetupCounters();
        GameController.instance.SetupCards();

        CounterController counter = GameController.instance.turnCounter;
        int counterlocation = counter.position;
        counter.portfolio.AddAsset(new Cash(500));

        List<Property> list = new List<Property>();
        Property property;
        PropertyGroup thisgroup = GameController.instance.groups[0];
        for (int i = 0; i < thisgroup.GetProperties().Length; i++)
        {
            property = (Property)thisgroup.GetProperties()[i];
            counter.portfolio.AddAsset(new Cash(property.GetValue()));
            list.Add(property);
            property.Purchase(counter);
        }
        list[0].Upgrade();




        GameController.instance.DrawLuck(counter);
        if (counterlocation != counter.position || counter.portfolio.GetCashBalance() != 500)
        {
            Assert.AreEqual(1, 1);
        }
        else
        {
            Assert.AreEqual(1, 2);
        }


    }

    [UnityTest]
    public IEnumerator DrawOpportunityTest()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        GameController.instance.SetupBoard();
        GameController.instance.SetupCounters();
        GameController.instance.SetupCards();

        CounterController counter = GameController.instance.turnCounter;
        int counterlocation = counter.position;
        counter.portfolio.AddAsset(new Cash(500));

        List<Property> list = new List<Property>();
        Property property;
        PropertyGroup thisgroup = GameController.instance.groups[0];
        for (int i = 0; i < thisgroup.GetProperties().Length; i++)
        {
            property = (Property)thisgroup.GetProperties()[i];
            counter.portfolio.AddAsset(new Cash(property.GetValue()));
            list.Add(property);
            property.Purchase(counter);
        }
        list[0].Upgrade();


        GameController.instance.DrawOpportunity(counter);
        if (counterlocation != counter.position || counter.portfolio.GetCashBalance() != 500)
        {
            Assert.AreEqual(1, 1);
        }
        else
        {
            Assert.AreEqual(1, 2);
        }
    }

    [UnityTest]
    public IEnumerator ShuffleOpportunityTest()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        GameController.instance.SetupBoard();
        GameController.instance.SetupCounters();
        GameController.instance.SetupCards();

        Card beforeshuffle = GameController.instance.PeekOpportunity();
        GameController.instance.ShuffleOpportunity();
        Assert.AreNotEqual(beforeshuffle, GameController.instance.PeekOpportunity());
    }

    [UnityTest]
    public IEnumerator ShufflePotluckTest()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        GameController.instance.SetupBoard();
        GameController.instance.SetupCounters();
        GameController.instance.SetupCards();

        Card beforeshuffle = GameController.instance.PeekLuck();
        GameController.instance.ShufflePotluck();
        Assert.AreNotEqual(beforeshuffle, GameController.instance.PeekLuck());
    }
}
