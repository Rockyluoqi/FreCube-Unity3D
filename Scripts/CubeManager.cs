using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

struct Point3D
{
    public int x;
    public int y;
    public int z;
}

public class CubeManager : MonoBehaviour {

    //--------------------------------------------------------管理所有的Cube和Axis  GameObject   该类现用于处理选中高亮效果

    private static List<GameObject> AllCubePre_xyz = new List<GameObject>();//-------------------------------------------------------------------------------------------
    List<int> AllCubePre_xyz_State = new List<int>();


    List<GameObject>[] xList = new List<GameObject>[4];                     //--------------------------------------------------------------------------------------------共三个轴,每个轴都有4个边
    List<GameObject>[] yList = new List<GameObject>[4];
    List<GameObject>[] zList = new List<GameObject>[4];




    //XYZ个轴的长度
    int Num_x;
    int Num_y;
    int Num_z;

    //三轴坐标状态,实际上代表12个边   鼠标 0 未在其上   1 鼠标在其上  2 选中  
    List<int> CoordSelectStateX = new List<int>();//鼠标 0 未在其上   1 鼠标在其上  2 选中  
    List<int> CoordSelectStateY = new List<int>();//鼠标 0 未在其上   1 鼠标在其上  2 选中  
    List<int> CoordSelectStateZ = new List<int>();//鼠标 0 未在其上   1 鼠标在其上  2 选中  

    public CoordManager CoordManager_;


    public MsgMouseFollow MouseFollow;


    int n_cur = -1, n_ori = -1;

    Point3D cur_mouse_on;

    /// <summary>  
    /// 当前视角的摄像机
    /// </summary>  
    public Camera _camPlayer;
    /// <summary>  
    /// 鼠标射线  
    /// </summary>  
    private Ray _ray;
    /// <summary>  
    /// 射线碰撞的结构  
    /// </summary>  
    private RaycastHit _rayhit;
    /// <summary>  
    /// 鼠标拾取的有效距离  
    /// </summary>  
    private float _fDistance = 20f;

	// Use this for initialization
	void Start () {

        cur_mouse_on.x = -1;

	}
	
