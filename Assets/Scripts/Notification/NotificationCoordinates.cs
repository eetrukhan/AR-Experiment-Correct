using System.Collections.Generic;
using UnityEngine;

namespace Logic
{
    public class NotificationCoordinates : MonoBehaviour
    {
        public static List<Coordinates> formInFrontOfMobileCoordinatesArray()
        {
            /* float dist = 0f;
            List<Coordinates> coordinates = new List<Coordinates>();
            coordinates.Add(new Coordinates(new Vector3(0f, 2.5f, dist), new Quaternion(0, 0, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(0f, 1.4f, dist), new Quaternion(0, 0, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(0f, 0.3f, dist), new Quaternion(0, 0, 0, 0), new Vector3(1f, 1f, 1f)));
            return coordinates; */
            
            Vector3 scale = new Vector3(0.15f,0.15f,0.15f);
           float dist = 0f;
           List<Coordinates> coordinates = new List<Coordinates>();
           float startY = 0.08f;
           float diff = 0.16f;
           coordinates.Add(new Coordinates(new Vector3(0f, startY, dist), new Quaternion(0, 0, 0, 0), scale));
           coordinates.Add(new Coordinates(new Vector3(0f, startY - diff, dist), new Quaternion(0, 0, 0, 0), scale));
           coordinates.Add(new Coordinates(new Vector3(0f, startY - 2*diff, dist), new Quaternion(0, 0, 0, 0),scale));
           return coordinates;
           
        }

        public static List<Coordinates> formInFrontOfStickerCoordinatesArray()
        {
            Vector3 scale = new Vector3(0.15f,0.15f,0.15f);
            
            //Vector3 scale = new Vector3(0.5f,0.5f,0.5f);
            float dist = 0f;
            float startY = 0.08f;
            float diff = 0.3f;
            List<Coordinates> coordinates = new List<Coordinates>();
             coordinates.Add(new Coordinates(new Vector3(0f, startY, dist), new Quaternion(0, 0, 0, 0), scale));
             coordinates.Add(new Coordinates(new Vector3(0f, startY - diff, dist), new Quaternion(0, 0, 0, 0), scale));
             coordinates.Add(new Coordinates(new Vector3(0f, startY - diff*2, dist), new Quaternion(0, 0, 0, 0),scale));
           // coordinates.Add(new Coordinates(new Vector3(0f, 2.4f, dist), new Quaternion(0, 0, 0, 0), scale));
           // coordinates.Add(new Coordinates(new Vector3(0f, 0.6f, dist), new Quaternion(0, 0, 0, 0), scale));
           // coordinates.Add(new Coordinates(new Vector3(0f, -1.2f, dist), new Quaternion(0, 0, 0, 0),scale));
            return coordinates;
        }

