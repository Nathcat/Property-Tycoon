using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;


/// <summary>
/// CounterController: A class used to control a counter, either controlled by a player or an AI.
/// </summary>
public class HumanCounter : CounterController
{
    public override bool isControllable { get { return true; } }

    override public IEnumerator GoToJail()
    {
        MoveAbsolute(GameController.instance.jailSpace.position);

        Debug.Log(name + " has gone to jail, can they pay?");

        if (getOutOfJailFree)
        {
            Debug.Log("... they have a get out of jail free card!");
            //Utils.RunAfter(1, GameController.instance.NextTurn);
            yield return new WaitForSeconds(1f);
        }

        if (portfolio.GetCashBalance() >= 50)
        {
            Debug.Log("... they can pay, asking if they want to...");

            yield return GameUIManager.instance.YesNoPrompt("Pay £50 to get out of jail?");
            bool reply = GameUIManager.instance.promptState.response;

            if (reply)
            {
                Cash fine = new Cash(50);
                portfolio.RemoveCash(fine);
                GameController.instance.freeParking.AddCash(fine);
                Debug.Log(name + " pays to leave jail!");

                //Utils.RunAfter(1, GameController.instance.NextTurn);
            }
            else
            {
                isInJail = true;
                Debug.Log(name + " is now in jail! Jail space is at position " + GameController.instance.jailSpace.position);

                //Utils.RunAfter(1, GameController.instance.NextTurn);
            }
        }
        else
        {
            Debug.Log("... they cannot pay.");
            isInJail = true;
            Debug.Log(name + " is now in jail! Jail space is at position " + GameController.instance.jailSpace.position);
            //Utils.RunAfter(1, GameController.instance.NextTurn);
            yield return new WaitForSeconds(1f);
        }
    }

    /// <summary>
    /// Rolls both dice, and moves the counter. If the roll is a double roll, the dice are rolled again and counter moved again.
    /// </summary>
    override public IEnumerator PlayTurn()
    {
        // Enable the end turn button
        if (!isInJail)
        {
            // Roll three times
            yield return GameUIManager.instance.RollDice();

            MoveCounter(lastRoll.dice1, lastRoll.dice2);
            int doubles = 0;
            while (lastRoll.doubleRoll)
            {
                doubles++;

                if (doubles == 3)
                {
                    break;
                }

                yield return GameUIManager.instance.RollDice();

                MoveCounter(lastRoll.dice1, lastRoll.dice2);
            }

            // If 3 doubles have been rolled, go to jail
            if (doubles == 3)
            {
                Debug.Log(name + " has rolled 3 doubles, going to jail!");
                yield return GoToJail();
                yield break;
            }

            int oldPos = position;
            do
            {
                oldPos = position;
                yield return GameController.instance.spaces[position].action.Run(this);
            } while (oldPos != position);


            Space space = GameController.instance.spaces[position];

            if (space is Property)
            {
                Property p = space as Property;

                if (!p.isOwned && canPurchaseProperties)
                {
                    if (p.CanPurchase(this))
                    {
                        yield return GameUIManager.instance.YesNoPrompt("Would you like to buy " + p.name + " for £" + p.GetValue() + "?");

                        bool reply = GameUIManager.instance.promptState.response;
                        if (reply)
                        {
                            p.Purchase(this);
                            Debug.Log(name + " has purchased " + p.name);
                            yield return GameUIManager.instance.OkPrompt(p.owner.name + " now owns " + p.name);
                        }
                        else
                        {
                            yield return GameUIManager.instance.StartAuction();
                            yield return GameUIManager.instance.OkPrompt(p.owner.name + " now owns " + p.name);
                        }
                    }
                    else
                    {
                        yield return GameUIManager.instance.StartAuction();
                        yield return GameUIManager.instance.OkPrompt(p.owner.name + " now owns " + p.name);
                    }
                }
            }

            CameraController.instance.SetTarget(spaceController.gameObject);
        }
        else
        {
            turnsInJail++;

            Debug.Log(name + " has been in jail for " + turnsInJail + " turns");
            if (turnsInJail == 2)
            {
                Debug.Log(name + " has done their time in jail!");
                LeaveJail();
            }
        }

        GameUIManager.instance.Endable();
    }
}
