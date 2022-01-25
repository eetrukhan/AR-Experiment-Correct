using System.IO;
using Logic;
using UnityEngine;
using UnityEngine.XR.iOS;

public class WorldMapManager : MonoBehaviour
{
    [SerializeField]
    UnityARCameraManager m_ARCameraManager;
    public Material planesMaterial;
    public Material particlesMaterial;
    public GameObject cube;
    public GameObject particles;
    public GameObject load;
    public GameObject save;

    ARWorldMap m_LoadedMap;

	serializableARWorldMap serializedWorldMap;

    ARTrackingStateReason m_LastReason;

    // Use this for initialization
    void Start ()
    {
        save.SetActive(true);
        load.SetActive(false);
        UnityARSessionNativeInterface.ARFrameUpdatedEvent += OnFrameUpdate;
        UnityARSessionNativeInterface.ARSessionInterruptedEvent += OnARInterrupted;
    }

    void OnDisable()
    {
        UnityARSessionNativeInterface.ARFrameUpdatedEvent -= OnFrameUpdate;
        UnityARSessionNativeInterface.ARSessionInterruptedEvent -= OnARInterrupted;
    }

    void OnARInterrupted()
    {
        processHide();
    }

    void processHide()
    {
        Color color = planesMaterial.color;
        color.a = 0;
        planesMaterial.color = color;
        color = particlesMaterial.color;
        color.a = 0;
        particlesMaterial.color = color;
        cube.SetActive(false);
        cube.GetComponent<UnityARHitTestExample>().enabled = false;
        particles.SetActive(false);
        ParticleSystem.Particle[] system = new ParticleSystem.Particle[0];
        particles.GetComponent<PointCloudParticleExample>().currentPS.SetParticles(system, 0);
        particles.GetComponent<PointCloudParticleExample>().enabled = false;
    }

    void OnFrameUpdate(UnityARCamera arCamera)
    {
        if (arCamera.trackingReason != m_LastReason)
        {
            Application.SetStackTraceLogType(LogType.Log, StackTraceLogType.None);
            Debug.LogFormat("worldTransform: {0}", arCamera.worldTransform.column3);
            Debug.LogFormat("trackingState: {0} {1}", arCamera.trackingState, arCamera.trackingReason);
            m_LastReason = arCamera.trackingReason;
        }
    }

    static UnityARSessionNativeInterface session
    {
        get { return UnityARSessionNativeInterface.GetARSessionNativeInterface(); }
    }

    static string path
    {
        get { return Path.Combine(Application.persistentDataPath, "myFirstWorldMap.worldmap"); }
    }

    void OnWorldMap(ARWorldMap worldMap)
    {
        if (worldMap != null)
        {
            worldMap.Save(path);
            Debug.LogFormat("ARWorldMap saved to {0}", path);
            processHide();
        }
    }

    public void Save()
    {
        session.GetCurrentWorldMapAsync(OnWorldMap);
        save.SetActive(false);
        load.SetActive(true);
    }

    public void Load()
    {
        Debug.LogFormat("Loading ARWorldMap {0}", path);
        var worldMap = ARWorldMap.Load(path);
        if (worldMap != null)
        {
            m_LoadedMap = worldMap;
            Debug.LogFormat("Map loaded. Center: {0} Extent: {1}", worldMap.center, worldMap.extent);

            UnityARSessionNativeInterface.ARSessionShouldAttemptRelocalization = true;

            var config = m_ARCameraManager.sessionConfiguration;
            config.worldMap = worldMap;
			UnityARSessionRunOption runOption = UnityARSessionRunOption.ARSessionRunOptionRemoveExistingAnchors | UnityARSessionRunOption.ARSessionRunOptionResetTracking;

			Debug.Log("Restarting session with worldMap");
			session.RunWithConfigAndOptions(config, runOption);

            startExperiment();
        }
    }


	void OnWorldMapSerialized(ARWorldMap worldMap)
	{
		if (worldMap != null)
		{
			//we have an operator that converts a ARWorldMap to a serializableARWorldMap
			serializedWorldMap = worldMap;
			Debug.Log ("ARWorldMap serialized to serializableARWorldMap");
            processHide();
        }
	}


	public void SaveSerialized()
	{        
		session.GetCurrentWorldMapAsync(OnWorldMapSerialized);
        save.SetActive(false);
        load.SetActive(true);
    }

	public void LoadSerialized()
	{
		Debug.Log("Loading ARWorldMap from serialized data");
		//we have an operator that converts a serializableARWorldMap to a ARWorldMap
		ARWorldMap worldMap = serializedWorldMap;
		if (worldMap != null)
		{
			m_LoadedMap = worldMap;
			Debug.LogFormat("Map loaded. Center: {0} Extent: {1}", worldMap.center, worldMap.extent);

			UnityARSessionNativeInterface.ARSessionShouldAttemptRelocalization = true;

			var config = m_ARCameraManager.sessionConfiguration;
			config.worldMap = worldMap;
			UnityARSessionRunOption runOption = UnityARSessionRunOption.ARSessionRunOptionRemoveExistingAnchors | UnityARSessionRunOption.ARSessionRunOptionResetTracking;

			Debug.Log("Restarting session with worldMap");
			session.RunWithConfigAndOptions(config, runOption);

            startExperiment();
		}

	}

    private void startExperiment()
    {
        GameObject.Find("Canvas").SetActive(false);
        FindObjectOfType<GlobalAround>().position = cube.transform.position;
        cube.SetActive(false);
        FindObjectOfType<GeneratorRunner>().isRunning = true;
    }
}
