using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameUIManager : MonoBehaviour
{
    /// <summary>
    /// The active instance of this class
    /// </summary>
    public static GameUIManager instance { get; private set; }

    /// <summary>
    /// This is the help and rules menu canvas
    /// </summary>
    [SerializeField] private GameObject helpAndRulesMenu;
    /// <summary>
    /// The main UI to be displayed during the game
    /// </summary>
    [SerializeField] private GameObject mainUI;
    /// <summary>
    /// The pause menu
    /// </summary>
    [SerializeField] private GameObject pauseMenu;
    /// <summary>
    /// The UI which displays the player's dice roll
    /// </summary>
    [SerializeField] private GameObject diceRollUI;
    /// <summary>
    /// The yes / no prompt UI
    /// </summary>
    [SerializeField] private GameObject yesNoPromptUI;
    /// <summary>
    /// Menu controlling the auction process
    /// </summary>
    [SerializeField] private GameObject auctionMenu;
    /// <summary>
    /// Called on completion of a yes / no prompt
    /// </summary>
    private System.Action<bool> onYesNoResponse;
    /// <summary>
    /// The player cards displayed in the main UI
    /// </summary>
    [SerializeField] private GameObject[] playerCardElements;
    /// <summary>
    /// The state the UI was in before its current state
    /// </summary>
    [SerializeField] private bool[] previousUIState = new bool[] { true, false, false, false };
    [SerializeField] private bool[] currentUIState = new bool[] { true, false, false, false };


    [SerializeField] private GameObject SetUpUI = null;
    [SerializeField] private GameObject Player1NameInput = null;
    [SerializeField] private GameObject Player2NameInput = null;
    [SerializeField] private GameObject Player3NameInput = null;
    [SerializeField] private GameObject Player4NameInput = null;
    [SerializeField] private GameObject Player5NameInput = null;
    [SerializeField] private GameObject Player6NameInput = null;
    private string Player1Name;
    private string Player2Name;
    private string Player3Name;
    private string Player4Name;
    private string Player5Name;
    private string Player6Name;
    [SerializeField] private GameObject Player1TypeInput = null;
    [SerializeField] private GameObject Player2TypeInput = null;
    [SerializeField] private GameObject Player3TypeInput = null;
    [SerializeField] private GameObject Player4TypeInput = null;
    [SerializeField] private GameObject Player5TypeInput = null;
    [SerializeField] private GameObject Player6TypeInput = null;
    private string Player1Type;
    private string Player2Type;
    private string Player3Type;
    private string Player4Type;
    private string Player5Type;
    private string Player6Type;
    [SerializeField] private GameObject GamemodeInput = null;
    private string Gamemode;
    [SerializeField] private GameObject CSVInput = null;
    private string CSV;
    [SerializeField] private GameObject Error = null;
    [SerializeField] private GameObject HourInput = null;
    [SerializeField] private GameObject MinInput = null;
    [SerializeField] private GameObject SecInput = null;
    private string Hour;
    private string Min;
    private string Sec;









    /// <summary>
    /// Set the state of the UI displays (active or inactive)
    /// </summary>
    /// <param name="helpAndRulesMenu">The desired state of the <see cref="helpAndRulesMenu"/></param>
    /// <param name="mainUI">The desired state of the <see cref="mainUI"/></param>
    /// <param name="pauseMenu">The desired state of the <see cref="pauseMenu"/></param>
    /// <param name="diceRollUI">The desired state of the <see cref="diceRollUI"></param>
    private void SetUIState(bool mainUI, bool helpAndRulesMenu, bool pauseMenu, bool diceRollUI)
    {
        previousUIState = (bool[]) currentUIState.Clone();
        currentUIState = new bool[] { mainUI, helpAndRulesMenu, pauseMenu, diceRollUI };
    }

    /// <summary>
    /// Revert the UI state back to its previous state
    /// </summary>
    private void RevertToPreviousUIState()
    {
        bool[] tmp = (bool[]) currentUIState.Clone();
        currentUIState = previousUIState;
        previousUIState = tmp;
    }

    void Start()
    {
        instance = this;
        SetUpUI.SetActive(true);
        


        // Disable all but the main UI
        SetUIState(false, false, false, false);
        this.yesNoPromptUI.SetActive(false);
        this.auctionMenu.SetActive(false);
        this.helpAndRulesMenu.transform.GetChild(0).gameObject.SetActive(true);
        this.helpAndRulesMenu.transform.GetChild(1).gameObject.SetActive(false);
    }

    void Update()
    {
        this.mainUI.SetActive(currentUIState[0]);
        this.helpAndRulesMenu.SetActive(currentUIState[1]);
        this.pauseMenu.SetActive(currentUIState[2]);
        this.diceRollUI.SetActive(currentUIState[3]);
    }

    /// <summary>
    /// Update the UI for a new turn
    /// </summary>
    /// <param name="currentTurn">The player whose turn it is now</param>
    public void UpdateUIForNewTurn(CounterController currentTurn)
    {
        SetUIState(true, false, false, false);
        UpdateAllPlayerCardData();
        SetCurrentTurnLabel(currentTurn);
        UpdateLeaderboard();
    }

    /// <summary>
    /// Set the data on the player cards
    /// </summary>
    private void UpdateAllPlayerCardData()
    {
        for (int i = 0; i < GameController.instance.counters.Length; i++)
        {
            playerCardElements[i].transform.Find("Name").GetComponent<TextMeshProUGUI>().text = GameController.instance.counters[i].name;
            playerCardElements[i].transform.Find("Money").GetComponent<TextMeshProUGUI>().text = GameController.instance.counters[i].portfolio.GetCashBalance().ToString();
        }
    }

    /// <summary>
    /// Set the value of the current turn label
    /// </summary>
    /// <param name="counterController">The counter controller whose name will be set in the current turn label</param>
    private void SetCurrentTurnLabel(CounterController counterController)
    {
        mainUI.transform.Find("CurrentTurn").GetChild(0).GetComponent<TextMeshProUGUI>().text = counterController.name + "'s turn";
    }

    /// <summary>
    /// Update the current state of the leaderboard
    /// </summary>
    private void UpdateLeaderboard()
    {
        List<CounterController> order = new List<CounterController>();
        int i;
        foreach (CounterController counterController in GameController.instance.counters)
        {
            if (order.Count == 0)
            {
                order.Add(counterController);
                continue;
            }

            i = 0;
            while (i < order.Count && order[i].portfolio.TotalValue() >= counterController.portfolio.TotalValue())
            {
                i++;
            }

            order.Insert(i, counterController);
        }

        string s = "";
        i = 1;
        foreach (CounterController counterController in order)
        {
            s += i.ToString() + (i == 1 ? "st" : (i == 2 ? "nd" : (i == 3 ? "rd" : "th"))) + " " + counterController.name + ": " + counterController.portfolio.TotalValue() + "\n";
            i++;
        }

        this.mainUI.transform.Find("Leaderboard").GetChild(0).GetComponent<TextMeshProUGUI>().text = s;
    }

    /// <summary>
    /// Open the pause menu and freeze the game flow
    /// </summary>
    public void PauseButtonClicked()
    {
        SetUIState(false, false, true, false);
        Time.timeScale = 0;
    }

    /// <summary>
    /// Close the pause menu, show the main UI and unfreeze the game flow
    /// </summary>
    public void ResumeButtonClicked()
    {
        SetUIState(true, false, false, false);
        Time.timeScale = 1;
    }

    /// <summary>
    /// Exit the game
    /// </summary>
    public void QuitButtonClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Open the help and rules menu
    /// </summary>
    public void HelpAndRulesButtonClicked()
    {
        SetUIState(false, true, false, false);
    }

    /// <summary>
    /// Change to the previous UI state
    /// </summary>
    public void BackButtonClicked()
    {
        RevertToPreviousUIState();
    }

    /// <summary>
    /// Display the help section of the help and rules menu
    /// </summary>
    public void HelpButtonClicked()
    {
        this.helpAndRulesMenu.transform.GetChild(0).gameObject.SetActive(true);
        this.helpAndRulesMenu.transform.GetChild(1).gameObject.SetActive(false);
    }

    /// <summary>
    /// Display the rules section of the help and rules menu
    /// </summary>
    public void RulesButtonClicked()
    {
        this.helpAndRulesMenu.transform.GetChild(0).gameObject.SetActive(false);
        this.helpAndRulesMenu.transform.GetChild(1).gameObject.SetActive(true);
    }

    /// <summary>
    /// Create a yes or no prompt on the UI
    /// </summary>
    /// <param name="prompt">The prompt message to display to the user</param>
    /// <param name="onResponse">The callback to execute once the user has replied</param>
    public void YesNoPrompt(string prompt, System.Action<bool> onResponse)
    {
        SetUIState(true, false, false, false);
        this.yesNoPromptUI.transform.Find("Prompt").GetComponent<TextMeshProUGUI>().text = prompt;
        this.yesNoPromptUI.SetActive(true);
        this.onYesNoResponse = onResponse;
    }

    /// <summary>
    /// Called when a yes / no prompt has a positive answer
    /// </summary>
    public void PromptReplyYes()
    {
        onYesNoResponse(true);
        onYesNoResponse = null;
        this.yesNoPromptUI.SetActive(false);
        RevertToPreviousUIState();
    }

    /// <summary>
    /// Called when a yes / no prompt has a negative answer
    /// </summary>
    public void PromptReplyNo()
    {
        onYesNoResponse(false);
        onYesNoResponse = null;
        this.yesNoPromptUI.SetActive(false);
        RevertToPreviousUIState();
    }

    /// <summary>
    /// Start an auction
    /// </summary>
    public void StartAuction() {
        Debug.Log("Starting auction.");
        SetUIState(false, false, false, false);
        previousUIState = new bool[4];
        this.auctionMenu.SetActive(true);
        this.auctionMenu.GetComponent<AuctionManager>().StartAuction(GameController.instance.spaces[GameController.instance.turnCounter.position] as Property);
    }

    /// <summary>
    /// End an auction
    /// </summary>
    public void FinishAuction() {
        this.auctionMenu.SetActive(false);
        SetUIState(true, false, false, false);
    }

    //----------Game Setup Menu(need to decide where we are putting this)----------
    public void SetupStart()
    {
        
        
        Debug.Log("logging");
        Player1Name = Player1NameInput.GetComponent<TMP_InputField>().text;
        Player2Name = Player2NameInput.GetComponent<TMP_InputField>().text;
        Player3Name = Player3NameInput.GetComponent<TMP_InputField>().text;
        Player4Name = Player4NameInput.GetComponent<TMP_InputField>().text;
        Player5Name = Player5NameInput.GetComponent<TMP_InputField>().text;
        Player6Name = Player6NameInput.GetComponent<TMP_InputField>().text;
        Hour = HourInput.GetComponent<TMP_InputField>().text;
        Min = MinInput.GetComponent<TMP_InputField>().text;
        Sec = SecInput.GetComponent<TMP_InputField>().text;
        CSV = CSVInput.GetComponent<TMP_InputField>().text;
        Player1Type = Player1TypeInput.GetComponent<TMP_Dropdown>().value.ToString();
        Player2Type = Player2TypeInput.GetComponent<TMP_Dropdown>().value.ToString();
        Player3Type = Player3TypeInput.GetComponent<TMP_Dropdown>().value.ToString();
        Player4Type = Player4TypeInput.GetComponent<TMP_Dropdown>().value.ToString();
        Player5Type = Player5TypeInput.GetComponent<TMP_Dropdown>().value.ToString();
        Player6Type = Player6TypeInput.GetComponent<TMP_Dropdown>().value.ToString();
        Gamemode = GamemodeInput.GetComponent<TMP_Dropdown>().value.ToString();
        Debug.Log(Gamemode +":" + Hour + ":" + Min + ":" + Sec + ":" + CSV + ":");
        Debug.Log(Player1Name + ":" + Player1Type);
        Debug.Log(Player2Name + ":" + Player2Type);
        Debug.Log(Player3Name + ":" + Player3Type);
        Debug.Log(Player4Name + ":" + Player4Type);
        Debug.Log(Player5Name + ":" + Player5Type);
        Debug.Log(Player6Name + ":" + Player6Type);

    }
}
