using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{

    [SerializeField] private GameObject StartScreen = null;
    [SerializeField] private GameObject Credits = null;
    [SerializeField] private GameObject HelpAndRules = null;
    [SerializeField] private GameObject HelpAndRules2 = null;
    [SerializeField] private GameObject ConfirmClose = null;
    [SerializeField] private GameObject Settings = null;
    [SerializeField] private GameObject Help = null;
    [SerializeField] private GameObject Rules = null;
    [SerializeField] private GameObject HelpAndEscape = null;
    [SerializeField] private GameObject Dice = null;
    [SerializeField] private GameObject[] PlayerNameElements;
    [SerializeField] private Scene Scene;
    [SerializeField] private string CurrentScene = "";
    [SerializeField] private string GameScene = "Tyler's Testing Scene";
    [SerializeField] private CounterController[] counters = GameController.instance.counters;

    //private TMP_Text text;


    // Start is called before the first frame update
    void Start()
    {
        Scene = SceneManager.GetActiveScene();
        CurrentScene = Scene.name;
        if (Scene.name == "MainMenu")
        {
            StartScreen.SetActive(true);
            Credits.SetActive(false);
            HelpAndRules.SetActive(false);
            ConfirmClose.SetActive(false);
            Settings.SetActive(false);
            Rules.SetActive(false);
            Help.SetActive(false);
        }
        else if (Scene.name == GameScene)
        {
            //SetupScreen.SetActive(true);
            //PauseMenu.SetActive(false);
            HelpAndEscape.SetActive(false);
            HelpAndRules.SetActive(false);
            Rules.SetActive(false);
            Help.SetActive(false);
            Dice.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //----------main menu----------
    public void BackButton() //This would be called by ALL back buttons on the main menu
    {
        if (Scene.name == "MainMenu")
        {
            StartScreen.SetActive(true);
            Rules.SetActive(false);
            Help.SetActive(false);
            Credits.SetActive(false);
            ConfirmClose.SetActive(false);
            Settings.SetActive(false);
        }
        else if (Scene.name == GameScene)
        {
            HelpAndEscape.SetActive(true);
            HelpAndRules.SetActive(true);
            //HelpMenu.SetActive(false);
            Rules.SetActive(false);
            Help.SetActive(false);
        }
    }

    public void MMStartButton()
    {
        SceneManager.LoadScene(GameScene);
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

    public void IGPauseButton()
    {
        //Stop all functions of the game
        HelpAndEscape.SetActive(true);
    }
    public void PMBackButton()
    {
        HelpAndEscape.SetActive(false);
        //Start all functions of the game
    }
    public void PMHelpButton()
    {
        HelpAndRules.SetActive(true);
        HelpAndEscape.SetActive(false);
        Help.SetActive(true);
    }
    public void PMExitButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void IGSettingCurrentTurn()
    {
        PlayerTurn.text = GameController.instance.turnCounter.name;
    }
    public void IGInitialiseUI()
    {
        //Activates the names, money, etc
    }
    public void SetAllCardNames()
    {
        for (int i = 0; i < counters.Length; i++)
        {
            PlayerNameElements[i].transform.Find("Name").GetComponent<TextMeshProUGUI>().text = counters[i].name;
            PlayerNameElements[i].transform.Find("Money").GetComponent<TextMeshProUGUI>().text = counters[i].portfolio.TotalValue().ToString();
        }
    }
    public void SetPlayerTurnDullCard(int cardToDull)
    {
        //We'd dull everyone's colours but the Player who's ID we have, maybe we dull all then lighten the right card
        //Some array of colours saved, 6 for regular colours and 6 for dull

        //gameObject.GetComponent<Renderer>().material.color = new Color(0f, 0f, 0f);

    }
    public void SetLeaderboard()
    {
        //text = this.gameObject.GetComponent<TextMeshPro>();
        //text.text = "name1:score1\nname2:score2\nname3:score3\nname4:score4\nname5:score5\nname6:score6"

        //Set the leaderboard name by getting the name from the player controller (For loop)
        //Set the leaderboard money by getting the money from the player controller (For loop)
    }



    //----------game composition menu----------



}
