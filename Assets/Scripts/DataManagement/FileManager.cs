using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

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
    /// Read the CSV file containing information about the spaces on the board
    /// </summary>
    /// <param name="path">The path to the CSV file</param>
    public static void ReadBoardCSV(string path, List<Space> spaces, List<PropertyGroup> propertyGroups)
    {
        string[] content = new string[0];

        using (StreamReader sr = new StreamReader(path))
        {
            content = sr.ReadToEnd().Split("\n");
        }

        Dictionary<string, PropertyGroup> groups = new Dictionary<string, PropertyGroup>();

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
                catch (KeyNotFoundException e)
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

        // Add all the parsed property groups to the given list
        foreach(KeyValuePair<string, PropertyGroup> entry in groups)
        {
            propertyGroups.Add(entry.Value);
        }
    }
}
