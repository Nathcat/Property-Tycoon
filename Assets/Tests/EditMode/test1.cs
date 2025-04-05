using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class test1
{
    // A Test behaves as an ordinary method
    [Test]
    public void test1SimplePasses()
    {
        // Use the Assert class to test conditions
        int i = 1;
        int j = 1;
        Assert.AreEqual(i, j);
    }

}
