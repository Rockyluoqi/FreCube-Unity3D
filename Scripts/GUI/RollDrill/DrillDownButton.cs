using UnityEngine;
using System.Collections;

public class DrillDownButton : MonoBehaviour {

    public bool showCoord = false;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClick()
    {
        showCoord = !showCoord;
        if (showCoord)
        {
            //显示MiniCoordContainer
            GameObject.Find("MiniCoordContainer").GetComponent<UIWidget>().alpha = 1;
        }
        else
        {
            //隐藏MiniCoordContainer
            GameObject.Find("MiniCoordContainer").GetComponent<UIWidget>().alpha = 0;
        }
    }
}
