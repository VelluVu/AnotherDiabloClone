using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Siirtymä Class stateen sisällytetty lista.
/// </summary>
[System.Serializable]
public class Transition
{
    [Header ("Päätös minkä perusteella vaihtaa statee")]
    public Decision decision;
    [Header ( "State mihin siirrytään jos päätös palauttaa true" )]
    public State trueState;
    [Header ( "State mihin vaihdetaan jos päätös palauttaa false" )]
    public State falseState;
}
