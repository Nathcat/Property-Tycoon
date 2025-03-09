using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

/// <summary>Base class of all board spaces</summary>
public class Space : MonoBehaviour
{
    public int position;
    public string name;
    public PropertyGroup propertyGroup;
    public Action action;
    public int cost;

    //also needs an upgrade level, if applicable
    public int upgradelevel;

    public int[] rentArray;

    



    public Space(int position, string name, PropertyGroup propertyGroup, Action action, int cost)
    {
        this.rentArray = new int[action.getCommandStringLexed().Length - 1];
        for (int i = 1; i < this.rentArray.Length; i++)
        {
            this.rentArray[i-1] = Int32.Parse(action.getCommandStringLexed()[i]);
        }
        this.position = position;
        this.name = name;
        this.propertyGroup = propertyGroup;
        this.action = action;
        this.cost = cost;
        

        //if the space is a property, give it an upgrade level, otherwise dont
        if(propertyGroup == null){
            this.upgradelevel = -1;
        }
        else{
            this.upgradelevel = 0;
        }
        
    }

    public void Upgrade()
    {
        if ((this.upgradelevel >= 0 && this.upgradelevel < 5))
        {
            this.upgradelevel++;
        }
    }



    public Space() { }


}
