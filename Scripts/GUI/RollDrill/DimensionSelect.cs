using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 8.4模拟上卷下钻的基本功能，交互功能还不够完善，将继续优化交互的功能
/// 8.12此类废弃，这个类服务于一个PopupList,弃用，改成了上卷下钻按钮
/// </summary>
public class DimensionSelect : MonoBehaviour {

    //上卷下钻的维度列表
    private UIPopupList DimensionList;

    //圆形进度条
    public GameObject ProgressBar;

	// Use this for initialization
	void Start () {
        //DimensionList = GetComponent<UIPopupList>();
        //EventDelegate.Add(DimensionList.onChange, OnChange);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnChange()
    {
        if(UIPopupList.current.value.Equals("2020"))
        {
            DeleteAll();

            Debug.Log(UIPopupList.current.value);
            ProgressBar.SetActive(true);
            this.Invoke("TurnOffProgressBar", 3f);

            SendRequesetToServer(UIPopupList.current.value);
        }
    }

    /// <summary>
    /// 更新PopList中的item的值
    /// </summary>
    public void UpdateList(List<string> list)
    {
        DimensionList.items = list;
        //Popup List默认的显示项就是第一个item
        //GameObject.Find("DimensionSelection").transform.GetChild(1).GetComponent<UILabel>().text = list[0];
    }

    //配合Invoke()延迟调用，并且使得progressBar消失
    void TurnOffProgressBar()
    {
        ProgressBar.SetActive(false);
    }

    void SendRequesetToServer(string value)
    {
        GameObject.Find("ActionGameObject").GetComponent<DataHandlerTest>().ReceiveJson(value+".json");
        GameObject.Find("ActionGameObject").GetComponent<DataHandlerTest>().DeserializeAndTransform();
    }

    private static void DeleteAll()
    {
        //delete原有的FreCube
        GameObject.Find("ActionGameObject").GetComponent<Action>().DeleteCubeFromAction();
        GameObject.Find("ActionGameObject").GetComponent<Action>().DeleteCoordsFromAction();
    }

}
