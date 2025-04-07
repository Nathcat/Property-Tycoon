using UnityEngine;

public class CollectFreeParking : Command
{
    public CollectFreeParking(string value) : base(value) { }

    override public void Execute(CounterController counterController, Argument[] args)
    {
        Debug.Log(counterController.name + " collects free parking, worth " + GameController.instance.freeParking.GetValue());

        counterController.portfolio.AddAsset(GameController.instance.freeParking);
        GameController.instance.freeParking = new Cash(0);
    }
}
