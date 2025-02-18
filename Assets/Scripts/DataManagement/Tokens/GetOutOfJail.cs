using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetOutOfJail : Command
{
    public GetOutOfJail(string value) : base(value) {}
    
    override public void Execute(CounterController counterController, Argument[] args) {
        string s = "----- GETOUTOFJAIL -----\n";

        for (int i = 0; i < args.Length; i++) {
            s += args[i].value + "\n";
        }

        Debug.Log(s);
    }
}
