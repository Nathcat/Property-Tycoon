using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionTest : MonoBehaviour
{
    void Start()
    {
        Action a1 = new Action("Fine 10 ; Move relative -2 ; PayOut 200 ; PropertyRent 1 2 3 4 5 ; StationRent ; UtilityRent ;");
        Action a2 = new Action("Fine 10 ; ;");

        a1.Run(null);  // Runs each command sequentially
        a2.Run(null);  // Syntax error
    }
}
