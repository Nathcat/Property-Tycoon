using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpaceController : MonoBehaviour
{

    public TextMeshProUGUI output;
    public Space space;

    public GameObject counterExample;

    public void Setup(Space space) {
        this.space = space;
        output.text = space.name;

        // Set the color of the mesh material to that of the property group
        if (space.propertyGroup != null)
        {
            transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[0].color = space.propertyGroup.GetColor();
        }
    }


}
