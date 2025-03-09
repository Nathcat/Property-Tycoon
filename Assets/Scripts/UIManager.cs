using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject StartScreen;

    // Start is called before the first frame update
    void Start()
    {
        StartScreen.SetActive(true);
        Credits.SetActive(false);
        HelpAndRules.SetActive(false);
        ConfirmClose.SetActive(false);
        Settings.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //----------main menu----------
    void BackButton() //This would be called by ALL back buttons on the main menu
    {
        StartScreen.SetActive(true);
        HelpAndRules().SetActive(false);
        Rules().SetActive(false);
        Help().SetActive(false);
        Credits().SetActive(false);
        ConfirmClose().SetActive(false);
        SettingsButton().SetActive(false);
    }

    void MMStartButton()
    {
        //SceneManager.LoadScene("MainGameScene");
    }
    void MMSettingsButton()
    {
        StartScreen.SetActive(false);
        SettingsButton().SetActive(true);
    }
    void MMHelpButton()
    {
        StartScreen.SetActive(false);
        HelpAndRules().SetActive(true);
        Help().SetActive(true);
        Rules().SetActive(false);
    }
    void MMCreditsButton()
    {
        StartScreen.SetActive(false);
        Credits().SetActive(true);
    }
    void MMExitButton()
    {
        StartScreen.SetActive(false);
        ConfirmClose().SetActive(true);
    }

    void HMHelpButton()
    {
        Help().SetActive(true);
        Rules().SetActive(false);
    }
    void HMRulesButton()
    {
        Rules().SetActive(true);
        Help().SetActive(false);
    }

    void EMYesButton()
    {
        //Close the program, I think we should do a while true loop that eats all their RAM >:3
    }

    //----------game----------





    //----------game composition menu----------



}
