using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DisplayName : MonoBehaviour
{

    public TextMeshProUGUI output;
    public Space space;

    public void Setup(Space space) {
        this.space = space;
        output.text = space.name;
    }


}
