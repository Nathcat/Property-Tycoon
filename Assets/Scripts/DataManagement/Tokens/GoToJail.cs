using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToJail : Command
{
    public GoToJail(string value) : base(value) {}
    
    override public IEnumerator Execute(CounterController counterController, Argument[] args) {
        yield return counterController.GoToJail();
        Debug.Log(counterController.gameObject.name + " goes to jail.");
    }
}
