using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PreNextButtonControl : MonoBehaviour {

    public GameObject PreButton;
    public GameObject NextButton;

    public GameObject ActionGameObject;

    //freCubeCacheList中的FreCube的数量
    private int freCubeCurrentCount = 0;

    //Next和Pre的计数游标
    private int currentIndex = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    /// <summary>
    /// PreviousButton的处理，查看上一个FreCube
    /// </summary>
    public void PreButtonHandler()
    {
        List<FreCube> freCubeList = ActionGameObject.GetComponent<DataHandlerTest>().FreCubeCacheList;
        freCubeCurrentCount = freCubeList.Count;
        //点击一次PreButton将currentIndex渐增1，但是不能超过freCube的count上限
        if (currentIndex < freCubeCurrentCount-1) {
            ++currentIndex;
        }

        //Debug.Log("freCubeCurrentCount " + freCubeCurrentCount + "currentIndex: " + currentIndex + "freCubeCurrentCount - currentIndex: " + (freCubeCurrentCount - currentIndex));
        //如果游标合法没有越界错误，则将当前位置的前一个freCube发送给DataHandler中重新处理生成函数
        if (freCubeCurrentCount - currentIndex > 0)
        {
            ActionGameObject.GetComponent<DataHandlerTest>().ReceiveFreCube(freCubeList[freCubeCurrentCount - currentIndex-1]);
        }
    }

    /// <summary>
    /// NextButton的处理，查看下一个FreCube
    /// </summary>
    public void NextbuttonHandler()
    {
        List<FreCube> freCubeList = ActionGameObject.GetComponent<DataHandlerTest>().FreCubeCacheList;
        freCubeCurrentCount = freCubeList.Count;

        if(currentIndex > 0)
            --currentIndex;
        
        //Debug.Log("freCubeCurrentCount " + freCubeCurrentCount + "currentIndex: " + currentIndex + "freCubeCurrentCount - currentIndex: " + (freCubeCurrentCount - currentIndex));
        if ((freCubeCurrentCount - currentIndex ) > 0)
        {
            ActionGameObject.GetComponent<DataHandlerTest>().ReceiveFreCube(freCubeList[freCubeCurrentCount - currentIndex-1]);
        }
    }
}
