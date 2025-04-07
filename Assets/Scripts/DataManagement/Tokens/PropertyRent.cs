using UnityEngine;

public class PropertyRent : Command
{
    public PropertyRent(string value) : base(value) { }

    override public void Execute(CounterController counterController, Argument[] args)
    {
        Space space = GameController.instance.spaces[counterController.position];

        if (!(space is Property))
        {
            Debug.LogWarning("PropertyRent must be applied to a property!");
            return;
        }

        Property property = (Property)space;

        if (property.isMortgaged)
        {
            Debug.LogWarning("Cannot take rent on mortgaged property!");
            return;
        }

        if (property.owner == null)
        {
            Debug.LogWarning("Property must be owned for rent to be taken!");
            return;
        }

        if (property.owner.isInJail)
        {
            Debug.LogWarning("Owner is in jail and cannot collect rent!");
            return;
        }

        int rent = int.Parse(args[property.upgradeLevel].value);

        if (counterController.portfolio.GetCashBalance() >= rent)
        {
            property.owner.portfolio.AddAsset(counterController.portfolio.RemoveCash(new Cash(rent)));
            Debug.Log(counterController.name + " pays " + rent + " to " + property.owner.name + " in rent for property " + property.name);
        }
        else
        {
            // TODO Here we should ask the player to sell their assets!
            Debug.LogWarning("Incident player does not have enough money to pay rent!");
        }
    }
}
