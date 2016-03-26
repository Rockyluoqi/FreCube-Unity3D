using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RollDrillHandler : MonoBehaviour {

    //Roll up按钮
    public GameObject Rollup;
    //Drill down按钮
    public GameObject DrillDown;

    public bool RollDrill = false;
    public bool clickRollDrill = false;

    public GameObject MiniCoordContainer;
    public GameObject ProgressBar;
    public GameObject ActionGameObject;

    private string CurrentHandlerCoord = "";
    private List<string> Coords;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    /// <summary>
    /// 点击RollUp触发
    /// </summary>
    public void RollUpHandler()
    {
        //clickRollDrill = !clickRollDrill;
        //if (clickRollDrill)
        //{
        //    MiniCoordContainer.SetActive(true);
        //    clickRollDrill = !clickRollDrill;
        //}

        //if(MiniCoordContainer.activeSelf&&RollDrill)
        //{
        //    SendMessgeToServer("Roll");
        //    MiniCoordContainer.SetActive(false);
        //    RollDrill = false;
        //}
        MiniCoordContainer.SetActive(true);
        MiniCoordContainer.GetComponent<TweenAlpha>().PlayForward();
        MiniCoordContainer.GetComponent<MiniCoord>().RollOrDrill = 1;

        //ProgressBar.SetActive(true);
        //this.Invoke("DisactivateProgressBar", 3f);
        if (RollDrill)
        {
            SendMessgeToServer("Roll");
            MiniCoordContainer.GetComponent<TweenAlpha>().PlayReverse();
            RollDrill = false;
            DeleteAll();
            ProgressBar.SetActive(true);
        }
    }

    /// <summary>
    /// 点击DrillDown触发
    /// </summary>
    public void DrillDownHandler()
    {
        MiniCoordContainer.SetActive(true);
        MiniCoordContainer.GetComponent<TweenAlpha>().PlayForward();

        MiniCoordContainer.GetComponent<MiniCoord>().RollOrDrill = 2;
        //ProgressBar.SetActive(true);
        //this.Invoke("DisactivateProgressBar", 3f);
        if (RollDrill)
        {
            SendMessgeToServer("Drill");
            MiniCoordContainer.GetComponent<TweenAlpha>().PlayReverse();
            RollDrill = false;
            DeleteAll();
            ProgressBar.SetActive(true);
        }
    }

    private static void DeleteAll()
    {
        //delete原有的FreCube
        GameObject.Find("ActionGameObject").GetComponent<Action>().DeleteCubeFromAction();
        GameObject.Find("ActionGameObject").GetComponent<Action>().DeleteCoordsFromAction();
    }

    /// <summary>
    /// 设置需要上卷下钻的维度
    /// </summary>
    public void SetHandlerCoord(string handlerCoord)
    {
        CurrentHandlerCoord = handlerCoord;
    }

    void DisactivateProgressBar()
    {
        ProgressBar.SetActive(false);
    }

    /// <summary>
    /// 调用外部Js函数，发送请求
    /// </summary>
    /// <param name="requestType"></param>
    private void SendMessgeToServer(string requestType)
    {
        string request = ActionGameObject.GetComponent<DataHandlerTest>().SerializeFreCubeRequest(requestType, CurrentHandlerCoord);
        //print(request);
        Application.ExternalCall("PostToServer",request);
    }
}
