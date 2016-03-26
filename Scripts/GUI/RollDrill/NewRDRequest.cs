using UnityEngine;
using System.Collections;

public class NewRDRequest : MonoBehaviour {

    public GameObject ActionGameObject;
    private string CurrentHandlerCoord;

    //黄字，要上卷下钻的维级别
    //public GameObject Px;
    //public GameObject Nx;
    //public GameObject Py;
    //public GameObject Ny;
    //public GameObject Pz;
    //public GameObject Nz;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //public void RollUpHandler()
    //{
    //    SendMessgeToServer("Roll");

    //    DeleteAll();
    //}

    //public void DrillDownHandler()
    //{
    //    SendMessgeToServer("Drill");
    //    DeleteAll();
    //}

    private static void DeleteAll()
    {
        //delete原有的FreCube
        GameObject.Find("ActionGameObject").GetComponent<Action>().DeleteCubeFromAction();
        GameObject.Find("ActionGameObject").GetComponent<Action>().DeleteCoordsFromAction();
    }

    public void SetHandlerCoord(string handlerCoord)
    {
        CurrentHandlerCoord = handlerCoord;
    }

    public void SendMessgeToServer(string requestType)
    {
        string request = ActionGameObject.GetComponent<DataHandlerTest>().SerializeFreCubeRequest(requestType, CurrentHandlerCoord);
        //print(request);
        Application.ExternalCall("PostToServer", request);
    }
}
