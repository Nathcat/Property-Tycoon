using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;
using System.Runtime.CompilerServices;

/// <summary>Base class of all board spaces</summary>
public class Space
{
    public int position;
    public string name;
    public Action action;

    public Space(int position, string name, Action action)
    {
        this.position = position;
        this.name = name;
        this.action = action;
    }

    public Space() { }
}
