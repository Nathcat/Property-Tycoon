using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationRent : Command
{
    public StationRent(string value) : base(value) {}

    override public void Execute(CounterController counterController, Argument[] args) {
        Space space = GameController.instance.spaces[counterController.position];

        if (!(space is Station)) {
            Debug.LogError("StationRent must be applied to a station!");
            return;
        }
    
        Station station = (Station) space;

        if (station.isMortgaged) {
            Debug.LogError("Cannot take rent on mortgaged property!");
            return;
        }

        if (!station.isOwned) {
            Debug.LogError("Station must be owned for rent to apply!");
            return;
        }

        int stationsOwned = 0;
        foreach (Space s in GameController.instance.spaces) {
            if (s is Station && (s as Station).owner == station.owner) stationsOwned++;
        }

        int rent = 0;

        switch (stationsOwned) {
            case 1: rent = 25; break;
            case 2: rent = 50; break;
            case 3: rent = 100; break;
            case 4: rent = 200; break;
            default: Debug.LogError("The owner of the incident station owns more than 4 stations, or no stations, this should be impossible!"); break;
        }

        if (counterController.portfolio.GetCashBalance() >= rent) {
            station.owner.portfolio.AddAsset(counterController.portfolio.RemoveCash(new Cash(rent)));
            Debug.Log(counterController.name + " pays " + rent + " in rent to " + station.owner.name + " for station " + station.name);
        }
        else {
            // TODO Here we should ask the player to sell their assets!
            Debug.LogError("Incident player does not have enough money to pay rent!");
        }
    }
}
