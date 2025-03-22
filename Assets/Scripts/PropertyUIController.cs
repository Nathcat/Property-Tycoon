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
    [SerializeField] private string propertySellPrice;
    [SerializeField] private string propertyRent0House;
    [SerializeField] private string propertyRent1House;
    [SerializeField] private string propertyRent2House;
    [SerializeField] private string propertyRent3House;
    [SerializeField] private string propertyRent4House;
    [SerializeField] private string propertyRent5House;
    [SerializeField] private string propertyMortgage;
    [SerializeField] private string propertyHouseValue;
    [SerializeField] private string propertyHouseCost;

    [SerializeField] private GameObject propertyNameUI;
    [SerializeField] private GameObject propertyColorUI;
    [SerializeField] private GameObject propertyPriceUI;
    [SerializeField] private GameObject propertySellPriceUI;
    [SerializeField] private GameObject propertyRent0HouseUI;
    [SerializeField] private GameObject propertyRent1HouseUI;
    [SerializeField] private GameObject propertyRent2HouseUI;
    [SerializeField] private GameObject propertyRent3HouseUI;
    [SerializeField] private GameObject propertyRent4HouseUI;
    [SerializeField] private GameObject propertyRent5HouseUI;
    [SerializeField] private GameObject propertyMortgageUI;
    [SerializeField] private GameObject propertyHouseValueUI;
    [SerializeField] private GameObject propertyHouseCostUI;


    // Start is called before the first frame update
    void Start()
    {
        CameraController.onUpdateCamera.AddListener(getPropertyDetails);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void getPropertyDetails(CameraController camera) {
        Property space = camera.space;
        propertyName = space.propertyGroup.name;
        propertyColor = space.propertyGroup.GetColor();
        propertyPrice = space.cost.ToString();
        propertySellPrice = propertyPrice;
        propertyMortgage =  (space.cost / 2).ToString();
        propertyHouseCost = space.upgradeCost.ToString();
        propertyHouseValue = propertyHouseCost;
        string[] rents = space.GetRentDescription().Split("\n");
        propertyRent0House = rents[0];
        propertyRent1House = rents[1];
        propertyRent2House = rents[2];
        propertyRent3House = rents[3];
        propertyRent4House = rents[4];
        propertyRent5House = rents[5];
        UpdateUI();
    }



    void UpdateUI() {
        propertyNameUI.GetComponent<TMP_Text>().text = propertyName;
        propertyPriceUI.GetComponent<TMP_Text>().text = propertyPrice;
        propertySellPriceUI.GetComponent<TMP_Text>().text = propertySellPrice;
        propertyRent0HouseUI.GetComponent<TMP_Text>().text = propertyRent0House;
        propertyRent1HouseUI.GetComponent<TMP_Text>().text = propertyRent1House;
        propertyRent2HouseUI.GetComponent<TMP_Text>().text = propertyRent2House;
        propertyRent3HouseUI.GetComponent<TMP_Text>().text = propertyRent3House;
        propertyRent4HouseUI.GetComponent<TMP_Text>().text = propertyRent4House;
        propertyRent5HouseUI.GetComponent<TMP_Text>().text = propertyRent5House;
        propertyMortgageUI.GetComponent<TMP_Text>().text = propertyMortgage;
        propertyHouseValueUI.GetComponent<TMP_Text>().text = propertyHouseValue;
        propertyHouseCostUI.GetComponent<Image>().color = propertyColor;
    }
}
