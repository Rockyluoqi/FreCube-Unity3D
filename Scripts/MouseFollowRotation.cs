/**
 * 脚本功能:摄像头旋转实现场景漫游，浏览Cube
 * 已经实现功能：1鼠标滑轮缩放
 * 			    2按住鼠标右键绕轴旋转
 * 待扩展的功能：1鼠标左键的绕轴旋转，识别左右滑动实现左右旋转，上下滑动实现上下旋转
 * 			    2限制非水平或者竖直方向的旋转
 **/
using UnityEngine;
using System;

public class MouseFollowRotation : MonoBehaviour {

	public Transform target;

    //***坐标轴跟随旋转
    public Transform AxisCam;	
	
	public float xSpeed=200, ySpeed=200, mSpeed=10;
	public float yMinLimit=-360, yMaxLimit=360;
	public float distance=12, minDistance=4, maxDistance=50;

	public float x = 0.0f;
	public float y = 0.0f;

    private Vector2 first = Vector2.zero;
    private Vector2 second = Vector2.zero;

    public CoordManager CoordManager_;


	//bool needDamping = false;
	public bool needDamping =true; 
	float damping = 5.0f;

    Vector3[] directions = new Vector3[6];


	public void SetTarget( GameObject go )
	{
		target = go.transform;
        
	}

