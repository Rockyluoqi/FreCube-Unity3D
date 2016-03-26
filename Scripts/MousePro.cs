using UnityEngine;
using System.Collections;

public class MousePro : MonoBehaviour {

    /// <summary>  
    /// 当前视角的摄像机  
    /// </summary>  
    public Camera _camPlayer;
    /// <summary>  
    /// 鼠标射线  
    /// </summary>  
    private Ray _ray;
    /// <summary>  
    /// 射线碰撞的结构  
    /// </summary>  
    private RaycastHit _rayhit;
    /// <summary>  
    /// 鼠标拾取的有效距离  
    /// </summary>  
    private float _fDistance = 20f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        
        if (Input.GetMouseButtonUp(0))
        {
            
            //鼠标的屏幕坐标空间位置转射线  
            _ray = _camPlayer.ScreenPointToRay(Input.mousePosition);
            //射线检测，相关检测信息保存到RaycastHit 结构中  
            if (Physics.Raycast(_ray, out _rayhit, _fDistance))
            {
                //打印射线碰撞到的对象的名称  
                print(_rayhit.collider.gameObject.name);
            }
            
        }

	}



}
