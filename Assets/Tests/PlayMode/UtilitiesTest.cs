using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class UtilitiesTest
{
    [UnityTest]
    public IEnumerator UtilitiesRentTest()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        GameController.instance.SetupBoard();
        GameController.instance.SetupCounters();

        List<Utility> list = new List<Utility>();
        Utility property;
        CounterController counter = GameController.instance.turnCounter;
        CounterController victim = GameController.instance.counters[1];
        PropertyGroup thisgroup = GameController.instance.groups[1];

        victim.portfolio.AddAsset(new Cash(1000));

        int i = 0;
        foreach (var item in GameController.instance.groups)
        {
            if (item.name.Equals("Utilities"))
            {
                thisgroup = GameController.instance.groups[i];
                break;
            }
            i++;

        }
        counter.StartCoroutine(GameUIManager.instance.RollDice());
        victim.MoveAbsolute(thisgroup.GetProperties()[0].position);

        property = (Utility)thisgroup.GetProperties()[0];
        counter.portfolio.AddAsset(new Cash(property.GetValue()));
        list.Add(property);
        property.Purchase(counter);
        victim.StartCoroutine(property.action.Run(victim));

        Assert.AreEqual((1000 - (victim.lastRoll.dice1 + victim.lastRoll.dice2) * 4) + Portfolio.STARTING_CASH, victim.portfolio.GetCashBalance());
    }

    [UnityTest]
    public IEnumerator UtilitiesChangeRentTest()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        GameController.instance.SetupBoard();
        GameController.instance.SetupCounters();

        List<Utility> list = new List<Utility>();
        Utility property;
        CounterController counter = GameController.instance.turnCounter;
        CounterController victim = GameController.instance.counters[1];
        PropertyGroup thisgroup = GameController.instance.groups[1];

        victim.portfolio.AddAsset(new Cash(1000));


        int i = 0;
        foreach (var item in GameController.instance.groups)
        {
            if (item.name.Equals("Utilities"))
            {
                thisgroup = GameController.instance.groups[i];
                break;
            }
            i++;
        }
        Debug.Log(thisgroup.name);
        counter.StartCoroutine(GameUIManager.instance.RollDice());
        victim.MoveAbsolute(thisgroup.GetProperties()[0].position);
        property = (Utility)thisgroup.GetProperties()[0];
        counter.portfolio.AddAsset(new Cash(property.GetValue()));
        list.Add(property);
        property.Purchase(counter);

        counter.StartCoroutine(property.action.Run(victim));

        int oneutilityrent = ((victim.lastRoll.dice1 + victim.lastRoll.dice2) * 4);


        property = (Utility)thisgroup.GetProperties()[1];
        counter.portfolio.AddAsset(new Cash(property.GetValue()));
        list.Add(property);
        property.Purchase(counter);
        victim.StartCoroutine(property.action.Run(victim));
        int twoutilityrent = ((victim.lastRoll.dice1 + victim.lastRoll.dice2) * 10);


        Assert.AreEqual((1000 - (oneutilityrent + twoutilityrent)) + Portfolio.STARTING_CASH, victim.portfolio.GetCashBalance());
    }
}
