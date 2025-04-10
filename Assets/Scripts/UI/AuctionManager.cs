using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;

public class AuctionManager : MonoBehaviour
{
    /// <summary>
    /// The value of each player's current bid
    /// </summary>
    public Cash[] bids { get; private set; }
    private bool[] withdrawn;
    /// <summary>
    /// The index of the player whose turn it currently is
    /// </summary>
    [HideInInspector] public int currentTurn { get; private set; } = 0;
    private CounterController currentPlayer { get { return GameController.instance.counters[currentTurn]; } }
    public Property targetProperty;

    private bool auctioning = false;
    private class WaitForComplete : CustomYieldInstruction
    {
        public override bool keepWaiting { get { return GameUIManager.instance.auctionManager.auctioning; } }
    }

    /// <summary>
    /// Set the target property of this auction
    /// </summary>
    /// <param name="p">The target property</param>
    public void SetTargetProperty(Property p)
    {
        targetProperty = p;
        transform.Find("Prompt").GetComponent<TextMeshProUGUI>().text = p.name + " is put up for auction, it is worth £" + p.GetValue();
    }

    /// <summary>
    /// Start an auction
    /// </summary>
    /// <param name="p">The property up for auction</param>
    public void StartAuction(Property p)
    {
        auctioning = true;
        SetTargetProperty(p);

        currentTurn = -1;

        bids = new Cash[GameController.instance.counters.Length];
        withdrawn = new bool[GameController.instance.counters.Length];

        for (int x = 0; x < bids.Length; x++)
        {
            bids[x] = new Cash();
            withdrawn[x] = false;
        }

        int i = 0;
        foreach (Transform playerPanel in transform.Find("PlayerPanels"))
        {
            if (i >= GameController.instance.counters.Length)
            {
                playerPanel.gameObject.SetActive(false);
                i++;
            }
            else
            {
                playerPanel.GetChild(0).GetComponent<TextMeshProUGUI>().text = GameController.instance.counters[i].name;
                playerPanel.GetChild(1).GetComponent<TextMeshProUGUI>().text = "£0";
                i++;
            }
        }

        NextBid();
    }

    /// <summary>
    /// Restart the currently active auction.
    /// </summary>
    private IEnumerator RestartAuction() {
        yield return GameUIManager.instance.OkPrompt(currentPlayer.name + " cannot afford their bid, the auction for " + targetProperty.name + " will restart!");
        StartAuction(targetProperty);
    }

    /// <summary>
    /// Move to the next bidding turn and update the UI accordingly
    /// </summary>
    public void NextBid()
    {
        currentTurn = (currentTurn + 1) % GameController.instance.counters.Length;

        if (withdrawn[currentTurn])
        {
            NextBid();
            return;
        }

        // Count the number of remaining players
        int numberOfPlayers = 0;
        foreach (bool w in withdrawn)
        {
            if (!w) numberOfPlayers++;
        }

        if (numberOfPlayers == 1)
        {
            // This is the only remaining player, hence they have won
            Debug.Log(currentPlayer.name + " wins " + targetProperty.name + " for " + bids[currentTurn].GetValue());

            if (currentPlayer.portfolio.GetCashBalance() >= bids[currentTurn].GetValue()) {
                targetProperty.AuctionPurchase(currentPlayer, bids[currentTurn]);
                Debug.Log(currentPlayer.name + " obtains " + targetProperty.name);
                GameUIManager.instance.FinishAuction();
            }
            else {
                Debug.Log(currentPlayer.name + " cannot afford their bid!");
                StartCoroutine(RestartAuction());
            }

            return;
        }

        // Disable bid buttons which the playe cannot afford
        // Removing this because players should be able to bid as much as they want, regardless of their
        // current assets
        /*
        foreach (Transform button in transform.Find("BidButtons"))
        {
            int value = int.Parse(button.name);
            if (value > (GameController.instance.counters[currentTurn].portfolio.GetCashBalance() - bids[currentTurn].GetValue()))
            {
                button.GetComponent<UnityEngine.UI.Image>().color = new Color(0f, 0f, 0f, 0.5f);
            }
            else
            {
                button.GetComponent<UnityEngine.UI.Image>().color = new Color(1f, 1f, 1f, 1f);
            }
        }*/

        // Highlight the player card of the player whose bidding turn it currently is and reset all the others
        // back to the default color
        int i = 0;
        foreach (Transform playerPanel in transform.Find("PlayerPanels"))
        {
            if (playerPanel.gameObject.activeSelf)
            {
                if (!withdrawn[i])
                {
                    playerPanel.GetComponent<UnityEngine.UI.Image>().color = new Color(1f, 1f, 1f, 1f);
                }
                else
                {
                    playerPanel.GetComponent<UnityEngine.UI.Image>().color = new Color(0f, 0f, 0f, 0.5f);
                }
            }
            else
            {
                break;
            }

            i++;
        }

        transform.Find("PlayerPanels").GetChild(currentTurn).GetComponent<UnityEngine.UI.Image>().color = new Color(1f, 1f, 0f, 1f);

        // Display the player's current balance
        transform.Find("CurrentBalance").GetComponent<TextMeshProUGUI>().text = "You have £" + GameController.instance.counters[currentTurn].portfolio.GetCashBalance();

        // Enable / disable the bid buttons depending on whether or not the counter is controllable
        transform.Find("BidButtons").gameObject.SetActive(currentPlayer.isControllable);
        StartCoroutine(currentPlayer.DoAuctionTurn());
    }

