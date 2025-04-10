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
                GameController.instance.NextTurn();
                yield break;
            }

            int oldPos = position;
            do
            {
                oldPos = position;
                yield return GameController.instance.spaces[position].action.Run(this);
            } while (oldPos != position);

            GameUIManager.instance.AIStartThinking(name);
            yield return new WaitForSeconds(2f);
            GameUIManager.instance.AIStopThinking();

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
                            EndTurn();

                        }
                        else
                        {
                            yield return GameUIManager.instance.StartAuction();
                            yield return GameUIManager.instance.OkPrompt(p.owner.name + " now owns " + p.name);
                            Develop();
                            EndTurn();
                        }
                    }
                    else
                    {
                        yield return GameUIManager.instance.StartAuction();
                        yield return GameUIManager.instance.OkPrompt(p.owner.name + " now owns " + p.name);
                        Develop();
                        EndTurn();
                    }
                }
                else
                {
                    Develop();
                    EndTurn();
                }
            }
            else
            {
                Develop();
                EndTurn();
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
        int chance;
        AuctionManager auction = GameUIManager.instance.auctionManager;
        Property p = auction.targetProperty;
        int percentage = 100;
        if (portfolio.GetCashBalance() > 0)
        {
            percentage = GetPercentage(auction.bids[auction.currentTurn].GetValue(), portfolio.GetCashBalance());
        }
        Debug.Log(percentage);
        int currentbid = auction.bids[auction.currentTurn].GetValue();
        if (percentage < 80)
        {
            int offset = Random.Range(-20, 30);
            int bid = Mathf.RoundToInt(((p.GetValue() * 0.85f) + offset));
            Debug.Log("bid" + bid);
            chance = Random.Range(1, 100);
            bool bidded = false;
            if (bid-currentbid >= 100 && chance > 70 && bidded == false)
            {
                auction.Bid100();
                yield return null;
                bidded = true;
                
            }
            Debug.Log("didnt bid 100");
            chance = Random.Range(1, 100);
            if (bid-currentbid >= 50 && chance > 70 && bidded == false)
            {
                auction.Bid50();
                yield return null;
                bidded = true;
            }
            Debug.Log("didnt bid 50");
            chance = Random.Range(1, 100);
            if (bid-currentbid >= 20 && chance > 70 && bidded == false)
            {
                auction.Bid20();
                yield return null;
                bidded = true;
            }
            Debug.Log("didnt bid 20");
            chance = Random.Range(1, 100);
            if (bid - currentbid >= 10 && chance > 70 && bidded == false)
            {
                auction.Bid10();
                yield return null;
                bidded = true;
            }
            Debug.Log("didnt bid 10");
            chance = Random.Range(1, 100);
            if (bid - currentbid >= 5 && chance > 70 && bidded == false)
            {
                bidded = true;
                auction.Bid5();
                yield return null;

            }
            Debug.Log("didnt bid 5");
            if (bid - currentbid >= 1 && bidded == false)
            {
                auction.Bid1();
                Debug.Log("bid 1");
                bidded = true;
                yield return null;
            }
            else if(bidded == false)
            {
                Debug.Log("withdrew!");
                auction.Withdraw();
                yield return null;
                bidded = true;
            }


        }
        else
        {
            Debug.Log("withdrew!");
            auction.Withdraw();
            yield return null;
        }
    }

    /// <summary>
    /// devlop function will be called at the end of each turn, it will get a list of properties for 
    /// </summary>
    public void Develop()
    {
        int chance;
        Debug.Log("developing");
        List<Property> properties = portfolio.GetProperties();
        for (int i = 0; i < properties.Count; i++)
        {
            Debug.Log(i);
            Property p = properties[i];
            if (p.CanUnMortgage())
            {
                Debug.Log(this.name + "unmortgaged" + p.name);
                p.UnMortgage();
            }
            Debug.Log(p.CanUpgrade());
            if (p.CanUpgrade())
            {
                int percentage = GetPercentage(p.upgradeCost, portfolio.GetCashBalance());
                Debug.Log(percentage);
                chance = Random.Range(1, 100);
                Debug.Log(chance);
                if (percentage < 25 && chance > 50)
                {
                    Debug.Log(this.name + "upgraded" + p.name);
                    p.Upgrade();
                }

            }
        }
        chance = Random.Range(1, 100);
        if (chance < 50)
        {
            Develop();
        }
    }
    public void EndTurn() 
    {
        while (portfolio.GetCashBalance() < 0) 
        {
            bool sold = false;
            List<Property> properties = portfolio.GetProperties();
            for (int i = 0; i < properties.Count - 1; i++)
            {
                Debug.Log(i);
                Property p = properties[i];
                if (p.CanDowngrade())
                {
                    Debug.Log(this.name + "downgraded" + p.name);
                    p.Downgrade();
                    sold = true;
                }
                else if (!p.CanUnMortgage())
                {
                    Debug.Log(this.name + "mortgaged" + p.name);
                    p.Mortgage();
                    sold = true;
                }
                else if (p.CanSell())
                {
                    Debug.Log(this.name + "sold" + p.name);
                    p.Sell();
                    sold = true;
                }
            }
            if (sold == false) 
            {
                GameController.instance.forefit(this);
            }
        }
        GameController.instance.NextTurn();
    }


    public int GetPercentage(int value1, int value2)
    {
        return Mathf.RoundToInt((value1 / value2) * 100);
    }

}
