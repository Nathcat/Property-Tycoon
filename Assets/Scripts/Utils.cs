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
}

// Fix https://stackoverflow.com/a/64749403
namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit { }
}
