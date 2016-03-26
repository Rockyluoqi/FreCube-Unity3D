using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 控制三维小坐标
/// </summary>
public class MiniCoord : MonoBehaviour {

    //选中与否Flag
    public bool XSelected = false;
    public bool YSelected = false;
    public bool ZSelected = false;

    //有交互的几个GameObject，有的初始是Disactivated的，所以需要先增加联系
    public GameObject progressBar;
    public GameObject DimensionSelection;
    public GameObject RollDrillContainer;
    public GameObject MiniCoordContainer;

    public GameObject xLabel;
    public GameObject yLabel;
    public GameObject zLabel;

    private string x_content;
    private string y_content;
    private string z_content;

    public int RollOrDrill;

    private List<string> currentCoords = new List<string>();
	
	// Update is called once per frame
	void Update() 
    {
        SelectCoord();
        SetToDefult();
	}

    void SetToDefult()
    {
        XSelected = false;
        YSelected = false;
        ZSelected = false;
    }

    /// <summary>
    /// 更新Coords的Label内容
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="z"></param>
    public void SetCoords(string x,string y,string z)
    {
        x_content = x;
        y_content = y;
        z_content = z;

        currentCoords.Add(x);
        currentCoords.Add(y);
        currentCoords.Add(z);

        xLabel.GetComponent<UILabel>().text =x;
        yLabel.GetComponent<UILabel>().text =y ;
        zLabel.GetComponent<UILabel>().text =z ;
    }

    /// <summary>
    /// 选择坐标并且将坐标信息发送到RollDrillHandler进行处理
    /// </summary>
    void SelectCoord()
    {
        if(XSelected)
        {
            YSelected = false;
            ZSelected = false;
            //GetComponent<UIWidget>().alpha = 0;
            RollDrillContainer.GetComponent<RollDrillHandler>().SetHandlerCoord(x_content);
            RollDrillContainer.GetComponent<RollDrillHandler>().RollDrill = true;
            if(RollOrDrill == 1)
            {
                RollDrillContainer.GetComponent<RollDrillHandler>().RollUpHandler();
            }
            else
            {
                RollDrillContainer.GetComponent<RollDrillHandler>().DrillDownHandler();
            }
        }

        if(YSelected)
        {
            XSelected = false;
            ZSelected = false;
            //GetComponent<UIWidget>().alpha = 0;
            RollDrillContainer.GetComponent<RollDrillHandler>().SetHandlerCoord(y_content);
            RollDrillContainer.GetComponent<RollDrillHandler>().RollDrill = true;
            if (RollOrDrill == 1)
            {
                RollDrillContainer.GetComponent<RollDrillHandler>().RollUpHandler();
            }
            else
            {
                RollDrillContainer.GetComponent<RollDrillHandler>().DrillDownHandler();
            }
        }

        if(ZSelected)
        {
            XSelected = false;
            YSelected = false;
            //GetComponent<UIWidget>().alpha = 0;
            RollDrillContainer.GetComponent<RollDrillHandler>().SetHandlerCoord(z_content);
            RollDrillContainer.GetComponent<RollDrillHandler>().RollDrill = true;
            if (RollOrDrill == 1)
            {
                RollDrillContainer.GetComponent<RollDrillHandler>().RollUpHandler();
            }
            else
            {
                RollDrillContainer.GetComponent<RollDrillHandler>().DrillDownHandler();
            }
        }
    }

}

//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;

//public class MiniCoord : MonoBehaviour {

//    public int DrillOrRoll;

//    public bool XSelected = false;
//    public bool YSelected = false;
//    public bool ZSelected = false;

//    //public static string _XLabel;
//    //public static string _YLabel;
//    //public static string _ZLabel;

//    public GameObject progressBar;
//    public GameObject DimensionSelection;
//    public GameObject RollDrillContainer;

//    private string x_content;
//    private string y_content;
//    private string z_content;

//    void Awake()
//    {
//        //progressBar = GameObject.Find("CircularBar");
//        //progressBar.SetActive(false);
//    }

//    // Use this for initialization
//    void Start () {
	
//    }
	
//    // Update is called once per frame
//    void Update () {
//        SelectCoord();
//        SetToDefult();
//    }

//    void SetToDefult()
//    {
//        XSelected = false;
//        YSelected = false;
//        ZSelected = false;
//    }

//    public void SetCoords(string x,string y,string z)
//    {
//        x_content = x;
//        y_content = y;
//        z_content = z;

//        GameObject.Find("YLabel").GetComponent<UILabel>().text =y ;
//        GameObject.Find("XLabel").GetComponent<UILabel>().text =x;
//        GameObject.Find("ZLabel").GetComponent<UILabel>().text =z ;
//    }

//    void SelectCoord()
//    {
//        if(XSelected)
//        {
//            YSelected = false;
//            ZSelected = false;
//            GetComponent<UIWidget>().alpha = 0;

//            RollDrillContainer.GetComponent<RollDrillHandler>().SetHandlerCoord(x_content);
            
//            DeleteAll();

//            //progressBar.SetActive(true);
//            //DimensionSelection.SetActive(true);
//            //this.Invoke("TurnOffProgressBar", 3f);
            

//            //为了保证在点击任意一条坐标轴之后再次点击RollUp按钮后，只按一次就再次出现MiniCoord，下面同理，耦合略高，后期优化
//            GameObject.Find("RollUpButton").GetComponent<RollUpButton>().showCoord = false;
//            //GameObject.Find("DrillDownButton").GetComponent<DrillDownButton>().showCoord = false;
//        }

//        if(YSelected)
//        {
//            XSelected = false;
//            ZSelected = false;
//            GetComponent<UIWidget>().alpha = 0;
//            GameObject.Find("RollUpButton").GetComponent<RollUpButton>().showCoord = false;
//            GameObject.Find("DrillDownButton").GetComponent<DrillDownButton>().showCoord = false;
//        }

//        if(ZSelected)
//        {
//            XSelected = false;
//            YSelected = false;
//            GetComponent<UIWidget>().alpha = 0;
//            GameObject.Find("RollUpButton").GetComponent<RollUpButton>().showCoord = false;
//            GameObject.Find("DrillDownButton").GetComponent<DrillDownButton>().showCoord = false;
//        }
//    }

//    private static void DeleteAll()
//    {
//        //delete原有的FreCube
//        GameObject.Find("ActionGameObject").GetComponent<Action>().DeleteCubeFromAction();
//        GameObject.Find("ActionGameObject").GetComponent<Action>().DeleteCoordsFromAction();
//    }

//    //配合Invoke()延迟调用，并且使得progressBar消失
//    void TurnOffProgressBar()
//    {
//        progressBar.SetActive(false);
//        //test
//        SendRequesetToServer();
//    }

//    /// <summary>
//    /// 8.2
//    /// 发送上卷请求，拿到数据生成新的Cube，模拟实现生成新的Cube
//    /// </summary>
//    void SendRequesetToServer()
//    {
//        GameObject.Find("ActionGameObject").GetComponent<DataHandlerTest>().ReceiveJson("test2.json");

//        GameObject.Find("ActionGameObject").GetComponent<DataHandlerTest>().DeserializeAndTransform();


//        List<string> list = new List<string>();
//        list.Add("2015");
//        list.Add("2020");
//        list.Add("2021");

//        DimensionSelection.GetComponent<DimensionSelect>().UpdateList(list);
//    }

//}

