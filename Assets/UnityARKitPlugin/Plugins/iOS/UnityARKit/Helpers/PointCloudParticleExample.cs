using Logic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class PointCloudParticleExample : MonoBehaviour 
{
    public ParticleSystem pointCloudParticlePrefab;
    public int maxPointsToShow;
    public float particleSize = 1.0f;
    public GameObject sceneCamera;
    public float trayShowAngle = 0.2f;
    public float trayHideAngle = -0.35f;
    Vector3[] m_PointCloudData;
    bool frameUpdated = false;
    internal ParticleSystem currentPS;
    ParticleSystem.Particle [] particles;
    private bool isTrayOn = false;

    // Use this for initialization
    void Start () 
    {
        UnityARSessionNativeInterface.ARFrameUpdatedEvent += ARFrameUpdated;
        UnityARSessionNativeInterface.ARSessionInterruptedEvent += OnARInterrupted;
        currentPS = Instantiate (pointCloudParticlePrefab);
        m_PointCloudData = null;
        frameUpdated = false;
    }

    void OnDisable()
    {
        UnityARSessionNativeInterface.ARFrameUpdatedEvent -= ARFrameUpdated;
        UnityARSessionNativeInterface.ARSessionInterruptedEvent -= OnARInterrupted;
    }

    public void OnARInterrupted()
    {
        UnityARSessionNativeInterface.ARFrameUpdatedEvent -= ARFrameUpdated;
    }

    public void ARFrameUpdated(UnityARCamera camera)
    {
        if(sceneCamera.transform.rotation.x > trayShowAngle && !isTrayOn)
        {
            isTrayOn = true;
            EventManager.Broadcast(EVENT.ShowTray);
            return;
        }
        if (sceneCamera.transform.rotation.x < trayHideAngle && isTrayOn)
        {
            isTrayOn = false;
            EventManager.Broadcast(EVENT.HideTray);
            return;
        }
        if (camera.pointCloud != null)
        {
           m_PointCloudData = camera.pointCloud.Points;
        }
        frameUpdated = true;
    }

    // Update is called once per frame
    void Update () 
    {
        if (frameUpdated) 
        {
            if (m_PointCloudData != null && m_PointCloudData.Length > 0 && maxPointsToShow > 0) 
            {
                int numParticles = Mathf.Min (m_PointCloudData.Length, maxPointsToShow);
                ParticleSystem.Particle[] particles = new ParticleSystem.Particle[numParticles];
                int index = 0;
                foreach (Vector3 currentPoint in m_PointCloudData) 
                {     
                    particles [index].position = currentPoint;
                    particles [index].startColor = new Color (1.0f, 1.0f, 1.0f);
                    particles [index].startSize = particleSize;
                    index++;
                    if (index >= numParticles) break;
                }
                currentPS.SetParticles (particles, numParticles);
            } 
            else 
            {
                ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1];
                particles [0].startSize = 0.0f;
                currentPS.SetParticles (particles, 1);
            }
            frameUpdated = false;
        }
    }
}
