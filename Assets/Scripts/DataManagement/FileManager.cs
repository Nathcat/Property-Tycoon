using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text.RegularExpressions;

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
}
