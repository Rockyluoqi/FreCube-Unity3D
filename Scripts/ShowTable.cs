using UnityEngine;

using System.Collections;
using System.Collections.Generic;
public class ShowTable : MonoBehaviour {

    //***XY轴的信息
    string Xtext;
    string Ytext;

    public CubeManager CubeManager_;
    public CoordManager CoordManager_;


    int Width;
    int Height;

    public GameObject Table;

    public GameObject TableUnit;

    public ArrayList TableUnits = new ArrayList();

    int IsShow = 0;


    //数据集合

    List<string> Data = new List<string>();

    List<GameObject> tables = new List<GameObject>();


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    //设置两轴信息
    public void SetTableAxis(string Xtext_,string Ytext_, int width_, int height_)
    {

        Xtext = Xtext_;
        Ytext = Ytext_;
        //Width = width_;
        //Height = height_;
        //Debug.Log(Xtext + Ytext);

    }

    public void SetTableData(string[,] values_, int width_, int height_)
    {
        Width = width_;
        Height = height_;
        Data.Clear();
        for (int y = 0; y < height_; y++)
        {
            for (int x = 0; x < width_; x++)
            {
                if (x == 0 && y == height_ - 1)
                {
                    
                    Data.Add("         "+Xtext+"\r\n"+Ytext+"       ");
                }
                else
                {
                    Data.Add(values_[height_ - y - 1, x]);
                }
            }
        }
    }


    public void SetTableDataRand()
    {
        int width_=10;
        int height_=5;
        Width = width_;
        Height = height_;
        Data.Clear();
        for (int y = 0; y < height_; y++)
        {
            for (int x = 0; x < width_; x++)
            {
                Data.Add(((float)Random.Range(0,50000)).ToString());

            }
        }
        //Debug.Log(Data[0]);

    }

    public void Show()
    {
        if (IsShow == 1)
        {
            Hide();
            return;

        }
        SetTableData(CubeManager_.GetTableData(CoordManager_.GetCurDirection()), CoordManager_.GetTableXn()+1, CoordManager_.GetTableYn()+1);


        GameObject temp;

        TableUnits.Clear();

        int w,h;

        w=820/Width;
        h=460/Height;

        NGUITools.AddChild(Table);
        

        Table.transform.localPosition = new Vector3(31, 162, 0);

        for (int n_ = 0; n_ < tables.Count; n_++)
        {
            Destroy(tables[n_] as GameObject);

        }
        tables.Clear();

        Color col = new Color();

        col.r = 237.0f / 255.0f;
        col.g = 237.0f / 255.0f;
        col.b = 105.0f / 255.0f;
        col.a=1.0f;


        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                

                temp = Instantiate(TableUnit, new Vector3(x * w - 410 + w / 2, y * h - 230 + h / 2, 0), Quaternion.EulerAngles(0, 0, 0)) as GameObject;

                tables.Add(temp);

                temp.transform.SetParent(Table.transform, false);
                temp.transform.GetComponent<UISprite>().width = w;
                temp.transform.GetComponent<UISprite>().height = h;

                if (x == 0 || y == Height - 1)
                {
                    temp.transform.GetComponent<UISprite>().color = col;

                }

                UILabel a;
                a = temp.transform.FindChild("Label").GetComponent<UILabel>();
                a.text = (Data[y * Width + x]);


                TableUnits.Add(temp);


            }
        }

        IsShow = 1;

        
    }

    public void Hide()
    {
        IsShow = 0;

        NGUITools.AddChild(Table);


        Table.transform.localPosition = new Vector3(-1374, 162, 0);
    }





}
