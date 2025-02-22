using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Property: A subclass of Asset, used to represent a property in PropertyTycoon.
/// </summary>
public class Property : Asset
{
    /// <param name="isMortgaged"> A boolean to signify if the property has been mortgaged.</param>
    private bool isMortgaged;

    /// <summary>
    /// A constructor which takes an integer, and sets the value of the property to that, as well as setting the mortgage status to false.
    /// </summary>
    /// <param name="valueIn"> An integer denoting the value of the property.</param>
    public Property(int valueIn)
    {
        isMortgaged = false;
        value = valueIn;
    }
    /// <summary>
    /// Returns the mortgage status of the property.
    /// </summary>
    /// <returns>True if mortgaged, False if not.</returns>
    public bool IsMortgaged()
    {
        return isMortgaged;
    }
    /// <summary>
    /// Sets the property for mortgage, and returns the amount of cash gained from doing so.
    /// </summary>
    /// <returns>Cash from mortgaging the property.</returns>
    public Cash Mortgage()
    {
        Cash output = new Cash();
        output.AddCash(value / 2);
        isMortgaged = true;
        return output;
    }
}
