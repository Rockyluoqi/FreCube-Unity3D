using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIAct : MonoBehaviour {

    public CubeManager CubeManager_;
    public GameObject ActionGameObject;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void cut()
    {
		//GameObject.Find("ActionGameObject").GetComponent<Action>().DeleteCubeFromAction();
        //Debug.Log(CubeManager_.GetCutString()[0]);
        if (CubeManager_.GetCutString().Count != 0)
        {
            DiceMain(CubeManager_.GetCutString());
        }
        else
        {
            Debug.Log("No coord is selected!");
        }
    }

     /// <summary>
    /// 这是切块的主函数，选中三维坐标后，生成一个一般意义上的Cube
    /// </summary>
    private void DiceMain(List<string> coords)
    {
        ActionGameObject.GetComponent<DataHandlerTest>().CreateDiceData(coords);
    }

}
