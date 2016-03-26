using UnityEngine;
using System.Collections;

public class HandAction : MonoBehaviour {

    public MouseFollowRotation follow;

    Vector3 speed;
    Vector3 ori_pos;

    public int HandDirecion; //1 right 2 left

 

	// Use this for initialization
	void Start () {
        follow = Camera.main.transform.GetComponent<MouseFollowRotation>();

	}
	
	// Update is called once per frame
	void Update () {



        
        speed = ori_pos-transform.position;

        if (HandDirecion == 1)
        {
            if (speed.x > 0)
            {
                follow.x += speed.x;

            }
            if (speed.y > 0)
            {
                follow.y += speed.y;

            }
            if (speed.z > 0 && speed.z <5.0f)
            {
                follow.distance += speed.z;

            }
        }
        else
        {
            if (speed.x < 0)
            {
                follow.x += speed.x;

            }
            if (speed.y < 0)
            {
                follow.y += speed.y;

            }
            if (speed.z < 0 && speed.z>-5.0f)
            {
                follow.distance += speed.z;

            }
        }

        /*Debug.Log(speed.x + "x");
        Debug.Log(speed.y + "y");
        Debug.Log(speed.z + "z");*/
        //Debug.Log(transform.forward.z + "f");

        ori_pos = transform.position;


	}

}
