using UnityEngine;

public class PayPerUpgrade : Command
{
    public PayPerUpgrade(string value) : base(value) {}
    
    override public IEnumerator Execute(CounterController counterController, Argument[] args) {
        int houseCost = int.Parse(args[0].value);
        int hotelCost = int.Parse(args[1].value);
        int totalCost = 0;

        foreach (Property p in counterController.portfolio.GetProperties())
        {
            totalCost += p.upgradeLevel == 5 ? hotelCost : (houseCost * p.upgradeLevel);
        }

        Debug.Log(counterController.name + " pays " + houseCost + " per house and " + hotelCost + " per hotel, for a total of " + totalCost);
        counterController.portfolio.RemoveCash(new Cash(totalCost));

        yield break;
    }
}
