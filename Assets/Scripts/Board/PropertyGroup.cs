using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>A group of properties</summary>
public class PropertyGroup
{
    public string name;
    private List<Space> properties;  // Change this to List<Property> once implemented

    public PropertyGroup(string name, List<Space> properties)
    {
        this.name = name;
        this.properties = properties;
    }

    public PropertyGroup(string name)
    {
        this.name = name;
        this.properties = new List<Space>();
    }

    /// <summary>
    /// Get the list of properties in this group
    /// </summary>
    /// <returns>A list of properties in this group</returns>
    public Space[] GetProperties()
    {
        return properties.ToArray();
    }

    /// <summary>
    /// Determine whether or not this group contains property p
    /// </summary>
    /// <param name="p">The property to search for</param>
    /// <returns>True if this group contains p, false otherwise</returns>
    public bool ContainsProperty(Space p)
    {
        foreach (Space prop in properties)
        {
            if (prop == p) return true;
        }

        return false;
    }

    /// <summary>
    /// Determine whether or not the supplied list of properties matches the list of properties in this group,
    /// i.e. Determine if this group is a subset of the supplied properties.
    /// </summary>
    /// <param name="pList">The list of properties to check.</param>
    /// <returns>True if this list contains all the properties in this group, false otherwise</returns>
    public bool HasCompleteGroup(Space[] pList)
    {
        int containedProperties = 0;
        foreach (Space p in pList)
        {
            if (ContainsProperty(p)) containedProperties++;

            if (containedProperties == properties.Count) return true;
        }

        return false;
    }

    /// <summary>
    /// Add a property to the group
    /// </summary>
    /// <param name="p">The property to add</param>
    public void AddProperty(Space p)
    {
        properties.Add(p);
    }
}
