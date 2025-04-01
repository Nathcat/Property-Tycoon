using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UtilityRent : Command
{
    public UtilityRent(string value) : base(value) {}
    
    override public void Execute(CounterController counterController, Argument[] args) {
        Space space = GameController.instance.spaces[counterController.position];

        if (!(space is Utility)) {
            Debug.LogError("UtilityRent must be applied to a utility!");
            return;
        }
    
        Utility utility = (Utility) space;

        if (utility.isMortgaged) {
            Debug.LogError("Cannot take rent on mortgaged property!");
            return;
        }

        if (!utility.isOwned) {
            Debug.LogError("Station must be owned for rent to apply!");
            return;
        }

        int stationsOwned = 0;
        foreach (Space s in GameController.instance.spaces) {
            if (s is Utility && (s as Utility).owner == utility.owner) stationsOwned++;
        }

        int rent = 0;

        switch (stationsOwned) {
            case 1: rent = (counterController.lastRoll.dice1 + counterController.lastRoll.dice2) * 4; break;
            case 2: rent = (counterController.lastRoll.dice1 + counterController.lastRoll.dice2) * 10; break;
            default: Debug.LogError("The owner of the incident utility owns more than 2 utilities, or no stations, this should be impossible!"); break;
        }

        if (counterController.portfolio.GetCashBalance() >= rent) {
            utility.owner.portfolio.AddAsset(counterController.portfolio.RemoveCash(new Cash(rent)));
        }
        else {
            // TODO Here we should ask the player to sell their assets!
            Debug.LogError("Incident player does not have enough money to pay rent!");
        }
    }
}
