using System.Collections;
using UnityEngine;

public class PayIn : Command
{
    public PayIn(string value) : base(value) { }

    override public IEnumerator Execute(CounterController counterController, Argument[] args)
    {
        Cash fine = new Cash(int.Parse(args[0].value));

        Debug.Log(counterController.name + " pays " + args[0].value + " to the bank");
        counterController.portfolio.RemoveCash(fine);

        yield break;
    }
}
