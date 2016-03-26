using UnityEngine;
using System.Collections;

public class DiceSliceButton : MonoBehaviour {

    public bool _IsClick = false;

    public GameObject DiceContainer;

    public GameObject Guide;

    public GameObject SelectionObject;

    public GameObject DiceFinished;
    public GameObject DiceReset;

    public GameObject RollButton;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if(RollButton.GetComponent<RollUpButton>().showCoord)
        {
            DestroyInDice();
        } 
	}

    void OnClick()
    {
        //RollButton.GetComponent<RollUpButton>().showCoord = false;
        //这里的Tween的使用为这个Guide页面的出现与退出增加了渐进动态效果，酷炫一些
        Guide.SetActive(true);
        _IsClick = !_IsClick;
        if(_IsClick)
        {
            //play tween动画
            Guide.GetComponent<TweenFOV>().PlayForward();
            Guide.GetComponent<TweenAlpha>().PlayForward();
            Guide.GetComponent<TweenPosition>().PlayForward();
            SelectionObject.SetActive(true);
        }
        else
        {
            //反向播放tween动画
            Guide.GetComponent<TweenFOV>().PlayReverse();
            Guide.GetComponent<TweenAlpha>().PlayReverse();
            Guide.GetComponent<TweenPosition>().PlayReverse();
            SelectionObject.SetActive(false);
        }
    }

    /// <summary>
    /// 在上卷下钻的Guide页面中点击I Know按钮将会调用这个函数
    /// 使按钮生效
    /// </summary>
    public void HideGuide()
    {
        Guide.GetComponent<TweenFOV>().PlayReverse();
        Guide.GetComponent<TweenAlpha>().PlayReverse();
        Guide.GetComponent<TweenPosition>().PlayReverse();
        //DiceFinished.SetActive(true);
        //DiceReset.SetActive(true);
        DiceContainer.SetActive(true);
    }

    private void DestroyInDice()
    {
        //GameObject.Find("ActionGameObject").GetComponent<Action>().DeleteCubeFromAction();
        //GameObject.Find("ActionGameObject").GetComponent<Action>().DeleteCoordsFromAction();
        //DiceReset.SetActive(false);
        //DiceFinished.SetActive(false);
        DiceContainer.SetActive(false);
        SelectionObject.GetComponent<CubeSelection>().ResetAllSelection();
        SelectionObject.SetActive(false);
    }
}
