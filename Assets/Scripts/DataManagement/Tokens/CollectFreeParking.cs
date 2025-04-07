using System.Collections;
using UnityEngine;

public class CollectFreeParking : Command
{
    public CollectFreeParking(string value) : base(value) {}
    
    override public IEnumerator Execute(CounterController counterController, Argument[] args) {
        Debug.Log(counterController.name + " collects free parking, worth " + GameController.instance.freeParking.GetValue());

        counterController.portfolio.AddAsset(GameController.instance.freeParking);
        GameController.instance.freeParking = new Cash(0);

        yield break;
    }
}
