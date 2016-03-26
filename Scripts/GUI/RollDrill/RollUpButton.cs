using UnityEngine;
using System.Collections;

public class RollUpButton : MonoBehaviour {

    public bool showCoord = false;

    public GameObject coord;
    public GameObject popupList;
    public GameObject NextPreButtonContainer;
    public GameObject RollDrillContainer;

    public GameObject DiceButton;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(DiceButton.GetComponent<DiceSliceButton>()._IsClick)
        {
            DestroyInRoll();
//            Debug.Log("DestroyInRoll();");
        }
	}

    void OnClick()
    {
        DiceButton.GetComponent<DiceSliceButton>()._IsClick = false;
        showCoord  = !showCoord;
        Debug.Log("showCoord is "+showCoord);
        if(showCoord)
        {
            NextPreButtonContainer.SetActive(true);
            RollDrillContainer.SetActive(true);
            ////显示坐标
            //coord.SetActive(true);
            //GameObject.Find("MiniCoordContainer").GetComponent<UIWidget>().alpha = 1;

            //1 is Roll Up flag
            //GameObject.Find("MiniCoordContainer").GetComponent<MiniCoord>().DrillOrRoll = 1; 
        }
        else
        {
            DestroyInRoll();
        }
    }

    private void DestroyInRoll()
    {
        coord.SetActive(false);
        NextPreButtonContainer.SetActive(false);
        RollDrillContainer.SetActive(false);
    }
}
