using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/// <summary>
/// 8.5增加了选择的功能，并且实现了单选坐标和取消
/// 在单选操作中取消的方式，开始想有两种，一种是选中当前的坐标会选中，但是无法实现选择其他的坐标的同时恢复原状，不得不点击两次才能将原坐标恢复原状
/// 最后采取的这种方法是点击非坐标物体时就取消当前选中物体，这种实现最后看来也是最为合理的一种方法，所以选择了这种实现，具体的实现在MousePickOnePoint()中
/// 
/// 8.6成功实现切片功能
/// </summary>
public class CubeSelection : MonoBehaviour {

    //使用显示轮廓的简单材质
    public Material mSimpleMat;
    //使用显示轮廓的高级材质
    public Material mAdvanceMat;
    //默认材质
    public Material mDefaultMat;

    //点击取消flag
    private static bool _Click = true;
    //点击次数计数
    private static int _ClickCount = 0;
    //判断切片切块的请求是否有效
    private bool _IsValid = true;
    //点击Go取消
    private bool buttonIsClicked = false;
    private bool goButton = false;

    //所有的当前CoordPoint GameObject的集合
    private List<GameObject> AllCoordObjects;

    public GameObject ActionGameObject;

    public GameObject InvalidSelection;

    public GameObject ProgressBar;

    //public CubeManager CubeManger_;

    void Update()
    {
        MousePick();
    }

    /// <summary>
    /// 手机触摸选中
    /// </summary>
    void MobilePick()
    {
        if (Input.touchCount != 1)
            return;

        if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.name);
                //Debug.Log(hit.transform.tag);  
            }
        }
    }

    /// <summary>
    /// 鼠标选中,选中更换材质
    /// </summary>
    void MousePick()
    {
        if (Input.GetMouseButtonUp(0))
        {
            //碰撞射线检测
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.name);

                if (hit.transform.name.Equals("CoordCube(Clone)"))
                {
                    Debug.Log(hit.collider.gameObject.renderer.material.name);
                    foreach (var gameObject in AllCoordObjects)
                    {
                        if(hit.transform.gameObject == gameObject)
                        {
                            //如果这个Cube已经被选中再次点击将取消选中
                            if (gameObject.renderer.material.name.Equals("FlareParticleMaterial (Instance)"))
                            {
                                print("replace to DefalutMaterial");
                                gameObject.renderer.material = mDefaultMat;
                                _Click = false;
                            }
                            if (_Click)
                            {
                                gameObject.renderer.material = mSimpleMat;
                            }
                            _Click = true;
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 坐标轴点的单选
    /// 实现细节，使用了一个新的List<GameObject>来装入所有的CoordCube的GameObject，通过foreach中的判断实现了原子操作
    /// </summary>
    void MousePickOnePoint()
    {
        if(Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit))
            {
                //foreach实现单选
                foreach (var gameobject in AllCoordObjects)
                {
                    if (hit.transform.name.Equals("CoordCube(Clone)"))
                    {
                        if (hit.transform.gameObject == gameobject)
                        {
                            gameobject.renderer.material = mSimpleMat;
                        }
                        else
                        {
                            gameobject.renderer.material = mDefaultMat;
                        }
                    }
                    //如果点击的不是CoordCube则取消当前的选中
                    else
                    {
                        gameobject.renderer.material = mDefaultMat;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 从其他类中获得当前坐标集合
    /// </summary>
    /// <param name="coords"></param>
    public void GetAllCoordObject(List<GameObject> coords)
    {
        AllCoordObjects = coords;
    }

    /// <summary>
    /// Reset all coordinates to default material.
    /// 重置所有的材质到初始材质
    /// </summary>
    public void ResetAllSelection()
    {
        if (AllCoordObjects != null)
        {
            foreach (var coord in AllCoordObjects)
            {
                coord.renderer.material = mDefaultMat;
            }
        }
    }
    
    /// <summary>
    /// 判断选中操作是否合法，合法则调用数据中转函数
    /// </summary>
    public void SelectionIsValid()
    {
        ProgressBar.SetActive(true);
        List<string> coords = new List<string>();
        int coordsCount = 0;

        string singleSelectionPoint = "";

        //遍历所有的坐标GameObject，通过材质比较判断是否被选中，选中则将该点的text放入 
        foreach (var coord in AllCoordObjects)
        {
            if(coord.renderer.material.name.Equals("DrawersMaterial (Instance)"))
            {
                //_IsValid = false;
                continue;
            }
            if(coord.renderer.material.name.Equals("FlareParticleMaterial (Instance)"))
            {
                _IsValid = true;
                coordsCount++;
                singleSelectionPoint = coord.GetComponent<CubeBase>().GetText();
                coords.Add(singleSelectionPoint);
                //break;
            }
        }

        //Debug.Log(coordsCount);

        //如果一个都没被选中，则提交请求不合法
        if(coordsCount == 0)
        {
            _IsValid = false;
        }

        //if(coordsCount == 1)
        //{
        //    //SliceMain(singleSelectionPoint);
        //    DiceMain(coords);
        //}

        //选中的点大于等于1则请求合法
        if(coordsCount >= 1)
        {
            DiceMain(coords);
        }

        //Debug.Log("Isvalid");
        if(_IsValid == false)
        {
            InvalidSelection.SetActive(true);
            InvalidSelection.GetComponent<TweenAlpha>().PlayForward();
            //Debug.Log("_Isvalid is false");
        }
    }

    /// <summary>
    /// 这就是切片操作的主处理类，只选中一个坐标就是切块操作
    /// </summary>
    private void SliceMain(string coordName)
    {
        ActionGameObject.GetComponent<DataHandlerTest>().CreateSliceData(coordName);
    }

    /// <summary>
    /// 这是切块的主函数，选中三维坐标后，生成一个一般意义上的Cube
    /// </summary>
    private void DiceMain(List<string> coords)
    {
        ActionGameObject.GetComponent<DataHandlerTest>().CreateDiceData(coords);

    }

    public void KnowButtonHandler()
    {
        //InvalidSelection.GetComponent<TweenAlpha>().PlayReverse();
        //InvalidSelection.SetActive(false);
    }

    /// <summary>
    /// 如果选择不合法就是指没有选中任何的坐标点将出现，之后会做的更漂亮
    /// </summary>
    //void OnGUI()
    //{
    //    if (!goButton)
    //        return;

    //    if(_IsValid == false && goButton) 
    //    {
    //        Debug.Log("GUI is producing");
    //        GUI.Box(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 100, 250, 50), "Invalid request, pleast select again!");
    //        if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2, 80, 20), "Go"))
    //        {
    //            if (buttonIsClicked == false)
    //                buttonIsClicked = true;
    //            else
    //                buttonIsClicked = false;
    //        }

    //        if(buttonIsClicked == true)
    //        {
                
    //        }
    //    }
    //}
}
