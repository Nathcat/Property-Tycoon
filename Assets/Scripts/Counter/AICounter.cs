using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICounter : CounterController
{
    public override bool isControllable { get { return false; } }
    // Start is called before the first frame update
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


            if (portfolio.GetCashBalance() >= 500)
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
                        int percent = GetPercentage(p.GetValue(), portfolio.GetCashBalance());
                        if (percent < 50)
                        {
                            p.Purchase(this);
                            Debug.Log(name + " has purchased " + p.name);
                            yield return GameUIManager.instance.OkPrompt(p.owner.name + " now owns " + p.name);
                            Develop();
                            GameController.instance.NextTurn();

                        }
                        else
                        {
                            yield return GameUIManager.instance.StartAuction();
                            yield return GameUIManager.instance.OkPrompt(p.owner.name + " now owns " + p.name);
                            Develop();
                            GameController.instance.NextTurn();
                        }
                    }
                    else
                    {
                        yield return GameUIManager.instance.StartAuction();
                        yield return GameUIManager.instance.OkPrompt(p.owner.name + " now owns " + p.name);
                        Develop();
                        GameController.instance.NextTurn();
                    }
                }
                else
                {
                    GameController.instance.NextTurn();
                }
            }
            else
            {
                GameController.instance.NextTurn();
            }
        }
        else
        {
            turnsInJail++;

            Debug.Log(name + " has been in jail for " + turnsInJail + " turns");
            if (turnsInJail == 2)
            {
                Debug.Log(name + " has done their time in jail!");
                LeaveJail();
                GameController.instance.NextTurn();
            }
            else
            {
                GameController.instance.NextTurn();
            }
        }

    }

    override public IEnumerator DoAuctionTurn()
    {
        AuctionManager auction = GameUIManager.instance.auctionManager;
        Property p = auction.targetProperty;
        int percentage = 100;
        if (portfolio.GetCashBalance() > 0)
        {
            percentage = GetPercentage(p.GetValue(), portfolio.GetCashBalance());
        }
        Debug.Log(percentage);
        if (percentage < 80)
        {
            int offset = Random.Range(-20, 20);
            int bid = Mathf.RoundToInt(((p.GetValue() * 0.7f) + offset));
            Debug.Log(bid);
            int chance = Random.Range(1, 100);
            if (bid >= 100 && chance > 70)
            {
                auction.Bid100();
                yield return null;
            }
            chance = Random.Range(1, 100);
            if (bid >= 50 && chance > 70)
            {
                auction.Bid50();
                yield return null;
            }
            chance = Random.Range(1, 100);
            if (bid >= 20 && chance > 70)
            {
                auction.Bid20();
                yield return null;
            }
            chance = Random.Range(1, 100);
            if (bid >= 10 && chance > 70)
            {
                auction.Bid10();
                yield return null;
            }
            chance = Random.Range(1, 100);
            if (bid >= 5 && chance > 70)
            {

                auction.Bid5();
                yield return null;

            }
            if (bid >= 1 && chance > 70)
            {
                auction.Bid1();
                yield return null;
            }

        }
        else
        {
            auction.Withdraw();
            yield return null;
        }
    }
    public void Develop()
    {
        List<Property> properties = portfolio.GetProperties();
        for (int i = 0; i == properties.Count - 1; i++)
        {
            Debug.Log(i);
            Property p = properties[i];
            if (p.CanUpgrade())
            {
                int percentage = GetPercentage(p.upgradeCost, portfolio.GetCashBalance());
                int chance = Random.Range(1, 100);
                if (percentage < 25 && chance > 50)
                {
                    p.Upgrade();
                }
            }
        }
    }

    public int GetPercentage(int value1, int value2)
    {
        return Mathf.RoundToInt((value1 / value2) * 100);
    }

}
