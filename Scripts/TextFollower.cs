using UnityEngine;
using System.Collections;
 
 public class TextFollower : MonoBehaviour {
 
/*     public GameObject gameObject;
     public float DistanceFromCamera;
 
     //让文字一直面向镜头
     void Update() {
           gameObject.transform.LookAt(Camera.main.transform);
           //gameObject.transform.up = new Vector3(0, -1, 0);
 
         //Transform game = gameObject.transform.GetChild(1).gameObject.GetComponent<TextMesh>().transform;
         //game.parent = Camera.main.transform;
     }*/


     public Camera camera;
     Quaternion direction = new Quaternion();

     // Use this for initialization  
     void Start()
     {
         if(camera==null)
            camera = Camera.main;

         direction.x = transform.localRotation.x;
         direction.y = transform.localRotation.y;
         direction.z = transform.localRotation.z;
         direction.w = transform.localRotation.w;
     }

     // Update is called once per frame  
     void Update()
     {
         Camera cam = null;
         if (camera != null)
         {
             cam = camera;
         }
         else
         {
             cam = Camera.current;
             if (!cam)
                 return;
         }
         transform.rotation = cam.transform.rotation * direction;
     }  
}
