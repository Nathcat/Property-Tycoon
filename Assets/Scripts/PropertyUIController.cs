using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using Funky;

public class PropertyUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI propertyNameUI;
    [SerializeField] private Image propertyColorUI;
    [SerializeField] private TextMeshProUGUI propertyPriceUI;
    [SerializeField] private TextMeshProUGUI propertyMortgageUI;
    [SerializeField] private TextMeshProUGUI propertyHouseValueUI;
    [SerializeField] private TextMeshProUGUI propertyRentDescriptionUI;
    [SerializeField] private GameObject needsOwenership;
    [SerializeField] private TextMeshProUGUI mortgageButtonText;
    private Property space;


    // Start is called before the first frame update
    void Start()
    {
        CameraController.onUpdateCamera.AddListener(GetPropertyDetails);
    }

    public void GetPropertyDetails(CameraController camera)
    {
        space = camera.property;

        propertyNameUI.text = space.name;
        propertyPriceUI.text = $"Value:\n£{space.cost}";
        propertyColorUI.color = space.propertyGroup.GetColor();
        propertyMortgageUI.text = $"Mortgage value: £{space.mortgageValue}";
        mortgageButtonText.text = space.isMortgaged ? "Unmortgage" : "Mortgage";
        propertyRentDescriptionUI.text = space.GetRentDescription();
        if (space.owner == GameController.instance.turnCounter)
        {
            needsOwenership.SetActive(true);
        }
        else
        {
            needsOwenership.SetActive(false);
        }

        if (camera.property is not Station && camera.property is not Utility)
        {
            propertyHouseValueUI.text = $"Upgrade:\n£{space.upgradeCost}";
            propertyHouseValueUI.gameObject.SetActive(true);
        }
        else
        {
            propertyHouseValueUI.gameObject.SetActive(false);
        }
    }

    public void AddHouse()
    {
        if (space.CanUpgrade())
        {
            space.Upgrade();
        }
    }
    public void DowngradeHouse()
    {
        if (space.CanDowngrade()) 
        {
            space.Downgrade();
        }
    }
    public void Mortgage() 
    {
        if (space.isMortgaged)
        {
            space.UnMortgage();
        } else
        {
            space.Mortgage();
        }
    }
    public void Sell() 
    {
        if (space.CanSell()) 
        {
            space.Sell();
        }
    }
}
