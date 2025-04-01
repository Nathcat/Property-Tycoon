using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropertyRent : Command
{
    public PropertyRent(string value) : base(value) {}

    override public void Execute(CounterController counterController, Argument[] args) {
        Space space = GameController.instance.spaces[counterController.position];

        if (!(space is Property)) {
            Debug.LogError("PropertyRent must be applied to a property!");
            return;
        }
    
        Property property = (Property) space;

        if (property.isMortgaged) {
            Debug.LogError("Cannot take rent on mortgaged property!");
            return;
        }

        if (property.owner == null) {
            Debug.LogError("Property must be owned for rent to be taken!");
            return;
        }

        int rent = int.Parse(args[property.upgradeLevel].value);

        if (counterController.portfolio.GetCashBalance() >= rent) {
            property.owner.portfolio.AddAsset(counterController.portfolio.RemoveCash(new Cash(rent)));
        }
        else {
            // TODO Here we should ask the player to sell their assets!
            Debug.LogError("Incident player does not have enough money to pay rent!");
        }
    }
}
