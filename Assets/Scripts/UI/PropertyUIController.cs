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
    [SerializeField] private GameObject root;
    [SerializeField] private Image propertyOwner;
    private Property property;


    // Start is called before the first frame update
    void Start()
    {
        CameraController.onUpdateCamera.AddListener(updateCamera);
    }

    public void updateCamera(CameraController camera)
    {
        if (camera.space == null)
        {
            root.SetActive(false);
            return;
        }
        else root.SetActive(true);

        Space space = camera.space.space;

        if (space is not Property)
        {
            propertyNameUI.text = space.name;
            propertyColorUI.color = Color.gray;
            propertyOwner.gameObject.SetActive(false);
            propertyDetails.SetActive(false);
        }
        else updatePropertyDetails(space as Property);
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
        propertyOwner.gameObject.SetActive(property.isOwned);
        if (property.isOwned) propertyOwner.sprite = property.owner.icon;
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
        if (!property.CanUpgrade())
        {
            GameUIManager.instance.OkPrompt($"Cannot add a house to {property.name}");
            return;
        }

        property.Upgrade();
        updatePropertyDetails(property);
        GameUIManager.instance.OkPrompt($"A house has been added to {property.name}");
    }

    public void downgradeHouse()
    {
        if (!property.CanDowngrade())
        {
            GameUIManager.instance.OkPrompt($"Cannot remove a house from {property.name}");
            return;
        }

        property.Downgrade();
        updatePropertyDetails(property);
        GameUIManager.instance.OkPrompt($"A house has been removed from {property.name}");
    }

    public void mortgage()
    {
        if (property.isMortgaged)
        {
            if (!property.CanUnMortgage()) GameUIManager.instance.OkPrompt($"Cannot unmortgage {property.name}");
            property.UnMortgage();

            updatePropertyDetails(property);
            GameUIManager.instance.OkPrompt($"{property.name} has been unmortgaged for £{property.mortgageValue}");
        }
        else
        {
            property.Mortgage();

            updatePropertyDetails(property);
            GameUIManager.instance.OkPrompt($"{property.name} has been mortgaged for £{property.mortgageValue}");
        }

    }

    public void sell()
    {
        if (!property.CanSell())
        {
            GameUIManager.instance.OkPrompt($"Cannot sell {property.name}");
            return;
        }

        property.Sell();
        updatePropertyDetails(property);
        GameUIManager.instance.OkPrompt($"{property.name} has been sold");
    }
}
