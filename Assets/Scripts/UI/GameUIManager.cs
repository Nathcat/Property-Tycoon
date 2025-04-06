using System;
using System.Collections.Generic;
using Codice.Client.BaseCommands;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    /// Timer for the abridged version of the game
    /// </summary>
    [SerializeField] private GameObject gameTimer;
    /// <summary>
    /// Screen used when the game ends.
    /// </summary>
    [SerializeField] private GameObject gameEndScreen;
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

    /// <summary>
    /// Set the state of the UI displays (active or inactive)
    /// </summary>
    /// <param name="helpAndRulesMenu">The desired state of the <see cref="helpAndRulesMenu"/></param>
    /// <param name="mainUI">The desired state of the <see cref="mainUI"/></param>
    /// <param name="pauseMenu">The desired state of the <see cref="pauseMenu"/></param>
    /// <param name="diceRollUI">The desired state of the <see cref="diceRollUI"></param>
    private void SetUIState(bool mainUI, bool helpAndRulesMenu, bool pauseMenu, bool diceRollUI)
    {
        previousUIState = (bool[])currentUIState.Clone();
        currentUIState = new bool[] { mainUI, helpAndRulesMenu, pauseMenu, diceRollUI };
    }

    /// <summary>
    /// Revert the UI state back to its previous state
    /// </summary>
    private void RevertToPreviousUIState()
    {
        bool[] tmp = (bool[])currentUIState.Clone();
        currentUIState = previousUIState;
        previousUIState = tmp;
    }

    void Start()
    {
        instance = this;

        // Disable all but the main UI
        SetUIState(true, false, false, false);
        this.gameTimer.SetActive(false);
        this.yesNoPromptUI.SetActive(false);
        this.auctionMenu.SetActive(false);
        this.gameEndScreen.SetActive(false);
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
    /// Set up the timer.
    /// </summary>
    /// <param name="inputTimer"></param>
    public void SetUpTimer(float inputTimer)
    {
        gameTimer.SetActive(true);
        gameTimer.transform.Find("Background").GetChild(1).GetComponent<TextMeshProUGUI>().text = inputTimer.ToString();
        Debug.Log("timer set up");
    }
    /// <summary>
    /// Update the timer to show the current remaining time, in hours, mins and seconds.
    /// </summary>
    /// <param name="inputTimer"></param>
    public void UpdateTimer(float inputTimer)
    {
        float hours = (float)System.Math.Truncate(inputTimer / 3600);
        float mins = (float)System.Math.Truncate(inputTimer / 60);
        float seconds = (float)System.Math.Truncate(inputTimer % 60);
        if (mins < 10)
        {
            if (seconds < 10)
            {
                gameTimer.transform.Find("Background").GetChild(1).GetComponent<TextMeshProUGUI>().text = hours + ":0" + mins + ":0" + seconds;
            }
            else
            {
                gameTimer.transform.Find("Background").GetChild(1).GetComponent<TextMeshProUGUI>().text = hours + ":0" + mins + ":" + seconds;
            }
            
        }
        else
        {
            if (seconds < 10)
            {
                gameTimer.transform.Find("Background").GetChild(1).GetComponent<TextMeshProUGUI>().text = hours + ":" + mins + ":0" + seconds;
            }
            else
            {
                gameTimer.transform.Find("Background").GetChild(1).GetComponent<TextMeshProUGUI>().text = hours + ":" + mins + ":" + seconds;
            }
        }
    }
    /// <summary>
    /// Sets the timer to show the time limit has expired.
    /// </summary>
    public void EndTimer()
    {
        gameTimer.transform.Find("Background").GetChild(1).GetComponent<TextMeshProUGUI>().text = "Time expired!";
    }
    /// <summary>
    /// Sets the timer to show that the final round has been reached.
    /// </summary>
    public void FinalRound()
    {
        gameTimer.transform.Find("Background").GetChild(0).GetComponent<TextMeshProUGUI>().text = "Final Round!";
        gameTimer.transform.Find("Background").GetChild(1).GetComponent<TextMeshProUGUI>().text = "";
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
    public void StartAuction()
    {
        Debug.Log("Starting auction.");
        SetUIState(false, false, false, false);
        previousUIState = new bool[4];
        this.auctionMenu.SetActive(true);
        this.auctionMenu.GetComponent<AuctionManager>().StartAuction(GameController.instance.spaces[GameController.instance.turnCounter.position] as Property);
    }

    /// <summary>
    /// End an auction
    /// </summary>
    public void FinishAuction()
    {
        this.auctionMenu.SetActive(false);
        SetUIState(true, false, false, false);
    }

    /// <summary>
    /// Brings up the game end screen, and displays the winner & their score.
    /// </summary>
    /// <param name="winner"> name of the winner. </param>
    /// <param name="score"> score of the winner. </param>
    public void EndGame(string winner, int score)
    {
        // hode everything but the end screen
        this.gameEndScreen.SetActive(true);
        SetUIState(false, false, false, false);
        this.gameTimer.SetActive(false);
        this.yesNoPromptUI.SetActive(false);
        this.auctionMenu.SetActive(false);
        gameEndScreen.transform.Find("winner").GetComponent<TextMeshProUGUI>().text = winner + " with a score of " + score;
    }

    /// <summary>
    /// Returns to the main menu.
    /// </summary>
    public void EndMenuClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
