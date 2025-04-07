using UnityEngine;

public class GoToJail : Command
{
    public GoToJail(string value) : base(value) { }

    override public void Execute(CounterController counterController, Argument[] args)
    {
        counterController.GoToJail();
        Debug.Log(counterController.gameObject.name + " goes to jail.");
    }
}
