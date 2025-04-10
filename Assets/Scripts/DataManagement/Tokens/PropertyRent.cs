using System.Collections;
using UnityEngine;

public class PropertyRent : Command
{
    public PropertyRent(string value) : base(value) { }

    override public IEnumerator Execute(CounterController counterController, Argument[] args) {
        // countercontroller position - 1??????????
        Space space = GameController.instance.spaces[counterController.position];

        if (!(space is Property))
        {
            Debug.LogWarning("PropertyRent must be applied to a property!");
            yield break;
        }

        Property property = (Property)space;

        if (property.isMortgaged)
        {
            Debug.LogWarning("Cannot take rent on mortgaged property!");
            yield break;
        }

        if (property.owner == null)
        {
            Debug.LogWarning("Property must be owned for rent to be taken!");
            yield break;
        }

        if (property.owner.isInJail)
        {
            Debug.LogWarning("Owner is in jail and cannot collect rent!");
            yield break;
        }

        int rent = int.Parse(args[property.upgradeLevel].value);

        property.owner.portfolio.AddAsset(counterController.portfolio.RemoveCash(new Cash(rent)));
        Debug.Log(counterController.name + " pays " + rent + " to " + property.owner.name + " in rent for property " + property.name);

        yield break;
    }
}
