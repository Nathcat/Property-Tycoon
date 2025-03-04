using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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



    public Space(int position, string name, PropertyGroup propertyGroup, Action action, int cost)
    {
        this.position = position;
        this.name = name;
        this.propertyGroup = propertyGroup;
        this.action = action;
        this.cost = cost;

        //if the space is a property, give it an upgrade level, otherwise dont
        if(propertyGroup == null){
            upgradelevel = -1;
        }
        else{
            upgradelevel = 0;
        }
        
    }



    public Space() { }


}
