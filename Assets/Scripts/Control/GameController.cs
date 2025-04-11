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
    [SerializeField] public Mesh[] upgradeMeshes;

    /// <summary> The currently active <see cref="GameController"/> instance. </summary>
    public static GameController instance { get; private set; }

    /// <summary> The space prefab to instantiate along egdes of the board. </summary>
    [SerializeField] private GameObject normalSpace;

    /// <summary> The space prefab to instantiate at the four corners of the board. </summary>
    [SerializeField] private GameObject cornerSpace;

    /// <summary> Event invoked when the next turn is started. </summary>
    public readonly UnityEvent<CounterController> onNextTurn = new UnityEvent<CounterController>();
    /// <summary>
    /// Invoked when a counter is physically moved
    /// </summary>
    public readonly UnityEvent<CounterController> onCounterMove = new UnityEvent<CounterController>();

    /// <summary> List of all <see cref="CounterController"/> particiapting in the game. </summary>
    public CounterController[] counters { get; private set; }

    /// <summary> Array of <see cref="Space"/> objects the board consists of. </summary>
    public Space[] spaces { get { return board.spaces; } }

    /// <summary> The number of spaces in the board. </summary>
    public int boardLength { get { return spaces.Length; } }

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
    [HideInInspector] public bool abridged { get; private set; }

    /// <summary> A float to hold the remaining time (in seconds), if playing the abridged version of the game. </summary>
    [HideInInspector] public float timeRemaining { get; private set; }

    /// <summary> a flag to show if the timer has expired </summary>
    [HideInInspector] public bool timeExpired { get { return abridged && timeRemaining <= 0; } }

    /// <summary> a flag to show when the game has ended. </summary>
    [HideInInspector] public bool gameOver;

    [SerializeField] private CounterController humanCounterPrefab;
    [SerializeField] private CounterController aiCounterPrefab;

    /// <summary> Stores the models of each counter </summary>
    public GameObject[] counterModels;

    /// <summary> Icons to represent each counter </summary>
    public Sprite[] counterIcons;
    public Terrain terrain;
    public GameObject environment;

    private void Awake()
    {
        if (instance != null && !instance.IsDestroyed())
            throw new DuplicateInstanceException();
        instance = this;
    }

    /// <summary>
    /// Starts the game by calling the first counter's turn.
    /// </summary>
    public void StartGame()
    {
        turnIndex = -1;
        NextTurn();
    }

    /// <summary>
    /// Setup the gamemode for the game.
    /// </summary>
    /// <param name="abridged">Wether this game is abridge or not</param>
    /// <param name="time">The total time for the game if its abridged</param>
    public void SetupGamemode(bool abridged, float time)
    {
        this.abridged = abridged;
        this.timeRemaining = time;
    }

    /// <summary> Increment <see cref="turnIndex"/> and start the next turn.</summary>
    public void NextTurn()
    {
        turnIndex = (turnIndex + 1) % counters.Length;

        if (counters.Length == 1 || (turnIndex == 0 && timeExpired)) EndGame();
        else
        {
            Debug.Log(turnCounter);
            GameUIManager.instance.UpdateUIForNewTurn(turnCounter);
            StartCoroutine(turnCounter.PlayTurn());
            onNextTurn.Invoke(turnCounter);
        }

        onNextTurn.Invoke(turnCounter);
    }

    /// <summary> Parse board configuration and place spaces - assumes board.csv in the Assets directory. </summary>
    public void SetupBoard()
    {
        SetupBoard(Path.Combine(Application.dataPath, "board.csv"));
    }

    /// <summary> Parse board configuration and place spaces. </summary>
    public void SetupBoard(string dir)
    {
        // Cleanup old counter controllers
        if (spaceControllers != null)
            foreach (SpaceController space in spaceControllers)
                Destroy(space.gameObject);

        board = FileManager.ReadBoardCSV(dir);
        spaceControllers = BoardGenerator.GenerateBoard(transform, 2, 1, normalSpace, cornerSpace, spaces);

        float environmentScale = Camera.main.GetComponent<CameraController>().boardRadius / 4.5f;
        environment.transform.localScale = new Vector3(environmentScale, environmentScale, environmentScale);
        terrain.terrainData.size = new Vector3(50f * environmentScale, 600f * environmentScale, 50f * environmentScale);
        terrain.transform.position = new Vector3((50f * environmentScale) / -2f, -4.54f * environmentScale, (50f * environmentScale) / -2f);

        Debug.Log(BoardGenerator.GetBoardDimensions(spaces.Count()));
    }

    /// <summary> Put the card information from the csv file into the relevant card decks, and shuffle both decks - assumes cards.csv in the Assets directory. </summary>
    public void SetupCards()
    {
        SetupCards(Path.Combine(Application.dataPath, "cards.csv"));
    }


    /// <summary> Put the card information from the csv file into the relevant card decks, and shuffle both decks. </summary>
    public void SetupCards(string dir)
    {
        cards = FileManager.ReadCardCSV(dir);
        luckDeck = cards.luck;
        opportunityDeck = cards.opportunity;
        //shuffle the pot luck cards
        ShufflePotluck();
        //shuffle the opportunity knocks cards
        ShuffleOpportunity();

    }

    /// <summary> Shuffle the opportunitydeck card deck using a BogoSort style method. </summary>
    public void ShuffleOpportunity()
    {
        opportunityDeck = new Queue<Card>(ShuffleList<Card>(opportunityDeck.ToList<Card>()));
    }

    /// <summary> Shuffle the potluck deck card deck using a BogoSort style method. </summary>
    public void ShufflePotluck()
    {
        luckDeck = new Queue<Card>(ShuffleList<Card>(luckDeck.ToList<Card>()));
    }

    /// <summary>
    /// Shuffle the provided list, returning the result. The provided list is not affected
    /// </summary>
    /// <typeparam name="T">The type stored by the list</typeparam>
    /// <param name="l">The list to shuffle</param>
    /// <returns>The shuffled list</returns>
    public List<T> ShuffleList<T>(List<T> l)
    {
        List<T> res = new List<T>();

        while (res.Count != l.Count)
        {
            res.Add(l[Random.Range(0, l.Count)]);
        }

        return res;
    }


    /// <summary>
    /// Draw and run the action for the top card of the Pot Luck deck on the given counter
    /// </summary>
    /// <param name="counterController">The counter to run the card action on</param>
    public Card DrawLuck(CounterController counterController)
    {
        if (luckDeck.Count != 0)
        {
            Card removed = luckDeck.Dequeue();
            StartCoroutine(removed.action.Run(counterController));
            DiscardLuck(removed);
            return removed;
        }
        else
        {
            Debug.Log("Deck is empty");
            return null;
        }
    }


    /// <summary>
    /// Draw and run the action for the top card of the Opportunity Knocks deck on the given counter
    /// </summary>
    /// <param name="counterController">The counter to run the card action on</param>
    public Card DrawOpportunity(CounterController counterController)
    {
        if (opportunityDeck.Count != 0)
        {
            Card removed = opportunityDeck.Dequeue();
            StartCoroutine(removed.action.Run(counterController));
            DiscardOpportunity(removed);
            return removed;

        }
        else
        {
            Debug.Log("Deck is empty");
            return null;
        }
    }

    /// <summary>
    /// Peek at the next card to be drawn from the Pot Luck deck; For the purposes of testing, should not be called otherwise.
    /// 
    /// </summary>
    /// <returns>the next card to be drawn from the Pot Luck deck</returns>
    public Card PeekLuck()
    {
        return luckDeck.Peek();
    }

    /// <summary>
    /// peek at the next card to be drawn from the Opportunity Knocks deck; For the purposes of testing, should not be called otherwise.
    /// </summary>
    /// <returns>the next card to be drawn from the Opportunity Knocks deck</returns>
    public Card PeekOpportunity()
    {
        return opportunityDeck.Peek();
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
    /// Register 6 test counters to the game.
    /// </summary>
    public void SetupCounters()
    {
        SetupCounters(new CounterConfig[6].Select((_, i) =>
            new CounterConfig($"Player {i}", CounterType.Human, i)).ToArray()
        );
    }

    /// <summary>
    /// Register counters to the game.
    /// </summary>
    /// <param name="counters">An array of the counters in this game.</param>
    public void SetupCounters(CounterConfig[] counters)
    {
        this.counters = counters.Select((c, i) =>
        {
            CounterController prefab = c.type == CounterType.Human ? humanCounterPrefab : aiCounterPrefab;
            CounterController o = Instantiate(prefab);
            o.gameObject.name = c.name;
            o.PickModel(c.model);
            return o;
        }).ToArray();
    }

    public void Update()
    {
        if (abridged && !timeExpired) timeRemaining -= Time.deltaTime;
    }

    /// <summary>
    /// Displays the winner of the game.
    /// </summary>
    public void EndGame()
    {
        gameOver = true;

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

        GameUIManager.instance.EndGame(counters[winner].name, totals[winner]);
    }

    /// <summary>
    /// Removes the <paramref name="counter"/> from the game.
    /// </summary>
    /// <param name="counter">The <see cref="CounterController"/> to remove</param>
    public void forefit(CounterController counter)
    {
        bool current = turnIndex == counter.order;

        counters = counters.RemoveAt(counter.order);
        counter.portfolio.forefit();
        counter.gameObject.SetActive(false);

        Debug.Log(counters);

        if (turnIndex > 0) turnIndex--;
        if (current) NextTurn();
    }
}
