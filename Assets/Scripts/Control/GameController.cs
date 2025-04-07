using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

/// <summary> Main script for controlling high level flow of the game. </summary>
public class GameController : MonoBehaviour
{
    /// <summary> Thrown when attempting to instantiate multiple instances of <see cref="GameController"/> </summary>
    public class DuplicateInstanceException : System.Exception
    {
        public DuplicateInstanceException() : base("Attempted to instantiate a second GameController instance") { }
    }

    /// <summary> The currently active <see cref="GameController"/> instance. </summary>
    public static GameController instance { get; private set; }

    /// <summary> The space prefab to instantiate along egdes of the board. </summary>
    [SerializeField] private GameObject normalSpace;

    /// <summary> The space prefab to instantiate at the four corners of the board. </summary>
    [SerializeField] private GameObject cornerSpace;

    /// <summary> Event invoked when the next turn is started. </summary>
    public readonly UnityEvent<CounterController> onNextTurn = new UnityEvent<CounterController>();

    /// <summary> List of all <see cref="CounterController"/> particiapting in the game. </summary>
    public CounterController[] counters { get; private set; }

    /// <summary> Array of <see cref="Space"/> objects the board consists of. </summary>
    public Space[] spaces { get { return board.spaces; } }

    /// <summary>
    /// Get the jail space on the board
    /// </summary>
    public Space jailSpace { get { return board.jailSpace; } }

    /// <summary> The index in <see cref="counters"/> of the <see cref="CounterController"/> who currently has their turn. </summary>
    public int turnIndex { get; private set; }

    /// <summary> The <see cref="CounterController"/> who currently has their turn. </summary>
    public CounterController turnCounter { get { return counters[turnIndex]; } }

    /// <summary> Array of <see cref="SpaceController"/> objects the board consists of. </summary>
    public SpaceController[] spaceControllers { get; private set; }

    /// <summary> Array of <see cref="PropertyGroup"/>s on the board. </summary>
    public PropertyGroup[] groups { get { return board.groups; } }

    /// <summary> The configuration data of the currently loaded board. </summary>
    private FileManager.BoardData board;

    /// <summary> The configuration data for the two decks of cards. </summary>
    private FileManager.CardData cards;

    /// <summary> The card deck for the Opportunity Knocks cards. </summary>
    private Queue<Card> opportunityDeck;

    /// <summary> The card deck for the Pot Luck cards. </summary>
    private Queue<Card> luckDeck;

    /// <summary>
    /// The total cash collected in free parking
    /// </summary>
    [HideInInspector] public Cash freeParking = new Cash(0);

    /// <summary> A flag to show whether the game is abridged or not. </summary>
    public bool abridged { get; private set; }

    /// <summary> A float to hold the remaining time (in seconds), if playing the abridged version of the game. </summary>
    public float timeRemaining { get; private set; }

    /// <summary> a flag to show if the timer has expired </summary>
    public bool timeExpired { get  { return timeRemaining <= 0; } }


    [Header("Testing")]
    [SerializeField] private CounterController counterPrefab;

    private void Awake()
    {
        if (instance != null && !instance.IsDestroyed())
            throw new DuplicateInstanceException();
        instance = this;
    }

    private void Start()
    {
        SetupBoard();
        SetupCards();
        abridged = true;
        timeRemaining = (65);
        SetupTimer();
        //SetupCounters(new CounterController[6].Select(_ => Instantiate(counterPrefab)).ToArray());
        // this seems to break, but bring back if needed :)
        SetupCounters(new CounterController[6].Select((c, index) =>
        {
            CounterController o = Instantiate(counterPrefab);
            o.gameObject.name = "Player " + index;
            return o;
        }).ToArray());

        turnCounter.PlayTurn();
    }


    /// <summary> Increment <see cref="turnIndex"/> and start the next turn.</summary>
    public void NextTurn()
    {
        turnIndex = (turnIndex + 1) % counters.Length;
        
        if (turnIndex == 0) EndGame();
        else
        {
            GameUIManager.instance.UpdateUIForNewTurn(turnCounter);
            turnCounter.PlayTurn();
            onNextTurn.Invoke(turnCounter);
        }
    }

    /// <summary> Parse board configuration and place spaces. </summary>
    public void SetupBoard()
    {
        // Cleanup old counter controllers
        if (spaceControllers != null)
            foreach (SpaceController space in spaceControllers)
                Destroy(space.gameObject);

        board = FileManager.ReadBoardCSV(Path.Combine(Application.dataPath, "board.csv"));
        spaceControllers = BoardGenerator.GenerateBoard(transform, 2, 1, normalSpace, cornerSpace, spaces);
    }

