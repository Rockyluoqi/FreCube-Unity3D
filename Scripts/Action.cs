using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
/// <summary>
/// 在场景中绘制数据Cube和坐标Cube的主类
/// </summary>
/**
 * 修改记录：
 * 7.22 增加Json接受读取功能
 * 8.1 增加删除方法，发送到MiniCoord控制
 * 8.6 切片及切块功能增加
 **/
public class Action : MonoBehaviour {


    //引用外部CubeManager 进行高亮现操作
    public CubeManager CubeManager_;

    public GameObject ProgressBar;

    //动态加载坐标选择GameObject而设
    public GameObject CubeSelection;

    //旋转锚点，设置了旋转的中心点
    public GameObject AxisPoint;

    //接收Cube prefab，核心的GameObject,显示数据
    public GameObject cube;

    //接收坐标 prefab，用来显示坐标数据
    public GameObject coord_cube;

    //接收传入的camera对象，这个用来使坐标的Text LookAt这个camera,也可以是其他的camera
    public Camera CameraToFollow;

    //***小坐标轴旋转用
    public Camera PositionerCam;
    public GameObject Positioner;

    //生成的每个cube GameObject的引用
    private GameObject obj;

    //List<CubeBase> cubeGroup;     初版使用的一个临时集合
    //一个临时测试的集合，用来显示cube的6各面每个面的方向
    //private Dictionary<int, List<GameObject>> cubeSurface;

    //坐标的核心集合，key:x,y,z三轴，List<Gameobject>每个轴上的点（GameObject）的集合
    private Dictionary<string, List<GameObject>[]> Coordinates;
    //private Dictionary<int, List<GameObject>> SubCoordinates;
    //private List<GameObject> CoordinatePoints;

    //初版测试
    //private Dictionary<string, List<GameObject>> EverySurfaceData;  

    //判断Esc键是否被按下的flag
    private bool EscIsPressed = false;

    //存储每个Cube的GameObject和它的真实数据
    private static Dictionary<Cube, GameObject> cubeInstanceSet = new Dictionary<Cube, GameObject>();

    //用来在本类中存储传来的Cube数据
    private static List<Cube> cubeDataSet = new List<Cube>();
    private static List<Cube> tempDataSet = new List<Cube>();

    //用来整体删除CubePre的集合
    private static List<GameObject> AllCubePre = new List<GameObject>();    //---------------------------------------------------------------------------------------------所有Cube的集合
    private static List<int> AllCubePre_x = new List<int>();    //---------------------------------------------------------------------------------------------所有Cube的坐标信息
    private static List<int> AllCubePre_y = new List<int>();
    private static List<int> AllCubePre_z = new List<int>();

    private static List<GameObject> AllCubePre_xyz = new List<GameObject>();

    //接受传入的每个Cube上的真实数据，其中包括了Cube的坐标和Cube的值
    public void SetCubeDataSet(List<Cube> cubeDatas)
    {
        //8.8加入空值检测，robustness and convienient debugging  
        if (cubeDatas.Count == 0)
        {
            Debug.Log("CubeDataSet is empty. Method:"+System.Reflection.MethodBase.GetCurrentMethod().Name);
        }
        cubeDataSet = cubeDatas;        
        //Debug
        //foreach(var item in cubeDataSet)
        //{
        //    //print(string.Format("value: {0} Dimension: {1} {2} {3}", item.value, item.dimension.x, item.dimension.y, item.dimension.z));
        //}    

        //11.1复制了新的临时FreCube 
		foreach (var item in cubeDataSet) {
			Cube cube = new Cube();
			cube.value = item.value;
			Dimension dim = new Dimension();
			dim.x = item.dimension.x;
			dim.y = item.dimension.y;
			dim.z = item.dimension.z;
			cube.dimension = dim;
			tempDataSet.Add(cube);
		}
	}

    //三维坐标字典
    private static Dictionary<int, string> x_dictionary = new Dictionary<int,string>();
    private static Dictionary<int, string> y_dictionary = new Dictionary<int, string>();
    private static Dictionary<int, string> z_dictionary = new Dictionary<int, string>();

    //定义3个List用来存储实际坐标中GameObject,方便批量操作修改这些GameObject//--------------------------------------------------------------------------------------------坐标轴GameObject
    List<GameObject>[] xList = new List<GameObject>[4];                     //--------------------------------------------------------------------------------------------共三个轴,每个轴都有4个边
    List<GameObject>[] yList = new List<GameObject>[4];
    List<GameObject>[] zList = new List<GameObject>[4];

    //用来获得排序好的x y z _dictionary中的第一个KeyValuePair的Key
    private static int x_firstIndex = 0;
    private static int y_firstIndex = 0;
    private static int z_firstIndex = 0;

