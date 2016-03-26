using UnityEngine;
using System.Collections;

public class LockButton : MonoBehaviour {

    public GameObject MainCamera;
    private bool _IsClick = false;

    private Vector3 TempTransform = Vector3.zero;
    private Quaternion TempRotation = Quaternion.Euler(0, 0, 0);
    private float TempDistance = 0;

    private float x_size, y_size, z_size;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    //点击按钮将会锁定cube的一个面正对用户，用户无法进行旋转操作
    //再次点击将会恢复到锁定前的状态，用户可以进行旋转等操纵
    void OnClick()
    {
        float x,y,z;

        x_size = GameObject.Find("ActionGameObject").GetComponent<Action>().GetNum_x();
        y_size = GameObject.Find("ActionGameObject").GetComponent<Action>().GetNum_y();
        z_size = GameObject.Find("ActionGameObject").GetComponent<Action>().GetNum_z();

        //Debug.Log("Lock button X SIZE " + x_size);
        _IsClick = !_IsClick;
        if (_IsClick)
        {
            //改变按钮的Text为UNLOCK
            transform.GetChild(0).gameObject.transform.GetComponent<UILabel>().text = "UNLOCK";

            //暂存未点击LOCK按钮状态下的MainCamera的位置，为了第二次点击恢复到原来的状态
            TempRotation = MainCamera.transform.rotation;
            TempTransform = MainCamera.transform.position;
            TempDistance = MainCamera.GetComponent<MouseFollowRotation>().distance;

            //x=MainCamera.transform.position.x;
            //y=MainCamera.transform.position.y;
            //z=MainCamera.transform.position.z;
            //请务必注意这里的取角度的问题，这里需要使用.eulerAngles属性而不是Rotation属性
            y = MainCamera.transform.eulerAngles.y;
            if ((y >= 315 && y <= 360) || (y >= 0 && y <= 45))
            {
                MainCamera.transform.position = new Vector3(z_size+1,y_size/2+1, -z_size-3);
                MainCamera.transform.rotation = Quaternion.Euler(0, 0, 0);
            }

            if (y > 45 && y <= 135)
            {
                MainCamera.transform.position = new Vector3(-z_size, y_size/2+1, y_size/2+1);
                MainCamera.transform.rotation = Quaternion.Euler(0, 90, 0);
            }

            if ((y > 135 && y <= 180) || (y < 225 && y >= 180))
            {
                MainCamera.transform.position = new Vector3(z_size+1, z_size/2+1, x_size);
                MainCamera.transform.rotation = Quaternion.Euler(0, 180, 0);
            }

            if (y < 315 && y >= 225)
            {
                MainCamera.transform.position = new Vector3(x_size+11, y_size/2+1, y_size/2+1);
                MainCamera.transform.rotation = Quaternion.Euler(0, -90, 0);
            }

            Destroy(MainCamera.GetComponent<MouseFollowRotation>());
        }
        else 
        {
            transform.GetChild(0).gameObject.transform.GetComponent<UILabel>().text = "LOCK";

            MainCamera.AddComponent<MouseFollowRotation>();
            MainCamera.GetComponent<MouseFollowRotation>().target = GameObject.Find("AnchorPoint0").transform;
            MainCamera.AddComponent<MouseFollowRotation>();
            MainCamera.GetComponent<MouseFollowRotation>().AxisCam = GameObject.Find("Camera").transform;
            
            MainCamera.transform.position = TempTransform;
            MainCamera.transform.rotation = TempRotation;
            MainCamera.GetComponent<MouseFollowRotation>().distance = TempDistance;

        }

    }

}
