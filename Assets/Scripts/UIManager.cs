using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManager;

public class UIManager : MonoBehaviour
{
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


    //----------game----------





    //----------game composition menu----------



}
