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

        List<Card> potLuck = new List<Card>();
        List<Card> opportunityKnocks = new List<Card>();

        FileManager.ReadCardCSV(cardsPath, potLuck, opportunityKnocks);

        for (int i = 0; i < potLuck.Count; i++) {
            Debug.Log("Pot Luck card: " + potLuck[i].ToString());
            potLuck[i].action.Run(null);
        }

        for (int i = 0; i < opportunityKnocks.Count; i++) {
            Debug.Log("Opportunity Knocks card: " + opportunityKnocks[i].ToString());
            opportunityKnocks[i].action.Run(null);
        }

        FileManager.BoardData data = FileManager.ReadBoardCSV(boardPath);

        foreach (PropertyGroup g in data.groups)
        {
            Debug.Log("PropertyGroup: " + g.name);
        }

        foreach (Space s in data.spaces)
        {
            string o = "Space: " + s.name;
            if (s.propertyGroup != null)
            {
                o += " of group " + s.propertyGroup.name;
            }

            Debug.Log(o);
        }
    }
}
