using UnityEngine;

public class GetOutOfJail : Command
{
    public GetOutOfJail(string value) : base(value) { }

    override public void Execute(CounterController counterController, Argument[] args)
    {
        counterController.getOutOfJailFree = true;
        Debug.Log(counterController.gameObject.name + " can get out of jail for free!");
    }
}
