using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rajapinta Tehtäville Decision Scriptable Objecteille joita voi liittäää stateihiin
/// </summary>
public abstract class Decision : ScriptableObject
{
    public abstract bool Decide ( StateController controller );
}
