using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DiceContainerControl : MonoBehaviour {

    public GameObject ActionGameObject;

	// Use this for initialization
	void Start () {
	
	}       
	
	// Update is called once per frame
	void Update () {
	
	}
    
    
    /// <summary>
    /// 点击Restore按钮将恢复到上卷下钻前的FreCube
    /// </summary>
    public void Restore()
    {
        List<FreCube> freCubeList =  ActionGameObject.GetComponent<DataHandlerTest>().FreCubeCacheList;
        ActionGameObject.GetComponent<DataHandlerTest>().ReceiveFreCube(freCubeList[freCubeList.Count - 1]);
    }
}
