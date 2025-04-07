using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetOutOfJail : Command
{
    public GetOutOfJail(string value) : base(value) {}
    
    override public IEnumerator Execute(CounterController counterController, Argument[] args) {
        counterController.getOutOfJailFree = true;
        Debug.Log(counterController.gameObject.name + " can get out of jail for free!");
        yield break;
    }
}
