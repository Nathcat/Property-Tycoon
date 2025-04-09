using System.Collections;
using UnityEngine;

public class Fine : Command
{
    public Fine(string value) : base(value) { }

    override public IEnumerator Execute(CounterController counterController, Argument[] args)
    {
        Cash fine = new Cash(int.Parse(args[0].value));

        counterController.portfolio.RemoveCash(fine);
        GameController.instance.freeParking.AddCash(fine);

        Debug.Log(counterController.name + " is fined " + args[0].value);

        yield break;
    }
}