	// Use this for initialization
	void Start () {
		Vector3 angles = transform.eulerAngles;
    	x = angles.y;
    	y = angles.x;

        directions[0] = new Vector3(1, 0, 0);
        directions[1] = new Vector3(0, 1, 0);
        directions[2] = new Vector3(0, 0, 1);
        directions[3] = new Vector3(-1, 0, 0);
        directions[4] = new Vector3(0, -1, 0);
        directions[5] = new Vector3(0, 0, -1);




	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
        int minAngleN=-1;
        float minAngle = 360,temp;

        for (int i = 0; i < 6; i++)
        {
            temp = Vector3.Angle(transform.position - target.position, directions[i]);
            if (temp < minAngle)
            {
                minAngle = temp;
                minAngleN = i;

            }
        }
        //Debug.Log(minAngleN);
        CoordManager_.SetTableXY_int(minAngleN);



            if (target)
            {
                //use the light button of mouse to rotate the camera
                if (Input.GetMouseButton(1))
                {
                    x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                    y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;

                    y = ClampAngle(y, yMinLimit, yMaxLimit);

                    //print(Input.GetAxis("Mouse X"));
                    //print( Input.GetAxis("Mouse Y"));
                    //print(x);
                    //print(y);
                }

                distance -= Input.GetAxis("Mouse ScrollWheel") * mSpeed;
                distance = Mathf.Clamp(distance, minDistance, maxDistance);

                Quaternion rotation = Quaternion.Euler(y, x, 0.0f);
                Vector3 disVector = new Vector3(0.0f, 0.0f, -distance);
                Vector3 position = rotation * disVector + target.position;

                if (Input.GetMouseButton(0))
                {
                    x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
                    y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;


                    y = ClampAngle(y, yMinLimit, yMaxLimit);
                }

                //adjust the camera

                

                if (needDamping)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * damping);


                    //***设置Axis旋转
                    AxisCam.rotation = transform.rotation;
                    AxisCam.position = Vector3.Normalize(transform.position - target.position) / 3.0f;



                    transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * damping);
                }
                else
                {
                    transform.rotation = rotation;


                    //***设置Axis旋转
                    AxisCam.rotation = transform.rotation;
                    AxisCam.position = Vector3.Normalize(transform.position - target.position) / 3.0f;



                    transform.position = position;
                }
            }
	}
	
	static float ClampAngle (float angle, float min, float max) 
	{
	    if (angle < -360)
		    angle += 360;
	    if (angle > 360)
		    angle -= 360;
	    return Mathf.Clamp (angle, min, max);
	}

    float angle_360(Vector3 from_, Vector3 to_)
    {

        Vector3 v3 = Vector3.Cross(from_, to_);

        if (v3.z > 0)

            return Vector3.Angle(from_, to_);

        else

            return 360 - Vector3.Angle(from_, to_);

    }


}
/*
 * 
	
	private int MouseWheelSensitivity = 50; //control zoom speed
	private int MouseZoomMin = 20; //min distance
	private int MouseZoomMax = 112; //max distance
	private float normalDistance = 500;
	public bool flag_Roable = false; //auto rotate tag
	public Vector3 normalized;
	
	private System.DateTime oldTime;
	
	private System.DateTime nowTime;  
	
	float mY = 5.0f;  
	public float HorizontalSpeed = 2.0F;
	public float VerticalSpeed = 2.0F;  
	
		//物体通过鼠标左键上下移动，中间键缩放、右键旋转，30秒没操作，物体自动旋转
		nowTime = System.DateTime.Now;
		System.TimeSpan ts1 = new System.TimeSpan(oldTime.Ticks);
		System.TimeSpan ts2 = new System.TimeSpan(nowTime.Ticks);
		System.TimeSpan ts = ts2.Subtract(ts1).Duration();
		//mCamera = GameObject.Find("Camera");
		
		if(ts.Seconds>5 && !Input.anyKey) 
		{
			flag_Roable = true;
			oldTime = System.DateTime.Now;
		}
		
		if (flag_Roable && Input.anyKey)
		{
			flag_Roable = false;
		}
		
		if(flag_Roable)
		{
			x -= Time.deltaTime * 30;
			var rotation = Quaternion.Euler(0, x, 0);
			transform.RotateAround(target.position, Vector3.up, 0.3f);
		}
		else
		{
			if(Input.GetMouseButton(1))
			{
				if(Input.GetAxis("Mouse X")<0)
				{
					transform.RotateAround(target.position, Vector3.down, 4);
				}
				if (Input.GetAxis("Mouse X") > 0)
				{
					transform.RotateAround(target.position, Vector3.up, 4);
				}
			}
			else if(Input.GetAxis("Mouse ScrollWheel") != 0)
			{
				if (normalDistance >= MouseZoomMin && normalDistance <= MouseZoomMax)
				{
					normalDistance -= Input.GetAxis("Mouse ScrollWheel") * MouseWheelSensitivity;
				}
				if (normalDistance < MouseZoomMin)
				{
					normalDistance = MouseZoomMin;
				}
				if (normalDistance > MouseZoomMax)
				{
					normalDistance = MouseZoomMax;
				}
				
				transform.camera.fieldOfView = normalDistance;
			}
			else if(Input.GetMouseButton(0))
			{
				//将按住鼠标左键从上下移动cube变为上下旋转cube
				
				if (Input.GetAxis("Mouse Y") < 0)
				{
					transform.RotateAround(target.position, Vector3.right, 4);
				}
				if (Input.GetAxis("Mouse Y") > 0)
				{
					transform.RotateAround(target.position, Vector3.left, 4);
				}
				//if (Input.GetAxis("Mouse Y") < 0)  //down  
				//{  
				//    Vector3 temp = Vector3.up * 60.0f * Time.deltaTime;  
				//    print("wyz==up===" + transform.localPosition.y);  
				//    if (transform.localPosition.y > 300)  
				//    {  
				//        temp = Vector3.up * 5.0f * Time.deltaTime;  
				//        transform.Translate(temp);  
				//    }  
				//    else  
				//    {  
				//        transform.Translate(temp);  
				//    }  
				//}  
				
				//if (Input.GetAxis("Mouse Y") > 0)  //up   
				//{  
				//    print("wyz==donw===" + transform.localPosition.y);  
				//    Vector3 temp = Vector3.down * 60.0f * Time.deltaTime;  
				
				//    if (transform.localPosition.y < -300)  
				//    {  
				//        temp = Vector3.up * 5.0f * Time.deltaTime;  
				//        transform.Translate(temp);  
				//    }  
				//    else  
				//    {  
				//        transform.Translate(temp);  
				//    }  
				//}   
			}
		}
		*/