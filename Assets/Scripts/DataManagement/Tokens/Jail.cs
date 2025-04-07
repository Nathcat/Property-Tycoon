using UnityEngine;

public class Jail : Command
{
    public Jail(string value) : base(value) { }

    override public void Execute(CounterController counterController, Argument[] args)
    {
        Debug.LogWarning("Jail command does nothing, should it exist?");
    }
}