    /// <summary> Put the card information from the csv file into the relevant card decks, and shuffle both decks. </summary>
    public void SetupCards()
    {
        cards = FileManager.ReadCardCSV(Path.Combine(Application.dataPath, "cards.csv"));
        luckDeck = cards.luck;
        opportunityDeck = cards.opportunity;
        //shuffle the pot luck cards
        //Shuffle(luckDeck);
        //shuffle the opportunity knocks cards
        //Shuffle(opportunityDeck);
    }

    public void SetupTimer()
    {
        if (abridged)
        {
            GameUIManager.instance.SetUpTimer(timeRemaining);
        }
    }

    /// <summary> Shuffle the given card deck using a BogoSort style method. </summary>
    /// <param name="cards"> The card deck to be shuffled. </param>
    public void Shuffle(Queue<Card> input)
    {
        int random;
        Card temp = null;
        Card[] cards = new Card[input.Count];
        for (int i = 0; i < input.Count; i++)
        {
            cards[i] = input.Dequeue();
        }

        for (int i = 0; i < cards.Length; i++)
        {
            random = Random.Range(i, cards.Length);
            temp = cards[random];
            cards[random] = cards[i];
            cards[i] = temp;
        }

        for (int i = 0; i < cards.Length; i++)
        {
            input.Enqueue(cards[i]);
        }

    }

    /// <summary> Draw and run the action for the top card from the Pot Luck deck. </summary>
    public void DrawLuck()
    {
        if (luckDeck.Count != 0)
        {
            Card removed = luckDeck.Dequeue();
            removed.action.Run(turnCounter);
            DiscardLuck(removed);
        }
        else
        {
            Debug.Log("Deck is empty");
        }
    }

    /// <summary>
    /// Draw and run the action for the top card of the Pot Luck deck on the given counter
    /// </summary>
    /// <param name="counterController">The counter to run the card action on</param>
    public void DrawLuck(CounterController counterController)
    {
        if (luckDeck.Count != 0)
        {
            Card removed = luckDeck.Dequeue();
            removed.action.Run(counterController);
            DiscardLuck(removed);
        }
        else
        {
            Debug.Log("Deck is empty");
        }
    }

    /// <summary>
    /// Draw and run the action for the top card of the Opportunity Knocks deck
    /// </summary>
    public void DrawOpportunity()
    {
        if (opportunityDeck.Count != 0)
        {
            Card removed = opportunityDeck.Dequeue();

            removed.action.Run(turnCounter);

            DiscardOpportunity(removed);
        }
        else
        {
            Debug.Log("Deck is empty");
        }
    }

    /// <summary>
    /// Draw and run the action for the top card of the Opportunity Knocks deck on the given counter
    /// </summary>
    /// <param name="counterController">The counter to run the card action on</param>
    public void DrawOpportunity(CounterController counterController)
    {
        if (opportunityDeck.Count != 0)
        {
            Card removed = opportunityDeck.Dequeue();

            removed.action.Run(counterController);

            DiscardOpportunity(removed);
        }
        else
        {
            Debug.Log("Deck is empty");
        }
    }

    /// <summary> Place a card onto the bottom of the Pot Luck deck. </summary>
    /// <param name="luckCard"> A card to be placed onto the bottom of the Pot Luck deck. </param>
    public void DiscardLuck(Card luckCard)
    {
        luckDeck.Enqueue(luckCard);
    }

    /// <summary> Place a card onto the bottom of the Opportunity Knocks deck. </summary>
    /// <param name="luckCard"> A card to be placed onto the bottom of the Opportunity Knocks deck. </param>
    public void DiscardOpportunity(Card opportunityCard)
    {
        opportunityDeck.Enqueue(opportunityCard);
    }



    /// <summary>
    /// Register <paramref name="counters"/> to the game.
    /// </summary>
    /// <param name="counters">An array of the counters in this game.</param>
    public void SetupCounters(CounterController[] counters)
    {
        this.counters = counters;
    }

    public void Update()
    {
        if (abridged && !timeExpired) timeRemaining -= Time.deltaTime;

    }

    /// <summary>
    /// Prints the winner of the game.
    /// </summary>
    public void EndGame()
    {
        int[] totals = new int[counters.Length];
        Debug.Log("number of players: " + counters.Length);
        for (int i = 0; i < counters.Length; i++)
        {
            totals[i] = counters[i].portfolio.TotalValue();
            Debug.Log("player " + i + " got a score of " + totals[i]);
        }
        int winner = 0;
        for (int i = 0; i < totals.Length; i++)
        {
            if (totals[i] > totals[winner])
            {
                winner = i;
            }
        }

        Debug.Log("Winner is " + counters[winner].name + " with a score of " + totals[winner]);

    }
}
