using UnityEngine;
using System.Collections;

public class CubesCon : MonoBehaviour {

    public MouseFollowRotation mouseCon;



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Y_add()
    {
        mouseCon.y += 20.0f;
        if (mouseCon.y > mouseCon.yMaxLimit)
        {
            mouseCon.y = mouseCon.yMaxLimit;
        }

    }

    void Y_sub()
    {
        mouseCon.y -= 20.0f;
        if (mouseCon.y < mouseCon.yMinLimit)
        {
            mouseCon.y = mouseCon.yMinLimit;
        }
    }

    void X_add()
    {
        mouseCon.x += 20.0f;
        
    }

    void X_sub()
    {
        mouseCon.x -= 20.0f;
        
    }

    void Z_add()
    {
        mouseCon.distance += 10.0f;
        if (mouseCon.distance > mouseCon.maxDistance)
            mouseCon.distance = mouseCon.maxDistance;


    }

    void Z_sub()
    {
        mouseCon.distance -= 10.0f;
        if (mouseCon.distance < mouseCon.minDistance)
            mouseCon.distance = mouseCon.minDistance;
    }
}
