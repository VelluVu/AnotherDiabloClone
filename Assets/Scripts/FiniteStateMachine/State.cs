using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Statemachinen State Scriptable Object, 
/// Jos haluaa tehdä uudenlaista tekoälyä täytyy kirjoittaa lisää evaluointi decision scriptejä, sekä lisää action scriptejä, 
/// mutta näitä voi yhdistellä tähän objectiin saadakseen erinlaisia AI-toimintamalleja, tällä hetkellä:
/// Decisions : Look, Scan (Parannettavissa) , ActiveState (Parannettavissa)
/// Actions : Attack, Chase, Patrol
/// Stateja : AlertScan, ChaseScan, Chase, PatrolScan, Patrol, Remain
/// </summary>
[CreateAssetMenu (menuName = "PluggableAI/State")]
public class State : ScriptableObject
{
    [Header ( "Actionit, mitä state toteuttaa järjestyksessä" )]
    public Action[] actions;
    [Header ( "Staten vaihdot lisää listan kokoon numero, jos haluat tehdä uuden staten koneeseen" )]
    public Transition [ ] transitions;

    /// <summary>
    /// Päivittää statee Siirtymien perusteella
    /// </summary>
    /// <param name="controller"></param>
    public void UpdateState(StateController controller)
    {
        DoActions ( controller );
        CheckTransitions ( controller );
    }

    /// <summary>
    /// Tekee Lisättyjä Actioneita järjestyksessä
    /// </summary>
    /// <param name="controller"></param>
    private void DoActions(StateController controller)
    {
        for ( int i = 0 ; i < actions.Length ; i++ )
        {
            actions [ i ].Act ( controller );
        }
    }

    /// <summary>
    /// Tarkistaa onko päätetty vaihtaa statee
    /// </summary>
    /// <param name="controller"></param>
    void CheckTransitions(StateController controller)
    {
        for ( int i = 0 ; i < transitions.Length ; i++ )
        {
            bool decisionSucceded = transitions [ i ].decision.Decide ( controller );

            if(decisionSucceded)
            {
                controller.TransitionToState ( transitions [ i ].trueState );
            }
            else
            {
                controller.TransitionToState ( transitions [ i ].falseState );
            }
        }
    }

}
