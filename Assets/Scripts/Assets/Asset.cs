using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Asset: A class used to represent owned assests in a counter's Portfolio.
/// </summary>
public class Asset
{
    /// <param name="value"> An integer denoting the value of the asset.</param>
    protected int value;
    /// <summary>
    /// A constructor which takes no inputs, and sets the value of the asset to 0.
    /// </summary>
    public Asset()
    {
        value = 0;
    }
    /// <summary>
    /// A constructor which takes an integer, and sets the value of the asset to that.
    /// </summary>
    /// <param name="valueIn"> An integer denoting the value of the asset.</param>
    public Asset(int valueIn)
    {
        value = valueIn;
    }
    /// <summary>
    /// Returns the value of the asset as an integer.
    /// </summary>
    /// <returns> The value of the asset.</returns>
    public int GetValue()
    {
        return value;
    }
}
