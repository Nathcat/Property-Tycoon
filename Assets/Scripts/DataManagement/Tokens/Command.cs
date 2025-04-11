using System.Collections;

public class Command : Token
{
    public Command(string value) : base(value) { }

    virtual public IEnumerator Execute(CounterController counterController, Argument[] args)
    {
        yield break;
    }
}
