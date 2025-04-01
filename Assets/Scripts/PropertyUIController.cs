using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PropertyUIController : MonoBehaviour
{
    [SerializeField] private string propertyName;
    [SerializeField] private Color propertyColor;
    [SerializeField] private string propertyPrice;
    /*[SerializeField] private string propertyRent0House;
    [SerializeField] private string propertyRent1House;
    [SerializeField] private string propertyRent2House;
    [SerializeField] private string propertyRent3House;
    [SerializeField] private string propertyRent4House;
    [SerializeField] private string propertyRent5House;*/
    [SerializeField] private string propertyMortgage;
    [SerializeField] private string propertyHouseValue;
    [SerializeField] private string propertyRentDescription;

    [SerializeField] private GameObject propertyNameUI;
    [SerializeField] private GameObject propertyColorUI;
    [SerializeField] private GameObject propertyPriceUI;
    /*[SerializeField] private GameObject propertySellPriceUI;
    [SerializeField] private GameObject propertyRent0HouseUI;
    [SerializeField] private GameObject propertyRent1HouseUI;
    [SerializeField] private GameObject propertyRent2HouseUI;
    [SerializeField] private GameObject propertyRent3HouseUI;
    [SerializeField] private GameObject propertyRent4HouseUI;
    [SerializeField] private GameObject propertyRent5HouseUI;*/
    [SerializeField] private GameObject propertyMortgageUI;
    [SerializeField] private GameObject propertyHouseValueUI;
    //[SerializeField] private GameObject propertyHouseCostUI;
    [SerializeField] private GameObject propertyRentDescriptionUI;


    // Start is called before the first frame update
    void Start()
    {
        CameraController.onUpdateCamera.AddListener(GetPropertyDetails);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetPropertyDetails(CameraController camera) {
        // Tyler what the fuck
        Property space = camera.property;
        propertyName = space.name;
        propertyColor = space.propertyGroup.GetColor();
        propertyPrice = space.cost.ToString();
        propertyMortgage =  (space.cost / 2).ToString();
        propertyRentDescription = space.GetRentDescription();

        if (!(camera.property is Station) && !(camera.property is Utility))
        {
            propertyHouseValue = space.upgradeCost.ToString();
        }

        UpdateUI();
    }



    void UpdateUI() {
        CameraController camera = Camera.main.GetComponent<CameraController>();

        propertyNameUI.GetComponent<TMP_Text>().text = propertyName;
        propertyPriceUI.GetComponent<TMP_Text>().text = "Value: £" + propertyPrice;
        propertyColorUI.GetComponent<Image>().color = propertyColor;
        propertyMortgageUI.GetComponent<TMP_Text>().text = "Mortgage value: £" + propertyMortgage;
        propertyRentDescriptionUI.GetComponent<TMP_Text>().text = propertyRentDescription;
        //propertySellPriceUI.GetComponent<TMP_Text>().text = propertyPrice;
        if (!(camera.property is Station) && !(camera.property is Utility))
        {
            propertyHouseValueUI.GetComponent<TMP_Text>().text = "House value: £" + propertyHouseValue;
            propertyHouseValueUI.SetActive(true);
        }
        else
        {
            propertyHouseValueUI.SetActive(false);
        }
    }
}
