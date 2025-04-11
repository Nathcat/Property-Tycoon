using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
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
        GameController.instance.ShufflePotluck();

        Card after1shuffle = GameController.instance.PeekOpportunity();
        GameController.instance.ShufflePotluck();

        Card after2shuffle = GameController.instance.PeekOpportunity();
        GameController.instance.ShufflePotluck();

        bool isshuffled = beforeshuffle.Equals(GameController.instance.PeekOpportunity());
        bool isshuffled2 = after1shuffle.Equals(after2shuffle);
        bool final = isshuffled.Equals(isshuffled2);
        Assert.IsTrue(final);
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

        Card after1shuffle = GameController.instance.PeekLuck();
        GameController.instance.ShufflePotluck();

        Card after2shuffle = GameController.instance.PeekLuck();
        GameController.instance.ShufflePotluck();

        bool isshuffled = beforeshuffle.Equals(GameController.instance.PeekLuck());
        bool isshuffled2 = after1shuffle.Equals(after2shuffle);
        bool final = isshuffled.Equals(isshuffled2);
        Assert.IsTrue(final);
    }

    [UnityTest]
    public IEnumerator GetOutOfJailTest()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        GameController.instance.SetupBoard();
        GameController.instance.SetupCounters();
        GameController.instance.SetupCards();
        CounterController counter = GameController.instance.turnCounter;


    }
}
