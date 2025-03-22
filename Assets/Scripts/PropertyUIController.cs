using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] private GameObject camera;

    // Start is called before the first frame update
    void Start()
    {

        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void getPropertyDetails() {
        Space space = camera.GetComponent<CameraController>().space;
        propertyName = space.propertyGroup.name;
        propertyColor = space.propertyGroup.GetColor();


    }



    void UpdateUI() {
        propertyNameUI.GetComponent<Text>().text = propertyName;
        propertyPriceUI.GetComponent<Text>().text = propertyPrice;
        propertySellPriceUI.GetComponent<Text>().text = propertySellPrice;
        propertyRent0HouseUI.GetComponent<Text>().text = propertyRent0House;
        propertyRent1HouseUI.GetComponent<Text>().text = propertyRent1House;
        propertyRent2HouseUI.GetComponent<Text>().text = propertyRent2House;
        propertyRent3HouseUI.GetComponent<Text>().text = propertyRent3House;
        propertyRent4HouseUI.GetComponent<Text>().text = propertyRent4House;
        propertyRent5HouseUI.GetComponent<Text>().text = propertyRent5House;
        propertyMortgageUI.GetComponent<Text>().text = propertyMortgage;
        propertyHouseValueUI.GetComponent<Text>().text = propertyHouseValue;
        propertyHouseCostUI.GetComponent<Image>().color = propertyColor;
    }
}
