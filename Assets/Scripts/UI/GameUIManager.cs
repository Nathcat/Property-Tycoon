using System;
using System.Collections;
using System.Collections.Generic;
using Codice.Client.BaseCommands;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameUIManager : MonoBehaviour
{

    /// <summary>
    /// A record used to return dice roll data.
    /// </summary>
    /// <param name="dice1"> First dice value. </param>
    /// <param name="dice2"> Second dice value. </param>
    /// <param name="doubleRoll"> Whether the dice rolls are the same. </param>
    public record RollData(int dice1, int dice2, bool doubleRoll);

    /// <summary>
    /// Used to represent the current state of a yes / no prompt
    /// </summary>
    /// <param name="waiting">Whether or not we are currently waiting for a response</param>
    /// <param name="response">The actual response itself</param>
    public record PromptState(bool waiting, bool response);

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
    /// The Ok prompt UI
    /// </summary>
    [SerializeField] private GameObject okPromptUI;
    /// <summary>
    /// Menu controlling the auction process
    /// </summary>
    [SerializeField] private GameObject auctionMenu;
    [HideInInspector] public AuctionManager auctionManager { get { return auctionMenu.GetComponent<AuctionManager>(); } }
    /// <summary>
    /// UI for displaying cards
    /// </summary>
    [SerializeField] private GameObject cardUI;
    /// <summary>
    /// Card title
    /// </summary>
    [SerializeField] private TextMeshProUGUI cardTitle;
    /// <summary>
    /// Card description
    /// </summary>
    [SerializeField] private TextMeshProUGUI cardDesc;
    /// <summary>
    /// Timer for the abridged version of the game
    /// </summary>
    [SerializeField] private GameObject gameTimer;
    /// <summary>
    /// Screen used when the game ends.
    /// </summary>
    [SerializeField] private GameObject gameEndScreen;
    /// <summary>
    /// The actual text for the game timer.
    /// </summary>
    [SerializeField] private TextMeshProUGUI gameTimerText;
    /// <summary>
    /// Popup to show the player has a get out of jail free card available
    /// </summary>
    [SerializeField] private GameObject GetOutOfJailFree;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private GameObject AIThinkingUI;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField] private TextMeshProUGUI AIThinkingText;
    /// <summary>
    /// Called on completion of a yes / no prompt
    /// </summary>
    private System.Action<bool> onYesNoResponse;
    /// <summary>
    /// The player cards displayed in the main UI
    /// </summary>

    [SerializeField] private GameObject[] playerCardElements;
    /// <summary>
    /// The textures used to display the dice roll
    /// </summary>
    [SerializeField] private Texture[] diceTextures;
    /// <summary>
    /// The state the UI was in before its current state
    /// </summary>
    [SerializeField] private bool[] previousUIState = new bool[] { true, false, false, false };
    [SerializeField] private bool[] currentUIState = new bool[] { true, false, false, false };

    /// <summary>
    /// The last response from a yes / no prompt
    /// </summary>
    [HideInInspector] public PromptState promptState { get; private set; } = null;

    [HideInInspector] public RollData lastDiceRoll { get; private set; } = null;
    [HideInInspector] public bool waitingForDiceRollComplete { get; private set; } = false;
    [HideInInspector] public bool waitingForAuction { get; private set; } = false;

    /// <summary>
    /// Used to wait for a yes / no prompt to complete
    /// </summary>
    private class WaitForPromptReply : CustomYieldInstruction
    {
        public override bool keepWaiting { get { return instance.promptState.waiting; } }
    }

    /// <summary>
    /// Used to wait for the dice roll to complete
    /// </summary>
    private class WaitForDiceRoll : CustomYieldInstruction
    {
        public override bool keepWaiting { get { return instance.waitingForDiceRollComplete; } }
    }

    /// <summary>
    /// Used to wait for an auction to complete
    /// </summary>
    private class WaitForAuction : CustomYieldInstruction
    {
        public override bool keepWaiting { get { return instance.waitingForAuction; } }
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
        this.okPromptUI.SetActive(false);
        this.GetOutOfJailFree.SetActive(false);
        this.auctionMenu.SetActive(false);
        this.cardUI.SetActive(false);
        this.gameEndScreen.SetActive(false);
        this.AIThinkingUI.SetActive(false);
        this.helpAndRulesMenu.transform.GetChild(0).gameObject.SetActive(true);
        this.helpAndRulesMenu.transform.GetChild(1).gameObject.SetActive(false);
    }

    void Update()
    {
        this.mainUI.SetActive(currentUIState[0]);
        this.helpAndRulesMenu.SetActive(currentUIState[1]);
        this.pauseMenu.SetActive(currentUIState[2]);
        this.diceRollUI.SetActive(currentUIState[3]);

        if (GameController.instance.abridged) UpdateTimer(GameController.instance.timeRemaining);

        bool GOJF = GameController.instance.turnCounter.getOutOfJailFree;
        if (GOJF)
        {
            this.GetOutOfJailFree.SetActive(true);
        }
        else
        {
            this.GetOutOfJailFree.SetActive(false);
        }
    }
    /// <summary>
    /// Set up the timer.
    /// </summary>
    /// <param name="inputTimer"></param>
    public void SetUpTimer(float inputTimer)
    {
        gameTimer.SetActive(true);
        UpdateTimer(inputTimer);
        Debug.Log("timer set up");
    }
    /// <summary>
    /// Update the timer to show the current remaining time, in hours, mins and seconds.
    /// </summary>
    /// <param name="inputTimer"></param>
    public void UpdateTimer(float inputTimer)
    {
        if (inputTimer <= 0)
        {
            gameTimerText.text = "Last Round";
        }
        else
        {
            float hours = Mathf.FloorToInt(inputTimer / 3600);
            float mins = Mathf.FloorToInt(inputTimer / 60);
            float seconds = Mathf.FloorToInt(inputTimer % 60);
            if (mins < 10)
            {
                if (seconds < 10)
                {
                    gameTimerText.text = hours + ":0" + mins + ":0" + seconds;
                }
                else
                {
                    gameTimerText.text = hours + ":0" + mins + ":" + seconds;
                }

            }
            else
            {
                if (seconds < 10)
                {
                    gameTimerText.text = hours + ":" + mins + ":0" + seconds;
                }
                else
                {
                    gameTimerText.text = hours + ":" + mins + ":" + seconds;
                }
            }
        }
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
    public IEnumerator YesNoPrompt(string prompt)
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
    /// Create a prompt with a simple Ok prompt.
    /// </summary>
    /// <param name="prompt"></param>
    /// <returns></returns>
    public IEnumerator OkPrompt(string prompt)
    {
        promptState = new PromptState(true, false);
        SetUIState(false, false, false, false);
        this.okPromptUI.transform.Find("Prompt").GetComponent<TextMeshProUGUI>().text = prompt;
        this.okPromptUI.SetActive(true);

        return new WaitForPromptReply();
    }

    /// <summary>
    /// End an OK prompt
    /// </summary>
    public void EndOkPrompt()
    {
        promptState = new PromptState(false, false);
        this.okPromptUI.SetActive(false);
        RevertToPreviousUIState();
    }

    /// <summary>
    /// Start an auction
    /// </summary>
    public IEnumerator StartAuction()
    {
        Debug.Log("Starting auction.");
        SetUIState(false, false, false, false);
        previousUIState = new bool[4];
        this.auctionMenu.SetActive(true);
        this.auctionMenu.GetComponent<AuctionManager>().StartAuction(GameController.instance.spaces[GameController.instance.turnCounter.position] as Property);
        waitingForAuction = true;
        yield return new WaitForAuction();
    }

    public void ShowCard(string type, Card input)
    {
        this.cardUI.SetActive(true);
        this.cardTitle.text = type;
        this.cardDesc.text = input.description;
    }

    public void CloseCard()
    {
        this.cardUI.SetActive(false);
    }

    /// <summary>
    /// End an auction
    /// </summary>
    public void FinishAuction()
    {
        this.auctionMenu.SetActive(false);
        SetUIState(true, false, false, false);
        waitingForAuction = false;
    }

    /// <summary>
    /// Rolls two dice, and returns them, along with whether the dice rolls are the same.
    /// </summary>
    /// <returns> a record containing the two dice rolls as integers, as well as a boolean to denote whether the dice rolls were the same. </returns>
    private RollData DoDiceRoll()
    {
        // Gets the first dice's value
        int dice1 = UnityEngine.Random.Range(1, 7);
        // Gets the second dice's value
        int dice2 = UnityEngine.Random.Range(1, 7);

        return new RollData(dice1, dice2, dice1 == dice2);
    }

    /// <summary>
    /// Start a dice roll through the UI
    /// </summary>
    /// <returns></returns>
    public IEnumerator RollDice()
    {
        waitingForDiceRollComplete = true;
        SetUIState(false, false, false, true);
        lastDiceRoll = DoDiceRoll();
        diceRollUI.transform.GetChild(0).GetComponent<RawImage>().texture = diceTextures[lastDiceRoll.dice1 - 1];
        diceRollUI.transform.GetChild(1).GetComponent<RawImage>().texture = diceTextures[lastDiceRoll.dice2 - 1];
        diceRollUI.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = lastDiceRoll.dice1 + lastDiceRoll.dice2 + (lastDiceRoll.doubleRoll ? "\nDouble! Roll again!" : "");

        yield return new WaitForDiceRoll();
    }

    /// <summary>
    /// Complete a dice roll
    /// </summary>
    public void CompleteDiceRoll()
    {
        SetUIState(true, false, false, false);
        waitingForDiceRollComplete = false;
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
    /// Clears all UI and shows which AI is thinking
    /// </summary>
    /// <param name="name"> name of the AI. </param>
    public void AIStartThinking(string name)
    {
        AIThinkingUI.gameObject.SetActive(true);
        SetUIState(false, false, false, false);
        AIThinkingText.text = name + " is thinking...";
    }

    /// <summary>
    /// Clears the AI thinking screen and restores previous UI
    /// </summary>
    public void AIStopThinking()
    {
        RevertToPreviousUIState();
        AIThinkingUI.gameObject.SetActive(false);
    }

    /// <summary>
    /// Returns to the main menu.
    /// </summary>
    public void EndMenuClicked()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
