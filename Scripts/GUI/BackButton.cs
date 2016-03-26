using UnityEngine;
using System.Collections;

public class BackButton : MonoBehaviour {

    private bool BackIsPressed = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnClick()
    {
        //BackIsPressed = !BackIsPressed;
        //if(BackIsPressed)
        //{
        //回到开始的提交页面
        //string url = Application.absoluteURL+"//firstPage";
        Application.ExternalCall("backQuery");
        //Application.OpenURL("http://www.baidu.com");
        //}
    }
}
