using UnityEngine;
using System.Collections;

// Simple Editor Script that fills a bar in the given seconds.
public class ProgressBar : MonoBehaviour
{
    float fadeOutTime = 3.0f;
    float time = 0f;
    float index = 1.0f;
    bool _lock = false;
    void Start()
    {
        //StartCoroutine(RadialProgress(fadeOutTime));
        //StopCoroutine(RadialProgress(10f));
    }
    
    void Update()
    {
        //使用这两行代码，实现顺时针Circular ProgressBar旋转
        float revealOffset = 1 - (float)(Time.timeSinceLevelLoad % 1) / 1.1f;
        //插值优化闪烁效果
        gameObject.renderer.material.SetFloat("_Cutoff", Mathf.InverseLerp(0,1.05f,revealOffset));
    }
    //IEnumerator RadialProgress(float time)
    //{
    //    float rate = 1 / time;
    //    float i = 1;
    //    while (i > 0)
    //    {
    //        i -= Time.deltaTime * rate;
    //        renderer.material.SetFloat("_Cutoff", i);
    //        yield return 0;
    //    }
    //}
}