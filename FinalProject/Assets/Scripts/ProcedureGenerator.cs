using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcedureGenerator : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    private GameObject[] wallPrefabs;

    [SerializeField]
    private GameObject[] roofPrefabs;

    [SerializeField]
    private GameObject floorPrefab;

    [SerializeField]
    private GameObject lightPrefab;

    [SerializeField]
    private GameObject cornerPrefab;

    [SerializeField]
    private Material wallMat;

    [SerializeField]
    private Texture[] albedoList;

    [SerializeField]
    private Texture[] normalMapList;

    [Header("Basic Parameters")]
    [SerializeField]
    private int cellSize = 1;

    [SerializeField]
    private int numberOfFloor = 1;

    [SerializeField]
    private int baseRow = 3;

    [SerializeField]
    private int baseColumn = 3;

    [SerializeField]
    private bool wallInside = true;

    [SerializeField]
    private Color wallColor = new Color(75.0f, 75.0f, 75.0f);

    [SerializeField]
    private bool changeMaterial = false;

    [SerializeField]
    private bool hasCorner = false;

    [Header("Rates")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float windowRate = 0.5f;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float lightRate = 0.5f;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float shrinkRate = 0.1f;

    [Header("Container")]
    [SerializeField]
    private GameObject lightContainer;

    private Floor[] floors;

    public List<GameObject> lights = new List<GameObject>();

    public List<bool> lightsOn = new List<bool>();

    private void Awake() {

        GenerateBuilding();

    }

    public void GenerateBuilding()
    {
        ClearGenerator();

        generateData();

        generatePrefab();
    }

    private void generateData()
    {
        // Init Data:
        floors = new Floor[numberOfFloor];
        List<bool> axisX = new List<bool>();
        for (int i = 0; i < baseRow; i ++)
        {
            axisX.Add(true);
        }

        List<bool> axisY = new List<bool>();
        for (int j = 0; j < baseColumn; j++)
        {
            axisY.Add(true);
        }

        Floor prevFloor = new Floor(-1, null, axisX, axisY);

        // First Step: Building Floors
        for (int n = 0; n < numberOfFloor; n ++)
        {
            Room[,] rooms = new Room[baseRow, baseColumn];

            // Second Step: Building Rooms
            for (int i = 0; i < baseRow; i ++)
            {
                // TO DO: Edge Room
                for (int j = 0; j < baseColumn; j ++)
                {
                    Vector2 position = new Vector2(i * cellSize, j * cellSize);
                    // Thrid Step: Building Walls
                    Wall[] walls = new Wall[4];
                    walls[0] = new Wall(Quaternion.Euler(0, 0, 0));
                    walls[1] = new Wall(Quaternion.Euler(0, 90, 0));
                    walls[2] = new Wall(Quaternion.Euler(0, 180, 0));
                    walls[3] = new Wall(Quaternion.Euler(0, 270, 0));

                    bool hasRoof = (n == numberOfFloor - 1);
                    bool isEdge = (j == baseColumn - 1) || (i == baseRow - 1) || (j == 0) || (i == 0);

                    rooms[i,j] = new Room(new Vector3(i * cellSize, n, j * cellSize), walls, hasRoof, isEdge);
                }
            }

            // Shrink if less than shrink rate
            bool isShrink = UnityEngine.Random.Range(0.0f, 1.0f) <= shrinkRate;
            if (isShrink)
            {
                floors[n] = new Floor();
                floors[n] = Floor.ShrinkFloor(prevFloor, rooms);
            }
            else
            {
                floors[n] = new Floor();
                floors[n] = Floor.InheritFloor(prevFloor, rooms);
            }
            prevFloor = floors[n];
        }
    }

    private void generatePrefab()
    {
        // Init Material:
        if (changeMaterial)
        {
            int index = UnityEngine.Random.Range(0, normalMapList.Length);
            wallMat.color = wallColor;
            wallMat.SetTexture("_MainTex", albedoList[index]);
            wallMat.SetTexture("_BumpMap", normalMapList[index]);
        }
        
        // Init Prefab:
        for(int n = 0; n < numberOfFloor; n ++)
        {

            // Second Step: Building Rooms
            for (int i = 0; i < baseRow; i ++)
            {
                // TO DO: Edge Room
                for (int j = 0; j < baseColumn; j ++)
                {
                    if (floors[n].HasRooms[i,j])
                    {
                        GameObject roomObject = new GameObject($"Room_{i}_{j}");
                        roomObject.transform.SetParent(this.transform);
                        Room room = floors[n].Rooms[i, j];

                        bool[] isEdge = detecteEdge(i, j, floors[n]);

                        if (n < numberOfFloor - 1)
                        {
                            room.HasRoof = !floors[n + 1].HasRooms[i, j];
                        }
                        placeRoom(roomObject, room, isEdge, n == 0);
                    }
                }
            }
        }
    }

    private WallType randomWallType(bool isGround, bool isEdge, float windowRate)
    {
        WallType result = WallType.Default;

        if (!isEdge)
        {
            if (!wallInside)
            {
                result = WallType.None;
            }
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
                    int numberOfTypes = Enum.GetNames(typeof(WallType)).Length - 3;
                    int type = UnityEngine.Random.Range(0, numberOfTypes);

                    result = (WallType)(type + 3);
                }
            }
        }

        return result;
    }
    
    private void placeRoom(GameObject roomObject, Room room, bool[] isEdge, bool isGroud = false)
    {
        if (hasCorner)
        {
            if (isEdge[0] & isEdge[1] & !isEdge[2] & !isEdge[3])
            {
                GameObject corner = Instantiate(cornerPrefab, room.GetPosition, Quaternion.Euler(0, 270, 0));
                corner.transform.SetParent(roomObject.transform);
                return;
            }

            else if (!isEdge[0] & isEdge[1] & isEdge[2] & !isEdge[3])
            {
                GameObject corner = Instantiate(cornerPrefab, room.GetPosition, Quaternion.Euler(0, 0, 0));
                corner.transform.SetParent(roomObject.transform);
                return;
            }

            else if (!isEdge[0] & !isEdge[1] & isEdge[2] & isEdge[3])
            {
                GameObject corner = Instantiate(cornerPrefab, room.GetPosition, Quaternion.Euler(0, 90, 0));
                corner.transform.SetParent(roomObject.transform);
                return;
            }

            else if (isEdge[0] & !isEdge[1] & !isEdge[2] & isEdge[3])
            {
                GameObject corner = Instantiate(cornerPrefab, room.GetPosition, Quaternion.Euler(0, 180, 0));
                corner.transform.SetParent(roomObject.transform);
                return;
            }
        }

        GameObject[] walls = new GameObject[4];
        for (int i = 0; i < 4; i ++)
        {
            room.Walls[i].WallTypeSelected = randomWallType(isGroud, isEdge[i], windowRate);
            walls[i] = Instantiate(wallPrefabs[(int)room.Walls[i].WallTypeSelected], room.GetPosition, room.Walls[i].rotation);
            walls[i].transform.SetParent(roomObject.transform);
        }

        if (isEdge[0] | isEdge[1] | isEdge[2] | isEdge[3])
        {
            GameObject light = Instantiate(lightPrefab, room.GetPosition, Quaternion.identity);
            light.transform.SetParent(lightContainer.transform);


            bool lightOn = UnityEngine.Random.Range(0.0f, 1.0f) <= lightRate;
            lightsOn.Add(lightOn);
            light.SetActive(lightOn);

            lights.Add(light);
        }
        
        if (room.HasRoof)
        {
            GameObject roof = Instantiate(roofPrefabs[UnityEngine.Random.Range(0, roofPrefabs.Length)], room.GetPosition, Quaternion.Euler(0, 90 * UnityEngine.Random.Range(0, 4), 0));
            roof.transform.SetParent(roomObject.transform);
        }

        GameObject floor = Instantiate(floorPrefab, room.GetPosition, Quaternion.identity);
        floor.transform.SetParent(roomObject.transform);
    }

    private bool[] detecteEdge(int i, int j, Floor floor)
    {
        bool[] isEdge = new bool[4];
        for (int x = 0; x < 4; x ++)
        {
            isEdge[x] = false;
        }

        int row = floor.AxisX.Count;
        int column = floor.AxisY.Count;
        bool[,] hasRooms = floor.HasRooms;

        // Detect UP Room
        if (j == column - 1)
        {
            isEdge[0] = true;
        }
        else if (!hasRooms[i, j+1])
        {
            isEdge[0] = true;
        }
        // Detect Right Room
        if (i == row - 1)
        {
            isEdge[1] = true;
        }
        else if (!hasRooms[i + 1, j])
        {
            isEdge[1] = true;
        }
        // Detect Bottom Room
        if (j == 0)
        {
            isEdge[2] = true;
        }
        else if (!hasRooms[i, j - 1])
        {
            isEdge[2] = true;
        }
        // Detect Left Room
        if (i == 0)
        {
            isEdge[3] = true;
        }
        else if (!hasRooms[i - 1, j])
        {
            isEdge[3] = true;
        }

        return isEdge;
    }

    public void ClearGenerator()
    {
        wallMat.color = new Color(75.0f, 75.0f, 75.0f);
        wallMat.SetTexture("_MainTex", null);
        wallMat.SetTexture("_BumpMap", null);

        foreach (Transform child in this.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Transform child in lightContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        lights = new List<GameObject>();
        lightsOn = new List<bool>();
    }
}
