using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Asset is a class used to represent owned assests in a counter's Portfolio.
/// </summary>
public class Asset
{
    protected int value;

    public Asset()
    {
        value = 0;
    }

    public Asset(int valueIn)
    {
        value = valueIn;
    }

    public int GetValue()
    {
        return value;
    }
}
