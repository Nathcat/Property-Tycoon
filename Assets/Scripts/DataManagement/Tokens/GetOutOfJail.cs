using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetOutOfJail : Command
{
    public GetOutOfJail(string value) : base(value) {}
    
    override public void Execute(CounterController counterController, Argument[] args) {
        counterController.LeaveJail();
        Debug.Log(counterController.gameObject.name + " leaves jail.");
    }
}
