using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fine : Command
{
    public Fine(string value) : base(value) {}

    override public void Execute(CounterController counterController, Argument[] args) {
        GameController.instance.freeParking.AddCash(new Cash(int.Parse(args[0].value)));
    }
}
