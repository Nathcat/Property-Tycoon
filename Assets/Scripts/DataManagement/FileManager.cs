using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using System.Linq;

/// <summary>
/// Utility class which provides methods for reading the required setup files.
/// </summary>
public class FileManager 
{
    public class InvalidFormatException : Exception {
        public InvalidFormatException(string m) : base(m) {}
    }

    /// <summary>
    /// Read the CSV file containing the data of cards
    /// </summary>
    /// <param name="path">Path to the CSV file</param>
    public static CardData ReadCardCSV(string path) {
        
        List<Card> potLuck = new List<Card>();
        List<Card> opportunityKnocks = new List<Card>();

        string[] content = new string[0];

        using (StreamReader sr = new StreamReader(path)) {
            content = sr.ReadToEnd().Split("\n");
        }

        // Skip over the first line, since this specifies the headers of the CSV.
        for (int i = 1; i < content.Length; i++) {
            string[] elements = Regex.Split(content[i], ",\\s*", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500));

            if (elements.Length != 3) {
                throw new InvalidFormatException("Expected 3 items in row " + i + ", but got " + elements.Length);
            }

            if (elements[1].ToLower() == "pot luck") {
                try {
                    potLuck.Add(new Card(elements[0], new Action(elements[2])));
                }
                catch (Action.SyntaxError e) {
                    Debug.LogError(e.ToString());
                }
            }
            else if (elements[1].ToLower() == "opportunity knocks") {
                try {
                    opportunityKnocks.Add(new Card(elements[0], new Action(elements[2])));
                }
                catch (Action.SyntaxError e) {
                    Debug.LogError(e.ToString());
                }
            }  
        }
        // add two queues of cards, one for opportunity knocks, and one for pot luck.
        Queue<Card> opportunity = new Queue<Card>();
        Queue<Card> luck = new Queue<Card>();
        
        // enqueue cards into the relevant queues.
        for (int i = 0; i < opportunityKnocks.Count; i++)
        {
            opportunity.Enqueue(opportunityKnocks[i]);
        }

        for (int i = 0; i <= potLuck.Count; i++)
        {
            luck.Enqueue(potLuck[i]);
        }
        // return cards
        return new CardData(opportunity, luck);

    }

    /// <summary>
    /// Parse the given CSV file containing information about the spaces on the board
    /// </summary>
    /// <param name="path">The path to the CSV file</param>
    /// <returns>The resultant <see cref="BoardData"/> read from the CSV at <paramref name="path"/>.</returns>
    public static BoardData ReadBoardCSV(string path)
    {
        string[] content = new string[0];

        using (StreamReader sr = new StreamReader(path))
        {
            content = sr.ReadToEnd().Split("\n");
        }

        Dictionary<string, PropertyGroup> groups = new Dictionary<string, PropertyGroup>();
        List<Space> spaces = new List<Space>();

        // Skip over the first line, since this specifies the headers of the CSV.
        for (int i = 1; i < content.Length; i++)
        {
            string[] elements = Regex.Split(content[i], ",\\s*", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500));

            if (elements.Length != 5)
            {
                throw new InvalidFormatException("Expected 5 items in row " + i + ", but got " + elements.Length);
            }

            // Parse data from the line
            PropertyGroup g = null;
            int cost = -1;

            if (elements[3] != "null")
            {
                cost = Int32.Parse(elements[3]);
            }

            if (elements[2] != "null")
            {
                try
                {
                    g = groups[elements[2]];
                }
                catch (KeyNotFoundException)
                {
                    g = new PropertyGroup(elements[2]);
                    groups[elements[2]] = g;
                }
            }

            Space s = new Space(
                Int32.Parse(elements[0]),
                elements[1],
                g,
                new Action(elements[4]),
                cost
            );

            spaces.Add(s);
            if (g != null) g.AddProperty(s);
        }

        return new BoardData(spaces.ToArray(), groups.Values.ToArray());
    }

    /// <summary>
    /// Container for holding the result of <see cref="ReadBoardCSV(string)"/>
    /// </summary>
    /// <param name="spaces">Array of <see cref="Space"/> objects read from the board.</param>
    /// <param name="groups">Array of <see cref="PropertyGroup"/> objects read from the board.</param>
    public record BoardData(Space[] spaces, PropertyGroup[] groups);

    public record CardData(Queue<Card> opportunity, Queue<Card> luck);
}
