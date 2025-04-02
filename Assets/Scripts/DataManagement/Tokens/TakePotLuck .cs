using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakePotLuck : Command
{
    public TakePotLuck(string value) : base(value) {}
    
    override public void Execute(CounterController counterController, Argument[] args) {
        GameController.instance.DrawLuck(counterController);
    }
}
