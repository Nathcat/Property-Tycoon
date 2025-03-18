
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using System.Runtime.InteropServices;
using Unity.VisualScripting;

/// <summary>
/// Abstracts the use of action strings from the setup files.
/// </summary>
public class Action
{
    public class SyntaxError : Exception {
        public SyntaxError(string m) : base(m) {}
    }

    /// <summary>
    /// The action string specifying the action
    /// </summary>
    private Token[] commandStringLexed;

    /// <summary>
    /// The original string. Used to provide debugging help
    /// </summary>
    private string originalString;

    public Action(string commandString) {
        this.commandStringLexed = Lex(commandString);
        this.originalString = commandString;
    }


    /// <summary>
    /// Returns the command string.
    /// </summary>
    /// <returns>The command string</returns>
    public String[] getCommandStringLexed(){
        Debug.Log(this.commandStringLexed);
        String[] lexedoutput = new String[commandStringLexed.Length];
        for (int i = 0; i < commandStringLexed.Length; i++){
            Debug.Log(i);
            lexedoutput[i] = commandStringLexed[i].getTokenAsString();
        } 
        return lexedoutput;
    }

    /// <summary>
    /// Execute the action specified by the action string given to this object
    /// </summary>
    /// <param name="counterController">The CounterController which initiated the action</param>
    public void Run(CounterController counterController) {
        List<Token> commandState = new List<Token>();
        string processedString = "";

        for (int i = 0; i < commandStringLexed.Length; i++) {
            Token next = commandStringLexed[i];

            if (commandState.Count == 0) {
                if (next is Command) {
                    commandState.Add(next);
                }
                else {
                    throw new SyntaxError("\"" + originalString + "\", expected Command at position " + processedString.Length);
                }
            }
            else if (next is CommandEnd) {
                if (commandState.Count == 0) {
                    throw new SyntaxError("\"" + originalString + "\", unexpected ';' at position " + processedString.Length);
                }

                Command c = ((Command) commandState[0]);
                commandState.RemoveAt(0);
                Argument[] args = new Argument[commandState.Count];

                for (int x = 0; x < commandState.Count; x++) {
                    args[x] = (Argument) commandState[x];
                }

                commandState = new List<Token>();
                c.Execute(counterController, args);

                if (i != commandStringLexed.Length - 1)
                {
                    Debug.LogWarning("First command has been executed, but there are more commands in this action string and will be ignored!");
                    return;
                }
            }
            else if (commandState[commandState.Count - 1] is Command || commandState[commandState.Count - 1] is Argument) {
                if (next is Command) {
                    throw new SyntaxError("\"" + originalString + "\", expected Argument at position " + processedString.Length);
                }
                else {
                    commandState.Add(next);
                }
            }

            processedString += next.value;
        }
    }

    /// <summary>
    /// Lex the given action string
    /// </summary>
    /// <param name="s">The action string</param>
    /// <returns>A list of tokens in the action string</returns>
    private static Token[] Lex(string s) {
        string[] symbols = Regex.Split(s, "\\s+", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(500));

        List<Token> tokens = new List<Token>();

        for (int i = 0; i < symbols.Length; i++) {            
            if (symbols[i] == ";") {
                tokens.Add(new CommandEnd(";"));
            }
            else if (tokens.Count == 0 || tokens[tokens.Count - 1] is CommandEnd) {
                switch (symbols[i]) {
                    case "Fine": tokens.Add(new Fine(symbols[i])); break;
                    case "Move": tokens.Add(new Move(symbols[i])); break;
                    case "PayOut": tokens.Add(new PayOut(symbols[i])); break;
                    case "PayIn": tokens.Add(new PayIn(symbols[i])); break;
                    case "PropertyRent": tokens.Add(new PropertyRent(symbols[i])); break;
                    case "UtilityRent": tokens.Add(new UtilityRent(symbols[i])); break;
                    case "StationRent": tokens.Add(new StationRent(symbols[i])); break;
                    case "Jail": tokens.Add(new Jail(symbols[i])); break;
                    case "GoToJail": tokens.Add(new GoToJail(symbols[i])); break;
                    case "TakePotLuck": tokens.Add(new TakePotLuck(symbols[i])); break;
                    case "TakeOppoKnocks": tokens.Add(new TakeOppoKnocks(symbols[i])); break;
                    case "CollectFreeParking": tokens.Add(new CollectFreeParking(symbols[i])); break;
                    case "GetOutOfJail": tokens.Add(new GetOutOfJail(symbols[i])); break;
                    case "PayPerUpgrade": tokens.Add(new PayPerUpgrade(symbols[i])); break;
                    case "Collect": tokens.Add(new Collect(symbols[i])); break;
                    case "": break;
                    default: throw new SyntaxError("Command \"" + symbols[i] + "\" not recognised!");
                }
            }
            else {
                tokens.Add(new Argument(symbols[i]));
            }
        }

        return tokens.ToArray();
    }

    /// <summary>
    /// Determine whether or not this action contains the specified command
    /// </summary>
    /// <typeparam name="T">The command type</typeparam>
    /// <returns>True if the action contains the command given by type T, false otherwise</returns>
    public bool ContainsCommand<T>() where T: Command
    {
        for (int i = 0; i < commandStringLexed.Length; i++)
        {
            Token next = commandStringLexed[i];
            if (next is T)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Return the arguments given to the first instance of the specified command in this action.
    /// </summary>
    /// <typeparam name="T">The type of the command</typeparam>
    /// <returns>The arguments provided to the first instance of the specified command.</returns>
    public Argument[] GetCommandArguments<T>() where T: Command
    {
        List<Argument> args = new List<Argument>();

        bool adding = false;
        foreach (Token token in commandStringLexed)
        {
            if (!adding && token is T)
            {
                adding = true;
            }
            else if (adding)
            {
                if (token is Argument)
                {
                    args.Add(token as Argument);
                }
                else if (token is CommandEnd)
                {
                    return args.ToArray();
                }
            }
        }

        return null;
    }
}
