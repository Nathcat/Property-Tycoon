using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayPerUpgrade : Command
{
    public PayPerUpgrade(string value) : base(value) {}
    
    override public void Execute(CounterController counterController, Argument[] args) {
        int houseCost = int.Parse(args[0].value);
        int hotelCost = int.Parse(args[1].value);
        int totalCost = 0;

        foreach (Property p in counterController.portfolio.GetProperties()) {
            totalCost += p.upgradeLevel == 5 ? hotelCost : (houseCost * p.upgradeLevel);
        }

        counterController.portfolio.RemoveCash(new Cash(totalCost));
    }
}
