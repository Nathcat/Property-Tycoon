/// <summary>Base class of all cards</summary>
public class Card
{
    public string description;
    public Action action;

    public Card(string description, Action action)
    {
        this.description = description;
        this.action = action;
    }

    public override string ToString()
    {
        return description;
    }
}
