using UnityEngine;
using System.Collections;

public class NY : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick()
    {
        //GameObject temp = GameObject.Find("Script");
        //temp.GetComponent<NewRDRequest>().SetHandlerCoord("时间");
        //temp.GetComponent<NewRDRequest>().SendMessgeToServer("Drill");
        GameObject temp = GameObject.Find("ActionGameObject");
        temp.GetComponent<DataHandlerTest>().ReceiveJson("time-month.json");
    }
}
