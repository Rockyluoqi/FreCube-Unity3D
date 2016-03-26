using UnityEngine;
using System.Collections;

public class ZButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnClick()
    {
        GameObject.Find("MiniCoordContainer").GetComponent<MiniCoord>().ZSelected = true;
    }
}
