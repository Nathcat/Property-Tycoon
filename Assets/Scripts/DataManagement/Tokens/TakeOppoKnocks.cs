using UnityEngine;

public class TakeOppoKnocks : Command
{
    public TakeOppoKnocks(string value) : base(value) { }

    override public void Execute(CounterController counterController, Argument[] args)
    {
        Debug.Log(counterController.name + " plays an Opportunity Knocks card");
        GameController.instance.DrawOpportunity(counterController);
    }
}
