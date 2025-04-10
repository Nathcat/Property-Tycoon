using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpaceController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI output;
    [SerializeField] private Image colour;
    [SerializeField] private Image skin;
    public Transform[] waypoints;

    [Header("Skins")]
    [SerializeField] private Sprite potluckSkin;
    [SerializeField] private Sprite opportunitySkin;
    [SerializeField] private Sprite payoutSkin;
    [SerializeField] private Sprite arrestSkin;
    [SerializeField] private Sprite jailSkin;
    [SerializeField] private Sprite propertySkin;
    [SerializeField] private Sprite parkingSkin;

    [HideInInspector] public Space space { get; private set; }

    /// <summary> The position of this space controller on the board. </summary>
    public int position { get {  return space.position; } }

    public void Setup(Space space)
    {
        this.space = space;
        output.text = space.name;

        if (space is Property)
        {
            colour.color = (space as Property).propertyGroup.GetColor();
            skin.sprite = propertySkin;
        }
        else if (space.action.ContainsCommand<PayOut>()) skin.sprite = payoutSkin;
        else if (space.action.ContainsCommand<Jail>()) skin.sprite = jailSkin;
        else if (space.action.ContainsCommand<GoToJail>()) skin.sprite = arrestSkin;
        else if (space.action.ContainsCommand<CollectFreeParking>()) skin.sprite = parkingSkin;
        else if (space.action.ContainsCommand<TakePotLuck>()) skin.sprite = potluckSkin;
        else if (space.action.ContainsCommand<TakeOppoKnocks>()) skin.sprite = opportunitySkin;
        else colour.color = new Color(91, 218, 187);
    }
}
