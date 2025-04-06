/// <summary>Base class of all board spaces</summary>
public class Space
{
    public int position;
    public string name;
    public Action action;

    public Space(int position, string name, Action action)
    {
        this.position = position;
        this.name = name;
        this.action = action;

        ValidateAction();
    }

    /// <summary>
    /// Verify that this space's action string is valid within the context of this space.
    /// i.e. for a normal space, the action should not contain any rent commands.
    /// </summary>
    virtual public void ValidateAction()
    {
        if (action.ContainsCommand<PropertyRent>() || action.ContainsCommand<StationRent>() || action.ContainsCommand<UtilityRent>())
        {
            throw new Action.SyntaxError("A normal space ('" + name + "') cannot specify a rent in it's action string!");
        }
    }
}
