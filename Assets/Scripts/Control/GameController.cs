using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;

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

    /// <summary> List of all <see cref="CounterController"/> particiapting in the game. </summary>
    public List<CounterController> counters { get; private set; }

    /// <summary> Array of <see cref="Space"/> objects the board consists of. </summary>
    public Space[] spaces { get; private set; }

    /// <summary> The index in <see cref="counters"/> of the <see cref="CounterController"/> who currently has their turn. </summary>
    public int turnIndex { get; private set; }

    /// <summary> The <see cref="CounterController"/> who currently has their turn. </summary>
    public CounterController turnCounter { get { return counters[turnIndex]; } }

    /// <summary> Array of <see cref="SpaceController"/> objects the board consists of. </summary>
    public SpaceController[] spaceControllers { get; private set; }

    /// <summary> Array of <see cref="PropertyGroup"/>s on the board. </summary>
    public PropertyGroup[] groups { get; private set; }

    private void Awake()
    {
        if (instance != null && !instance.IsDestroyed())
            throw new DuplicateInstanceException();
        instance = this;
    }

    private void Start()
    {
        InitBoard();
    }

    public void InitBoard()
    {
        FileManager.BoardData board = FileManager.ReadBoardCSV(Path.Combine(Application.dataPath, "board.csv"));
        spaces = board.spaces;
        groups = board.groups;

        spaceControllers = BoardGenerator.GenerateBoard(transform, 2, 1, normalSpace, cornerSpace, spaces);
    }

    public void InitPlayers()
    {

    }
}
