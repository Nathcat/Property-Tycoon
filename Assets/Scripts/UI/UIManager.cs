using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    //MainMenu Fields
    [SerializeField] private GameObject StartScreen = null;
    [SerializeField] private GameObject Credits = null;
    [SerializeField] private GameObject HelpAndRules = null;
    [SerializeField] private GameObject HelpAndRules2 = null;
    [SerializeField] private GameObject ConfirmClose = null;
    [SerializeField] private GameObject Settings = null;
    [SerializeField] private GameObject Help = null;
    [SerializeField] private GameObject Rules = null;


    //InGame Fields
    [SerializeField] private GameObject HelpAndEscape = null;
    [SerializeField] private GameObject PauseButton = null;
    [SerializeField] private GameObject[] Dice;
    [SerializeField] private GameObject[] PlayerCardElements;
    [SerializeField] private TextMeshProUGUI PlayerTurn;
    public int DiceValue1;
    public int DiceValue2;


    //AuctionMenu Fields
    [SerializeField] private GameObject AuctionScreen = null;


    //Scene Management
    [SerializeField] private Scene Scene;
    [SerializeField] private string CurrentScene = "";
    [SerializeField] private string GameScene = "Game";

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
            TurnOffPlayerCards();
            //SetupScreen.SetActive(true);
            PauseButton.SetActive(true);
            HelpAndEscape.SetActive(false);
            HelpAndRules.SetActive(false);
            Rules.SetActive(false);
            Help.SetActive(false);

            foreach (GameObject d in Dice)
            {
                d.SetActive(false);
            }
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
            TurnOffPlayerCards();
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
    public void EMYesButton()
    {
        //Close the program, I think we should do a while true loop that eats all their RAM >:3
        Application.Quit();
    }
    //----------help menu----------
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


    //----------game----------

    public void IGPauseButton()
    {
        //Stop all functions of the game
        HelpAndEscape.SetActive(true);
    }
    public void PMBackButton()
    {
        TurnOnPlayerCards();
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
        TurnOnPlayerCards();
        PauseButton.SetActive(true);
        //SetupScreen.SetActive(false);
        //Activates the names, money, etc
    }
    public void IGSetDiceValues()
    {
        //Roll dice with value
        //Pass to appropriate functions
        //Dice[0].text = 
        //Dice[1].text = 
    }
    public void SetAllCardNames()
    {
        for (int i = 0; i < GameController.instance.counters.Length; i++)
        {
            PlayerCardElements[i].transform.Find("Name").GetComponent<TextMeshProUGUI>().text = GameController.instance.counters[i].name;
            PlayerCardElements[i].transform.Find("Money").GetComponent<TextMeshProUGUI>().text = GameController.instance.counters[i].portfolio.GetCashBalance().ToString();
        }
    }
    public void TurnOffPlayerCards()
    {
        for (global::System.Int32 i = 0; i < PlayerCardElements.Length; i++)
        {
            PlayerCardElements[i].SetActive(false);
        }
    }
    public void TurnOnPlayerCards()
    {
        for (global::System.Int32 i = 0; i < PlayerCardElements.Length; i++)
        {
            PlayerCardElements[i].SetActive(true);
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
    //----------property buying----------------
    public void PBOption(bool Selection)
    {
        //If true, they bought property, deduct money and add it to their portfolio
        //If false, they auctioned, call AMOpen()
    }
    //----------auction menu-------------------
    public void AMOpen()
    {
        AuctionScreen.SetActive(true);
        TurnOnPlayerCards();
        PauseButton.SetActive(true);
        HelpAndEscape.SetActive(false);
        HelpAndRules.SetActive(false);
        Rules.SetActive(false);
        Help.SetActive(false);
        Dice[0].SetActive(false);
        Dice[1].SetActive(false);
        SetAllAuctionCardNames();
    }
    public void AMClose()
    {
        TurnOnPlayerCards();
        PauseButton.SetActive(true);
        HelpAndEscape.SetActive(false);
        HelpAndRules.SetActive(false);
        Rules.SetActive(false);
        Help.SetActive(false);
        Dice[0].SetActive(false);
        Dice[1].SetActive(false);
        AuctionScreen.SetActive(false);
        SetAllCardNames();
    }
    public void AMBid(int Bid, int PlayerID)
    {
        //Increment player's bid by X if their money is above X+HighestBid
        //If it can't, say error?
        //Check if this is highest bid, if so player is set as the 'winner' if the auction ends (All withdraw but them, if another player bids above this they are set to the 'winner') and update the ui element
    }
    public void AMWithdraw(int PlayerID)
    {
        //Remove player from list of bidders
        //If list is 1, call end auction
    }
    public void AMEndAuction()
    {
        //Awards player X with the property and deducts the money from their money
        //Calls AMClose()
    }
    public void SetAllAuctionCardNames()
    {
        for (int i = 0; i < GameController.instance.counters.Length; i++)
        {
            PlayerCardElements[i].transform.Find("Name").GetComponent<TextMeshProUGUI>().text = GameController.instance.counters[i].name;
            //PlayerCardElements[i].transform.Find("Money").GetComponent<TextMeshProUGUI>().text = //This would be wherever we get the bid amount from
        }
    }



    //----------game composition menu----------



}
