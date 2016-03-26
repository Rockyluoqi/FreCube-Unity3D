using UnityEngine;
using System.Collections;

public class SelfAdaption : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        int ManualWidth = 960;
        int ManualHeight = 320;
        UIRoot uiRoot = GameObject.FindObjectOfType<UIRoot>();
        if (uiRoot != null)
        {
            if (System.Convert.ToSingle(Screen.height) / Screen.width > System.Convert.ToSingle(ManualHeight) / ManualWidth)
                uiRoot.manualHeight = Mathf.RoundToInt(System.Convert.ToSingle(ManualWidth) / Screen.width * Screen.height);
            else
                uiRoot.manualHeight = ManualHeight;
        }
	}
}
