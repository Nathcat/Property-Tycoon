using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationRent : Command
{
    public StationRent(string value) : base(value) {}

    override public void Execute(CounterController counterController, Argument[] args) {
        string s = "----- STATIONRENT -----\n";

        for (int i = 0; i < args.Length; i++) {
            s += args[i].value + "\n";
        }

        Debug.Log(s);
    }
}
