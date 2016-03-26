using UnityEngine;
using System.Collections;

public class MsgMouseFollow : MonoBehaviour {

    public int show;

    public string x_msg;
    public string y_msg;
    public string z_msg;
    public string v_msg;



    public Texture tex;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {



	}
    void OnGUI()
    {
        int maxLen = 0;

        if(show ==1)
        {
            if (x_msg.Length > maxLen)
                maxLen = x_msg.Length;
            if (y_msg.Length > maxLen)
                maxLen = y_msg.Length;
            if (z_msg.Length > maxLen)
                maxLen = z_msg.Length;
            if (v_msg.Length > maxLen)
                maxLen = v_msg.Length;



            GUI.DrawTexture(new Rect(Input.mousePosition.x, Screen.height - Input.mousePosition.y, (float)maxLen*16.0f, 80.0f), tex);
            GUI.Label(new Rect(Input.mousePosition.x + 10, Screen.height - Input.mousePosition.y + 1, (float)maxLen * 16.0f, 80.0f), x_msg);
            GUI.Label(new Rect(Input.mousePosition.x + 10, Screen.height - Input.mousePosition.y + 21, (float)maxLen * 16.0f, 80.0f), y_msg);
            GUI.Label(new Rect(Input.mousePosition.x + 10, Screen.height - Input.mousePosition.y + 41, (float)maxLen * 16.0f, 80.0f), z_msg);
            GUI.Label(new Rect(Input.mousePosition.x + 10, Screen.height - Input.mousePosition.y + 61, (float)maxLen * 16.0f, 80.0f), v_msg);


        }
            

    }



}
