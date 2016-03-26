using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour
{
    //背景贴图
    public Texture2D bgTexture;
    // Use this for initialization
    void Start()
    {
        gameObject.AddComponent<GUITexture>();
    }

    // Update is called once per frame
    void Update()
    {
      //  GetComponent<GUITexture>().texture = bgTexture;
      //  transform.localScale = new Vector3(0, 0, 0);
      //  transform.position = new Vector3(0, 0, 0);
      //  GetComponent<GUITexture>().pixelInset = new Rect(0, 0, Screen.width, Screen.height);
    }
}