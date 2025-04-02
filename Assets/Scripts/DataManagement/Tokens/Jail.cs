using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jail : Command
{
    public Jail(string value) : base(value) {}
    
    override public void Execute(CounterController counterController, Argument[] args) {
        Debug.LogWarning("Should this command exist?");
    }
}
