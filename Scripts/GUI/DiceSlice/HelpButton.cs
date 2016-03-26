using UnityEngine;
using System.Collections;

public class HelpButton : MonoBehaviour {

    public bool _IsClick = false;

    public GameObject Guide;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    

    void OnClick()
    {
        //RollButton.GetComponent<RollUpButton>().showCoord = false;
        //这里的Tween的使用为这个Guide页面的出现与退出增加了渐进动态效果，酷炫一些
        Guide.SetActive(true);
        _IsClick = !_IsClick;
        if (_IsClick)
        {
            //play tween动画
            Guide.GetComponent<TweenFOV>().PlayForward();
            Guide.GetComponent<TweenAlpha>().PlayForward();
            Guide.GetComponent<TweenPosition>().PlayForward();
        }
        else
        {
            //反向播放tween动画
            Guide.GetComponent<TweenFOV>().PlayReverse();
            Guide.GetComponent<TweenAlpha>().PlayReverse();
            Guide.GetComponent<TweenPosition>().PlayReverse();
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
    }
}
