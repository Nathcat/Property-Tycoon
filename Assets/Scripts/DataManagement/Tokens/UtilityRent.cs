using System.Collections;
using UnityEngine;

public class UtilityRent : Command
{
    public UtilityRent(string value) : base(value) {}
    
    override public IEnumerator Execute(CounterController counterController, Argument[] args) {
        Space space = GameController.instance.spaces[counterController.position];

        if (!(space is Utility))
        {
            Debug.LogWarning("UtilityRent must be applied to a utility!");
            yield break;
        }

        Utility utility = (Utility)space;

        if (utility.isMortgaged)
        {
            Debug.LogWarning("Cannot take rent on mortgaged property!");
            yield break;
        }

        if (!utility.isOwned)
        {
            Debug.LogWarning("Station must be owned for rent to apply!");
            yield break;
        }

        if (utility.owner.isInJail)
        {
            Debug.LogWarning("Owner is in jail and cannot collect rent!");
            yield break;
        }

        int stationsOwned = 0;
        foreach (Space s in GameController.instance.spaces)
        {
            if (s is Utility && (s as Utility).owner == utility.owner) stationsOwned++;
        }

        int rent = 0;

        switch (stationsOwned)
        {
            case 1: rent = (counterController.lastRoll.dice1 + counterController.lastRoll.dice2) * 4; break;
            case 2: rent = (counterController.lastRoll.dice1 + counterController.lastRoll.dice2) * 10; break;
            default: Debug.LogWarning("The owner of the incident utility owns more than 2 utilities, or no stations, this should be impossible!"); break;
        }

        utility.owner.portfolio.AddAsset(counterController.portfolio.RemoveCash(new Cash(rent)));
        Debug.Log(counterController.name + " pays " + rent + " in rent to " + utility.owner.name + " for utility " + utility.name);
    }
}
