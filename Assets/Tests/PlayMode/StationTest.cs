using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class StationTest
{
   

    [UnityTest]
    public IEnumerator StationRentChangeTest()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        GameController.instance.SetupBoard();
        GameController.instance.SetupCounters();

        List<Station> list = new List<Station>();
        Station property;
        CounterController counter = GameController.instance.turnCounter;
        CounterController victim = GameController.instance.counters[1];
        PropertyGroup thisgroup = GameController.instance.groups[1];
        
        victim.portfolio.AddAsset(new Cash(1000));

        int i = 0;
        foreach (var item in GameController.instance.groups)
        {
            if (item.name.Equals("Station"))
            {
                thisgroup = GameController.instance.groups[i];
                break;
            }
            i++;
        }
        Debug.Log(thisgroup.name);
        counter.MoveAbsolute(thisgroup.GetProperties()[i].position);
        Debug.Log(thisgroup.GetProperties().Length);
        for (i = 0; i < thisgroup.GetProperties().Length; i++)
        {

            property = (Station)thisgroup.GetProperties()[i];
            victim.MoveAbsolute(thisgroup.GetProperties()[i].position);
            list.Add(property);
            counter.portfolio.AddAsset(new Cash(property.GetValue()));
            property.Purchase(counter);
            victim.StartCoroutine(property.action.Run(victim));
            Debug.Log(victim.portfolio.GetCashBalance());
            //check if theres (i) less money in the victim
        }
        Assert.AreEqual(625 + Portfolio.STARTING_CASH, victim.portfolio.GetCashBalance());  
    }

    [UnityTest]
    public IEnumerator StationRentTest()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        GameController.instance.SetupBoard();
        GameController.instance.SetupCounters();

        List<Station> list = new List<Station>();
        Station property;
        CounterController counter = GameController.instance.turnCounter;
        CounterController victim = GameController.instance.counters[1];
        PropertyGroup thisgroup = GameController.instance.groups[1];

        victim.portfolio.AddAsset(new Cash(1000));

        int i = 0;
        foreach (var item in GameController.instance.groups)
        {
            if (item.name.Equals("Station"))
            {
                thisgroup = GameController.instance.groups[i];
                break;
            }
            i++;

        }
        Debug.Log(thisgroup.name);
        victim.MoveAbsolute(thisgroup.GetProperties()[0].position);

        property = (Station)thisgroup.GetProperties()[0];
        counter.portfolio.AddAsset(new Cash(property.GetValue()));
        list.Add(property);
        property.Purchase(counter);
        victim.StartCoroutine(property.action.Run(victim));
        
        Assert.AreEqual(975 + Portfolio.STARTING_CASH, victim.portfolio.GetCashBalance());
    }
}
