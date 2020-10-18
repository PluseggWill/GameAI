using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookAction : GoapAction
{
    public float duration = 3.0f; // seconds
    public CookAction()
    {
        // precondition
        preconditions.hunger = 0;
        preconditions.academic = 0;
        preconditions.energy = 10;
        preconditions.stress = 100;
        preconditions.money = 0;

        // effect
        effects.hunger = 30;
        effects.academic = 0;
        effects.energy = -10;
        effects.stress = 0;
        effects.money = 0;

        cost = 2.0f;
        actionName = "Cook Action";
    }

    public override bool checkPreconditon (Roommate target)
    {
        return true;
    }

    public override bool moveTo(Roommate target)
    {
        target.MoveTo(eStatus.Cook);
        if (target.status == eStatus.Cook)
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
            target.takeEffect(effects);
            return true;
        }

        return false;
    }
}
