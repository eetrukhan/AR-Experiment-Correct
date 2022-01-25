using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HMDAdministratedTaskController : MonoBehaviour
{
    public enum CellNumber
    {
        EIGHT = 8,
        TWELVE = 12,
        SIXTEEN = 16
    }

    public CellNumber CellNumberHorizontally = CellNumber.EIGHT;
    [Range(0, 100)]
    public uint BoardDiagonal = 50; // Percentage
    public float Distance;

    public GameObject RedCubePrefab;
    public uint RedCubesNumber;

    public GameObject WhiteCubePrefab;
    public uint WhiteCubesNumber;

    public GameObject SpherePrefab;
    public float SphereVelocity = 0f; // Meters per second

    private static readonly float ASPECT_RATIO = 4 / 3f;
    private static readonly float ACUTE_ANGLE = 36.8698976f * Mathf.Deg2Rad;
    private static readonly float OBTUSE_ANGLE = 53.1301024f * Mathf.Deg2Rad;

    private static readonly float NorthStar_FoV = 72f; // In degrees

    private Transform _volume;
    private GameObject _generatedObjectsParent;
    private bool _canBePlayed;

    private Transform _sphere;

    private uint _collisionNumber;

    void Awake()
    {
        // Check whether everything is on its place
        if (Distance <= 0)
        {
            Debug.LogError("Error: The Distance value can't be negative. Disabling the script");
            enabled = false;
            return;
        }

        if (RedCubePrefab == null)
        {
            Debug.LogError("Error: The RedCubePrefab field cannot be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (WhiteCubePrefab == null)
        {
            Debug.LogError("Error: The WhiteCubePrefab field cannot be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (SpherePrefab == null)
        {
            Debug.LogError("Error: The SpherePrefab field cannot be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        _volume = transform.Find("Volume");
        if (_volume == null)
        {
            Debug.LogError("Error: HMDAdministratedTaskController couldn't find a child GO called 'Volume'. Disabling the script.");
            enabled = false;
            return;
        }
    }

    public void GenerateFiled()
    {
        if (!enabled || !gameObject.activeSelf)
            return;

        try
        {
            // Clear everything up
            DestroyParentObj();

            // Adjust the size of the board
            float boardWidth, boardHeight;
            GetBoardDimentions(BoardDiagonal, Distance, out boardWidth, out boardHeight);
            float cellSize = boardWidth / (float)CellNumberHorizontally;

            _volume.localScale = new Vector3(boardWidth, boardHeight, cellSize);

            // Create an object that will surve as a parent for all the cubes and a sphere
            _generatedObjectsParent = CreateParentObj();

            System.Random random = new System.Random();

            // Randomly place and instanciate the sphere
            int cellNumberVertically = Mathf.RoundToInt((float)CellNumberHorizontally / ASPECT_RATIO);
            Vector2 spherePosition = new Vector2(
                random.Next(2, (int)CellNumberHorizontally - 2),
                random.Next(2, cellNumberVertically - 2));

            _sphere = Instantiate(SpherePrefab).transform;
            _sphere.rotation = _generatedObjectsParent.transform.rotation;
            _sphere.parent = _generatedObjectsParent.transform;
            _sphere.localPosition = Vec2ToVec3(spherePosition, (int)CellNumberHorizontally, cellNumberVertically, cellSize);
            _sphere.localScale = Vector3.one * cellSize;

            _sphere.GetComponent<PhysicsController>().Parent = transform;
            _sphere.GetComponent<PhysicsController>().enabled = true;

            List<Vector2> CubePositions = new List<Vector2>();

            // Create a list of heuristics
            List<Func<Vector2, bool>> heuristics = new List<Func<Vector2, bool>>();
            heuristics.Add((positionOnBoard) =>
            {
                foreach (Vector2 cubePosition in CubePositions)
                {
                    if ((positionOnBoard.x == cubePosition.x && IsInVicinity(positionOnBoard.y, cubePosition.y, 4)) ||
                        (positionOnBoard.y == cubePosition.y && IsInVicinity(positionOnBoard.x, cubePosition.x, 4)))
                        return false;
                }
                return true;
            });
            heuristics.Add((positionOnBoard) =>
            {
                if (positionOnBoard.x == 2 || positionOnBoard.x == (int)CellNumberHorizontally - 3 ||
                    positionOnBoard.y == 2 || positionOnBoard.y == (int)cellNumberVertically - 3)
                    return false;
                return true;
            });
            /*heuristics.Add((positionOnBoard) =>
            {
                if (IsInVicinity(spherePosition.x, positionOnBoard.x, 1) ||
                    IsInVicinity(spherePosition.y, positionOnBoard.y, 1))
                    return false;
                return true;
            });
            heuristics.Add((positionOnBoard) =>
            {
                foreach (Vector2 cubePosition in CubePositions)
                {
                    if (positionOnBoard == cubePosition)
                        return false;
                }
                return true;
            });*/

            // Place red cubes
            for (int i = 0; i < RedCubesNumber; i++)
            {
                Vector2 cubePosition = new Vector2();
                do
                {
                    // Choosing direction
                    int direction = random.Next();
                    if (IsEven(direction)) // Placing the cube horizontally
                    {
                        cubePosition.x = random.Next(0, (int)CellNumberHorizontally);
                        cubePosition.y = random.Next(0, 2) * (cellNumberVertically - 1);
                    }
                    else // Placing the cube vertically
                    {
                        cubePosition.x = random.Next(0, 2) * ((int)CellNumberHorizontally - 1);
                        cubePosition.y = random.Next(0, cellNumberVertically);
                    }
                }
                while (!AllHeuristicsAreMet(cubePosition, heuristics));

                GameObject redCube = Instantiate(RedCubePrefab);
                redCube.transform.rotation = _generatedObjectsParent.transform.rotation;
                redCube.transform.parent = _generatedObjectsParent.transform;
                redCube.transform.localPosition = Vec2ToVec3(cubePosition, (int)CellNumberHorizontally, cellNumberVertically, cellSize);
                redCube.transform.localScale = Vector3.one * cellSize;

                CubePositions.Add(cubePosition);

                redCube.GetComponent<CollisionDetector>().OnCollisionDetected += () =>
                {
                    _collisionNumber++;
                };
            }

            // Place white cubes
            for (int i = 0; i < WhiteCubesNumber; i++)
            {
                Vector2 cubePosition = new Vector2();
                do
                {
                    // Choosing direction
                    int direction = random.Next();
                    if (IsEven(direction)) // Placing the cube horizontally
                    {
                        cubePosition.x = random.Next(0, (int)CellNumberHorizontally);
                        cubePosition.y = random.Next(0, 2) * (cellNumberVertically - 1);
                    }
                    else // Placing the cube vertically
                    {
                        cubePosition.x = random.Next(0, 2) * ((int)CellNumberHorizontally - 1);
                        cubePosition.y = random.Next(0, cellNumberVertically);
                    }
                }
                while (!AllHeuristicsAreMet(cubePosition, heuristics));

                GameObject whiteCube = Instantiate(WhiteCubePrefab);
                whiteCube.transform.rotation = _generatedObjectsParent.transform.rotation;
                whiteCube.transform.parent = _generatedObjectsParent.transform;
                whiteCube.transform.localPosition = Vec2ToVec3(cubePosition, (int)CellNumberHorizontally, cellNumberVertically, cellSize);
                whiteCube.transform.localScale = Vector3.one * cellSize;

                CubePositions.Add(cubePosition);
            }

            _canBePlayed = true;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    public void Play()
    {
        if (!enabled || !gameObject.activeSelf || !_canBePlayed)
            return;

        // Start counting from the beginning
        _collisionNumber = 0;

        // Push the sphere in random direction
        System.Random random = new System.Random();

        const int vicinity = 1; // In degrees
        Vector3 sphereVelocity = Vector3.right * SphereVelocity;
        int rotationAroundZ;
        do
        {
            rotationAroundZ = random.Next(2, 359);
        }
        while (IsInVicinity(rotationAroundZ, 90, vicinity) || IsInVicinity(rotationAroundZ, 180, vicinity) || IsInVicinity(rotationAroundZ, 270, vicinity));
        sphereVelocity = _sphere.rotation * Quaternion.Euler(0, 0, rotationAroundZ) * sphereVelocity;

        _sphere.GetComponent<PhysicsController>().SetVelocity(sphereVelocity);

        _canBePlayed = false;
    }

    public uint GetNumberOfColissions()
    {
        return _collisionNumber;
    }

    private GameObject CreateParentObj()
    {
        GameObject obj = new GameObject("GeneratedObjects");

        obj.transform.rotation = _volume.transform.rotation;
        obj.transform.position = _volume.transform.position;
        obj.transform.parent = transform;

        return obj;
    }

    private void DestroyParentObj()
    {
        if (_generatedObjectsParent != null)
        {
            UnityEngine.Object.Destroy(_generatedObjectsParent);
            _generatedObjectsParent = null;
        }
    }

    private bool AllHeuristicsAreMet(Vector2 positionOnBoard, List<Func<Vector2, bool>> heuristics)
    {
        foreach (Func<Vector2, bool> heuristic in heuristics)
        {
            if (!heuristic(positionOnBoard))
                return false;
        }

        return true;
    }

    private static Vector3 Vec2ToVec3(Vector2 positionOnBoard, int cellNumberHorizontally, int cellNumberVertically, float cellSize)
    {
        Vector3 origin = Vector3.zero;
        origin.x -= ((float) cellNumberHorizontally / 2f - 0.5f) * cellSize;
        origin.y -= ((float) cellNumberVertically / 2f - 0.5f) * cellSize;

        return new Vector3(
            origin.x + positionOnBoard.x * cellSize,
            origin.y + positionOnBoard.y * cellSize,
            origin.z);
    }

    private static void GetBoardDimentions(uint boardDiagonal, float distanceToTheObject, out float boardWidth, out float boardHeight)
    {
        float boardAngularSize = (boardDiagonal / 100f) * NorthStar_FoV;
        boardAngularSize *= Mathf.Deg2Rad; // Now in radians

        // Check this out https://en.wikipedia.org/wiki/Angular_diameter
        float boardDiagonalInMeters = 2f * distanceToTheObject * Mathf.Tan(boardAngularSize / 2f);

        boardWidth = boardDiagonalInMeters * Mathf.Sin(OBTUSE_ANGLE);
        boardHeight = boardDiagonalInMeters * Mathf.Sin(ACUTE_ANGLE);
    }

    private static bool IsInVicinity(int value1, int value2, int vicinity)
    {
        return Mathf.Abs(value2 - value1) <= vicinity;
    }

    private static bool IsInVicinity(float value1, float value2, float vicinity)
    {
        return Mathf.Abs(value2 - value1) <= vicinity;
    }

    private static bool IsEven(int value)
    {
        return value % 2 == 0;
    }
}