    /// <summary>
    /// Withdraw from bidding
    /// </summary>
    public void Withdraw()
    {
        withdrawn[currentTurn] = true;
        NextBid();
    }

    /// <summary>
    /// Add 1 to the current bid
    /// </summary>
    public void Bid1()
    {
        /*
        if (currentPlayer.portfolio.GetCashBalance() < 1)
        {
            Debug.LogWarning(currentPlayer.name + " cannot afford this bid!");
            return;
        }*/

        bids[currentTurn].AddCash(1);
        transform.Find("PlayerPanels").GetChild(currentTurn).GetChild(1).GetComponent<TextMeshProUGUI>().text = "£" + bids[currentTurn].GetValue();
        NextBid();
    }

    /// <summary>
    /// Add 5 to the current bid
    /// </summary>
    public void Bid5()
    {
        /*
        if (currentPlayer.portfolio.GetCashBalance() < 5)
        {
            Debug.LogWarning(currentPlayer.name + " cannot afford this bid!");
            return;
        }*/

        bids[currentTurn].AddCash(5);
        transform.Find("PlayerPanels").GetChild(currentTurn).GetChild(1).GetComponent<TextMeshProUGUI>().text = "£" + bids[currentTurn].GetValue();
        NextBid();
    }

    /// <summary>
    /// Add 10 to the current bid
    /// </summary>
    public void Bid10()
    {
        /*
        if (currentPlayer.portfolio.GetCashBalance() < 10)
        {
            Debug.LogWarning(currentPlayer.name + " cannot afford this bid!");
            return;
        }*/

        bids[currentTurn].AddCash(10);
        transform.Find("PlayerPanels").GetChild(currentTurn).GetChild(1).GetComponent<TextMeshProUGUI>().text = "£" + bids[currentTurn].GetValue();
        NextBid();
    }

    /// <summary>
    /// Add 20 to the current bid
    /// </summary>
    public void Bid20()
    {
        /*
        if (currentPlayer.portfolio.GetCashBalance() < 20)
        {
            Debug.LogWarning(currentPlayer.name + " cannot afford this bid!");
            return;
        }*/

        bids[currentTurn].AddCash(20);
        transform.Find("PlayerPanels").GetChild(currentTurn).GetChild(1).GetComponent<TextMeshProUGUI>().text = "£" + bids[currentTurn].GetValue();
        NextBid();
    }

    /// <summary>
    /// Add 50 to the current bid
    /// </summary>
    public void Bid50()
    {
        /*
        if (currentPlayer.portfolio.GetCashBalance() < 50)
        {
            Debug.LogWarning(currentPlayer.name + " cannot afford this bid!");
            return;
        }*/

        bids[currentTurn].AddCash(50);
        transform.Find("PlayerPanels").GetChild(currentTurn).GetChild(1).GetComponent<TextMeshProUGUI>().text = "£" + bids[currentTurn].GetValue();
        NextBid();
    }

    /// <summary>
    /// Add 100 to the current bid
    /// </summary>
    public void Bid100()
    {
        /*
        if (currentPlayer.portfolio.GetCashBalance() < 100)
        {
            Debug.LogWarning(currentPlayer.name + " cannot afford this bid!");
            return;
        }*/

        bids[currentTurn].AddCash(100);
        transform.Find("PlayerPanels").GetChild(currentTurn).GetChild(1).GetComponent<TextMeshProUGUI>().text = "£" + bids[currentTurn].GetValue();
        NextBid();
    }
}
