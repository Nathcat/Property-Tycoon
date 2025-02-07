using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardGeneratorTest : MonoBehaviour
{
    public GameObject normalSpace;
    public GameObject cornerSpace;

    // Start is called before the first frame update
    void Start()
    {
        BoardGenerator.GenerateBoard(transform, 11, 2, 1, normalSpace, cornerSpace, new Space[] {
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space(),
            new Space()
        });
    }
}
