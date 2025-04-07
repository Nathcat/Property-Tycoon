using System;
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
    /// The player cards displayed in the main UI
    /// </summary>
    [SerializeField] private GameObject[] playerCardElements;
    /// <summary>
    /// The state the UI was in before its current state
    /// </summary>
    [SerializeField] private bool[] previousUIState = new bool[] { true, false, false, false };
    [SerializeField] private bool[] currentUIState = new bool[] { true, false, false, false };

    public record PromptState(bool waiting, bool response);

    /// <summary>
    /// The last response from a yes / no prompt
    /// </summary>
    [HideInInspector] public PromptState promptState { get; private set; } = null;

    public class WaitForPromptReply : CustomYieldInstruction
    {
        public override bool keepWaiting { get { return instance.promptState.waiting; } }
    }

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

        // Disable all but the main UI
        SetUIState(true, false, false, false);
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
    public CustomYieldInstruction YesNoPrompt(string prompt)
    {
        promptState = new PromptState(true, false);
        SetUIState(true, false, false, false);
        this.yesNoPromptUI.transform.Find("Prompt").GetComponent<TextMeshProUGUI>().text = prompt;
        this.yesNoPromptUI.SetActive(true);

        return new WaitForPromptReply();
    }

    /// <summary>
    /// Called when a yes / no prompt has a positive answer
    /// </summary>
    public void PromptReplyYes()
    {
        //onYesNoResponse(true);
        promptState = new PromptState(false, true);
        this.yesNoPromptUI.SetActive(false);
        RevertToPreviousUIState();
    }

    /// <summary>
    /// Called when a yes / no prompt has a negative answer
    /// </summary>
    public void PromptReplyNo()
    {
        //onYesNoResponse(false);
        promptState = new PromptState(false, false);
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
}
