using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleFactory : MonoBehaviour
{
    public Transform Camera;
    public GameObject[] ObstaclePrefabs;
#pragma warning disable 649
    [SerializeField]
    private float[] _obstacleXPositions;
#pragma warning restore 649

    public float IntensityOfObstacleAppearance = 1f; // Obstacles per meter
    public float ClearPathLength = 5f; // Meters
    public float SpawnObstaclesAt = 5f; // Meters

    private GameObject _obstacleParent;
    private Transform _trackPose;

    private StartFrom _startFrom;
    private bool _producing;
    private Vector3 _nextObstaclePosition;

    private uint _totalNumberOfGroundObstacles;
    private uint _totalNumberOfHighObstacles;

    void Start()
    {
        if (Camera == null)
        {
            Debug.LogError("Error: The Camera field cannot be left unassigned. Disabling the script");
            enabled = false;
            return;
        }

        if (ObstaclePrefabs == null || ObstaclePrefabs.Length == 0)
        {
            Debug.LogError("Error: The GroundObstaclePrefab hasn't been set up. Disabling the script");
            enabled = false;
            return;
        }

        _trackPose = transform.Find("TrackItself");
        if (_trackPose == null)
        {
            Debug.LogError("Error: ObstacleFactory couldn't find a child GO named TrackItself. Disabling the script");
            enabled = false;
            return;
        }
    }

    void Update()
    {
        // Check the state: Do we need to produce obstacles at all?
        if (_producing)
        {
            System.Random random = new System.Random();

            // Get the user position on track and transform it from global to local coordinates
            Vector3 userLocalPositionOnTrack = GetUserLocalPositionOnTrack();

            Func<float, bool> checkTheDistance = (z) =>
            {
                // FIX IT: This doesn't take into account the size of obstacles
                if (_startFrom == StartFrom.TheBeginning)
                    return (z > _nextObstaclePosition.z - SpawnObstaclesAt);
                else
                    return (z < _nextObstaclePosition.z + SpawnObstaclesAt);
            };

            // If the distance to the next obstacle position is less than the SpawnObstaclesAt value
            if (checkTheDistance(userLocalPositionOnTrack.z))
            {
                // Spawn an obstacle
                int obstacleIndex = random.Next(ObstaclePrefabs.Length);
                int xPositionIndex = random.Next(_obstacleXPositions.Length);

                _nextObstaclePosition.x = _obstacleXPositions[xPositionIndex];

                GameObject obstacle = Instantiate(ObstaclePrefabs[obstacleIndex], _obstacleParent.transform);
                obstacle.transform.localPosition = _nextObstaclePosition;

                if (obstacle.tag == "GroundObstacle")
                    _totalNumberOfGroundObstacles++;
                else if (obstacle.tag == "HighObstacle")
                    _totalNumberOfHighObstacles++;

                // Update the position of the next obstacle
                _nextObstaclePosition.z += (_startFrom == StartFrom.TheBeginning) ? IntensityOfObstacleAppearance : -IntensityOfObstacleAppearance;

                // Do this untill we reach one of the track borders
                if (Mathf.Abs(_nextObstaclePosition.z) >= _trackPose.localScale.z / 2f)
                    _producing = false;
            }
        }
    }

    public void Destroy()
    {
        if (_producing)
            throw new Exception("ObstacleFactory is already working. You can't call this method while there is ongoing production");

        if (_obstacleParent != null)
        {
            UnityEngine.Object.Destroy(_obstacleParent);
            _obstacleParent = null;
        }
    }

    public void StartProducing(StartFrom startFrom)
    {
        if (!enabled)
            return;

        if (_producing)
            throw new Exception("ObstacleFactory is already working. You can't call this method while there is ongoing production");

        // Fool proofing
        if (_obstacleParent != null)
            Destroy();

        _startFrom = startFrom;
        _totalNumberOfGroundObstacles = 0;
        _totalNumberOfHighObstacles = 0;

        // The first obstacle position is at the ClearPathLength from the beginning of the track
        _nextObstaclePosition = new Vector3();
        _nextObstaclePosition.z = IntensityOfObstacleAppearance == float.PositiveInfinity ? float.NegativeInfinity : _trackPose.localScale.z / 2f - ClearPathLength;
        if (_startFrom == StartFrom.TheBeginning)
            _nextObstaclePosition.z = -_nextObstaclePosition.z;

        // Create an empty object as a child of the Track GO which will serve as a parent for all obstacles
        _obstacleParent = new GameObject();
        _obstacleParent.transform.parent = transform;

        _obstacleParent.transform.localPosition = _trackPose.localPosition;
        _obstacleParent.transform.localRotation = _trackPose.localRotation;

        // Here we go :-)
        _producing = true;
    }

    public void StopProducing()
    {
        _producing = false;

        if (_obstacleParent != null)
        {
            for (int i = 0; i < _obstacleParent.transform.childCount; i++)
                _obstacleParent.transform.GetChild(i).GetComponent<ObstacleController>().enabled = false;
        }
    }

    public bool IsWorking()
    {
        return _producing;
    }

    public uint GetTotalNumberOfGroundObstacles()
    {
        return _totalNumberOfGroundObstacles;
    }

    public uint GetTotalNumberOfHighObstacles()
    {
        return _totalNumberOfHighObstacles;
    }

    public void ProduceAllAtOnce(StartFrom startFrom)
    {
        if (!enabled)
            return;

        if (_producing)
            throw new Exception("ObstacleFactory is already working. You can't call this method while there is ongoing production");

        // Fool proofing
        if (_obstacleParent != null)
            Destroy();

        _totalNumberOfGroundObstacles = 0;
        _totalNumberOfHighObstacles = 0;

        // Create an empty object as a child of the Track GO
        _obstacleParent = new GameObject();
        _obstacleParent.transform.parent = transform;

        _obstacleParent.transform.localPosition = _trackPose.localPosition;
        _obstacleParent.transform.localRotation = _trackPose.localRotation;

        // Create obstacles
        System.Random random = new System.Random();
        Vector3 obstaclePosition = new Vector3();
        Func<float, StartFrom, bool> notEnoughObstacles = (z, sf) =>
        {
            // FIXIT: This doesn't take into account the size of obstacles
            if (sf == StartFrom.TheBeginning)
                return (z < _trackPose.localScale.z / 2f);
            else
                return (z > -_trackPose.localScale.z / 2f);
        };

        obstaclePosition.z = _trackPose.localScale.z / 2f - ClearPathLength;
        if (startFrom == StartFrom.TheBeginning)
            obstaclePosition.z = -obstaclePosition.z;
        do
        {
            int obstacleIndex = random.Next(ObstaclePrefabs.Length);
            int xPositionIndex = random.Next(_obstacleXPositions.Length);

            obstaclePosition.x = _obstacleXPositions[xPositionIndex];

            GameObject obstacle = Instantiate(ObstaclePrefabs[obstacleIndex], _obstacleParent.transform);
            obstacle.transform.localPosition = obstaclePosition;

            if (obstacle.tag == "GroundObstacle")
                _totalNumberOfGroundObstacles++;
            else if (obstacle.tag == "HighObstacle")
                _totalNumberOfHighObstacles++;

            obstaclePosition.z += (startFrom == StartFrom.TheBeginning) ? IntensityOfObstacleAppearance : -IntensityOfObstacleAppearance;
        }
        while (notEnoughObstacles(obstaclePosition.z, startFrom));
    }

    public Vector3 GetUserLocalPositionOnTrack()
    {
        // Project the position of a headset on the track
        Vector3 cameraPosition = Camera.position - _trackPose.position;
        Vector3 userPositionOnPlane = Vector3.ProjectOnPlane(cameraPosition, _trackPose.up);
        userPositionOnPlane += _trackPose.position;

        return _obstacleParent.transform.InverseTransformPoint(userPositionOnPlane);
    }
}