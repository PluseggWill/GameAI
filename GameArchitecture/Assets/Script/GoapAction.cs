using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GoapAction : MonoBehaviour
{
    public Property preconditions;
    public Property effects;
    public Property curProperty;
    public GameObject target;
    public bool isDone = false;
    public float startTime = 0;
    public float cost = 1.0f;
    public string actionName = "";

    private void Update() {
    }
    public GoapAction()
    {
        preconditions = new Property();
        effects = new Property();
    }
    
    public virtual bool resetAction()
    {
        isDone = false;
        startTime = 0;
        return true;
    }

    public abstract bool moveTo(Roommate target);

    public abstract bool checkPreconditon(Roommate target);

    public abstract bool perform(Roommate target);

}
