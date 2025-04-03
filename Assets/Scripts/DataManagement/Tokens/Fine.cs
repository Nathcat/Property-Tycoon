using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fine : Command
{
    public Fine(string value) : base(value) { }

    override public void Execute(CounterController counterController, Argument[] args)
    {
        Cash fine = new Cash(int.Parse(args[0].value));

        if (counterController.portfolio.GetCashBalance() >= fine.GetValue())
        {
            counterController.portfolio.RemoveCash(fine);
            GameController.instance.freeParking.AddCash(fine);
        }
        else
        {
            // TODO Ask the player to sell their assets to meet the fine here
            Debug.LogWarning(counterController.name + " is fined " + args[0].value + ", but they cannot afford it!");
        }

        Debug.Log(counterController.name + " is fined " + args[0].value);
    }
}
