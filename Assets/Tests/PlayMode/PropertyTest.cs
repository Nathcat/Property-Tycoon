using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;
using static UnityEditor.Progress;

public class PropertyTest
{

    [UnityTest]
    public IEnumerator MortgageTest()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        GameController.instance.SetupBoard();
        GameController.instance.SetupCounters();
        CounterController counter = GameController.instance.turnCounter;

        Space[] spaces = GameController.instance.spaces;
        Property property = (Property)GameController.instance.groups[1].GetProperties()[0];

        int value = property.GetValue();
        counter.portfolio.AddAsset(new Cash(property.GetValue()));
        property.Purchase(counter);
        Cash cash = property.Mortgage();
        Assert.AreEqual(cash.GetValue(), (property.GetValue()));
        Assert.AreEqual(value / 2, (property.GetValue()));
        Assert.IsTrue(property.isMortgaged);
        Assert.IsTrue(property.CanUnMortgage());
    }

    [UnityTest]
    public IEnumerator UnMortgageTest()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        GameController.instance.SetupBoard();
        GameController.instance.SetupCounters();
        CounterController counter = GameController.instance.turnCounter;

        Space[] spaces = GameController.instance.spaces;
        Property property = (Property)GameController.instance.groups[1].GetProperties()[0];

        int value = property.GetValue();
        counter.portfolio.AddAsset(new Cash(property.GetValue()));
        property.Purchase(counter);
        Cash cash = property.Mortgage();

        property.UnMortgage();
        Assert.AreNotEqual(value, (property.GetValue()) * 2);
    }

    [UnityTest]
    public IEnumerator UpgradeTest()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        GameController.instance.SetupBoard();
        GameController.instance.SetupCounters();

        List<Property> list = new List<Property>();
        Property property;
        CounterController counter = GameController.instance.turnCounter;
        PropertyGroup thisgroup = GameController.instance.groups[0];

        int i;
        for (i = 0; i < thisgroup.GetProperties().Length; i++)
        {
            property = (Property)thisgroup.GetProperties()[i];
            counter.portfolio.AddAsset(new Cash(property.GetValue()));
            list.Add(property);
            property.Purchase(counter);
        }

        Assert.AreEqual(0, list[0].upgradeLevel);

        counter.portfolio.AddAsset(new Cash(list[0].upgradeCost));
        list[0].Upgrade();
        Assert.IsFalse(list[0].CanUpgrade());
        i = 0;
        foreach (var item in list)
        {
            counter.portfolio.AddAsset(new Cash(list[i].upgradeCost));
            item.Upgrade();
            i += 1;
        }
        list[0].Upgrade();
        Assert.AreEqual(2, list[0].upgradeLevel);

    }


    [UnityTest]
    public IEnumerator UpgradeLimitTest()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        GameController.instance.SetupBoard();
        GameController.instance.SetupCounters();

        List<Property> list = new List<Property>();
        Property property;
        CounterController counter = GameController.instance.turnCounter;
        PropertyGroup thisgroup = GameController.instance.groups[0];

        property = (Property)thisgroup.GetProperties()[0];
        counter.portfolio.AddAsset(new Cash(property.GetValue()));
        list.Add(property);
        property.Purchase(counter);

        Assert.IsFalse(property.CanUpgrade());
        int i;
        for (i = 1; i < thisgroup.GetProperties().Length; i++)
        {
            property = (Property)thisgroup.GetProperties()[i];
            counter.portfolio.AddAsset(new Cash(property.GetValue()));
            list.Add(property);
            property.Purchase(counter);
        }

        counter.portfolio.AddAsset(new Cash(list[0].upgradeCost));
        list[0].Upgrade();
        i = 0;
        foreach (var item in list)
        {
            counter.portfolio.AddAsset(new Cash(list[i].upgradeCost));
            item.Upgrade();
            i += 1;
        }
        list[0].Upgrade();
        Assert.IsFalse(list[0].CanUpgrade());
        Assert.AreEqual(2, list[0].upgradeLevel);
    }

    [UnityTest]
    public IEnumerator DowngradeTest()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        GameController.instance.SetupBoard();
        GameController.instance.SetupCounters();

        List<Property> list = new List<Property>();
        Property property;
        CounterController counter = GameController.instance.turnCounter;
        PropertyGroup thisgroup = GameController.instance.groups[0];

        int i = 0;
        for (i = 0; i < thisgroup.GetProperties().Length; i++)
        {
            property = (Property)thisgroup.GetProperties()[i];
            counter.portfolio.AddAsset(new Cash(property.GetValue()));
            list.Add(property);
            property.Purchase(counter);
        }

        Assert.IsFalse(list[0].CanDowngrade());
        counter.portfolio.AddAsset(new Cash(list[0].upgradeCost));
        list[0].Upgrade();

        i = 0;
        foreach (var item in list)
        {
            counter.portfolio.AddAsset(new Cash(list[i].upgradeCost));
            item.Upgrade();
            i += 1;
        }
        list[0].Upgrade();


        Assert.IsFalse(list[1].CanDowngrade());
        Assert.IsTrue(list[0].CanDowngrade());
        list[0].Downgrade();
        Assert.AreEqual(1, list[0].upgradeLevel);
        Assert.AreEqual(counter.portfolio.GetCashBalance(), list[1].upgradeCost);
        Assert.IsTrue(list[1].CanDowngrade());


    }

    [UnityTest]
    public IEnumerator SellTest()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        GameController.instance.SetupBoard();
        GameController.instance.SetupCounters();
        CounterController counter = GameController.instance.turnCounter;

        Space[] spaces = GameController.instance.spaces;
        Property property = (Property)GameController.instance.groups[1].GetProperties()[0];

        int value = property.GetValue();
        counter.portfolio.AddAsset(new Cash(property.GetValue()));
        property.Purchase(counter);

        Assert.IsTrue(property.CanSell());
        property.Sell();
        Assert.IsFalse(property.isOwned);


    }

}
