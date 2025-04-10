using TMPro;
using UnityEngine;

public class SpaceController : MonoBehaviour
{
    public TextMeshProUGUI output;
    public Transform[] waypoints;
    public Space space;
    [SerializeField] private MeshFilter model;


    /// <summary> The position of this space controller on the board. </summary>
    public int position { get {  return space.position; } }

    public void Setup(Space space)
    {
        this.space = space;
        output.text = space.name;

        // Set the color of the mesh material to that of the property group
        if (space is Property)
        {
            transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[0].color = (space as Property).propertyGroup.GetColor();
        }
    }

    private void Update()
    {
        if (space is Property)
        {
            int index = (space as Property).upgradeLevel;
            model.mesh = GameController.instance.upgradeMeshes[index];
        }
        else 
        { 
            model.mesh = null;
        }
        
    }


}
