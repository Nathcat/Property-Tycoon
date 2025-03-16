using Unity.VisualScripting;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.Events;
using System.Collections.Generic;

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

    /// <summary> The card deck for the Opportunity Knocks cards. </summary>
    public List<Card> opportunityDeck;

    /// <summary> The card deck for the Pot Luck cards. </summary>
    public List<Card> luckDeck;

    /// <summary> Random used for shuffling the card decks. </summary>
    public System.Random rnd;



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
        SetupCounters(new CounterController[6].Select(_ => Instantiate(counterPrefab)).ToArray());
        turnCounter.PlayTurn();
    }

    /// <summary> Increment <see cref="turnIndex"/> and start the next turn.</summary>
    public void NextTurn()
    {
        turnIndex = (turnIndex + 1) % counters.Length;
        turnCounter.PlayTurn();
        onNextTurn.Invoke(turnCounter);
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
        FileManager.ReadCardCSV(Path.Combine(Application.dataPath, "card.csv"), luckDeck, opportunityDeck);
        rnd = new System.Random();
        //shuffle the pot luck cards
        Shuffle(luckDeck);
        //shuffle the opportunity knocks cards
        Shuffle(opportunityDeck);
    }

    /// <summary> Shuffle the given card deck using a BogoSort style method. </summary>
    /// <param name="cards"> The card deck to be shuffled. </param>
    public void Shuffle(List<Card> cards)
    {
        int random;
        Card temp = null;
        for (int i = 0; i < cards.Count; i++)
        {
            random  = rnd.Next(i, cards.Count);
            temp = cards[random];
            cards[random] = cards[i];
            cards[i] = temp;
        }
    }

    /// <summary> Draw and return the top card from the Pot Luck deck. </summary>
    /// <returns> The top card from the Pot Luck deck. </returns>
    public Card TakeLuck()
    {
        if(luckDeck.Count != 0)
        {
            return (luckDeck[0]);
        }
        else
        {
            return null;
        }
    }

    /// <summary> Draw and return the top card from the Opportunity Knocks deck. </summary>
    /// <returns> The top card from the Opportunity Knocks deck. </returns>
    public Card TakeOpportunity()
    {
        if (opportunityDeck.Count != 0)
        {
            return (opportunityDeck[0]);
        }
        else
        {
            return null;
        }
    }

    /// <summary> Place a card onto the bottom of the Pot Luck deck. </summary>
    /// <param name="luckCard"> A card to be placed onto the bottom of the Pot Luck deck. </param>
    public void ReturnLuck(Card luckCard)
    {
        luckDeck[luckDeck.Count] = luckCard;
    }

    /// <summary> Place a card onto the bottom of the Opportunity Knocks deck. </summary>
    /// <param name="luckCard"> A card to be placed onto the bottom of the Opportunity Knocks deck. </param>
    public void ReturnOpportunity(Card opportunityCard)
    {
        opportunityDeck[opportunityDeck.Count] = opportunityCard;
    }



    /// <summary>
    /// Register <paramref name="counters"/> to the game.
    /// </summary>
    /// <param name="counters">An array of the counters in this game.</param>
    public void SetupCounters(CounterController[] counters)
    {
        this.counters = counters;
    }
}
