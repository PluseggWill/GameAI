using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAction : GoapAction
{
    private bool done = false;
    //private float startTime = 0.0f;
    //public float cost = 1.0f;
    public float duration = 4.0f; // seconds
    public PlayAction()
    {
        // precondition
        preconditions.hunger = 10;
        preconditions.academic = 0;
        preconditions.energy = 20;
        preconditions.stress = 100;
        preconditions.money = 0;

        // effect
        effects.hunger = -10;
        effects.academic = 0;
        effects.energy = -20;
        effects.stress = -40;
        effects.money = 0;

        cost = 1.0f;
        actionName = "Play Action";
    }

    public override bool checkPreconditon (Roommate target)
    {
        return true;
    }

    public override bool moveTo(Roommate target)
    {
        target.MoveTo(eStatus.Play);
        if (target.status == eStatus.Play)
        {
            return true;
        }
        return false;
    }

    public override bool perform(Roommate target)
    {
        if (startTime == 0)
        {
            startTime = Time.time;
        }

        if (Time.time - startTime > duration)
        {
            done = true;
            target.takeEffect(effects);
            return true;
        }

        return false;
    }
}