	// Update is called once per frame
	void Update () {

        int on_mouse = 0;

        GameObject cur_cube;
        GameObject ori_cube;
        //if (Input.GetMouseButtonUp(0))
        {

            int x, y, z;
            int coord = -1;


            //鼠标的屏幕坐标空间位置转射线  
            _ray = _camPlayer.ScreenPointToRay(Input.mousePosition);

            for (int x_ = 0; x_ < Num_x; x_++)
            {
                //if (!(CoordSelectStateX[x_] == 1 && coord == 0 && x == x_) && CoordSelectStateX[x_] != 2)
                if (CoordSelectStateX[x_] != 2)
                {
                    ChangeCoordCubeCol(0, x_, 1, 5);
                    CoordSelectStateX[x_] = 0;

                }

            }

            for (int y_ = 0; y_ < Num_y; y_++)
            {
                //if (!(CoordSelectStateX[y_] == 1 && coord == 0 && y == y_) && CoordSelectStateY[y_] != 2)
                if (CoordSelectStateY[y_] != 2)
                {
                    ChangeCoordCubeCol(1, y_, 1, 5);
                    CoordSelectStateY[y_] = 0;
                }

            }

            for (int z_ = 0; z_ < Num_z; z_++)
            {
                //if (!(CoordSelectStateX[z_] == 1 && coord == 0 && z == z_) && CoordSelectStateZ[z_] != 2)
                if (CoordSelectStateZ[z_] != 2)
                {
                    ChangeCoordCubeCol(2, z_, 1, 5);
                    CoordSelectStateZ[z_] = 0;
                }

            }

            MouseFollow.show = 0;

            if (Physics.Raycast(_ray, out _rayhit))
            {

                x = (int)_rayhit.collider.gameObject.transform.position.x - 1;
                y = (int)_rayhit.collider.gameObject.transform.position.y - 1;
                z = (int)_rayhit.collider.gameObject.transform.position.z - 1;


                


                if (_rayhit.collider.gameObject.name == "CubePre(Clone)")
                {

                    //Debug.Log(_rayhit.collider.gameObject.transform.position.x + "x");
                    //Debug.Log(_rayhit.collider.gameObject.transform.position.y + "y");
                   // Debug.Log(_rayhit.collider.gameObject.transform.position.z + "z");
                    MouseFollow.show = 1;

                    //计算XYZ坐标并找到数组中的位置
                    
                    n_cur = x * Num_y * Num_z + y * Num_z + z;

                    

                    cur_cube = AllCubePre_xyz[n_cur];

                    MouseFollow.v_msg = cur_cube.transform.FindChild("Text1").GetComponent<TextMesh>().text;
                    MouseFollow.x_msg = xList[0][x].transform.FindChild("Text1").GetComponent<TextMesh>().text;
                    MouseFollow.y_msg = yList[0][y].transform.FindChild("Text1").GetComponent<TextMesh>().text;
                    MouseFollow.z_msg = zList[0][z].transform.FindChild("Text1").GetComponent<TextMesh>().text;


                    if (cur_mouse_on.x != -1)
                    {
                        n_ori = cur_mouse_on.x * Num_y * Num_z + cur_mouse_on.y * Num_z + cur_mouse_on.z;

                        if (n_ori != n_cur)
                        {
                            
                            ori_cube = AllCubePre_xyz[n_ori];
                            
                            ori_cube.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                            if (AllCubePre_xyz_State[n_ori] != 2)
                            {
                                ori_cube.transform.GetComponent<CubeBase>().ResetColor();
                            }

                        }
                        
                    }

                    cur_cube.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                    cur_cube.transform.GetComponent<CubeBase>().ChangeColor();
                    on_mouse = 1;

                    cur_mouse_on.x = x;
                    cur_mouse_on.y = y;
                    cur_mouse_on.z = z;

                    n_ori = n_cur;

                }



                if (_rayhit.collider.gameObject.name == "CoordCube(Clone)")
                {
                    
                    x = (int)_rayhit.collider.gameObject.transform.position.x;
                    y = (int)_rayhit.collider.gameObject.transform.position.y;
                    z = (int)_rayhit.collider.gameObject.transform.position.z;

                    

                    coord = GetCoord(x, y, z);

                    x--;
                    y--;
                    z--;
                    
                    

                        switch (coord)
                        {
                            case -1:
                                for (int x_ = 0; x_ < Num_x; x_++)
                                {
                                    if (CoordSelectStateX[x_] == 1)//鼠标在其上
                                    {
                                        CoordSelectStateX[x_] = 0;
                                        ChangeCoordCubeCol(0, x_, 1, 0);

                                    }
                                }
                                for (int y_ = 0; y_ < Num_y; y_++)
                                {
                                    if (CoordSelectStateY[y_] == 1)//鼠标在其上
                                    {
                                        CoordSelectStateY[y_] = 0;
                                        ChangeCoordCubeCol(1, y_, 1, 0);
                                    }
                                }
                                for (int z_ = 0; z_ < Num_z; z_++)
                                {
                                    if (CoordSelectStateZ[z_] == 1)//鼠标在其上
                                    {
                                        ChangeCoordCubeCol(2, z_, 1, 0);

                                    }
                                }
                                break;

                            case 0:
                            case 1:
                            case 2:
                                for (int x_ = 0; x_ < Num_x; x_++)
                                {

                                    if (x == x_)//鼠标在其上
                                    {
                                        if (CoordSelectStateX[x_] == 2)
                                        {
                                            if (Input.GetMouseButtonDown(0))
                                            {
                                                //Debug.Log(x_);


                                                ChangeCoordCubeCol(0, x_, 1, 4);
                                                CoordSelectStateX[x_] = 0;
                                            }
                                            
                                            //ChangeCoordCubeCol(0, x_, 0, Input.GetMouseButtonDown(0) ? 2 : 4);
                                        }
                                        else
                                        {
                                            if(Input.GetMouseButtonDown(0))
                                            {
                                                //Debug.Log(x_ + "asd" + CoordSelectStateX[x_]);
                                                if (CoordSelectStateX[x_] == 2)
                                                {
                                                    ChangeCoordCubeCol(0, x_, 1, 4);
                                                    CoordSelectStateX[x_] = 0;
                                                }
                                                else
                                                {
                                                    //Debug.Log(x_ + "asd" + CoordSelectStateX[x_]);
                                                    ChangeCoordCubeCol(0, x_, 0, 2);
                                                    CoordSelectStateX[x_] = 2;
                                                }
                                                
                                            }
                                            else
                                            {
                                                ChangeCoordCubeCol(0, x_, 0, 1);
                                                CoordSelectStateX[x_] = 1;
                                            }
                                            
                                            /*if (Input.GetMouseButtonDown(0))
                                            {
                                                Debug.Log(x_);

                                            }*/
                                        }

                                    }
                                    else
                                    {
                                        if (CoordSelectStateX[x_] != 2)
                                        {
                                            ChangeCoordCubeCol(0, x_, 1, 0);
                                            CoordSelectStateX[x_] = 0;
                                        }
                                        
                                    }
                                }
                                for (int y_ = 0; y_ < Num_y; y_++)
                                {
                                    if (y == y_)//鼠标在其上
                                    {
                                        if (CoordSelectStateY[y_] == 2)
                                        {
                                            if (Input.GetMouseButtonDown(0))
                                            {
                                                ChangeCoordCubeCol(1, y_, 1, 4);
                                                CoordSelectStateY[y_] = 0;
                                            }
                                            //ChangeCoordCubeCol(1, y_, 0, Input.GetMouseButtonDown(0) ? 2 : 4);
                                        }
                                        else
                                        {
                                            if (Input.GetMouseButtonDown(0))
                                            {
                                                if (CoordSelectStateY[y_] == 2)
                                                {
                                                    ChangeCoordCubeCol(1, y_, 1, 4);
                                                    CoordSelectStateY[y_] = 0;
                                                }
                                                else
                                                {
                                                    ChangeCoordCubeCol(1, y_, 0, 2);
                                                    CoordSelectStateY[y_] = 2;
                                                }
                                            }
                                            else
                                            {
                                                ChangeCoordCubeCol(1, y_, 0, 1);
                                                CoordSelectStateY[y_] = 1;
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (CoordSelectStateY[y_] != 2)
                                        {
                                            ChangeCoordCubeCol(1, y_, 1, 0);
                                            CoordSelectStateY[y_] = 0;
                                        }
                                    }
                                }
                                for (int z_ = 0; z_ < Num_z; z_++)
                                {
                                    if (z == z_)
                                    {
                                        if (CoordSelectStateZ[z_] == 2)
                                        {
                                            if (Input.GetMouseButtonDown(0))
                                            {
                                                ChangeCoordCubeCol(2, z_, 1, 4);
                                                CoordSelectStateZ[z_] = 0;
                                            }
                                            //ChangeCoordCubeCol(2, z_, 0, Input.GetMouseButtonDown(0) ? 2 : 4);
                                        }
                                        else
                                        {
                                            if (Input.GetMouseButtonDown(0))
                                            {
                                                if (CoordSelectStateZ[z_] == 2)
                                                {
                                                    ChangeCoordCubeCol(2, z_, 1, 4);
                                                    CoordSelectStateZ[z_] = 0;
                                                }
                                                else
                                                {
                                                    ChangeCoordCubeCol(2, z_, 0, 2);
                                                    CoordSelectStateZ[z_] = 2;
                                                }
                                            }
                                            else
                                            {
                                                ChangeCoordCubeCol(2, z_, 0, 1);
                                                CoordSelectStateZ[z_] = 1;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (CoordSelectStateZ[z_] != 2)
                                        {
                                            ChangeCoordCubeCol(2, z_, 1, 0);
                                            CoordSelectStateZ[z_] = 0;
                                        }
                                    }
                                }
                                break;





                        }


                }


            }

            //鼠标不在方块上的时候,还原上个块
            if (on_mouse == 0 && n_ori!=-1)
            {
                
                ori_cube = AllCubePre_xyz[n_ori];

                ori_cube.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);

                n_ori = -1;
            }


        }

	}

    //设置欲进行操作的Cube集合,
    public void SetCubesData(List<GameObject> cubes, int num_x_, int num_y_, int num_z_)
    {
        n_ori = -1;
        n_cur = -1;

        cur_mouse_on.x = -1;
        cur_mouse_on.y = -1;
        cur_mouse_on.z = -1;


        AllCubePre_xyz = cubes;
        
        Num_x = num_x_;
        Num_y = num_y_;
        Num_z = num_z_;

        CoordManager_.SetCoordNum(num_x_,num_y_,num_z_);


        /*List<int> CoordSelectStateX = new List<int>();//鼠标 0 未在其上   1 鼠标在其上  2 选中  
        List<int> CoordSelectStateY = new List<int>();//鼠标 0 未在其上   1 鼠标在其上  2 选中  
        List<int> CoordSelectStateZ = new List<int>();//鼠标 0 未在其上   1 鼠标在其上  2 选中  
        */
        int x,y,z,n;
        CoordSelectStateX.Clear();
        CoordSelectStateY.Clear();
        CoordSelectStateZ.Clear();
        AllCubePre_xyz_State.Clear();

        for (x = 0; x < Num_x; x++)
        {
            CoordSelectStateX.Add(0);
        }
        for (y = 0; y < Num_y; y++)
        {
            CoordSelectStateY.Add(0);
        }
        for (z = 0; z < Num_z; z++)
        {
            CoordSelectStateZ.Add(0);
        }

        for (n = 0; n < Num_x * Num_y*Num_z; n++)
        {
            AllCubePre_xyz_State.Add(0);
        }

    }



    //设置坐标轴的集合
    public void SetCoordsData(List<GameObject>[] coord_x, List<GameObject>[] coord_y, List<GameObject>[] coord_z)
    {
        /*for (int i = 0; i < 4; i++)
        {
            CoordSelectStateX.Clear();
            CoordSelectStateY.Clear();
            CoordSelectStateZ.Clear();
        }*/


        xList = coord_x;
        yList = coord_y;
        zList = coord_z;

        Num_x = coord_x[0].Count;
        Num_y = coord_y[0].Count;
        Num_z = coord_z[0].Count;
    }

    // 返回 0 X轴    1 Y轴    2 Z轴
    int GetCoord(int x,int y,int z)
    {
        if ((y == 0 && z == 0) || (y == 0 && z == Num_z + 1) || (y == Num_y + 1 && z == 0) || (y == Num_y + 1 && z == Num_z + 1))
        {
            return 0;
        }
        else if ((x == 0 && z == 0) || (x == 0 && z == Num_z + 1) || (x == Num_x + 1 && z == 0) || (x == Num_x + 1 && z == Num_z + 1))
        {
            return 1;
        }
        else if ((x == 0 && y == 0) || (x == 0 && y == Num_y + 1) || (x == Num_x + 1 && y == 0) || (x == Num_x + 1 && y == Num_y + 1))
        {
            return 2;
        }
        return -1;
        
    }

    void ChangeCoordCubeCol(int coord_, int n, int Reset, int state)//state 鼠标 0 未在其上   1 鼠标在其上  2 选中   4 取消选中   5 取消在其上
    {
        int cur_n;
        int cur_x, cur_y, cur_z;
        int x_, y_, z_;
        GameObject cur_cube;

        switch (coord_)
        {
            case 0:
                cur_x = n;
                cur_y = 0;
                cur_z = 0;
                for (y_ = 0; y_ < Num_y; y_++)
                {
                    for (z_ = 0; z_ < Num_z; z_++)
                    {
                        cur_y = y_;
                        cur_z = z_;
                        cur_n = cur_x * Num_y * Num_z + cur_y * Num_z + cur_z;
                        
                        if (AllCubePre_xyz[cur_n] == null) { continue; }
                        cur_cube = AllCubePre_xyz[cur_n];

                        if (Reset == 0)
                        {
                            cur_cube.GetComponent<CubeBase>().ChangeColor();
                            if (state == 2)
                            {
                                AllCubePre_xyz_State[cur_n] = 2;
                            }
                            else
                            {
                                if(AllCubePre_xyz_State[cur_n] != 2)
                                    AllCubePre_xyz_State[cur_n] = 1;
                            }
                            


                            
                        }
                        else
                        {
                            if (state == 4)
                            {
                                if (CoordSelectStateY[y_] != 2 && CoordSelectStateZ[z_] != 2)
                                {
                                    AllCubePre_xyz_State[cur_n] = 0;
                                    cur_cube.GetComponent<CubeBase>().ResetColor();
                                }
                                continue;
                            }
                            else if (state == 5 && AllCubePre_xyz_State[cur_n] == 1)
                            {
                                AllCubePre_xyz_State[cur_n] = 0;
                                cur_cube.GetComponent<CubeBase>().ResetColor();
                                continue;
                            }


                            if (AllCubePre_xyz_State[cur_n] == 2 || AllCubePre_xyz_State[cur_n]==1)
                            {
                                continue;
                            }

                            cur_cube.GetComponent<CubeBase>().ResetColor();
                        }
                    }
                }

                break;
            case 1:
                cur_x = 0;
                cur_y = n;
                cur_z = 0;
                for (x_ = 0; x_ < Num_x; x_++)
                {
                    for (z_ = 0; z_ < Num_z; z_++)
                    {
                        cur_x = x_;
                        cur_z = z_;
                        cur_n = cur_x * Num_y * Num_z + cur_y * Num_z + cur_z;
                        
                        if (AllCubePre_xyz[cur_n] == null) { continue; }
                        cur_cube = AllCubePre_xyz[cur_n];
                        
                        if (Reset == 0)
                        {
                            cur_cube.GetComponent<CubeBase>().ChangeColor();
                            if (state == 2)
                            {
                                AllCubePre_xyz_State[cur_n] = 2;
                            }
                            else
                            {
                                if (AllCubePre_xyz_State[cur_n] != 2)
                                    AllCubePre_xyz_State[cur_n] = 1;
                            }
                        }
                        else
                        {
                            if (state == 4)
                            {
                                if (CoordSelectStateX[x_] != 2 && CoordSelectStateZ[z_] != 2)
                                {
                                    AllCubePre_xyz_State[cur_n] = 0;
                                    cur_cube.GetComponent<CubeBase>().ResetColor();
                                }
                                continue;
                            }
                            else if (state == 5 && AllCubePre_xyz_State[cur_n] == 1)
                            {
                                AllCubePre_xyz_State[cur_n] = 0;
                                cur_cube.GetComponent<CubeBase>().ResetColor();
                                continue;
                            }

                            if (AllCubePre_xyz_State[cur_n] == 2 || AllCubePre_xyz_State[cur_n] == 1)
                            {
                                continue;
                            }
                            cur_cube.GetComponent<CubeBase>().ResetColor();
                        }
                    }
                    
                }
                break;
            case 2:
                cur_x = 0;
                cur_y = 0;
                cur_z = n;
                
                for (x_ = 0; x_ < Num_x; x_++)
                {
                    for (y_ = 0; y_ < Num_y; y_++)
                    {
                        cur_x = x_;
                        cur_y = y_;
                        
                        cur_n = cur_x * Num_y * Num_z + cur_y * Num_z + cur_z;

                        if (AllCubePre_xyz[cur_n] == null) { continue; }
                        cur_cube = AllCubePre_xyz[cur_n];

                        if (Reset == 0)
                        {
                            cur_cube.GetComponent<CubeBase>().ChangeColor();
                            if (state == 2)
                            {
                                AllCubePre_xyz_State[cur_n] = 2;
                            }
                            else
                            {
                                if (AllCubePre_xyz_State[cur_n] != 2)
                                     AllCubePre_xyz_State[cur_n] = 1;
                            }
                        }
                        else
                        {
                            if (state == 4)
                            {
                                if (CoordSelectStateY[y_] != 2 && CoordSelectStateX[x_] != 2)
                                {
                                    AllCubePre_xyz_State[cur_n] = 0;
                                    cur_cube.GetComponent<CubeBase>().ResetColor();
                                }
                                continue;
                            }
                            else if (state == 5 && AllCubePre_xyz_State[cur_n] == 1)
                            {
                                AllCubePre_xyz_State[cur_n] = 0;
                                cur_cube.GetComponent<CubeBase>().ResetColor();
                                continue;
                            }


                            if (AllCubePre_xyz_State[cur_n] == 2 || AllCubePre_xyz_State[cur_n] == 1)
                            {
                                continue;
                            }
                            cur_cube.GetComponent<CubeBase>().ResetColor();
                        }
                    }
                }
                
                break;


        }

 
    }

    public List<String> GetCutString()
    {
        List<String> ret = new List<string>();


        for (int i = 0; i < xList[0].Count;i++ )
        {
            if(CoordSelectStateX[i] == 2)
                ret.Add(xList[0][i].transform.FindChild("Text1").GetComponent<TextMesh>().text);

        }
        for (int i = 0; i < yList[0].Count;i++ )
        {
            if (CoordSelectStateY[i] == 2)
                ret.Add(yList[0][i].transform.FindChild("Text1").GetComponent<TextMesh>().text);

        }
        for (int i = 0; i < zList[0].Count;i++ )
        {
            if (CoordSelectStateZ[i] == 2)
                ret.Add(zList[0][i].transform.FindChild("Text1").GetComponent<TextMesh>().text);

        }
        return ret;

    }

    public string[,] GetTableData(int direction)
    {
        int cur_n;
        string[,] ret;

        switch (direction)
        {
            case 0:
                ret = new string[Num_y+1, Num_z+1];
                for (int i = 0; i < Num_y; i++)
                {
                    for (int j = 0; j < Num_z; j++)
                    {
                        cur_n = (Num_x-1) * Num_y * Num_z + i * Num_z + j;
                        ret[i + 1, j + 1] = AllCubePre_xyz[cur_n].transform.FindChild("Text1").GetComponent<TextMesh>().text;

                    }
                }
                for (int i = 0; i < Num_y; i++)
                {
                    ret[i + 1,0] = yList[0][i].transform.FindChild("Text1").GetComponent<TextMesh>().text;

                }
                for (int j = 0;j < Num_z; j++)
                {
                    ret[0, j+1] = zList[0][j].transform.FindChild("Text1").GetComponent<TextMesh>().text;

                }

                return ret;
                
            case 3:
                ret = new string[Num_y+1, Num_z+1];
                for (int i = 0; i < Num_y; i++)
                {
                    for (int j = 0; j < Num_z; j++)
                    {
                        cur_n =0 * Num_y * Num_z + i * Num_z + j;
                        ret[i + 1, j + 1] = AllCubePre_xyz[cur_n].transform.FindChild("Text1").GetComponent<TextMesh>().text;

                    }
                }
                for (int i = 0; i < Num_y; i++)
                {
                    ret[i + 1,0] = yList[0][i].transform.FindChild("Text1").GetComponent<TextMesh>().text;

                }
                for (int j = 0;j < Num_z; j++)
                {
                    ret[0, j+1] = zList[0][j].transform.FindChild("Text1").GetComponent<TextMesh>().text;

                }
                return ret;

               
            case 1:
                ret = new string[Num_z+1, Num_x+1];
                for (int i = 0; i < Num_z; i++)
                {
                    for (int j = 0; j < Num_x; j++)
                    {
                        cur_n = j * Num_y * Num_z + (Num_y-1) * Num_z + i;
                        ret[i + 1, j + 1] = AllCubePre_xyz[cur_n].transform.FindChild("Text1").GetComponent<TextMesh>().text;

                    }
                }
                for (int i = 0; i < Num_z; i++)
                {
                    ret[i + 1,0] = zList[0][i].transform.FindChild("Text1").GetComponent<TextMesh>().text;

                }
                for (int j = 0;j < Num_x; j++)
                {
                    ret[0, j+1] = xList[0][j].transform.FindChild("Text1").GetComponent<TextMesh>().text;

                }
                return ret;

               
            case 4:
                ret = new string[Num_z+1, Num_x+1];
                for (int i = 0; i < Num_z; i++)
                {
                    for (int j = 0; j < Num_x; j++)
                    {
                        cur_n = j * Num_y * Num_z + 0 * Num_z + i;
                        ret[i + 1, j + 1] = AllCubePre_xyz[cur_n].transform.FindChild("Text1").GetComponent<TextMesh>().text;

                    }
                }
                for (int i = 0; i < Num_z; i++)
                {
                    ret[i + 1,0] = zList[0][i].transform.FindChild("Text1").GetComponent<TextMesh>().text;

                }
                for (int j = 0;j < Num_x; j++)
                {
                    ret[0, j+1] = xList[0][j].transform.FindChild("Text1").GetComponent<TextMesh>().text;

                }
                return ret;

                
            case 2:
                ret = new string[Num_y+1, Num_x+1];
                for (int i = 0; i < Num_y; i++)
                {
                    for (int j = 0; j < Num_x; j++)
                    {
                        cur_n = j * Num_y * Num_z + i * Num_z + Num_z-1;
                        ret[i + 1, j + 1] = AllCubePre_xyz[cur_n].transform.FindChild("Text1").GetComponent<TextMesh>().text;

                    }
                }
                for (int i = 0; i < Num_y; i++)
                {
                    ret[i + 1,0] = yList[0][i].transform.FindChild("Text1").GetComponent<TextMesh>().text;

                }
                for (int j = 0;j < Num_x; j++)
                {
                    ret[0, j+1] = xList[0][j].transform.FindChild("Text1").GetComponent<TextMesh>().text;

                }
                return ret;

                
            case 5:
                ret = new string[Num_y+1, Num_x+1];
                
                for (int i = 0; i < Num_y; i++)
                {
                    for (int j = 0; j < Num_x; j++)
                    {
                        cur_n = j * Num_y * Num_z + i * Num_z + 0;
                        ret[i + 1, j + 1] = AllCubePre_xyz[cur_n].transform.FindChild("Text1").GetComponent<TextMesh>().text;

                    }
                }
                for (int i = 0; i < Num_y; i++)
                {
                    ret[i + 1,0] = yList[0][i].transform.FindChild("Text1").GetComponent<TextMesh>().text;

                }
                for (int j = 0;j < Num_x; j++)
                {
                    ret[0, j+1] = xList[0][j].transform.FindChild("Text1").GetComponent<TextMesh>().text;

                }
                return ret;


        }
        ret = null;

        return ret;
    }


}
