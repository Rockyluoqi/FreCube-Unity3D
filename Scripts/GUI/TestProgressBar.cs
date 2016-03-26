using UnityEngine;
using System.Collections;

public class TestProgressBar : MonoBehaviour {

    GameObject progressBar;

    void Awake()
    {
        progressBar = GameObject.Find("LifeBar");
    }

	// Use this for initialization
	void Start () {
        //ShowProgressBar(10);
        progressBar.SetActive(true);
        this.Invoke("ShowProgressBar", 5f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void ShowProgressBar()
    {
        progressBar.SetActive(false);
    }
}
