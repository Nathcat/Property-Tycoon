/// <summary>
/// Sub-type of property for utility spaces
/// </summary>
public class Utility : Property
{
    public Utility(int position, string name, PropertyGroup group, Action action, int cost) : base(position, name, group, action, cost, 0)
    {
        if (group.name != PropertyGroup.UTILITY_GROUP_NAME)
        {
            throw new InvalidPropertyGroupException("Attempted to assign property group '" + group.name + "' to a Utility property, a Property of type Utility must be assigned to the Utilities property group.");
        }
    }

    /// <summary>
    /// Verify that this space's action string is valid within the context of this space.
    /// i.e. for a property space, the action should use the PropertyRent command, and not StationRent, or UtilityRent.
    /// </summary>
    override public void ValidateAction()
    {
        if (!action.ContainsCommand<UtilityRent>())
        {
            throw new Action.SyntaxError("The property '" + name + "' must contain the UtilityRent command in its' action string.");
        }

        if (action.ContainsCommand<StationRent>() || action.ContainsCommand<PropertyRent>())
        {
            throw new Action.SyntaxError("The utility '" + name + "' must contain the cannot contain either StationRent or PropertyRent in its' action string.");
        }
    }

    public override string GetRentDescription()
    {
        return "If one utility is owned by the player who owns this utility, rent is 4 times the amount shown on the dice.\nIf both utilities are owned by the same player, rent is 10 times the amount shown on the dice.";
    }
}
