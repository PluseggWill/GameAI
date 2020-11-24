using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcedureGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] wallPrefabs;

    [SerializeField]
    private GameObject[] roofPrefabs;

    [SerializeField]
    private GameObject floorPrefab;

    [SerializeField]
    private int cellSize = 1;

    [SerializeField]
    private int numberOfFloor = 1;

    [SerializeField]
    private int row = 3;

    [SerializeField]
    private int column = 3;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float windowRate = 0.5f;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float lightRate = 0.5f;

    private Floor[] floors;

    private void Awake() {
        generateData();

        generatePrefab();
    }

    private void generateData()
    {
        // Init Data:
        floors = new Floor[numberOfFloor];

        // First Step: Building Floors
        for(int n = 0; n < numberOfFloor; n ++)
        {
            Room[,] rooms = new Room[row, column];

            // Second Step: Building Rooms
            for (int i = 0; i < row; i ++)
            {
                // TO DO: Edge Room
                for (int j = 0; j < column; j ++)
                {
                    Vector2 position = new Vector2(i * cellSize, j * cellSize);
                    // Thrid Step: Building Walls
                    Wall[] walls = new Wall[4];
                    walls[0] = new Wall(Quaternion.Euler(0, 0, 0), randomWallType(n == 0, j == column - 1, windowRate));
                    walls[1] = new Wall(Quaternion.Euler(0, 90, 0), randomWallType(n == 0, i == row - 1, windowRate));
                    walls[2] = new Wall(Quaternion.Euler(0, 180, 0), randomWallType(n == 0, j == 0, windowRate));
                    walls[3] = new Wall(Quaternion.Euler(0, 270, 0), randomWallType(n == 0, i == 0, windowRate));

                    bool hasRoof = (n == numberOfFloor - 1);
                    bool isEdge = (j == column - 1) || (i == row - 1) || (j == 0) || (i == 0);

                    rooms[i,j] = new Room(new Vector3(i * cellSize, n, j * cellSize), walls, hasRoof, isEdge);
                }
            }

            floors[n] = new Floor(n, rooms);
        }
    }

    private void generatePrefab()
    {
        for(int n = 0; n < numberOfFloor; n ++)
        {

            // Second Step: Building Rooms
            for (int i = 0; i < row; i ++)
            {
                // TO DO: Edge Room
                for (int j = 0; j < column; j ++)
                {
                    GameObject roomObject = new GameObject($"Room_{i}_{j}");
                    roomObject.transform.SetParent(this.transform);

                    Room room = floors[n].Rooms[i,j];
                    placeRoom(roomObject, room);
                }
            }
        }
    }

    private WallType randomWallType(bool isGround, bool isEdge, float windowRate)
    {
        WallType result = WallType.Default;

        if (!isEdge)
        {
            // TO DO: Bigger Room?
        }
        else 
        {
            bool hasWindow = UnityEngine.Random.Range(0.0f, 1.0f) <= windowRate;

            if (hasWindow)
            {
                if (isGround)
                {
                    result = WallType.Door;
                }
                else
                {
                    int numberOfTypes = Enum.GetNames(typeof(WallType)).Length - 2;
                    int type = UnityEngine.Random.Range(0, numberOfTypes);

                    result = (WallType)(type + 2);
                }
            }
        }

        return result;
    }
    
    private void placeRoom(GameObject roomObject, Room room)
    {
        GameObject[] walls = new GameObject[4];

        walls[0] = Instantiate(wallPrefabs[(int)room.Walls[0].WallTypeSelected], room.GetPosition, room.Walls[0].rotation);
        walls[0].transform.SetParent(roomObject.transform);

        walls[1] = Instantiate(wallPrefabs[(int)room.Walls[1].WallTypeSelected], room.GetPosition, room.Walls[1].rotation);
        walls[1].transform.SetParent(roomObject.transform);

        walls[2] = Instantiate(wallPrefabs[(int)room.Walls[2].WallTypeSelected], room.GetPosition, room.Walls[2].rotation);
        walls[2].transform.SetParent(roomObject.transform);

        walls[3] = Instantiate(wallPrefabs[(int)room.Walls[3].WallTypeSelected], room.GetPosition, room.Walls[3].rotation);
        walls[3].transform.SetParent(roomObject.transform);
        
        // TO DO: Lights Generation
        if (room.HasRoof)
        {
            GameObject roof = Instantiate(roofPrefabs[UnityEngine.Random.Range(0, roofPrefabs.Length)], room.GetPosition, Quaternion.identity);
            roof.transform.SetParent(roomObject.transform);
        }

        GameObject floor = Instantiate(floorPrefab, room.GetPosition, Quaternion.identity);
        floor.transform.SetParent(roomObject.transform);
    }
}
