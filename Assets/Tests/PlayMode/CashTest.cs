using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CashTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void CashValueTest()
    {
        // Use the Assert class to test conditions
        Cash cash = new Cash(0);
        Assert.AreEqual(0, cash.GetValue());
    }

        [Test]
        public void CashAddTest()
    {
        // Use the Assert class to test conditions
        Cash cash = new Cash(0);
        Cash cash1 = new Cash(0);
        cash1.AddCash(50);
        cash.AddCash(cash1);
        Assert.AreEqual(50, cash.GetValue());
    }

        [Test]
        public void CashRemoveTest()
    {
        // Use the Assert class to test conditions
        Cash cash = new Cash(0);
        Cash cash1 = new Cash(0);
        cash.AddCash(100);
        cash1.AddCash(50);
        cash.RemoveCash(cash1);
        Assert.AreEqual(cash.GetValue(), 50);
        cash.RemoveCash(10);
        Assert.AreEqual(40, cash.GetValue());
    }






    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator CashTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}