    /// <summary>
    /// 设置X坐标属性，填充X坐标字典
    /// </summary>
    /// <param name="Dx"></param>
    public void SetX(Dictionary<int,string> Dx)
    {
        if (Dx.Count == 0)
        {
            Debug.Log("Dictionary X is empty. Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        }
        x_dictionary = Dx;
        //foreach (var item in x_dictionary)
        //{
        //    print(item.Key + " " + item.Value);
        //}

        //拿到字典中第一个坐标的Key,YZ同理
        foreach (var item in x_dictionary)
        {
            x_firstIndex = item.Key;
            break;
        }


        ////////////////////////////////////////////////////////
        //11.1新增，判断是否有跨行选择的情况，如果有则转入Repackage函数，对cubeData的坐标重新进行修改，y和z相同
        ///////////////////////////////////////////////////////
		int[] xkeys = new int[x_dictionary.Count];
		int i=0;
		
		foreach (var item in Dx) {
			xkeys[i] = item.Key;
			i++;
		}
		
		bool isAddOne = true;
		for (int k = 0; k < xkeys.Length-1; k++) {
			if(xkeys[k+1]-xkeys[k]>1)
			{
				isAddOne = false;
				break;
			}
		}
		
		if(!isAddOne)
		{
            //Debug.Log("X jump!!!");
			RepackageCubeData(true,false,false);
		}
    }

    /// <summary>
    /// 设置Y坐标属性，填充Y坐标字典
    /// </summary>
    /// <param name="Dy"></param>
    public void SetY(Dictionary<int, string> Dy)
    {
        if (Dy.Count == 0)
        {
            Debug.Log("Dictionary Y is empty. Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        }
        y_dictionary = Dy;
        
        foreach (var item in y_dictionary)
        {
            y_firstIndex = item.Key;
            break;
        }

		int[] ykeys = new int[y_dictionary.Count];
		int i=0;

		foreach (var item in Dy) {
			ykeys[i] = item.Key;
			i++;
		}

		bool isAddOne = true;
		for (int k = 0; k < ykeys.Length-1; k++) {
			if(ykeys[k+1]-ykeys[k]>1)
			{
				isAddOne = false;
				break;
			}
		}

		if(!isAddOne)
		{
            //Debug.Log("Y jump!!!");
			RepackageCubeData(false,true,false);
		}
		//repeat set datas again,repair datas' xyz value.

    }

	/// <summary>
	/// 设置Z坐标属性，填充Z坐标字典
	/// </summary>
	/// <param name="Dz"></param>
	public void SetZ(Dictionary<int, string> Dz)
	{
		if (Dz.Count == 0)
		{
			Debug.Log("Dictionary Z is empty. Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
		}
		z_dictionary = Dz;
		
		foreach (var item in z_dictionary)
		{
			z_firstIndex = item.Key;
			break;
		}
		
		int[] zkeys = new int[z_dictionary.Count];
		int i=0;
		
		foreach (var item in Dz) {
			zkeys[i] = item.Key;
			i++;
		}
		
		bool isAddOne = true;
		for (int k = 0; k < zkeys.Length-1; k++) {
			if(zkeys[k+1]-zkeys[k]>1)
			{
				isAddOne = false;
				break;
			}
		}
		
		if(!isAddOne)
		{
            //Debug.Log("Z jump!!!");
			RepackageCubeData(false,false,true);
		}
	}

    ////////////////////////////////////////////////////////
    //11.1新增，重新修改cubeData的dimension数据，修改的是CubeDataSet的一个拷贝，防止元数据的修改
    //算法核心，首先求出跳跃选择数组的间隔数组，即数组distanceBTWCoord，d[1]存储的就是它与firstIndex的差，d[2]就是x_dictionary中第二个元素坐标的（以X
    //轴为例）x减去第一个元素的x，以此类题，然后将这个距离数组逐项与xkeys数组相减，就得到了修改后的值，然后赋值给拷贝的tempDataSet
    //有一些小的bug，还需修改，大部分跳跃选择都可以正常支持了，之前没有考虑到跳跃的状况，失误
    ///////////////////////////////////////////////////////
	private void RepackageCubeData(bool xJump,bool yJump,bool zJump)
	{
		if(xJump) {
            //distanceBTW数组的大小是要大于等于 x y z坐标点的最大值，合理估计选取了100
            int[]distanceBtwXCoords = new int[1000];
			int[] xkeys = new int[x_dictionary.Count];
			int i=0;
			
			foreach (var item in x_dictionary) 
            {
				xkeys[i] = item.Key;
				i++;
			}
			distanceBtwXCoords[0] = 0;
            int sum = 0;

			for (int k = 1; k < xkeys.Length; k++) 
            {
				distanceBtwXCoords[k] = xkeys[k]-xkeys[k-1]-1;
                sum += distanceBtwXCoords[k];
                distanceBtwXCoords[k] = sum;
			}
			
            //for (int j = 0; j < distanceBtwXCoords.Length; j++) 
            //{
            //    print(distanceBtwXCoords[j]);
            //}

            //对dataset中的数据按照坐标的index重新按升序排序，防止之后的减距离出错
            tempDataSet = tempDataSet.OrderBy(x => int.Parse(x.dimension.x)).ToList<Cube>();
			
			i=0;
            foreach (var d in x_dictionary)
            {
			    foreach (var item in tempDataSet) 
                {
					if(int.Parse(item.dimension.x) == d.Key) 
                    {
                        if (distanceBtwXCoords[i] != 0)
                        {
                            item.dimension.x= (int.Parse(item.dimension.x) - distanceBtwXCoords[i]).ToString();
                        }
					}
				}
                        i++;

                if (i == 50)
                    break;
			}
		}

        //主测试了Y轴，包括测试代码
		if(yJump) {
			int[] distanceBtwYCoords = new int[1000];
			int[] ykeys = new int[y_dictionary.Count];
			int i=0;
			
			foreach (var item in y_dictionary) 
            {
				ykeys[i] = item.Key;
				i++;
			}
			distanceBtwYCoords[0] = 0;
            //要求和，distanceBtwCoords存储到第一个index的总距离 11.2改
            int sum = 0;
			for (int k = 1; k < ykeys.Length; k++) 
            {
				distanceBtwYCoords[k] = ykeys[k]-ykeys[k-1]-1;
                sum += distanceBtwYCoords[k];
                distanceBtwYCoords[k] = sum;
			}

            ////Debug
            ////for (int j = 0; j < distanceBtwYCoords.Length; j++)
            ////{
            //    print(distanceBtwYCoords[j]);
            //}

            //对dataset中的数据按照坐标的index重新按升序排序，防止之后的减距离出错
            tempDataSet = tempDataSet.OrderBy(x => int.Parse(x.dimension.y)).ToList<Cube>();
            

            //foreach (var item in tempDataSet)
            //{
            //    Debug.Log(item.value+" "+item.dimension.x + " " + item.dimension.y + " " + item.dimension.z);
            //}

            i = 0;
            string f;

            foreach (var d in y_dictionary)
            {
			    foreach (var item in tempDataSet) 
                {
					if(int.Parse(item.dimension.y) == d.Key) 
                    {
                        //如果选择的空快下方的块有数据则将数据从dataset中删除
                        //例如数据的y值是13  11  10，但是位于13和11之间有没有数据的块12就会导致11的数据写到生成的新块的12上去，就会出错
                        //所以有了如下的一些判断及重置代码
                        //-----------------GG
                        if (distanceBtwYCoords[i] != 0)
                        {
                            //f = (int.Parse(item.dimension.y)-1).ToString();
                            //foreach (var key2 in tempDataSet)
                            //{
                            //    if (f == key2.dimension.y)
                            //    {

                            //        key2.value = "";
                            //    }
                            //}
                            item.dimension.y = (int.Parse(item.dimension.y) - distanceBtwYCoords[i]).ToString();
                            //与空数据块一起选择将会非常麻烦，会有很多种数据覆盖的情况，遍历略蛋疼
                            //所以当前解决方案要求服务器将所有块的数据都填满，空数据也要填写空字符串，已经要求服务器端完成 11.2
                        }
					}
				}
                i++;

                if (i == 50)
                    break;
			}

            //foreach (var item in tempDataSet)
            //{
            //    Debug.Log(item.value + " " + item.dimension.x + " " + item.dimension.y + " " + item.dimension.z);
            //}

		}

		if(zJump) {
            int[] distanceBtwZCoords = new int[1000];
			int[] zkeys = new int[z_dictionary.Count];
			int i=0;
			
			foreach (var item in z_dictionary) 
            {
				zkeys[i] = item.Key;
				i++;
			}

			distanceBtwZCoords[0] = 0;
            int sum = 0;
			for (int k = 1; k < zkeys.Length; k++) 
            {
				distanceBtwZCoords[k] = zkeys[k]-zkeys[k-1]-1;
                sum += distanceBtwZCoords[k];
                distanceBtwZCoords[k] = sum;
			}
			
            //for (int j = 0; j < distanceBtwZCoords.Length; j++) 
            //{
            //    print(distanceBtwZCoords[j]);
            //}

            //对dataset中的数据按照坐标的index重新按非降序排序，防止之后的减距离出错
            tempDataSet = tempDataSet.OrderBy(x => int.Parse(x.dimension.z)).ToList<Cube>();

			i=0;
            foreach (var d in z_dictionary)
            {
			    foreach (var item in tempDataSet) 
                {
					if(int.Parse(item.dimension.z) == d.Key) 
                    {
                        if (distanceBtwZCoords[i] != 0)
                        {
                            item.dimension.z = (int.Parse(item.dimension.z) - distanceBtwZCoords[i]).ToString();
                        }
					}
				}
                i++;

                if (i == 50)
                    break;
			}
		}
	}
    
    

    //生成Cube的尺寸值，为了控制之后的生成循环
    private static int num_x; 
    private static int num_y;
    private static int num_z;

    /// <summary>
    /// 接受从json中传入的坐标的大小，并且赋值给本地变量
    /// </summary>
    private void SetSizeOfXYZ()
    {
        num_x = x_dictionary.Count;
        num_y = y_dictionary.Count;
        num_z = z_dictionary.Count;
        //Debug.Log(string.Format("XYZ size: x:{0} y:{1} z:{2}", num_x, num_y, num_z));
    }

    public int GetNum_x()
    {
        return num_x;
    }
    public int GetNum_y()
    {
        return num_y;
    }
    public int GetNum_z()
    {
        return num_z;
    }

    //8.2
    //这个函数的定义方便其他函数按需调用这个函数所有的生成方法，之前写在start()中只能第一次加载的时候记载一次
    public void AllCreate()
    {
        SetSizeOfXYZ();
        CreateCube(num_x, num_y, num_z);
    }

    /// <summary>
    /// 切片切块创建一块Cube
    /// </summary>
    public void CreateSliceAndDice()
    {
        SetSizeOfXYZ();
        CreateDiceCube(num_x, num_y, num_z);
    }

	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
        //if(Input.GetKeyDown(KeyCode.Escape))
        //{
        //    print("esc is pressed.");
        //    EscIsPressed = true;
        //}





	}
    
    /// <summary>
    /// 创建出每个cube,并且将每个Cube的GameObject装箱
    /// </summary>
    private void CreateCube(int size_x,int size_y,int size_z)
    {
        //起始边界
        int start = 1;

        //设置旋转的锚点
        AxisPoint.transform.position = new Vector3(size_x / 2, size_y / 2, size_z/ 2);

        GameObject EmptyGameObject = null;

        //三层循环生成新的数据Cube GameObject并且将数据写入
        for (int x = start; x <= size_x; x++)
        {
            for (int y = start; y <=size_y; y++)
            {
                for (int z = start; z <=size_z; z++)
                {
                    //空心FreCube显著提高运行速度，而且合理
                    if (x > start && x < size_x)
                    {
                        if (y > start && y < size_y)
                        {
                            if (z > start && z < size_z)
                            {
                                AllCubePre_xyz.Add(EmptyGameObject);

                                continue;
                            }
                        }
                    }
                    
                    //生成cubePre GameObject
                    obj = Instantiate(cube, new Vector3(x,y,z), cube.transform.rotation) as GameObject;
                    //obj.GetComponent<CubeBase>().SetText("   " + x.ToString() + " " + y.ToString() + " " + z.ToString());

                    //---------------------------------------------------------------------------------------------------------------------------------Cube的三维数据集合,中间的Cube是空的
                    AllCubePre_xyz.Add(obj);

                    //添加入集合中方便delete
                    AllCubePre.Add(obj);
                    
                    /*
                    AllCubePre_x.Add(x);
                    AllCubePre_y.Add(y);
                    AllCubePre_z.Add(z);
                    */


                    //以x轴为基准线，每个x轴的点分割一片cube,按照顺序填充
					foreach (var item in cubeDataSet)
					{
						//8.2 modified
						//int.Parse(item.dimension.y)-y_firstIndex+1这一句将dimension中的Index坐标从只能由1开始，变为了可以从任意值开始设置，增加了灵活性
                        if ((int.Parse(item.dimension.x) - x_firstIndex + 1) == x
                            && (int.Parse(item.dimension.y) - y_firstIndex + 1) == y
                            && (int.Parse(item.dimension.z) - z_firstIndex + 1) == z)
                        //if ((int.Parse(item.dimension.x)== x)
                        //    && (int.Parse(item.dimension.y)== y)
                        //    && (int.Parse(item.dimension.z) == z))
						{
							cubeInstanceSet.Add(item, obj);
						}
					}

                    //if (x <= size_x)
                    //{
                    //    EveryXCube[x - 1].Add(obj);
                    //}
                    
                    //设置每个Cube的name,just for testing.
                    //mCube.cubeName = (x.ToString() + y.ToString() + z.ToString());
                }
            }
        }


        //设置CubeManager
        CubeManager_.SetCubesData(AllCubePre_xyz, size_x, size_y, size_z);

        Camera.main.transform.GetComponent<MouseFollowRotation>().minDistance = Mathf.Sqrt(size_x * size_x + size_y * size_y + size_z * size_z)/1.9f;
        Camera.main.transform.GetComponent<MouseFollowRotation>().distance = Mathf.Sqrt(size_x * size_x + size_y * size_y + size_z * size_z) * 1.2f;
        Camera.main.transform.GetComponent<MouseFollowRotation>().maxDistance = Mathf.Sqrt(size_x * size_x + size_y * size_y + size_z * size_z)*2.5f;
        
        //加载cube数据
        foreach (var cubeInstance in cubeInstanceSet)
        {
            cubeInstance.Value.GetComponent<CubeBase>().SetText(cubeInstance.Key.value);
        }

        //创建完Cube后创建坐标(Coordinate)，当然这里耦合略强，但是有着一定必要性和合理性，之后会考虑改进逻辑
        CreateCoordinate(start - 1, size_x, size_y, size_z);

        ////测试填充方法，将会给每个面随机填充 xxx件的数据，方便显示
        //List<GameObject>[] EveryXCube = new List<GameObject>[size_x];
        //for (int i = 0; i < EveryXCube.Length; i++)
        //{
        //    EveryXCube[i] = new List<GameObject>();
        //} 

        //EverySurfaceData = new Dictionary<string, List<GameObject>>();

        //for (int i = start; i <= size_x; i++)
        //{
        //    EverySurfaceData.Add(i.ToString(), EveryXCube[i - 1]);
        //}

        //System.Random random = new System.Random();
        //int R;

        //foreach (var surface in EverySurfaceData)
        //{
        //    foreach (var item in surface.Value)
        //    {
        //        R = random.Next(0, 1000);
        //        item.GetComponent<CubeBase>().SetText(R.ToString()+" 件");
        //    }
        //}
    }

    /// <summary>
    /// 创建-切割后-的FreCube，大部分逻辑和上面的方法相同，减少耦合而将其重写
    /// </summary>
    private void CreateDiceCube(int size_x, int size_y, int size_z)
    {
        //起始边界
        int start = 1;
		int flag = 0;
        GameObject EmptyGameObject = null;


        AxisPoint.transform.position = new Vector3(size_x / 2, size_y / 2, size_z / 2);



        for (int x = start; x <= size_x; x++)
        {
            for (int y = start; y <= size_y; y++)
            {
                for (int z = start; z <= size_z; z++)
                {


                    //-----------------------------------------------------------------------------------原来没有该段.....
                    if (x > start && x < size_x)
                    {
                        if (y > start && y < size_y)
                        {
                            if (z > start && z < size_z)
                            {
                                AllCubePre_xyz.Add(EmptyGameObject);

                                continue;
                            }
                        }
                    }


                    obj = Instantiate(cube, new Vector3(x, y, z), cube.transform.rotation) as GameObject;
                    //obj.GetComponent<CubeBase>().SetText("   " + x.ToString() + " " + y.ToString() + " " + z.ToString());

                    //添加入集合中方便delete
                    AllCubePre.Add(obj);
                    AllCubePre_xyz.Add(obj);

                    /*
                    AllCubePre_x.Add(x);
                    AllCubePre_y.Add(y);
                    AllCubePre_z.Add(z);
                    */

					

                    //以x轴为基准线，每个x轴的点分割一片cube,按照顺序填充
					foreach (var item in tempDataSet)
                    {
						flag = int.Parse(item.dimension.y);

                        //8.2 modified
                        //int.Parse(item.dimension.y)-y_firstIndex+1这一句将dimension中的Index坐标从只能由1开始，变为了可以从任意值开始设置，增加了灵活性
                        if ((int.Parse(item.dimension.x) - x_firstIndex + 1) == x
                            && (flag - y_firstIndex + 1) == y
                            && (int.Parse(item.dimension.z) - z_firstIndex + 1) == z)
						//if ((int.Parse(item.dimension.x)== x)
						//   && (int.Parse(item.dimension.y)== y)
						//   && (int.Parse(item.dimension.z) == z))
                        {
						
                            cubeInstanceSet.Add(item, obj);
                        }
                    }
                }
            }
        }

        //设置CubeManager
        CubeManager_.SetCubesData(AllCubePre_xyz,size_x,size_y,size_z);
        Camera.main.transform.GetComponent<MouseFollowRotation>().minDistance = Mathf.Sqrt(size_x * size_x + size_y * size_y + size_z * size_z) / 1.9f;
        Camera.main.transform.GetComponent<MouseFollowRotation>().distance = Mathf.Sqrt(size_x * size_x + size_y * size_y + size_z * size_z) * 1.2f;
        Camera.main.transform.GetComponent<MouseFollowRotation>().maxDistance = Mathf.Sqrt(size_x * size_x + size_y * size_y + size_z * size_z) * 2.5f;
       
        //加载cube数据
        foreach (var cubeInstance in cubeInstanceSet)
        {
            cubeInstance.Value.GetComponent<CubeBase>().SetText(cubeInstance.Key.value);
        }

        //Debug
        //foreach (var item in tempDataSet) {
        //    Debug.Log(item.dimension.x +" "+item.dimension.y+" "+item.dimension.z);
        //}

        CreateCoordinate(start - 1, size_x, size_y, size_z);
    }

    /// <summary>
    /// 接下来一系列的if判断是为了拿到整体Cube的每个面的每个text，更新text的功能也将在这个代码块中
    /// 实现
    /// </summary>
    /// <param name="size"></param>
    /// <param name="x"></param>
    /// <param name="z"></param>
    /// <param name="y"></param>
    //private void ChangeCoreCubeData(int start,int size,int x,int z,int y)
    //{
    //    //接下来一系列的if判断是为了拿到整体Cube的每个面的每个text，更新text的功能也将在这个代码块中
    //    //实现
    //    if (z == size - 1)
    //    {
    //        if (x > start && x < size - 1)
    //        {
    //            obj.GetComponent<CubeBase>().SetText(6, "right");
    //            //obj.GetComponent<CubeBase>().GetText(6);

    //            //cubeSurface.Add(1,)
    //        }
    //    }
    //    if (y == size - 1)
    //    {
    //        if (x > start && x < size - 1)
    //        {
    //            obj.GetComponent<CubeBase>().SetText(3, "up");
    //        }
    //    }
    //    if (z == start)
    //    {
    //        if (x > start && x < size - 1)
    //        {
    //            obj.GetComponent<CubeBase>().SetText(5, "left");
    //        }
    //    }
    //    if (y == start)
    //    {
    //        if (x > start && x < size - 1)
    //        {
    //            obj.GetComponent<CubeBase>().SetText(4, "down");
    //        }
    //    }
    //    if (x == start)
    //    {
    //        if (z == start)
    //        {
    //            obj.GetComponent<CubeBase>().SetText(5, "left");
    //        }
    //        if (z == size - 1)
    //        {
    //            obj.GetComponent<CubeBase>().SetText(6, "right");
    //        }
    //        if (y == size - 1)
    //        {
    //            obj.GetComponent<CubeBase>().SetText(3, "up");
    //        }
    //        if (y == start)
    //        {
    //            obj.GetComponent<CubeBase>().SetText(4, "down");
    //        }
    //        obj.GetComponent<CubeBase>().SetText(2, "back");
    //    }
    //    if (x == size - 1)
    //    {
    //        if (z == size - 1)
    //        {
    //            obj.GetComponent<CubeBase>().SetText(6, "right");
    //        }
    //        if (z == start)
    //        {
    //            obj.GetComponent<CubeBase>().SetText(5, "left");
    //        }
    //        if (y == size - 1)
    //        {
    //            obj.GetComponent<CubeBase>().SetText(3, "up");
    //        }
    //        if (y == start)
    //        {
    //            obj.GetComponent<CubeBase>().SetText(4, "down");
    //        }
    //        obj.GetComponent<CubeBase>().SetText(1, "forward");
    //    }
    //}

    /// <summary>
    /// 建立三维坐标系
    /// </summary>
    /// <param name="start">这是起始坐标在之前也曾见到，是为了控制坐标比数据Cube整体小1，控制包裹</param>
    /// <param name="cx"></param>
    /// <param name="cy"></param>
    /// <param name="cz"></param>
    /**
     * 建立坐标的算法是将数据Cube的整体之外再包上一层Cube然后再用算法整体剥离那些无用的Cube
     * 最后只留下12条棱上的Cube并且填充数据
     **/
    private void CreateCoordinate(int start,int cx,int cy,int cz)
    {
        //临时的x,y,z坐标
        int coord_x;
        int coord_y;
        int coord_z;
        bool clearFlag = true; //清空List的标志
        int c_start = start;
        
        //CoordinatePoints = new List<GameObject>();

        for (int i = 0; i < xList.Length; i++)
        {
            xList[i] = new List<GameObject>();
            yList[i] = new List<GameObject>();
            zList[i] = new List<GameObject>();
        }
        
        for (coord_x = c_start; coord_x <=cx+1; coord_x++)
        {
            for (coord_y = c_start; coord_y <= cy+1; coord_y++)
            {
                for (coord_z = c_start; coord_z <= cz+1; coord_z++)
                {
                    //删除除最外层以外的内部的所有Cube，Dig body
                    //7.22 cx,cy,cz 各加1 dig 正常
                    //删除除最外层以外的内部的所有Cube，可以大幅度提高效率
                    if (coord_x > start && coord_x < cx+1)
                    {
                        if (coord_y > start && coord_y < cy+1)
                        {
                            if (coord_z > start && coord_z < cz+1)
                            {
                                continue;
                            }
                        }
                    }

                    //lesson:下面的去除每个面除了棱之外的所有cube的代码块实践表明并不适宜封装成为独立的方法
                    //而存在，因为与原函数存在着较强的逻辑耦合，并且复用的次数不是很多，所以最后考虑
                    //还是留在了这个函数中 7.23
                    if(coord_z == start)
                    {
                        if (coord_x > c_start && coord_x <= cx)
                        {
                            if (coord_y > c_start && coord_y <= cy)
                                continue;
                        }
                    }

                    if (coord_z == cz+1)
                    {
                        if (coord_x > c_start && coord_x <= cx)
                        {
                            if (coord_y > c_start && coord_y <= cy)
                                continue;
                        }
                    }

                    if(coord_x == start)
                    {
                        if (coord_z > c_start && coord_z <= cz)
                        {
                            if (coord_y > c_start && coord_y <= cy)
                                continue;
                        }
                    }

                    if (coord_x == cx+1)
                    {
                        if (coord_z > c_start && coord_z <= cz)
                        {
                            if (coord_y > c_start && coord_y <= cy)
                                continue;
                        }
                    }

                    if (coord_y == start)
                    {
                        if (coord_z > c_start && coord_z <= cz)
                        {
                            if (coord_x > c_start && coord_x <= cx)
                                continue;
                        }
                    }

                    if(coord_y == cy+1)
                    {
                        if (coord_z > c_start && coord_z <= cz)
                        {
                            if (coord_x > c_start && coord_x <= cx)
                                continue;
                        }
                    }

                    //去除了坐标系上的 (0,0,0,)点和(0,N,0)
                    //if (coord_x == c_start && coord_z == c_start)
                    //{
                    //    if (coord_y == cx+1 || coord_y == c_start)
                    //    {
                    //        continue;
                    //    }
                    //}

                    //去除了坐标系上的 (N,0,0) (N,N,0) (0,0,N) (0,N,N) (N,0,N) (N,N,N)，这是正方体，长方体这里的N并不相等，但是这个函数仍旧可以正确运行
                    if ((coord_x == cx+1 || coord_x == c_start) && (coord_z == c_start || coord_z == cz+1))
                    {
                        if (coord_y == c_start || coord_y == cy+1)
                        {
                            continue;
                        }
                    }
                    
                    obj = Instantiate(coord_cube, new Vector3(coord_x,coord_y,coord_z), cube.transform.rotation) as GameObject;
                    //obj.GetComponent<CubeBase>().SetText(" " + coord_x.ToString() + " " + coord_y.ToString() + " " + coord_z.ToString());

                    //将每条坐标轴放进坐标Dictionary中,坐标的默认顺序是这样的，默认的坐标轴是x面朝屏幕，y轴朝上，z轴向右
                    //4条x轴以垂直屏幕的法线为轴顺时针旋转确定1,2,3,4轴，同理4条y轴以竖直向上为轴顺时针旋转确定，
                    //4条z轴以水平向右的方向为轴顺时针旋转确定
                    if (coord_x == c_start)
                    {
                        if (coord_y == c_start)
                        {
                            zList[0].Add(obj);
                        }
                        if (coord_y == cy+1)
                        {
                            zList[1].Add(obj);
                        }
                        if (coord_z == c_start)
                        {
                            yList[0].Add(obj);
                        }
                        if (coord_z == cz+1)
                        {
                            yList[1].Add(obj);
                        }
                    }

                    if (coord_y == c_start)
                    {
                        if (coord_z == c_start)
                        {
                            xList[0].Add(obj);
                        }
                        if (coord_z == cz+1)
                        {
                            xList[3].Add(obj);
                        }
                        if (coord_x == cx+1)
                        {
                            zList[3].Add(obj);
                        }
                    }

                    if (coord_z == c_start)
                    {
                        if(coord_x == cx+1)
                        {
                            yList[3].Add(obj);
                        }
                        if(coord_y == cy+1)
                        {
                            xList[1].Add(obj);
                        }

                    }

                    if(coord_y == cy+1)
                    {
                        if(coord_z == cz+1)
                            xList[2].Add(obj);
                    }

                    if(coord_x == cx+1)
                    {
                        if (coord_z == cz+1)
                            yList[2].Add(obj);
                        if (coord_y == cy+1)
                            zList[2].Add(obj);
                    }
                }
            }
        }

        CubeManager_.SetCoordsData(xList, yList, zList);



        //写数据
        WriteDataToPoint(xList, yList, zList);
    }

    /// <summary>
    /// 往生成的坐标轴上面写入接收的坐标数据
    /// </summary>
    /// <param name="xList">x轴数据List</param>
    /// <param name="yList">y轴数据List</param>
    /// <param name="zList">z轴数据List</param>
    private void WriteDataToPoint(List<GameObject>[] xList, List<GameObject>[] yList, List<GameObject>[] zList)
    {
        //至此，坐标轴初步封装完毕，得到Coordinate
        Coordinates = new Dictionary<string, List<GameObject>[]>();
        Coordinates.Add("x", xList);
        Coordinates.Add("y", yList);
        Coordinates.Add("z", zList);

        List<string> x_list = new List<string>();
        List<string> y_list = new List<string>();
        List<string> z_list = new List<string>();

        foreach (var item in x_dictionary)
        {
            x_list.Add(item.Value);
        }

        foreach (var item in y_dictionary)
        {
            y_list.Add(item.Value);
        }

        foreach (var item in z_dictionary)
        {
            z_list.Add(item.Value);
        }

        int count = 0;

        //下面的遍历是为了给坐标点赋予实际的意义
        foreach (KeyValuePair<string, List<GameObject>[]> coord in Coordinates)
        {
            if (coord.Key == "x")
            {
                foreach (List<GameObject> c in coord.Value)
                {
                    count = 0;
                    foreach (GameObject point in c)
                    {
                        //注意这里访问dictionary的方式，尤其注意key的取值，并不是从1开始，当然项目默认是从1开始
                        //8.2修改，将从dictionary里面取值变为了动态判断，关键就是前面加入了拿到首项的Key的循环，在这里配合IndexOf就可以渐增Key了
                        //point.GetComponent<CubeBase>().SetText(x_dictionary[x_firstIndex + c.IndexOf(point)]);
                        point.GetComponent<CubeBase>().SetText(x_list[count]);
                        count++;
                    }
                }
            }

            if (coord.Key == "y")
            {
                foreach (List<GameObject> c in coord.Value)
                {
                    count = 0;
                    foreach (GameObject point in c)
                    {
                        point.GetComponent<CubeBase>().SetText(y_list[count]);
                        count++;
                    }
                }
            }

            if (coord.Key == "z")
            {
                foreach (List<GameObject> c in coord.Value)
                {
                    count = 0;
                    foreach (GameObject point in c)
                    {
                        point.GetComponent<CubeBase>().SetText(z_list[count]);
                        count++;
                    }
                }
            }
        }

        //为了实现坐标轴的选择操作而定义的List,将所有的Coordnates中的GameObject装进这个List中
        List<GameObject> newList = new List<GameObject>();

        //foreach (var item in xList[0])
        //{
        //    newList.Add(item);
        //}
        //foreach (var item in yList[0])
        //{
        //    newList.Add(item);
        //} 
        //foreach (var item in zList[0])
        //{
        //    newList.Add(item);
        //}
        foreach (var lists in Coordinates.Values)
        {
            foreach (var list in lists)
            {
                foreach (var coordCube in list)
                {
                    newList.Add(coordCube);
                }
            }
        }

        //Debug.Log("newList's count is " + newList.Count);

        //将newList传给CubeSelection
        CubeSelection.GetComponent<CubeSelection>().GetAllCoordObject(newList);

        //8.13
        ProgressBar.SetActive(false);

    }

    /// <summary>
    /// 删除所有的CubePre
    /// </summary>
    /// <param name="CubeInstances"></param>
    public void DeleteCubeFromAction()
    {
        AllCubePre_x.Clear();
        AllCubePre_y.Clear();
        AllCubePre_z.Clear();

        AllCubePre_xyz.Clear();


        if (AllCubePre.Count > 0)
        {
            foreach (var item in AllCubePre)
            {
                Destroy(item);
            }
        }

        //清空CubeInstanceSet中的数据，方便新的数据写入
        if (cubeInstanceSet.Count > 0)
        {
            cubeInstanceSet.Clear();
        }

        //11.02 add
        if (tempDataSet.Count > 0)
        {
            tempDataSet.Clear();
        }
    }





    /// <summary>
    /// 删除所有的CoordPre
    /// </summary>
    /// <param name="CubeInstances"></param>
    public void DeleteCoordsFromAction()
    {
        if (xList != null)
        {
            if (xList[0] != null)
            {
                foreach (var item in xList)
                {
                    if (item.Count > 0)
                    {
                        foreach (var x in item)
                        {
                            Destroy(x);
                        }
                    }
                }
            }
        }

        if (yList != null)
        {
            if (yList[0] != null)
            {
                foreach (var item in yList)
                {
                    if (item.Count > 0)
                    {
                        foreach (var y in item)
                        {
                            Destroy(y);
                        }
                    }
                }
            }
        }

        if (zList != null)
        {
            if (zList[0] != null)
            {
                foreach (var item in zList)
                {
                    if (item.Count > 0)
                    {
                        foreach (var z in item)
                        {
                            Destroy(z);
                        }
                    }
                }
            }
        }
    }
}

//测试数据
//private string[] region = { "和平区",	
//                            "沈河区",	
//                            "大东区",	
//                            "皇姑区",	
//                            "铁西区",	
//                            "苏家屯区",	
//                            "东陵区",	
//                            "新城子区",	
//                            "于洪区",	
//                            "辽中县",	
//                            "康平县",	
//                            "法库县"
//                          };
//private string[] months = { "1月", "2月", "3月", "4月", "5月", "6月", "7月", "8月", "9月", "10月", "11月", "12月" };
//private int[] year = { 2001, 2002, 2003, 2004, 2005, 2006, 2007, 2008, 2009, 2010, 2011, 2012, 2013, 2014, 2015 };


/* 2015-7-7
 * 这块的代码目标实现的是在要给一个核心Cube的各个面上贴上小的Cube来显示数据，为了在大数据量的状态下，减少硬件压力而作
 * 目前来看难度较大，可行性并不高，暂时放弃
 * 
 * Vector3[] vector3 = new Vector3[24];
 * mCurrentMeshFilter = coreCube.GetComponent<MeshFilter>();
 * Mesh mesh = mCurrentMeshFilter.mesh;
 * coreCube = GameObject.Find("CubeT");
 * coreCube.transform.localScale = new Vector3(10.0f,10.0f,10.0f);
 * //for (int j = 0; j < 10; j++)
            //{
            //    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //    cube.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            //    //cube.transform.position = new Vector3(i,j,0);
            //    print((i+1)+"顶点的坐标为 "+mesh.vertices[i]);
            //    cube.transform.position = coreCube.transform.TransformPoint(mesh.vertices[i].x,(mesh.vertices[i].y+j-4)/10,mesh.vertices[i].z);
            //}

            //for (int k = 0; k < 10; k++)
            //{
            //    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            //    cube.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            //    //cube.transform.position = new Vector3(i,j,0);
            //    cube.transform.position = coreCube.transform.TransformPoint((mesh.vertices[i].x+k-5)/10, mesh.vertices[i].y, mesh.vertices[i].z);
            //}
 * /





/*
 * public Transform cube;

    public GameObject mCube;

    //标记一个抽象Cube的六个面，这个抽象Cube可以包括很多子Cube
    public string cubeDirectionTag { set; get; }


    private GameObject mCamera;

    public GameObject text1;

    public List<GameObject> cubeGroup { set; get; }
    public List<GameObject> textGroup;

    string[] CubeStr = new string[6];

    public CubeBase()
    {
        for (int i = 1; i <= CubeStr.Length; i++)
        {
            CubeStr[i - 1] = i.ToString();
        }
    }

	// Use this for initialization
	void Start () {
        mCube = GameObject.Find("Cube");

        GameObject.Find("text1").GetComponent<TextMesh>().text = "HEHEHE";
        GameObject.Find("text2").GetComponent<TextMesh>().text = "lll";
        GameObject.Find("text3").GetComponent<TextMesh>().text = "fff";
        GameObject.Find("text4").GetComponent<TextMesh>().text = "444";
        GameObject.Find("text5").GetComponent<TextMesh>().text = "555";
        GameObject.Find("text6").GetComponent<TextMesh>().text = "666";
	}
*/