using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sub-type of property for station spaces.
/// </summary>
public class Station : Property
{
    public Station(int position, string name, PropertyGroup group, Action action, int cost) : base(position, name, group, action, cost, 0)
    {
        if (group.name != PropertyGroup.STATION_GROUP_NAME)
        {
            throw new InvalidPropertyGroupException("Attempted to assign property group '" + group.name + "' to a Station property, a Property of type Station must be assigned to the Station property group.");
        }
    }

    /// <summary>
    /// Verify that this space's action string is valid within the context of this space.
    /// i.e. for a property space, the action should use the PropertyRent command, and not StationRent, or UtilityRent.
    /// </summary>
    override public void ValidateAction()
    {
        if (!action.ContainsCommand<StationRent>())
        {
            throw new Action.SyntaxError("The property '" + name + "' must contain the StationRent command in its' action string.");
        }

        if (action.ContainsCommand<PropertyRent>() || action.ContainsCommand<UtilityRent>())
        {
            throw new Action.SyntaxError("The property '" + name + "' must contain the cannot contain either PropertyRent or UtilityRent in its' action string.");
        }
    }

    override public string GetRentDescription()
    {
        return "If the player who owns this station owns only this station, rent is £25\nIf the player who owns this station owns 2 stations, rent is £50\nIf the player who owns this station owns 3 stations, rent is £100\nIf the player who owns this station owns 4 stations, rent is £200";
    }
}
