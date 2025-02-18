using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

/// <summary>
/// Portfolio: A class used to represent a counter's portfolio in Property Tycoon.
/// </summary>
public class Portfolio
{
    /// <param name="cashBalance"> A Cash object used to hold the current cash owned by the counter.</param> 
    private Cash cashBalance;
    /// <param name="properties"> A List of Property objects used to hold the properties owned by the counter.</param> 
    private List<Property> properties;

    /// <summary>
    /// Constructor for the Portfolio class.
    /// </summary>
    public Portfolio() 
    {
        cashBalance = new Cash();
        properties = new List<Property>();
    }

    /// <summary>
    /// Returns the total value of the Portfolio's cash and properties.
    /// </summary>
    /// <returns>An integer representing the total value of the Portfolio's cash and properties.</returns>
    public int TotalValue()
    {
        int total = 0;
        for (int i = 0; i < properties.Count; i++)
        {
            total += properties[i].GetValue();
        }
        total += cashBalance.GetValue();

        return total;
    }
    /// <summary>
    /// Adds a new asset to the portfolio.
    /// </summary>
    /// <param name="newAsset">The new asset to be added to the portfolio.</param>
    public void AddAsset(Asset newAsset)
    {
        Type t = newAsset.GetType();
        if (t.Equals(typeof(Cash)))
        {
            cashBalance.AddCash((Cash)newAsset);
        }
        else if (t.Equals(typeof(Property)))
        {
            properties[properties.Count] = (Property)newAsset;
        }
       
    }
    /// <summary>
    /// Removes a given value in cash from the portfolio.
    /// </summary>
    /// <param name="cashOut">The amount to be removed from the portfolio.</param>
    /// <returns>A Cash object containing the removed cash.</returns>
    public Cash RemoveCash(Cash cashOut)
    {
        cashBalance.RemoveCash(cashOut);
        // should this return the amount removed from the portfolio, or the amount remaining in the portfolio?? currently returning the amount removed
        return(cashOut);
    }
    /// <summary>
    /// Removes a given property from the portfolio.
    /// </summary>
    /// <param name="propertyOut">The property to be removed from the portfolio.</param>
    /// <returns>The removed property.</returns>
    public int RemoveProperty(Property propertyOut)
    {
        properties.Remove(propertyOut);
        return(propertyOut.GetValue());
    }
    /// <summary>
    /// Returns the portfolio's cash.
    /// </summary>
    /// <returns>A Cash object containing the amount owned by the portfolio.</returns>
    public int GetCashBalance()
    {
        return (cashBalance.GetValue());
    }
    /// <summary>
    /// Returns a list of the portfolio's properties.
    /// </summary>
    /// <returns>A List of the portfolio's properties.</returns>
    public List<Property> GetProperties()
    {
        return properties;
    }
}
