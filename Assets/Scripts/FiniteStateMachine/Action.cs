using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rajapinta Action Scriptable objecteille joita voi liittää stateihin
/// </summary>
public abstract class Action : ScriptableObject
{
    public abstract void Act ( StateController controller );
}
