using System.Collections;
using System.Collections.Generic;
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
        victim.MoveAbsolute(6);
        for (i = 0; i < thisgroup.GetProperties().Length; i++)
        {
            property = (Station)thisgroup.GetProperties()[i];
            counter.portfolio.AddAsset(new Cash(property.GetValue()));
            list.Add(property);
            property.Purchase(counter);
            property.action.Run(victim);
            //check if theres (i) less  money in the victim
        }
        Assert.AreEqual(625, victim.portfolio.GetCashBalance());  
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
        victim.MoveAbsolute(6);
        
        property = (Station)thisgroup.GetProperties()[0];
        counter.portfolio.AddAsset(new Cash(property.GetValue()));
        list.Add(property);
        property.Purchase(counter);
        property.action.Run(victim);
        
        Assert.AreEqual(975, victim.portfolio.GetCashBalance());
    }
}
