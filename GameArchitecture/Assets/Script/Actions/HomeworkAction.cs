using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeworkAction : GoapAction
{
    private bool done = false;
    //private float startTime = 0.0f;
    public float duration = 5.0f; // seconds
    public HomeworkAction()
    {
        // precondition
        preconditions.hunger = 30;
        preconditions.academic = 50;
        preconditions.energy = 30;
        preconditions.stress = 80;
        preconditions.money = 0;

        // effect
        effects.hunger = -30;
        effects.academic = 20;
        effects.energy = -20;
        effects.stress = 50;
        effects.money = 20;

        cost = 3.0f;
        actionName = "HomeworkAction";
    }

    public override bool checkPreconditon (Roommate target)
    {
        return true;
    }

    public override bool moveTo(Roommate target)
    {
        target.MoveTo(eStatus.Study);

        if (target.status == eStatus.Study)
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
