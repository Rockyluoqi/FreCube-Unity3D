using UnityEngine;
using System.Collections;

public class NX : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnClick()
    {
        GameObject temp = GameObject.Find("Script");
        temp.GetComponent<NewRDRequest>().SetHandlerCoord("区域");
        temp.GetComponent<NewRDRequest>().SendMessgeToServer("Drill");        
    }
}
