using System.Collections;
using UnityEngine;

public class StationRent : Command
{
    public StationRent(string value) : base(value) { }

    override public IEnumerator Execute(CounterController counterController, Argument[] args) {
        Space space = GameController.instance.spaces[counterController.position];

        if (!(space is Station))
        {
            Debug.LogWarning("StationRent must be applied to a station!");
            yield break;
        }

        Station station = (Station)space;

        if (station.isMortgaged)
        {
            Debug.LogWarning("Cannot take rent on mortgaged property!");
            yield break;
        }

        if (!station.isOwned)
        {
            Debug.LogWarning("Station must be owned for rent to apply!");
            yield break;
        }

        if (station.owner.isInJail)
        {
            Debug.LogWarning("Owner is in jail and cannot collect rent!");
            yield break;
        }

        int stationsOwned = 0;
        foreach (Space s in GameController.instance.spaces)
        {
            if (s is Station && (s as Station).owner == station.owner) stationsOwned++;
        }

        int rent = 0;

        switch (stationsOwned)
        {
            case 1: rent = 25; break;
            case 2: rent = 50; break;
            case 3: rent = 100; break;
            case 4: rent = 200; break;
            default: Debug.LogWarning("The owner of the incident station owns more than 4 stations, or no stations, this should be impossible!"); break;
        }

        station.owner.portfolio.AddAsset(counterController.portfolio.RemoveCash(new Cash(rent)));
        Debug.Log(counterController.name + " pays " + rent + " in rent to " + station.owner.name + " for station " + station.name);
    }
}
