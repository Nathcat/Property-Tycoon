using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpaceController : MonoBehaviour
{
    public TextMeshProUGUI output;
    public Transform[] waypoints;
    public Space space;

    public void Setup(Space space) {
        this.space = space;
        output.text = space.name;

        // Set the color of the mesh material to that of the property group
        if (space is Property)
        {
            transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[0].color = (space as Property).propertyGroup.GetColor();
        }
    }


}
