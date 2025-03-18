using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

/// <summary>A group of properties</summary>
public class PropertyGroup
{
    public static Color COLOR_BROWN = new Color(0.68627f, 0.39215f, 0f);
    public static Color COLOR_BLUE = new Color(0.67451f, 0.99216f, 1f);
    public static Color COLOR_PURPLE = new Color(0.78431f, 0f, 1f);
    public static Color COLOR_ORANGE = new Color(1f, 0.70196f, 0f);
    public static Color COLOR_RED = new Color(1f, 0f, 0f);
    public static Color COLOR_YELLOW = new Color(1f, 1f, 0f);
    public static Color COLOR_GREEN = new Color(0f, 1f, 0f);
    public static Color COLOR_DEEPBLUE = new Color(0f, 0f, 1f);
    public static Color COLOR_STATION = new Color(0.2f, 0.2f, 0.2f);
    public static Color COLOR_UTILITIES = new Color(0.9f, 0.9f, 0.9f);

    public string name;
    private List<Property> properties;  // Change this to List<Property> once implemented

    public PropertyGroup(string name, List<Property> properties)
    {
        this.name = name;
        this.properties = properties;
    }

    public PropertyGroup(string name)
    {
        this.name = name;
        this.properties = new List<Property>();
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
    public bool ContainsProperty(Property p)
    {
        foreach (Property prop in properties)
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
    public bool HasCompleteGroup(Property[] pList)
    {
        int containedProperties = 0;
        foreach (Property p in pList)
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
    public void AddProperty(Property p)
    {
        properties.Add(p);
    }

    /// <summary>
    /// Get the colour to display on properties of this group. This is determined by the name of the group
    /// </summary>
    /// <returns>The colour associated with each name</returns>
    public Color GetColor()
    {
        switch (name.ToLower())
        {
            case "brown": return COLOR_BROWN;
            case "blue": return COLOR_BLUE;
            case "purple": return COLOR_PURPLE;
            case "orange": return COLOR_ORANGE;
            case "red": return COLOR_RED;
            case "yellow": return COLOR_YELLOW;
            case "green": return COLOR_GREEN;
            case "deep blue": return COLOR_DEEPBLUE;
            case "station": return COLOR_STATION;
            case "utilities": return COLOR_UTILITIES;
            default: return Color.white;
        }
    }
}
