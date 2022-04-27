using System;
using System.Collections;
using System.Collections.Generic;
using Logic;
using UnityEngine;

public class GlobalAroundPlacer : MonoBehaviour
{
    [SerializeField]
    private RsDevice source;
    [SerializeField]
    private Transform originCube;

    private MeshRenderer myMesh;
    // Start is called before the first frame update
    void Start()
    {
        myMesh = GetComponent<MeshRenderer>();
        // source = (RsDevice)FindObjectOfType(typeof(RsDevice));
        try
        {
            LoadPosition();
        }
        catch (Exception e)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
      /*  if (Input.GetKeyDown(KeyCode.L))
        {
            if (source.GetARAnchor("origin", out Vector3 pos, out Quaternion rot))
            {
                originCube.position = pos;
                originCube.rotation = rot;

                Debug.Log("The cube was successfully placed at the origin");
            }
        }
*/
      if (Input.GetKeyDown(KeyCode.H))
      {
          myMesh.enabled = !myMesh.enabled;
      }
        float speed = 1f;
        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.position += new Vector3(0,speed,0)*Time.deltaTime;
            Debug.Log("Up");
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            transform.position += new Vector3(0,-speed,0)*Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.position += new Vector3(0,0,speed)*Time.deltaTime; 
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.position += new Vector3(0,0,-speed)*Time.deltaTime; 
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-speed,0,0)*Time.deltaTime; 
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(speed,0,0)*Time.deltaTime; 
        }
        
        if (Input.GetKeyDown(KeyCode.T))
        {
            transform.Rotate(0,1f,0); 
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            transform.Rotate(0,-1f,0); 
        }

        GlobalAround.position = transform.position;
        SavePosition();
    }
    
    public void SavePosition(){
		
        Transform CurrentPlayerPosition = this.gameObject.transform;
		
        PlayerPrefs.SetFloat("PosX", CurrentPlayerPosition.position.x); // т.к. автоматической работы 
        PlayerPrefs.SetFloat("PosY", CurrentPlayerPosition.position.y); // с массивами нет, разбиваем на
        PlayerPrefs.SetFloat("PosZ", CurrentPlayerPosition.position.z);  // отдельные float и записываем
		
        PlayerPrefs.SetFloat("AngX", CurrentPlayerPosition.eulerAngles.x); 
        PlayerPrefs.SetFloat("AngY", CurrentPlayerPosition.eulerAngles.y);
        
    }
	
    public void LoadPosition(){
	
        Transform CurrentPlayerPosition = this.gameObject.transform;
	
        Vector3 PlayerPosition = new Vector3(PlayerPrefs.GetFloat("PosX"), 
            PlayerPrefs.GetFloat("PosY"), PlayerPrefs.GetFloat("PosZ"));
        Vector3 PlayerDirection = new Vector3(PlayerPrefs.GetFloat("AngX"), // генерируем новые вектора 
            PlayerPrefs.GetFloat("AngY"), 0);  // на основе загруженных данных
	
        CurrentPlayerPosition.position = PlayerPosition; // и применяем их
        CurrentPlayerPosition.eulerAngles = PlayerDirection;
    }
}
