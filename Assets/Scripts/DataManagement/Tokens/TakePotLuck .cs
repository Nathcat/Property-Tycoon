using System.Collections;
using UnityEngine;

public class TakePotLuck : Command
{
    public TakePotLuck(string value) : base(value) {}
    
    override public IEnumerator Execute(CounterController counterController, Argument[] args) {
        Debug.Log(counterController.name + " plays a Pot Luck card.");
        GameController.instance.DrawLuck(counterController);

        yield break;
    }
}
