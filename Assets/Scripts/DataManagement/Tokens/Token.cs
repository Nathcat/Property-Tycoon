public class Token
{
    public string value;

    public Token(string value)
    {
        this.value = value;
    }

    public string getTokenAsString()
    {
        return (string)this.value;
    }
}
