using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectFreeParking : Command
{
    public CollectFreeParking(string value) : base(value) {}
    
    override public void Execute(CounterController counterController, Argument[] args) {
        counterController.portfolio.AddAsset(GameController.instance.freeParking);
        GameController.instance.freeParking = new Cash(0);
    }
}