        public static List<Coordinates> formAroundMobileCoordinatesArray()
        {
            GlobalAround globalAround = FindObjectOfType<GlobalAround>();
            //Vector3 center = globalAround.position;
            //Quaternion rotation = globalAround.rotation;
            Vector3 center = GlobalAround.position;
            Quaternion rotation =new Quaternion(60, GlobalAround.rotation.eulerAngles.y,0,0);
            List<Coordinates> coordinates = new List<Coordinates>();
            //float xDist = 0.15f;
            //float zDist = 0.05f;
            
            float xDist = 0.3f;
            float zDist = 0.1f;
            Vector3 scale = new Vector3(0.15f,0.15f,0.15f);   
            
            coordinates.Add(new Coordinates(new Vector3(center.x - xDist, center.y, center.z), rotation, scale));  
            coordinates.Add(new Coordinates(new Vector3(center.x, center.y, center.z), rotation, scale));          
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist, center.y, center.z), rotation, scale));  
            
            coordinates.Add(new Coordinates(new Vector3(center.x + 2*xDist, center.y, center.z), rotation, scale));  
            
            
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist, center.y , center.z - zDist), rotation, scale));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist * 2, center.y , center.z - zDist), rotation, scale));
            
            coordinates.Add(new Coordinates(new Vector3(center.x, center.y , center.z - zDist * 2), rotation, scale));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist, center.y , center.z - zDist * 2), rotation, scale));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist * 2, center.y , center.z - zDist * 2), rotation, scale));
            /*
            coordinates.Add(new Coordinates(new Vector3(center.x, center.y , center.z - zDist * 2), rotation, new Vector3(0.1f, 0.1f, 0.1f)));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist, center.y, center.z - zDist * 2), rotation, new Vector3(0.1f, 0.1f, 0.1f)));
            coordinates.Add(new Coordinates(new Vector3(center.x, center.y, center.z), rotation, new Vector3(0.1f, 0.1f, 0.1f)));

            coordinates.Add(new Coordinates(new Vector3(center.x, center.y , center.z - zDist), rotation, new Vector3(0.1f, 0.1f, 0.1f)));
            coordinates.Add(new Coordinates(new Vector3(center.x, center.y, center.z), rotation, new Vector3(0.1f, 0.1f, 0.1f)));
            
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist, center.y , center.z - zDist * 2), rotation, new Vector3(0.1f, 0.1f, 0.1f)));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist, center.y , center.z - zDist), rotation, new Vector3(0.1f, 0.1f, 0.1f)));
            coordinates.Add(new Coordinates(new Vector3(center.x, center.y, center.z), rotation, new Vector3(0.1f, 0.1f, 0.1f)));
            */
            
            
           /*
              coordinates.Add(new Coordinates(new Vector3(center.x, center.y, center.z), rotation, new Vector3(0.05f, 0.05f, 0.05f)));
            coordinates.Add(new Coordinates(new Vector3(center.x, center.y , center.z - zDist), rotation, new Vector3(0.05f, 0.05f, 0.05f)));
            coordinates.Add(new Coordinates(new Vector3(center.x, center.y , center.z - zDist * 2), rotation, new Vector3(0.05f, 0.05f, 0.05f)));

            coordinates.Add(new Coordinates(new Vector3(center.x + xDist, center.y, center.z), rotation, new Vector3(0.05f, 0.05f, 0.05f)));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist, center.y , center.z - zDist), rotation, new Vector3(0.05f, 0.05f, 0.05f)));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist, center.y , center.z - zDist * 2), rotation, new Vector3(0.05f, 0.05f, 0.05f)));

            coordinates.Add(new Coordinates(new Vector3(center.x + xDist * 2, center.y, center.z), rotation, new Vector3(0.05f, 0.05f, 0.05f)));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist * 2, center.y , center.z - zDist), rotation, new Vector3(0.05f, 0.05f, 0.05f)));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist * 2, center.y , center.z - zDist * 2), rotation, new Vector3(0.05f, 0.05f, 0.05f)));

            coordinates.Add(new Coordinates(new Vector3(center.x + xDist * 3, center.y, center.z), rotation, new Vector3(0.05f, 0.05f, 0.05f)));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist * 3, center.y , center.z - zDist), rotation, new Vector3(0.05f, 0.05f, 0.05f)));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist * 3, center.y , center.z - zDist * 2), rotation, new Vector3(0.05f, 0.05f, 0.05f)));

            coordinates.Add(new Coordinates(new Vector3(center.x + xDist * 4, center.y, center.z), rotation, new Vector3(0.05f, 0.05f, 0.05f)));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist * 4, center.y , center.z - zDist), rotation, new Vector3(0.05f, 0.05f, 0.05f)));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist * 4, center.y , center.z - zDist * 2), rotation, new Vector3(0.05f, 0.05f, 0.05f)));
            */
           
            return coordinates;
        }

        public static List<Coordinates> formAroundStickerCoordinatesArray()
        {
            GlobalAround globalAround = FindObjectOfType<GlobalAround>();
            //Vector3 center = globalAround.position;
            //Quaternion rotation = globalAround.rotation;
            
            Vector3 center = GlobalAround.position;
            Debug.Log("GA rotation: "+ GlobalAround.rotation);
            Quaternion rotation =new Quaternion(60, GlobalAround.rotation.eulerAngles.y,0,0);
            List<Coordinates> coordinates = new List<Coordinates>();
            //float xDist = 0.1f;
            //float zDist = 0.09f;
            
            float xDist = 0.20f;
            float zDist = 0.22f;
            
           // float xDist = 0.2f;
           // float zDist = 0.2f;
            
            Vector3 scale = new Vector3(0.15f,0.15f,0.15f); 
            
            coordinates.Add(new Coordinates(new Vector3(center.x - xDist, center.y, center.z), rotation, scale));
            coordinates.Add(new Coordinates(new Vector3(center.x, center.y, center.z), rotation, scale));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist, center.y, center.z), rotation, scale));
            coordinates.Add(new Coordinates(new Vector3(center.x + 2*xDist, center.y, center.z), rotation, scale));
            /*
            coordinates.Add(new Coordinates(new Vector3(center.x, center.y, center.z), rotation, scale));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist, center.y, center.z - zDist), rotation, scale));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist * 2, center.y, center.z - zDist*2), rotation, scale));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist * 3, center.y, center.z - zDist*3), rotation, scale));
           */
            
            
            /*
            coordinates.Add(new Coordinates(new Vector3(center.x, center.y, center.z), rotation, scale));
            coordinates.Add(new Coordinates(new Vector3(center.x - xDist, center.y, center.z + zDist), rotation, scale));
            coordinates.Add(new Coordinates(new Vector3(center.x - xDist * 2, center.y, center.z + zDist*2), rotation, scale));
            coordinates.Add(new Coordinates(new Vector3(center.x - xDist * 3, center.y, center.z + zDist*3), rotation, scale));
            
            coordinates.Add(new Coordinates(new Vector3(center.x, center.y , center.z - zDist), rotation, scale));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist, center.y , center.z - zDist), rotation, scale));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist * 2, center.y , center.z - zDist), rotation, scale));
            
            coordinates.Add(new Coordinates(new Vector3(center.x, center.y , center.z - zDist * 2), rotation, scale));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist, center.y , center.z - zDist * 2), rotation, scale));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist * 2, center.y , center.z - zDist * 2), rotation, scale));
            */
            
            
            
            
            
            
            /*
            coordinates.Add(new Coordinates(new Vector3(center.x, center.y, center.z), rotation, new Vector3(0.05f, 0.05f, 0.05f)));
            coordinates.Add(new Coordinates(new Vector3(center.x, center.y, center.z - zDist), rotation, new Vector3(0.05f, 0.05f, 0.05f)));
            coordinates.Add(new Coordinates(new Vector3(center.x, center.y, center.z - zDist * 2), rotation, new Vector3(0.05f, 0.05f, 0.05f)));

            coordinates.Add(new Coordinates(new Vector3(center.x + xDist, center.y, center.z), rotation, new Vector3(0.05f, 0.05f, 0.05f)));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist, center.y, center.z - zDist), rotation, new Vector3(0.05f, 0.05f, 0.05f)));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist, center.y, center.z), rotation, new Vector3(0.05f, 0.05f, 0.05f)));

            coordinates.Add(new Coordinates(new Vector3(center.x + xDist * 2, center.y, center.z), rotation, new Vector3(0.05f, 0.05f, 0.05f)));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist * 2, center.y, center.z - zDist), rotation, new Vector3(0.05f, 0.05f, 0.05f)));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist * 2, center.y, center.z - zDist * 2), rotation, new Vector3(0.05f, 0.05f, 0.05f)));

            coordinates.Add(new Coordinates(new Vector3(center.x + xDist * 3, center.y, center.z), rotation, new Vector3(0.05f, 0.05f, 0.05f)));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist * 3, center.y, center.z - zDist), rotation, new Vector3(0.05f, 0.05f, 0.05f)));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist * 3, center.y, center.z - zDist * 2), rotation, new Vector3(0.05f, 0.05f, 0.05f)));

            coordinates.Add(new Coordinates(new Vector3(center.x + xDist * 4, center.y, center.z), rotation, new Vector3(0.05f, 0.05f, 0.05f)));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist * 4, center.y, center.z - zDist), rotation, new Vector3(0.05f, 0.05f, 0.05f)));
            coordinates.Add(new Coordinates(new Vector3(center.x + xDist * 4, center.y, center.z - zDist * 2), rotation, new Vector3(0.05f, 0.05f, 0.05f)));
           */
            return coordinates;
            
        }

        public static List<Coordinates> formTrayCoordinatesArrayMobile()
        {
            List<Coordinates> coordinates = new List<Coordinates>();  
            
            Vector3 scale = new Vector3(0.15f,0.15f,0.15f); 
            
            float startY = 0f;
            float diff = 0.13f;
            
            coordinates.Add(new Coordinates(new Vector3(0f, startY + diff, 0), new Quaternion(0, 0, 0, 0),scale));
            coordinates.Add(new Coordinates(new Vector3(0f, startY, 0), new Quaternion(0, 0, 0, 0),scale));
            coordinates.Add(new Coordinates(new Vector3(0f, startY - diff, 0), new Quaternion(0, 0, 0, 0), scale));
            coordinates.Add(new Coordinates(new Vector3(0f, startY - 2*diff, 0), new Quaternion(0, 0, 0, 0), scale));
            coordinates.Add(new Coordinates(new Vector3(0f, 2.2f, 0), new Quaternion(0, 0, 0, 0), scale));
            
        
        
         coordinates.Add(new Coordinates(new Vector3(0f, 2.2f, 10), new Quaternion(0, 0, 0, 0), new Vector3(1f, 1f, 1f)));
         coordinates.Add(new Coordinates(new Vector3(0f, 1.1f, 10), new Quaternion(0, 0, 0, 0), new Vector3(1f, 1f, 1f)));
         coordinates.Add(new Coordinates(new Vector3(0f, 0f, 10), new Quaternion(0, 0, 0, 0), new Vector3(1f, 1f, 1f)));
         coordinates.Add(new Coordinates(new Vector3(0f, -1.1f, 10), new Quaternion(0, 0, 0, 0), new Vector3(1f, 1f, 1f)));
         coordinates.Add(new Coordinates(new Vector3(0f, -2.2f, 10), new Quaternion(0, 0, 0, 0), new Vector3(1f, 1f, 1f)));

       /*     coordinates.Add(new Coordinates(new Vector3(-7f, -2.2f, 8), new Quaternion(0, 315, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(-7f, -1.1f, 8), new Quaternion(0, 315, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(-7f, 0f, 8), new Quaternion(0, 315, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(-7f, 1.1f, 8), new Quaternion(0, 315, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(-7f, 2.2f, 8), new Quaternion(0, 315, 0, 0), new Vector3(1f, 1f, 1f)));

            coordinates.Add(new Coordinates(new Vector3(7f, -2.2f, 8), new Quaternion(0, 45, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(7f, -1.1f, 8), new Quaternion(0, 45, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(7f, 0f, 8), new Quaternion(0, 45, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(7f, 1.1f, 8), new Quaternion(0, 45, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(7f, 2.2f, 8), new Quaternion(0, 45, 0, 0), new Vector3(1f, 1f, 1f)));

            coordinates.Add(new Coordinates(new Vector3(-9f, -2.2f, 0), new Quaternion(0, 270, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(-9f, -1.1f, 0), new Quaternion(0, 270, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(-9f, 0f, 0), new Quaternion(0, 270, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(-9f, 1.1f, 0), new Quaternion(0, 270, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(-9f, 2.2f, 0), new Quaternion(0, 270, 0, 0), new Vector3(1f, 1f, 1f)));

            coordinates.Add(new Coordinates(new Vector3(9f, -2.2f, 0), new Quaternion(0, 90, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(9f, -1.1f, 0), new Quaternion(0, 90, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(9f, 0f, 0), new Quaternion(0, 90, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(9f, 1.1f, 0), new Quaternion(0, 90, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(9f, 2.2f, 0), new Quaternion(0, 90, 0, 0), new Vector3(1f, 1f, 1f)));

            coordinates.Add(new Coordinates(new Vector3(-7f, -2.2f, -8), new Quaternion(0, 225, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(-7f, -1.1f, -8), new Quaternion(0, 225, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(-7f, 0f, -8), new Quaternion(0, 225, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(-7f, 1.1f, -8), new Quaternion(0, 225, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(-7f, 2.2f, -8), new Quaternion(0, 225, 0, 0), new Vector3(1f, 1f, 1f)));

            coordinates.Add(new Coordinates(new Vector3(7f, -2.2f, -8), new Quaternion(0, 135, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(7f, -1.1f, -8), new Quaternion(0, 135, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(7f, 0f, -8), new Quaternion(0, 135, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(7f, 1.1f, -8), new Quaternion(0, 135, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(7f, 2.2f, -8), new Quaternion(0, 135, 0, 0), new Vector3(1f, 1f, 1f)));

            coordinates.Add(new Coordinates(new Vector3(0f, -2.2f, -10), new Quaternion(0, 180, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(0f, -1.1f, -10), new Quaternion(0, 180, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(0f, 0f, -10), new Quaternion(0, 180, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(0f, 1.1f, -10), new Quaternion(0, 180, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(0f, 2.2f, -10), new Quaternion(0, 180, 0, 0), new Vector3(1f, 1f, 1f)));
            */
            return coordinates;
        }

        public static List<Coordinates> formTrayCoordinatesArraySticker()
        {
            List<Coordinates> coordinates = new List<Coordinates>();
            coordinates.Add(new Coordinates(new Vector3(0f, -3.6f, 10), new Quaternion(0, 0, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(0f, -1.8f, 10), new Quaternion(0, 0, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(0f, 0f, 10), new Quaternion(0, 0, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(0f, 1.8f, 10), new Quaternion(0, 0, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(0f, 3.6f, 10), new Quaternion(0, 0, 0, 0), new Vector3(1f, 1f, 1f)));

            coordinates.Add(new Coordinates(new Vector3(-7f, -3.6f, 8), new Quaternion(0, 315, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(-7f, -1.8f, 8), new Quaternion(0, 315, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(-7f, 0f, 8), new Quaternion(0, 315, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(-7f, 1.8f, 8), new Quaternion(0, 315, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(-7f, 3.6f, 8), new Quaternion(0, 315, 0, 0), new Vector3(1f, 1f, 1f)));

            coordinates.Add(new Coordinates(new Vector3(7f, -3.6f, 8), new Quaternion(0, 45, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(7f, -1.8f, 8), new Quaternion(0, 45, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(7f, 0f, 8), new Quaternion(0, 45, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(7f, 1.8f, 8), new Quaternion(0, 45, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(7f, 3.6f, 8), new Quaternion(0, 45, 0, 0), new Vector3(1f, 1f, 1f)));

            coordinates.Add(new Coordinates(new Vector3(-9f, -3.6f, 0), new Quaternion(0, 270, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(-9f, -1.8f, 0), new Quaternion(0, 270, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(-9f, 0f, 0), new Quaternion(0, 270, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(-9f, 1.8f, 0), new Quaternion(0, 270, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(-9f, 3.6f, 0), new Quaternion(0, 270, 0, 0), new Vector3(1f, 1f, 1f)));

            coordinates.Add(new Coordinates(new Vector3(9f, -3.6f, 0), new Quaternion(0, 90, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(9f, -1.8f, 0), new Quaternion(0, 90, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(9f, 0f, 0), new Quaternion(0, 90, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(9f, 1.8f, 0), new Quaternion(0, 90, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(9f, 3.6f, 0), new Quaternion(0, 90, 0, 0), new Vector3(1f, 1f, 1f)));

            coordinates.Add(new Coordinates(new Vector3(-7f, -3.6f, -8), new Quaternion(0, 225, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(-7f, -1.8f, -8), new Quaternion(0, 225, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(-7f, 0f, -8), new Quaternion(0, 225, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(-7f, 1.8f, -8), new Quaternion(0, 225, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(-7f, 3.6f, -8), new Quaternion(0, 225, 0, 0), new Vector3(1f, 1f, 1f)));

            coordinates.Add(new Coordinates(new Vector3(7f, -3.6f, -8), new Quaternion(0, 135, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(7f, -1.8f, -8), new Quaternion(0, 135, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(7f, 0f, -8), new Quaternion(0, 135, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(7f, 1.8f, -8), new Quaternion(0, 135, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(7f, 3.6f, -8), new Quaternion(0, 135, 0, 0), new Vector3(1f, 1f, 1f)));

            coordinates.Add(new Coordinates(new Vector3(0f, -3.6f, -10), new Quaternion(0, 180, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(0f, -1.8f, -10), new Quaternion(0, 180, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(0f, 0f, -10), new Quaternion(0, 180, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(0f, 1.8f, -10), new Quaternion(0, 180, 0, 0), new Vector3(1f, 1f, 1f)));
            coordinates.Add(new Coordinates(new Vector3(0f, 3.6f, -10), new Quaternion(0, 180, 0, 0), new Vector3(1f, 1f, 1f)));
            return coordinates;
        }
    }
}