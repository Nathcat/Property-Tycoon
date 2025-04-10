using System.Collections;
using UnityEngine;

public class Move : Command
{
    public Move(string value) : base(value) { }

    override public IEnumerator Execute(CounterController counterController, Argument[] args) {
        if (args[0].value.ToLower() == "absolute") {
            counterController.MoveAbsolute(int.Parse(args[1].value) - 1);
            Debug.Log(counterController.name + " moves to space number " + args[1].value);
        }
        else if (args[0].value.ToLower() == "relative")
        {
            counterController.MoveAbsolute(int.Parse(args[1].value) + counterController.position);
            Debug.Log(counterController.name + " moves " + args[1].value + " spaces");
        }
        else
        {
            Debug.LogError("Invalid arguments");
        }

        yield break;
    }
}
