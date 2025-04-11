using System.Collections;
using UnityEngine;

public class Jail : Command
{
    public Jail(string value) : base(value) { }

    override public IEnumerator Execute(CounterController counterController, Argument[] args)
    {
        Debug.LogWarning("Jail command does nothing, should it exist?");
        yield break;
    }
}
