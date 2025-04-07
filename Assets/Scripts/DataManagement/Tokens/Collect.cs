using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collect : Command
{
    public Collect(string value) : base(value) {}

    override public IEnumerator Execute(CounterController counterController, Argument[] args) {
        Cash cost = new Cash(int.Parse(args[0].value));
        Cash total = new Cash(0);
        foreach (CounterController counter in GameController.instance.counters) {
            if (counter == counterController) continue;
            else if (counter.portfolio.GetCashBalance() < cost.GetValue()) {
                // TODO Ask player to sell assets to meet cost
            }

            total.AddCash(counter.portfolio.RemoveCash(cost));
        }

        counterController.portfolio.AddAsset(total);

        Debug.Log(counterController.gameObject.name + " collects " + cost.GetValue() + " from each player, for a total of " + total.GetValue());

        yield break;
    }
}
