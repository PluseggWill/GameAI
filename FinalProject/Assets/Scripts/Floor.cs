﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Floor
{
    public int FloorNumber {get; private set;}

    [SerializeField]
    public Room[,] Rooms;

    public Floor(int floorNumber, Room[,] rooms)
    {
        this.FloorNumber = floorNumber;
        this.Rooms = rooms;
    }


}
