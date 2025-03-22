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
    /// <param name="opportunityKnocks">The list of opportunity knocks cards</param>
    /// <param name="potLuck">The list of pot luck cards</param>
    public static void ReadCardCSV(string path, List<Card> potLuck, List<Card> opportunityKnocks) {
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

            if (elements.Length != 6)
            {
                throw new InvalidFormatException("Expected 6 items in row " + i + ", but got " + elements.Length);
            }

            // Parse data from the line
            PropertyGroup g = null;
            int cost = -1;

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

                if (elements[3] != "null")
                {
                    cost = Int32.Parse(elements[3]);
                }
                else
                {
                    Debug.LogWarning("Property group was supplied without a cost for property " + elements[1]);
                }
            }

            Space s;

            // If the property group given in the CSV is null, we assume we are dealing with a normal space, and
            // not a property. Otherwise we assume it is a property.
            if (g != null)
            {
                if (g.name == PropertyGroup.STATION_GROUP_NAME)
                {
                    s = new Station(
                        Int32.Parse(elements[0]),
                        elements[1],
                        g,
                        new Action(elements[4]),
                        cost
                    );
                }
                else if (g.name == PropertyGroup.UTILITY_GROUP_NAME)
                {
                    s = new Utility(
                        Int32.Parse(elements[0]),
                        elements[1],
                        g,
                        new Action(elements[4]),
                        cost
                    );

                }
                else
                {
                    s = new Property(
                        Int32.Parse(elements[0]),
                        elements[1],
                        g,
                        new Action(elements[4]),
                        cost,
                        Int32.Parse(elements[5])
                    );
                }

                g.AddProperty(s as Property);

                Debug.Log((s as Property).GetRentDescription());
            }
            else
            {
                s = new Space(Int32.Parse(elements[0]), elements[1], new Action(elements[4]));
            }

            spaces.Add(s);
        }

        return new BoardData(spaces.ToArray(), groups.Values.ToArray());
    }

    /// <summary>
    /// Container for holding the result of <see cref="ReadBoardCSV(string)"/>
    /// </summary>
    /// <param name="spaces">Array of <see cref="Space"/> objects read from the board.</param>
    /// <param name="groups">Array of <see cref="PropertyGroup"/> objects read from the board.</param>
    public record BoardData(Space[] spaces, PropertyGroup[] groups);
}
