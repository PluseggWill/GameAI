using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingAction : GoapAction
{
    private bool done = false;
    //private float startTime = 0.0f;
    //public float cost = 2.0f;
    public float duration = 3.0f; // seconds
    public ShoppingAction()
    {
        // precondition
        preconditions.hunger = 0;
        preconditions.academic = 0;
        preconditions.energy = 20;
        preconditions.stress = 100;
        preconditions.money = 30;

        // effect
        effects.hunger = 40;
        effects.academic = 0;
        effects.energy = -10;
        effects.stress = -10;
        effects.money = -30;

        cost = 1.0f;
        actionName = "Shopping Action";
    }

    public override bool checkPreconditon (Roommate target)
    {
        return true;
    }

    public override bool moveTo(Roommate target)
    {
        target.MoveTo(eStatus.Shopping);
        if (target.status == eStatus.Shopping)
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
