using System;
using System.Collections;
using UnityEngine;

public static class Utils
{
    /// <summary>
    /// Invokes the provided function after the specified delay in seconds.
    /// </summary>
    /// <param name="delay">The number of seconds to delay the function by.</param>
    /// <param name="action">The funciton to run after the specified delay.</param>
    public static void RunAfter(float delay, System.Action action)
    {
        IEnumerator ThrowDelay()
        {
            yield return new WaitForSeconds(delay);
            action();
        }

        GameController.instance.StartCoroutine(ThrowDelay());
    }

    /// <summary>
    /// Removes the element from the array at <paramref name="index"/>
    /// </summary>
    /// <typeparam name="T">The type of this array</typeparam>
    /// <param name="index">The index of the element to remove</param>
    /// <returns>The resultant array.</returns>
    public static T[] RemoveAt<T>(this T[] source, int index)
    {
        T[] dest = new T[source.Length - 1];
        if (index > 0)
            Array.Copy(source, 0, dest, 0, index);

        if (index < source.Length - 1)
            Array.Copy(source, index + 1, dest, index, source.Length - index - 1);

        return dest;
    }
}

// Fix https://stackoverflow.com/a/64749403
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }
}
