using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayIn : Command
{
    public PayIn(string value) : base(value) { }

    override public IEnumerator Execute(CounterController counterController, Argument[] args)
    {
        Cash fine = new Cash(int.Parse(args[0].value));

        if (counterController.portfolio.GetCashBalance() >= fine.GetValue())
        {
            Debug.Log(counterController.name + " pays " + args[0].value + " to the bank");
            counterController.portfolio.RemoveCash(fine);
        }
        else
        {
            // TODO Should ask the player to sell their assets here
            Debug.LogWarning(counterController.name + " must pay " + args[0].value + " to the bank, but they cannot afford it!");
        }

        yield break;
    }
}
