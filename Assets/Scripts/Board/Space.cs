using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Base class of all board spaces</summary>
public class Space
{
    public int position;
    public string name;
    public PropertyGroup propertyGroup;
    public Action action;
    public int cost;


    public Space(int position, string name, PropertyGroup propertyGroup, Action action, int cost)
    {
        this.position = position;
        this.name = name;
        this.propertyGroup = propertyGroup;
        this.action = action;
        this.cost = cost;


    }
}