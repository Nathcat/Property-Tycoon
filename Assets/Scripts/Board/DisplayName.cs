using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayName : MonoBehaviour
{

    public TextMeshProUGUI output;

    void Start()
    {
        output.text = this.name;
    }

    public void Setup(Space space) {
        output.text = space.name;
    }


}
