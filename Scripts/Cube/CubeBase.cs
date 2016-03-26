using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CubeBase:MonoBehaviour {

    public GameObject cube;
    
    //public class Text
    //{
    //    public int direction;
    //    public string content { set; get; }
    //}

    //public Text[] texts;

    //用于标记Cube,高维中用于区分Cube
    public string cubeName{set;get;}

    //一个抽象Cube中的Cube的总个数
    public int numOfCube { set; get; }

    //标记一个抽象Cube的六个面，这个抽象Cube可以包括很多子Cube
    public int cubeDirectionTag { set; get; }

    /// <summary>
    /// Cube的六个面
    /// </summary>
    public enum Direction
    {
        forward = 1,
        back = 2,
        up = 3,
        down = 4,
        left = 5,
        right = 6,
    }

    //六个面的text集合，用来统一操作这六个面的字符，弃用，使用Text3DGroup
    private string[] textGroup = new string[6];

    //六个面的3DText集合
    private  GameObject[] text3DGroup = new GameObject[6];

    /// <summary>
    /// 测试每个面朝向时设置TextGroup
    /// </summary>
    /// <param name="index"></param>
    /// <param name="data"></param>
    public void SetTextGroup(int index,string data)
    {
        //Text text = 
        textGroup[index] = data;
    }
   
    /// <summary>
    /// 设置text的文字，根据传入的方向
    /// </summary>
    /// <param name="direction">传入的方向，1-6</param>
    /// <param name="s">要显示的内容</param>
    public void SetText(int direction,string s)
    {
        //print("setText" + text3DGroup[direction].GetComponent<TextMesh>().text);
        text3DGroup[direction-1].GetComponent<TextMesh>().text = s;
    }

    /// <summary>
    /// 根据指定的面的方向，返回相应的3D Text
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    public GameObject GetText(int direction)
    {
        return text3DGroup[direction - 1];
    }

    /// <summary>
    /// 返回这个坐标点的内容
    /// </summary>
    /// <returns>string</returns>
    public string GetText()
    {
        return cube.transform.GetChild(0).gameObject.GetComponent<TextMesh>().text;
    }

    public void ChangeColor()
    {
        Color color_ = new Color();
        color_ = Color.white;

        transform.renderer.material.color = color_;

    }

    public void ResetColor()
    {
        Color color_ = new Color();
        color_.r = 220.0f / 255.0f;
        color_.g = 220.0f / 255.0f;
        color_.b = 220.0f / 255.0f;
        color_.a = 1.0f;


        transform.renderer.material.color = color_;

    }


    /// <summary>
    /// 便于观察坐标的测试方法
    /// </summary>
    /// <param name="s"></param>
    public void SetText(string s)
    {
        for (int i = 0; i < cube.transform.childCount; i++)
        {
            cube.transform.GetChild(i).gameObject.GetComponent<TextMesh>().text = s;
        }
    }

    /// <summary>
    /// 拿到当前的TextGroup 字符数组
    /// </summary>
    /// <returns></returns>
    public string[] GetTextGroup()
    {
        return textGroup;
    }

    /// <summary>
    /// 拿到当前的3DTextGroup 字符数组
    /// </summary>
    public GameObject[] GetText3DGroup()
    {
        return text3DGroup;
    }

    void Awake()
    {
        InitTexts();
    }

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}

    /// <summary>
    /// 初始化每个面的Text
    /// </summary>
    void InitTexts()
    {
         //遍历子物体，这里Cube的子物体就是Text,拿到每个子text,
        for (int i = 0; i < cube.transform.childCount; i++)
        {
            GameObject text = cube.transform.GetChild(i).gameObject;
            //text.GetComponent<TextMesh>().text = "";
            //print((i+4)%6+" "+text.name);
            //(i+4)%6,getChild的顺序是Text5,6,1,2,3,4,这里将下表正确Hash到新的text3DGroup中
            text3DGroup[(i+4)%6] = text;
        }
    }
}
