using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property
{
    [Range(0, 100)]
    public float academic;
    [Range(0, 100)]
    public float hunger;
    [Range(0, 100)]
    public float energy;
    [Range(0, 100)]
    public float stress;
    [Range(0, 100)]
    public float money;

    public Property()
    {
        academic = 0;
        hunger = 0;
        energy = 0;
        stress = 0;
        money = 0;
    }
}
