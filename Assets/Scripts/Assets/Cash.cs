using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Cash: A subclass of asset, used to represent any money used in Property Tycoon.
/// </summary>
public class Cash : Asset
{
    /// <summary>
    /// Adds the value of another Cash object to the selected Cash object.
    /// </summary>
    /// <param name="cashIn">Cash object to be added to the current cash object.</param>
    public void AddCash(Cash cashIn)
    {
        value = value + cashIn.value;
    }
    /// <summary>
    /// Adds an integer value to the value of the Cash object.
    /// </summary>
    /// <param name="cashIn">Integer value to be added to the object's value.</param>
    public void AddCash(int cashIn)
    {
        value = value + cashIn;
    }
    /// <summary>
    /// Removes an integer value from the value of the Cash object.
    /// </summary>
    /// <param name="cashOut">Integer value to be removed from the object's value.</param>
    public void RemoveCash(int cashOut)
    {
        value -= cashOut;
    }
    /// <summary>
    /// Removes the value of another Cash object from the selected Cash object.
    /// </summary>
    /// <param name="cashOut">Cash object to be removed from the current cash object.</param>
    public void RemoveCash(Cash cashOut)
    {
        value -= cashOut.value;
    }
}
