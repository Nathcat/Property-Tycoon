using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property : Asset
{
    private bool isMortgaged;

    public Property()
    {
        isMortgaged = false;
    }

    public int GetValue() 
    {
        return value;
    }

    public bool IsMortgaged()
    {
        return isMortgaged;
    }

    public Cash Mortgage()
    {
        Cash output = new Cash();
        output.AddCash(value / 2);
        isMortgaged = true;
        return output;
    }
}
