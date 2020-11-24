using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
    public Wall[] Walls;

    [SerializeField]
    private Vector3 position;

    public bool HasRoof {get; set;}

    public bool IsEdge {get; private set;}

    public Room(Vector3 pos, Wall[] walls, bool hasRoof = false, bool isEdge = false)
    {
        this.position = pos;
        this.Walls = walls;
        this.HasRoof = hasRoof;
        this.IsEdge = isEdge;
    }

    public Vector3 GetPosition
    {
        get
        {
            return this.position;
        }
    }
}
