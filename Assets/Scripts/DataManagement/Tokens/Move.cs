using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : Command
{
    public Move(string value) : base(value) {}

    override public void Execute(CounterController counterController, Argument[] args) {
        string s = "----- MOVE -----\n";

        for (int i = 0; i < args.Length; i++) {
            s += args[i].value + "\n";
        }

        Debug.Log(s);
    }
}
