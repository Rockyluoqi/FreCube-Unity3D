using UnityEngine;
using System.Collections;

public class CoordManager : MonoBehaviour
{

    string[] x_Message = new string[3];
    string[] y_Message = new string[3];
    string[] z_Message = new string[3];

    public GameObject x0;
    public GameObject x1;
    public GameObject x2;

    public GameObject y0;
    public GameObject y1;
    public GameObject y2;

    public GameObject z0;
    public GameObject z1;
    public GameObject z2;

    public GameObject x_coord;
    public GameObject y_coord;
    public GameObject z_coord;

    public GameObject TableX;
    public GameObject TableY;

    public GameObject TableMsg;

    public int[] XYZ_Num = new int[3];

    public int Cur_direction;

    public int tableXn,tableYn;

    public ShowTable showtable;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetCoordMessage(string[] x, string[] y, string[] z)
    {
        for (int i = 0; i < 3; i++)
        {
            x_Message[i] = x[i];
            y_Message[i] = y[i];
            z_Message[i] = z[i];
        }
        x0.transform.GetComponent<UILabel>().text = x[0];
        x1.transform.GetComponent<UILabel>().text = x[1];
        x2.transform.GetComponent<UILabel>().text = x[2];
        y0.transform.GetComponent<UILabel>().text = y[0];
        y1.transform.GetComponent<UILabel>().text = y[1];
        y2.transform.GetComponent<UILabel>().text = y[2];
        z0.transform.GetComponent<UILabel>().text = z[0];
        z1.transform.GetComponent<UILabel>().text = z[1];
        z2.transform.GetComponent<UILabel>().text = z[2];

        x_coord.transform.GetComponent<TextMesh>().text = x[1];
        y_coord.transform.GetComponent<TextMesh>().text = y[1];
        z_coord.transform.GetComponent<TextMesh>().text = z[1];
    }

    public void SetTableXY(string x, string y)
    {

        TableX.transform.GetComponent<UILabel>().text = x;
        TableY.transform.GetComponent<UILabel>().text = y;
        showtable.SetTableAxis(x, y, 0, 0);

    }

    public void SetCoordNum(int x, int y, int z)
    {
       
        XYZ_Num[0] = x;
        XYZ_Num[1] = y;
        XYZ_Num[2] = z;
    }


    public void SetTableXY_int(int n)
    {
        Cur_direction = n;

        switch (n)
        {
            case 0:
            case 3:
                tableXn = XYZ_Num[2];
                tableYn = XYZ_Num[1];

                SetTableXY(z_Message[1], y_Message[1]);
                TableMsg.GetComponent<UILabel>().text = XYZ_Num[2].ToString() + "X" + XYZ_Num[1].ToString();

                break;
            case 1:
            case 4:
                tableXn = XYZ_Num[0];
                tableYn = XYZ_Num[2];

                SetTableXY(x_Message[1], z_Message[1]);
                TableMsg.GetComponent<UILabel>().text = XYZ_Num[0].ToString() + "X" + XYZ_Num[2].ToString();
                break;
            case 2:
            case 5:
                tableXn = XYZ_Num[0];
                tableYn = XYZ_Num[1];

                SetTableXY(x_Message[1], y_Message[1]);
                TableMsg.GetComponent<UILabel>().text = XYZ_Num[0].ToString() + "X" + XYZ_Num[1].ToString();
                break;

        }
    }

    public int GetCurDirection()
    {
        return Cur_direction;

    }

    public int GetTableXn()
    {
        return tableXn;

    }

    public int GetTableYn()
    {
        return tableYn;

    }
}
