using NUnit.Framework;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;

public class CounterTest
{

    [UnityTest]
    public IEnumerator JailTest()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        GameController.instance.SetupBoard();
        GameController.instance.SetupCounters();
        CounterController counter = GameController.instance.turnCounter;

        counter.StartCoroutine(counter.GoToJail());
        Assert.AreEqual(counter.position, GameController.instance.jailSpace.position);
    }

    [UnityTest]
    public IEnumerator GetOutOfJailTest()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        GameController.instance.SetupBoard();
        GameController.instance.SetupCounters();
        CounterController counter = GameController.instance.turnCounter;
        counter.getOutOfJailFree = true;
        counter.StartCoroutine(counter.GoToJail());
        Assert.IsFalse(counter.isInJail);
    }


    [UnityTest]
    public IEnumerator moveAbsoluteTest()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        GameController.instance.SetupBoard();
        GameController.instance.SetupCounters();
        CounterController counter = GameController.instance.turnCounter;

        counter.MoveAbsolute(20);
        Assert.IsTrue(counter.position.Equals(20));
    }

    [UnityTest]
    public IEnumerator moveDice()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        GameController.instance.SetupBoard();
        GameController.instance.SetupCounters();
        CounterController counter = GameController.instance.turnCounter;

        counter.MoveCounter(2, 2);
        Assert.IsTrue(counter.position.Equals(4));
    }

    [UnityTest]
    public IEnumerator RollDice()
    {
        SceneManager.LoadScene("Game");
        yield return null;
        GameController.instance.SetupBoard();
        GameController.instance.SetupCounters();
        CounterController counter = GameController.instance.turnCounter;

        counter.StartCoroutine(GameUIManager.instance.RollDice());
        Assert.IsTrue(counter.lastRoll.dice1 + counter.lastRoll.dice2 >= 2 && counter.lastRoll.dice1 + counter.lastRoll.dice2 <= 12);
    }
}
