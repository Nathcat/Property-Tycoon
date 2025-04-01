using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : Command
{
    public Move(string value) : base(value) {}

    override public void Execute(CounterController counterController, Argument[] args) {
        if (args[0].value.ToLower() == "absolute") {
            counterController.MoveAbsolute(int.Parse(args[1].value));
        }
        else if (args[0].value.ToLower() == "relative") {
            counterController.MoveAbsolute(int.Parse(args[1].value) + counterController.position);
        }
        else {
            Debug.LogError("Invalid arguments");
        }
    }
}
