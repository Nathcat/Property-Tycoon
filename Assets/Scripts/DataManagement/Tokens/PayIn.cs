using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PayIn : Command
{
    public PayIn(string value) : base(value) {}

    override public void Execute(CounterController counterController, Argument[] args) {
        counterController.portfolio.RemoveCash(new Cash(int.Parse(args[0].value)));
    }
}
