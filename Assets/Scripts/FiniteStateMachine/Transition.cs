using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Siirtymä Class stateen sisällytetty lista.
/// </summary>
[System.Serializable]
public class Transition
{
    [Tooltip ("Päätös minkä perusteella vaihtaa statee")] public Decision decision;
    [Tooltip ( "State mihin siirrytään jos päätös palauttaa true" )] public State trueState;
    [Tooltip ( "State mihin vaihdetaan jos päätös palauttaa false" )] public State falseState;
}
