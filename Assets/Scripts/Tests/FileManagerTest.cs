using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class FileManagerTest : MonoBehaviour
{
    void Start()
    {
        string cardsPath = Path.Combine(Application.dataPath, "cards.csv");
        string boardPath = Path.Combine(Application.dataPath, "board.csv");

        Debug.Log(cardsPath);
        Debug.Log(boardPath);

        Queue<Card> potLuck;
        Queue<Card> opportunityKnocks;

        FileManager.CardData cards = FileManager.ReadCardCSV(cardsPath);
        potLuck = cards.luck;
        opportunityKnocks = cards.opportunity;


        for (int i = 0; i < potLuck.Count; i++) {
            Card tempCard = potLuck.Dequeue();
            Debug.Log("Pot Luck card: " + tempCard.ToString());
            tempCard.action.Run(null);
        }

        for (int i = 0; i < opportunityKnocks.Count; i++) {
            Card tempCard = opportunityKnocks.Dequeue();
            Debug.Log("Opportunity Knocks card: " + tempCard.ToString());
            tempCard.action.Run(null);
        }

        FileManager.BoardData data = FileManager.ReadBoardCSV(boardPath);

        foreach (PropertyGroup g in data.groups)
        {
            Debug.Log("PropertyGroup: " + g.name);
        }

        foreach (Space s in data.spaces)
        {
            string o = "Space: " + s.name;
            if (s is Property)
            {
                o += " of group " + (s as Property).propertyGroup.name;
            }

            Debug.Log(o);
        }
    }
}
