using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoapPlanner
{
    public List<GoapAction> plan(Roommate target, 
                                  List<GoapAction> availableActions,
                                  Property goal)
    {
        foreach (GoapAction a in availableActions)
        {
            a.resetAction();
        }

        List<GoapAction> result = new List<GoapAction>();
        List<Node> leaves = new List<Node>();
        Node start = new Node (null, 0, target.property, null);
        bool success = buildGraph(start, leaves, availableActions, goal);

        if (!success)
        {
            Debug.Log("No Plan!");
            return result;
        }

        Node cheapest = null;
        foreach (Node leaf in leaves)
        {
            if (cheapest == null)
            {
                cheapest = leaf;
            }
            else{
                if (leaf.cost < cheapest.cost)
                {
                    cheapest = leaf;
                }
            }
        }

        Node n = cheapest;
        while (n != null)
        {
            if (n.action != null)
            {
                result.Insert(0, n.action);
            }
            n = n.parent;
        }

        return result;
    }

    private bool buildGraph(Node parent, List<Node> leaves, List<GoapAction> availableActions, Property goal)
    {
        bool foundOne = false;

        foreach (GoapAction a in availableActions)
        {
            if (inState(a.preconditions, parent.curProperty))
            {
                Property reProperty = updateProperty(parent.curProperty, a.effects);

                Node node = new Node(parent, parent.cost + a.cost, reProperty, a);

                if (inState(goal, reProperty))
                {
                    leaves.Add(node);
                    foundOne = true;
                }
                else
                {
                    List<GoapAction> subAction = subsetAction(availableActions, a);
                    if (buildGraph(node, leaves, subAction, goal))
                    {
                        foundOne = true;
                    }
                }
            }
        }

        return foundOne;
    }

    private List<GoapAction> subsetAction(List<GoapAction> availableActions, GoapAction action)
    {
        List<GoapAction> result = new List<GoapAction>();
        foreach (GoapAction a in availableActions)
        {
            if (!a.Equals(action))
            {
                result.Add(a);
            }
        }
        return result;
    }

    private bool inState(Property preconditions, Property curProperty)
    {
        bool match = true;

        if (preconditions.academic > curProperty.academic)
        {
            match = false;
        }

        else if (preconditions.hunger > curProperty.hunger)
        {
            match = false;
        }

        else if (preconditions.energy > curProperty.energy)
        {
            match = false;
        }

        else if (preconditions.money > curProperty.money)
        {
            match = false;
        }
        
        else if (preconditions.stress < curProperty.stress)
        {
            match = false;
        }

        return match;
    }

    private Property updateProperty(Property curProperty, Property effects)
    {
        Property result = new Property();
        result.academic = curProperty.academic + effects.academic;
        result.energy = curProperty.energy + effects.energy;
        result.hunger = curProperty.hunger + effects.hunger;
        result.money = curProperty.money + effects.money;
        result.stress = curProperty.stress + effects.stress;
        return result;
    }
}

public class Node
{
    public Node parent;
    public float cost;
    public GoapAction action;
    public Property curProperty;

    public Node(Node parent, float cost, Property property, GoapAction action)
    {
        this.parent = parent;
        this.cost = cost;
        this.curProperty = property;
        this.action = action;
    }
}