using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

/// <summary>Provides utility functions for generating the game board</summary>
public class BoardGenerator
{
    public class InvalidSpaceCountException : Exception {
        public InvalidSpaceCountException() : base("Invalid number of spaces provided, must be >= 4, and a multiple of 4.") {}
    }

    /// <summary>Generate a new game board</summary>
    /// <param name="parent">
    ///     The transform which will be the parent of all objects the board is comprised of
    /// </param>
    /// <param name="LB">
    ///     The length / width of the board
    /// </param>
    /// <param name="LS">
    ///     The length of a normal space
    /// </param>
    /// <param name="WS">
    ///     The width of a normal space
    /// </param>
    /// <param name="normalSpace">
    ///     Prefab to instantiate as a normal space
    /// </param>
    /// <param name="cornerSpace">
    ///     Prefab to instantiate as a corner space
    /// </param>
    /// <param name="spaces">
    ///     The ordered set of board spaces
    /// </param>
    /// <exception cref="BoardGenerator.InvalidSpaceCountException">
    ///     Thrown if the number of provided spaces is invalid
    /// </exception>
    /// <returns>
    ///     An array of the created <see cref="SpaceController"/>s, in the same order as <paramref name="spaces"/>.
    /// </returns>
    public static SpaceController[] GenerateBoard(Transform parent, double LS, double WS, GameObject normalSpace, GameObject cornerSpace, Space[] spaces) {
        // Ensure that the number of spaces is >= 4, and is a multiple of 4
        if (spaces.Length < 4 || (spaces.Length % 4) != 0) throw new InvalidSpaceCountException();

        // Prepare the set of corner spaces
        int n = (spaces.Length / 4) - 1;   // The number of spaces per side excluding corners
        double LB = GetBoardDimensions(spaces.Length);  // The length / width of the board, i.e. the number of spaces per side, including corners
        Space[] corners = new Space[] {
            spaces[0], 
            spaces[n+1],
            spaces[2 * (n+1)], 
            spaces[3 * (n+1)]
        };

        List<SpaceController> controllers = new List<SpaceController>();

        int cornersPassed = 0;
        for (int I = 0; I < spaces.Length; I++) {
            GameObject o;
            if ((I % (n + 1)) == 0) {
                // This is a corner space
                o = MonoBehaviour.Instantiate(cornerSpace, PCorner(LB, LS, I / n), RCorner(I / n)) ;
                o.transform.SetParent(parent, false);
                o.name = I.ToString() ;
                cornersPassed++;
            }
            else {
                // This is a normal space
                o = MonoBehaviour.Instantiate(normalSpace, PSpace(LB, WS, I - cornersPassed, n), RSpace(I - cornersPassed, n));
                o.name = I.ToString() ;
                o.transform.SetParent(parent, false);
            }

            SpaceController c = o.GetComponent<SpaceController>();
            c.Setup(spaces[I]);
            controllers.Add(c);
        }

        return controllers.ToArray();
    }

    /// <summary>
    /// Get the dimensions of the board, i.e. the length and width of the board. This is a single intenger because the board is a square.
    /// </summary>
    /// <param name="noOfSpaces">The total number of space on the board</param>
    /// <returns>The length / width of the board</returns>
    public static int GetBoardDimensions(int noOfSpaces)
    {
        return (noOfSpaces / 4) + 1;
    }

    /// <summary>Determine the position of corner i</summary>
    /// <param name="i">The index of the corner</param>
    /// <param name="LB">The length / width of the board</param>
    /// <param name="LS">The length / width of a corner space</param>
    /// <returns>The vector position of corner i</returns>
    private static Vector3 PCorner(double LB, double LS, int i) {
        double theta = (Math.PI / 4) + ((Math.PI * i) / 2);

        return new Vector3(
            (float) ((LB / 2) * SignCeiling(Math.Sin(theta))),
            0f,
            (float) ((LB / 2) * SignCeiling(Math.Cos(theta)))
        );
    }

    /// <summary>Determine the position of space i</summary>
    /// <param name="i">The index of the space, not including corners!</param>
    /// <param name="LB">The length / width of the board</param>
    /// <param name="WS">The width of a normal space</param>
    /// <returns>The vector position of space i</returns>
    private static Vector3 PSpace(double LB, double WS, int i, int n) {
        int I = i % n;
        float x = (float) WS * (((1 - n) / 2) + I);
        double theta = (Math.PI / 2) * -((i / n) + 1);
        
        return new Vector3(
            (float) ((x * Math.Cos(theta)) - ((LB * Math.Sin(theta)) / 2)),
            0f,
            (float) ((x * Math.Sin(theta)) + ((LB * Math.Cos(theta)) / 2))
        );
    }

    /// <summary>Determine the rotation of corner i</summary>
    /// <param name="i">The index of the corner</param>
    /// <returns>The rotation of corner i</returns>
    private static Quaternion RCorner(int i) {
        return Quaternion.Euler(new Vector3(
            0f,
            (float) (Math.PI + ((i * Math.PI) / 2)) * Mathf.Rad2Deg,
            0f
        ));
    }

    /// <summary>Determine the rotation of space i</summary>
    /// <param name="i">The index of the space, not including corners!</param>
    /// <returns>The rotation of space i</returns>
    private static Quaternion RSpace(int i, int n) {
        int F = i / n;

        return Quaternion.Euler(new Vector3(
            0f,
            (float) (Math.PI + ((F * Math.PI) / 2)) * Mathf.Rad2Deg,
            0f
        ));
    }    

    private static float SignCeiling(double x) {
        if (x >= 0) return (float) Math.Ceiling(x);
        else return (float) Math.Floor(x);
    }
}
