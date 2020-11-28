using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wall
{
    
    public Quaternion rotation {get; private set;}

    public WallType WallTypeSelected {get; set; } = WallType.Default;

    public Wall(Quaternion rotation, WallType wall = WallType.Default)
    {
        this.WallTypeSelected = wall;
        this.rotation = rotation;
    }

}

public enum WallType
    {
        None,
        Default,
        Door,
        Window,
        Window_Balcony,
    }