using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Property: A subclass of Asset, used to represent a property in PropertyTycoon.
/// </summary>
[System.Serializable]
public class Property : Space, IAsset
{
    /// <summary>
    /// Thrown if the property group provided to a property is invalid in some way.
    /// </summary>
    public class InvalidPropertyGroupException : System.Exception
    {
        public InvalidPropertyGroupException(string m) : base(m) { }
    }

    /// <summary> A boolean to signify if the property has been mortgaged.</summary>
    public bool isMortgaged { get; private set; } = false;

    /// <summary>
    /// The cost to buy of this property. Also denotes is value as an asset.
    /// </summary>
    public int cost;

    /// <summary>
    /// The cost to upgrade this property
    /// </summary>
    public int upgradeCost { get; private set; }

    /// <summary>
    /// The property group this property is a part of
    /// </summary>
    public PropertyGroup propertyGroup { get; private set; }

    /// <summary>
    /// The current upgrade level of this property
    /// </summary>
    public int upgradeLevel { get; private set; } = 0;

    /// <summary>
    /// The owner of this property, <see cref="null"/> if not owned.
    /// </summary>
    public CounterController owner { get; private set; } = null;

    /// <summary>
    /// True if this property has an owner
    /// </summary>
    public bool isOwned { get { return owner != null; } }

    /// <summary>
    /// The of upgrading to a hotel
    /// </summary>
    public int hotelCost { get { return upgradeCost * 5; } }

