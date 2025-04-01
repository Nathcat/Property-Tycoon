using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeOppoKnocks : Command
{
    public TakeOppoKnocks(string value) : base(value) {}
    
    override public void Execute(CounterController counterController, Argument[] args) {
        GameController.instance.DrawOpportunity(counterController);
    }
}
