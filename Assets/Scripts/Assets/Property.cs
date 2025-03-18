using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Property: A subclass of Asset, used to represent a property in PropertyTycoon.
/// </summary>
public class Property : Space, IAsset
{
    /// <summary> A boolean to signify if the property has been mortgaged.</summary>
    private bool isMortgaged;

    /// <summary>
    /// The cost to buy of this property. Also denotes is value as an asset.
    /// </summary>
    private int cost;

    /// <summary>
    /// The property group this property is a part of
    /// </summary>
    public PropertyGroup propertyGroup { get; private set; }

    /// <summary>
    /// The current upgrade level of this property
    /// </summary>
    public int upgradeLevel { get; private set; } = 0;

    /// <summary>
    /// Initialise a property with the given information.
    /// </summary>
    /// <param name="position">The index position this property will lie on on the board.</param>
    /// <param name="name">The name of the property.</param>
    /// <param name="group">The property group this property is a part of.</param>
    /// <param name="action">The action to be performed upon landing on this property.</param>
    /// <param name="cost"> An integer denoting the value of the property.</param>
    public Property(int position, string name, PropertyGroup group, Action action, int cost) : base(position, name, action)
    {
        isMortgaged = false;
        this.propertyGroup = group;
        this.cost = cost;
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
        Cash output = new Cash(cost / 2);
        isMortgaged = true;
        return output;
    }

    /// <summary>
    /// Return the value of this property. The value of a property is the cost to buy it. Or, if the property is
    /// currently mortgaged, the value of the property is halved.
    /// </summary>
    /// <returns>The current value of the property.</returns>
    public int GetValue()
    {
        if (isMortgaged) { return cost / 2; } else { return cost;  }
    }

    /// <summary>
    /// Upgrade this property.
    /// </summary>
    public void Upgrade()
    {
        if (upgradeLevel >= 0 && upgradeLevel < 5)
        {
            upgradeLevel++;
        }
    }
}
