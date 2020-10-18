using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Roommate : MonoBehaviour
{
    private NavMeshAgent navAgent;

    // Properties
    public Property property;
    public eStatus status;
    public float timeForHomework = 30.0f;
    private float startTime = 0;

    // Reference Target
    public GameObject studyZone;
    public GameObject bedZone;
    public GameObject playZone;
    public GameObject kitchenZone;
    public GameObject shoppingZone;
    public Material mat;

    // Goap Properties
    private List<GoapAction> actionList;
    private List<GoapAction> availableActions;
    private GoapPlanner planner;
    private Property targetProperty;
    private GoapAction curAction;

    void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        status = eStatus.Idle;
        InitProperties();

        actionList = new List<GoapAction>();
        planner = new GoapPlanner();
        loadActions();
        setTarget();
    }
    // Start is called before the first frame update
    void Start()
    {
        curAction = null;
        actionList = planner.plan(this, availableActions, targetProperty);
    }

    // Update is called once per frame
    void Update()
    {
        if (curAction == null)
        {
            if (actionList.Count > 0)
            {
                curAction = actionList[0];
                actionList.RemoveAt(0);
            }
            else
            {
                actionList = planner.plan(this, availableActions, targetProperty);
            }
        }
        else 
        {
            if (curAction.moveTo(this))
            {
                if (curAction.perform(this))
                {
                    Debug.Log("Action: " + curAction.actionName + " Finished Performing!");
                    curAction = null;
                }
            }
        }

        if (startTime == 0)
        {
            startTime = Time.time;
        }

        if (Time.time - startTime > timeForHomework)
        {
            property.academic -= 30;
            startTime = Time.time;
        }
    }

    private void InitProperties()
    {
        property = new Property();
        property.academic = 70;
        property.hunger = 80;
        property.energy = 80;
        property.money = 100;
        property.stress = 20;

        // property.academic = 70;
        // property.hunger = 100;
        // property.energy = 10;
        // property.money = 100;
        // property.stress = 10;
    }

    public void MoveTo(eStatus status)
    {
        switch (status)
        {
            case eStatus.Study:
                navAgent.SetDestination(studyZone.transform.position);
                break;
            case eStatus.Cook:
                navAgent.SetDestination(kitchenZone.transform.position);
                break;
            case eStatus.Sleep:
                navAgent.SetDestination(bedZone.transform.position);
                break;
            case eStatus.Play:
                navAgent.SetDestination(playZone.transform.position);
                break;
            case eStatus.Shopping:
                navAgent.SetDestination(shoppingZone.transform.position);
                break;
        }
    }

    private void loadActions()
    {
        GoapAction[] actions = gameObject.GetComponents<GoapAction>();
        availableActions = new List<GoapAction>();
        foreach(GoapAction a in actions)
        {
            availableActions.Add(a);
            Debug.Log("Found actions!" );
        }
        //Debug.Log("Found actions!" );
    }

    private void setTarget()
    {
        targetProperty = new Property();
        targetProperty.academic = 100;
        targetProperty.energy = 0;
        targetProperty.hunger = 0;
        targetProperty.stress = 100;
        targetProperty.money = 0;
    }

    private void OnTriggerEnter(Collider other) {
        switch (other.tag)
        {
            case "Study":
                status = eStatus.Study;
                break;
            case "Cook":
                status = eStatus.Cook;
                break;
            case "Play":
                status = eStatus.Play;
                break;
            case "Shopping":
                status = eStatus.Shopping;
                break;
            case "Sleep":
                status = eStatus.Sleep;
                break;
        }
    }

    public void takeEffect(Property effects)
    {
        property.academic += effects.academic;
        property.energy += effects.energy;
        property.hunger += effects.hunger;
        property.money += effects.money;
        property.stress += effects.stress;

        // Debug.Log("Effects taken!");
        // Debug.Log("Academic: " + property.academic);
        // Debug.Log("Energy: " + property.energy);
        // Debug.Log("Hunber: " + property.hunger);
        // Debug.Log("Money:" + property.money);
        // Debug.Log("Stree: " + property.stress);
    }

}
