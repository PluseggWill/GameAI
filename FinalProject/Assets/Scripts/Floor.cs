using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Floor
{
    public int FloorNumber {get; private set;}

    [SerializeField]
    public Room[,] Rooms;

    [SerializeField]
    public bool[,] HasRooms;

    [SerializeField]
    public List<bool> AxisX;

    [SerializeField]
    public List<bool> AxisY;

    public Floor(int floorNumber, Room[,] rooms, List<bool> axisX, List<bool> axisY)
    {
        this.FloorNumber = floorNumber;
        this.Rooms = rooms;
        this.AxisX = axisX.GetRange(0, axisX.Count);
        this.AxisY = axisY.GetRange(0, axisY.Count);
        this.HasRooms = new bool[this.AxisX.Count, this.AxisY.Count];

        for (int i = 0; i < AxisX.Count; i++)
        {
            for (int j = 0; j < AxisY.Count; j++)
            {
                HasRooms[i, j] = this.AxisX[i] & this.AxisY[j];
            }
        }
    }

    public Floor()
    {

    }

    public static Floor InheritFloor(Floor prevFloor, Room[,] rooms)
    {
        List<bool> axisX = prevFloor.AxisX.GetRange(0, prevFloor.AxisX.Count);
        List<bool> axisY = prevFloor.AxisY.GetRange(0, prevFloor.AxisY.Count);

        Floor result = new Floor(prevFloor.FloorNumber + 1, rooms, axisX, axisY);
        return result;
    }

    public static Floor ShrinkFloor(Floor prevFloor, Room[,] rooms)
    {
        List<bool> axisX = prevFloor.AxisX.GetRange(0, prevFloor.AxisX.Count);
        List<bool> axisY = prevFloor.AxisY.GetRange(0, prevFloor.AxisY.Count);

        List<float> test = new List<float>() { 0, 2, 2.3f, 4.3f };

        bool isVertical = UnityEngine.Random.Range(0, 2) > 0;
        bool isLeft = UnityEngine.Random.Range(0, 2) > 0;

        if (isVertical)
        {
            if (isLeft)
            {
                int index = axisY.IndexOf(true);
                if (index >= 0)
                {
                    axisY[index] = false;
                }
            }
            else
            {
                int index = axisY.LastIndexOf(true);
                if (index >= 0)
                {
                    axisY[index] = false;
                }
            }
        }
        else
        {
            if (isLeft)
            {
                int index = axisX.IndexOf(true);
                if (index >= 0)
                {
                    axisX[index] = false;
                }
            }
            else
            {
                int index = axisX.LastIndexOf(true);
                if (index >= 0)
                {
                    axisX[index] = false;
                }
            }
        }

        Floor result = new Floor(prevFloor.FloorNumber + 1, rooms, axisX, axisY);
        return result;
    }

}
