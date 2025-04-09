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
    [SerializeField] private GameObject propertyDetails;
    private Property property;


    // Start is called before the first frame update
    void Start()
    {
        CameraController.onUpdateCamera.AddListener(updateCamera);
    }

    public void updateCamera(CameraController camera)
    {
        if (camera.space == null) return;
        Space space = camera.space.space;

        if (space is not Property)
        {
            propertyNameUI.text = space.name;
            propertyColorUI.color = Color.gray;
            propertyDetails.SetActive(false);
        } else updatePropertyDetails(space as Property);
    }

    public void updatePropertyDetails(Property property)
    {
        this.property = property;
        propertyDetails.SetActive(true);

        propertyNameUI.text = property.name;
        propertyPriceUI.text = $"Value:\n£{property.cost}";
        propertyColorUI.color = property.propertyGroup.GetColor();
        propertyMortgageUI.text = $"Mortgage value: £{property.mortgageValue}";
        mortgageButtonText.text = property.isMortgaged ? "Unmortgage" : "Mortgage";
        propertyRentDescriptionUI.text = property.GetRentDescription();
        if (property.owner == GameController.instance.turnCounter)
        {
            needsOwenership.SetActive(true);
        }
        else
        {
            needsOwenership.SetActive(false);
        }

        if (property is not Station && property is not Utility)
        {
            propertyHouseValueUI.text = $"Upgrade:\n£{property.upgradeCost}";
            propertyHouseValueUI.gameObject.SetActive(true);
        }
        else
        {
            propertyHouseValueUI.gameObject.SetActive(false);
        }
    }

    public void addHouse()
    {
        property.Upgrade();
        updatePropertyDetails(property);
    }
    public void downgradeHouse()
    {
        property.Downgrade();
        updatePropertyDetails(property);
    }
    public void mortgage() 
    {
        if (property.isMortgaged)
        {
            property.UnMortgage();
        } else
        {
            property.Mortgage();
        }
        
        updatePropertyDetails(property);
    }
    public void sell() 
    {
        property.Sell();
        updatePropertyDetails(property);
    }
}
