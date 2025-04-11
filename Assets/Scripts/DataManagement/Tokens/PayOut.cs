using System.Collections;
using UnityEngine;

public class PayOut : Command
{
    public PayOut(string value) : base(value) { }

    override public IEnumerator Execute(CounterController counterController, Argument[] args)
    {
        Debug.Log(counterController.name + " receives " + args[0].value + " from the bank.");
        counterController.portfolio.AddAsset(new Cash(int.Parse(args[0].value)));

        yield break;
    }
}
