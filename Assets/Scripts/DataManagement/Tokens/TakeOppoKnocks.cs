using UnityEngine;

public class TakeOppoKnocks : Command
{
    public TakeOppoKnocks(string value) : base(value) {}
    
    override public IEnumerator Execute(CounterController counterController, Argument[] args) {
        Debug.Log(counterController.name + " plays an Opportunity Knocks card");
        GameController.instance.DrawOpportunity(counterController);

        yield break;
    }
}
