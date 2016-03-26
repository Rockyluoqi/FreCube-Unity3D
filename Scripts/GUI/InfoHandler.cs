using UnityEngine;
using System.Collections;

public class InfoHandler : MonoBehaviour {

    public bool showMenu = false;
    public GameObject InfoPanel;

    public GameObject X;
    public GameObject Y;
    public GameObject Z;

    public GameObject Function;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}

    void OnClick()
    {
        showMenu = !showMenu;
        ShowInfoPanel();
    }

    void ShowInfoPanel()
    {
        InfoPanel.SetActive(true);
        if(showMenu)
        {
            InfoPanel.GetComponent<TweenAlpha>().PlayForward();
        }
        else
        {
            InfoPanel.GetComponent<TweenAlpha>().PlayReverse();
        }
    }

    public void SetInfo(string x,string y,string z)
    {
        X.GetComponent<UILabel>().text = x;
        Y.GetComponent<UILabel>().text = y;
        Z.GetComponent<UILabel>().text = z;
        //Function.GetComponent<UILabel>().text = function;
    }
}
