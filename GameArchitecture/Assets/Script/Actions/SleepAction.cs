using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepAction : GoapAction
{
    private bool done = false;
    //private float startTime = 0.0f;
    //public float cost = 1.0f;
    public float duration = 7.0f; // seconds
    public SleepAction()
    {
        // precondition
        preconditions.hunger = 30;
        preconditions.academic = 0;
        preconditions.energy = 0;
        preconditions.stress = 100;
        preconditions.money = 0;

        // effect
        effects.hunger = -30;
        effects.academic = 0;
        effects.energy = 40;
        effects.stress = -10;
        effects.money = 0;

        cost = 1.0f;
        actionName = "Sleep Action";
    }

    public override bool checkPreconditon (Roommate target)
    {
        return true;
    }

    public override bool moveTo(Roommate target)
    {
        target.MoveTo(eStatus.Sleep);
        if (target.status == eStatus.Sleep)
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
