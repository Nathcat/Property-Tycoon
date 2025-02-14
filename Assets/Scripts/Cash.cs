using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cash : Asset
{
    public void AddCash(Cash cashIn)
    {
        value = value + cashIn.value;
    }
    public void AddCash(int cashIn)
    {
        value = value + cashIn;
    }

    public void RemoveCash(int cashOut)
    {
        value -= cashOut;
    }

    public void RemoveCash(Cash cashOut)
    {
        value -= cashOut.value;
    }
}
