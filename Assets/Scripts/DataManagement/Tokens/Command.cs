using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command : Token
{
    public Command(string value) : base(value) {}
    
    virtual public IEnumerator Execute(CounterController counterController, Argument[] args) {
        yield break;
    }
}
