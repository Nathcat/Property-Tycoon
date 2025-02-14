using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Portfolio
{
    private Cash cashBalance;
    private List<Property> properties;

    

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

    public Cash RemoveCash(Cash cashOut)
    {
        cashBalance.RemoveCash(cashOut);
        // should this return the amount removed from the portfolio, or the amount remaining in the portfolio?? currently returning the amount removed
        return(cashOut);
    }

    public int RemoveProperty(Property propertyOut)
    {
        properties.Remove(propertyOut);
        return(propertyOut.GetValue());
    }

    public int GetCashBalance()
    {
        return (cashBalance.GetValue());
    }

    public List<Property> GetProperties()
    {
        return properties;
    }
}
