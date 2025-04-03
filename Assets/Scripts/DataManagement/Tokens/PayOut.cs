using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayOut : Command
{
    public PayOut(string value) : base(value) {}

    override public void Execute(CounterController counterController, Argument[] args) {
        Debug.Log(counterController.name + " receives " + args[0].value + " from the bank.");
        counterController.portfolio.AddAsset(new Cash(int.Parse(args[0].value)));
    }
}