    /// <summary>
    /// Initialise a property with the given information.
    /// </summary>
    /// <param name="position">The index position this property will lie on on the board.</param>
    /// <param name="name">The name of the property.</param>
    /// <param name="group">The property group this property is a part of.</param>
    /// <param name="action">The action to be performed upon landing on this property.</param>
    /// <param name="cost"> An integer denoting the value of the property.</param>
    public Property(int position, string name, PropertyGroup group, Action action, int cost, int upgradeCost) : base(position, name, action)
    {
        isMortgaged = false;
        this.propertyGroup = group;
        this.cost = cost;
        this.upgradeCost = upgradeCost;
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
    /// Determine whether or not you can unmortgage this property.
    /// </summary>
    /// <returns>True if the owner of this property can afford to unmortgage it, false otherwise</returns>
    public bool CanUnMortgage() {
        return owner != null && owner.portfolio.GetCashBalance() >= (cost / 2);
    }

    /// <summary>
    /// Unmortgage this property, if possible
    /// </summary>
    public void UnMortgage() {
        if (!CanUnMortgage()) return;

        if (owner.portfolio.GetCashBalance() >= (cost / 2)) {
            isMortgaged = false;
            owner.portfolio.RemoveCash(new Cash(cost / 2));
        }
    }

    /// <summary>
    /// Return the value of this property. The value of a property is the cost to buy it. Or, if the property is
    /// currently mortgaged, the value of the property is halved.
    /// </summary>
    /// <returns>The current value of the property.</returns>
    public int GetValue()
    {
        if (isMortgaged) { return cost / 2; } else { return cost + (upgradeLevel == 5 ? hotelCost : (upgradeLevel * upgradeCost)); }
    }

    /// <summary>
    /// Check whether or not this property can be upgraded
    /// </summary>
    /// <returns>True if the property can be upgraded, false otherwise</returns>
    public bool CanUpgrade()
    {
        if (!propertyGroup.HasCompleteGroup(owner.portfolio.GetProperties().ToArray())) return false;

        if (upgradeLevel != propertyGroup.GetMinimumUpgradeLevel())
        {
            Debug.LogWarning("Cannot upgrade property '" + name + "', the disparity in upgrades within the property group '" + propertyGroup.name + "' would be too great!");
            return false;
        }

        if (owner.portfolio.GetCashBalance() < (upgradeLevel == 4 ? hotelCost : upgradeCost)) {
            return false;
        }

        if (upgradeLevel >= 0 && upgradeLevel < 5)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Upgrade this property, if it can be upgraded.
    /// </summary>
    public void Upgrade()
    {
        if (CanUpgrade())
        {
            owner.portfolio.RemoveCash(new Cash(upgradeLevel == 4 ? hotelCost : upgradeCost));
            upgradeLevel++;
        }
    }

    /// <summary>
    /// Verify that this space's action string is valid within the context of this space.
    /// i.e. for a property space, the action should use the PropertyRent command, and not StationRent, or UtilityRent.
    /// </summary>
    override public void ValidateAction()
    {
        if (!action.ContainsCommand<PropertyRent>())
        {
            throw new Action.SyntaxError("The property '" + name + "' must contain the PropertyRent command in its' action string.");
        }

        if (action.ContainsCommand<StationRent>() || action.ContainsCommand<UtilityRent>())
        {
            throw new Action.SyntaxError("The property '" + name + "' must contain the cannot contain either StationRent or UtilityRent in its' action string.");
        }
    }

    /// <summary>
    /// Get a string describing the rent which should be paid upon landing on this space.
    /// Should be used to display to the user.
    /// </summary>
    /// <returns>A string describing the rent to be paid upon landing on this space.</returns>
    virtual public string GetRentDescription()
    {
        Argument[] args = action.GetCommandArguments<PropertyRent>();

        if (args.Length != 6)
        {
            throw new Action.SyntaxError("Action string for space '" + name + "' is invalid, PropertyRent should have 6 arguments!");
        }

        return "Undeveloped: �" + args[0].value + "\n1 house: �" + args[1] + "\n2 houses: �" + args[2] + "\n3 houses: �" + args[3] + "\n4 houses: " + args[4] + "\nHotel: �" + args[5];
    }

    /// <summary>
    /// Return <see cref="true"/> if <paramref name="counter"/> can purchase this property.
    /// </summary>
    /// <param name="counter">The <see cref="CounterController"/> to check can puchase this property</param>
    /// <returns>True if <paramref name="counter"/> can purcahse this property.</returns>
    public bool CanPurchase(CounterController counter)
    {
        return !isOwned && counter.portfolio.GetCashBalance() >= cost;
    }

    /// <summary>
    /// Purchases this property as <paramref name="counter"/>.
    /// </summary>
    /// <param name="counter">The <see cref="CounterController"/> purchasing the property.</param>
    public void Purchase(CounterController counter)
    {
        if (!CanPurchase(counter)) return;

        owner = counter;
        counter.portfolio.AddAsset(this);
        counter.portfolio.RemoveCash(new Cash(cost));
    }

    /// <summary>
    /// Purchase this property in the context of an auction
    /// </summary>
    /// <param name="counter">The winning player</param>
    /// <param name="auctionValue">The value of their bid</param>
    public void AuctionPurchase(CounterController counter, Cash auctionValue) {
        owner = counter;
        counter.portfolio.AddAsset(this);
        counter.portfolio.RemoveCash(auctionValue);
    } 

    /// <summary>
    /// Check weather this property can be downgraded
    /// </summary>
    /// <returns>True if the property can be downgraded, false otherwise</returns>
    public bool CanDowngrade()
    {
        if (upgradeLevel != propertyGroup.GetMaximumUpgradeLevel())
        {
            Debug.LogWarning("Cannot downgrade property '" + name + "', the disparity in upgrades within the property group '" + propertyGroup.name + "' would be too great!");
            return false;
        }

        if (upgradeLevel > 0 && upgradeLevel < 5)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Downgrades the property, refunding <see cref="upgradeCost"/> to the owner.
    /// </summary>
    public void Downgrade()
    {
        if (!CanDowngrade()) return;
        owner.portfolio.AddAsset(new Cash(upgradeLevel == 5 ? hotelCost : upgradeCost));
        upgradeLevel--;
    }

    /// <summary>
    /// Check weather this property can be sold
    /// </summary>
    /// <returns>True if the property can be sold, false otherwise</returns>
    public bool CanSell()
    {
        return propertyGroup.GetMaximumUpgradeLevel() == 0;
    }

    /// <summary>
    /// Sells the property, refundung <see cref="cost"/> to the owner.
    /// </summary>
    public void Sell()
    {
        if (!CanSell()) return;

        owner.portfolio.AddAsset(new Cash(isMortgaged ? (cost / 2) : cost));
        owner.portfolio.RemoveProperty(this);
        owner = null;
    }
}
