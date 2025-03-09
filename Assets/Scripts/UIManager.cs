using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject StartScreen = null;
    [SerializeField] private GameObject Credits = null;
    [SerializeField] private GameObject HelpAndRules = null;
    [SerializeField] private GameObject ConfirmClose = null;
    [SerializeField] private GameObject Settings = null;
    [SerializeField] private GameObject Help = null;
    [SerializeField] private GameObject Rules = null;

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
    public void BackButton() //This would be called by ALL back buttons on the main menu
    {
        StartScreen.SetActive(true);
        HelpAndRules.SetActive(false);
        Rules.SetActive(false);
        Help.SetActive(false);
        Credits.SetActive(false);
        ConfirmClose.SetActive(false);
        Settings.SetActive(false);
    }

    public void MMStartButton()
    {
        //SceneManager.LoadScene("MainGameScene");
    }
    public void MMSettingsButton()
    {
        StartScreen.SetActive(false);
        Settings.SetActive(true);
    }
    public void MMHelpButton()
    {
        StartScreen.SetActive(false);
        HelpAndRules.SetActive(true);
        Help.SetActive(true);
        Rules.SetActive(false);
    }
    public void MMCreditsButton()
    {
        StartScreen.SetActive(false);
        Credits.SetActive(true);
    }
    public void MMExitButton()
    {
        StartScreen.SetActive(false);
        ConfirmClose.SetActive(true);
    }

    public void HMHelpButton()
    {
        Help.SetActive(true);
        Rules.SetActive(false);
    }
    public void HMRulesButton()
    {
        Rules.SetActive(true);
        Help.SetActive(false);
    }

    public void EMYesButton()
    {
        //Close the program, I think we should do a while true loop that eats all their RAM >:3
    }

    //----------game----------





    //----------game composition menu----------



}
