using UnityEngine;
using System.Collections;

public class PX : MonoBehaviour {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void OnClick()
    {
        //Debug.Log("PX is clicked.");
        //GameObject temp = GameObject.Find("Script");
        //temp.GetComponent<NewRDRequest>().SetHandlerCoord("区域");
        //temp.GetComponent<NewRDRequest>().SendMessgeToServer("Roll");
        GameObject temp = GameObject.Find("ActionGameObject");
        temp.GetComponent<DataHandlerTest>().ReceiveJson("address-area.json");
    }
}
