using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using DB = UnityEngine.Debug;
using LitJson;
using Newtonsoft.Json;

//以下的几个简单的公共类是方便序列化和反序列化Json而设
public class FreCube
{
    public List<Cube> cubes{get;set;}
    public Coordinate coordinate{get;set;}
}

public class Coordinate
{
    public string P_xName { set; get; }
    public string N_xName { set; get; }
    public string P_yName { set; get; }
    public string N_yName { set; get; }
    public string P_zName { set; get; }
    public string N_zName { set; get; }

    /*
     * "P_xName": "国",
        "N_xName": "市",
        "P_yName": "年",
        "N_yName": "月",
        "P_zName": "电子",
        "N_zName": "Dell",
     * */
    public string xName { set; get; }
    public string yName { set; get; }
    public string zName { set; get; }
    public List<CoordPoint> x{get;set;}
    public List<CoordPoint> y{get;set;}
    public List<CoordPoint> z{get;set;}
}

public class CoordPoint
{
    public string name{set;get;}
    public string index { set; get; }
}

public class Cube
{
    public string value{get;set;}
    //public List<Dimension> Dimensions = new List<Dimension>();
    public Dimension dimension{get;set;}
}

public class Dimension
{
    public string x{get;set;}
    public string y{get;set;}
    public string z{get;set;}
}

public class CubeRequest
{
    public string requestType{get;set;}
    public string requestDimension{set;get;}
    public Coordinate coordinate{set;get;}
}

/// <summary>
/// 数据处理核心类，是整个项目的数据收集及转发点，在这个类中主要以Json数据的处理为核心，针对新的功能而增加新的处理函数
/// 可扩展性考量较多
/// </summary>
public class DataHandlerTest:MonoBehaviour
{
    //切块后的FreCube对象，重新封装
    public FreCube currentCube;
    public GameObject InfoPanel;
    public GameObject MiniCoordContainer;
    public GameObject ProgressBar;
    public GameObject Script;


    //
    public List<FreCube> FreCubeDiceList = new List<FreCube>();

    //将所有解析自Json的list缓存在这里
    public List<FreCube> FreCubeCacheList = new List<FreCube>();

    //测试WebPage连接的Label
    public GameObject label;

    /// <summary>
    /// FreCube类型，将Json转化成FreCube对象
    /// </summary>
    FreCube rawData;

    /// <summary>
    /// Json数据暂存的string 
    /// </summary>
    string _JsonData = "";

    /// <summary>
    /// 测试Json文件的存放路径
    /// </summary>
    string testPath = "assets/testData/";

    /// <summary>
    /// cube数据字典,将Json中解析出来每个Cube上的数据存于此
    /// </summary>
    private Dictionary<Dimension, string> cubeDataSet;

    /// <summary>
    /// X坐标数据字典,将Json中解析出来的坐标数据
    /// </summary>
    private Dictionary<int, string> x_dictionary;

    /// <summary>
    /// Y坐标数据字典
    /// </summary>
    private Dictionary<int, string> y_dictionary;

    /// <summary>
    /// Z坐标数据字典
    /// </summary>
    private Dictionary<int, string> z_dictionary;

    //lesson:7.23script的加载顺序主要和awake有关，所以要在Action之前加载DataHandlerTest就需要写在Awake()中
    //之前一直写在start中，总是Action先加载
    //要和服务器则将加载功能和unity内置函数解耦，弃用了此函数
    public void Awake()
    {
        //ReceiveJson("test.json");
        ReceiveJson("time-quarter.json");
    }

    /// <summary>
    /// 序列化请求
    /// </summary>
    /// <returns></returns>
    public string SerializeTheDiceRequest()
    {
        return null;
    }

    /// <summary>
    /// 序列化上卷下钻请求
    /// </summary>
    /// <param name="type"></param>
    /// <param name="coord"></param>
    /// <param name="coords"></param>
    /// <returns></returns>
    public string SerializeFreCubeRequest(string type, string coord)
    {
        string request;
        CubeRequest cubeRequest = new CubeRequest();
        cubeRequest.requestType = type;
        cubeRequest.requestDimension = coord;
        cubeRequest.coordinate = rawData.coordinate;

        request = JsonMapper.ToJson(cubeRequest);

        print(request);

        return request;
    }

    /// <summary>
    /// 序列化对象，并且保存在文件中方便测试，测试方法
    /// </summary>
    private void SerializeFreCube()
    {
        FreCube freCube = new FreCube();
        freCube.cubes = new List<Cube>();

        Cube cube1 = new Cube();
        cube1.dimension = new Dimension { x = "1", y = "11", z = "2" };
        cube1.value = "2450件";

        Cube cube2= new Cube();
        cube2.dimension = new Dimension { x = "2", y = "13", z = "1" };
        cube2.value = "8900件";

        freCube.cubes.Add(cube1);
        freCube.cubes.Add(cube2);

        Coordinate coord = new Coordinate();
        coord.xName = "Province";
        coord.yName = "Year";
        coord.zName = "Month";
        coord.x = new List<CoordPoint>();
        coord.y = new List<CoordPoint>();
        coord.z = new List<CoordPoint>();
        coord.x.Add(new CoordPoint { name = "辽宁省", index = "1" });
        coord.x.Add(new CoordPoint { name = "黑龙江省", index = "2" });
        coord.x.Add(new CoordPoint { name = "吉林省", index = "3" });
        coord.x.Add(new CoordPoint { name = "广东省", index = "4" });
        coord.x.Add(new CoordPoint { name = "浙江省", index = "5" });
        coord.x.Add(new CoordPoint { name = "江苏省", index = "6" });
        coord.x.Add(new CoordPoint { name = "河南省", index = "7" });
        coord.x.Add(new CoordPoint { name = "河北省", index = "8" });
        coord.x.Add(new CoordPoint { name = "湖南省", index = "9" });
        coord.x.Add(new CoordPoint { name = "湖北省", index = "10" });
        coord.x.Add(new CoordPoint { name = "山西省", index = "11" });
        coord.x.Add(new CoordPoint { name = "陕西省", index = "12" });
        coord.x.Add(new CoordPoint { name = "甘肃省", index = "13" });
        coord.x.Add(new CoordPoint { name = "宁夏回族自治区", index = "14" });
        coord.x.Add(new CoordPoint { name = "青海省", index = "15" });
        coord.x.Add(new CoordPoint { name = "福建省", index = "16" });
        coord.x.Add(new CoordPoint { name = "海南省", index = "17" });
        coord.x.Add(new CoordPoint { name = "云南省", index = "18" });
        coord.x.Add(new CoordPoint { name = "贵州省", index = "19" });
        coord.x.Add(new CoordPoint { name = "广西壮族自治区", index = "20" });
        coord.x.Add(new CoordPoint { name = "江西省", index = "21" });
        coord.x.Add(new CoordPoint { name = "内蒙古自治区", index = "22" });
        coord.x.Add(new CoordPoint { name = "台湾省", index = "23" });
        coord.x.Add(new CoordPoint { name = "安徽省", index = "24" });
        coord.x.Add(new CoordPoint { name = "新疆维吾尔自治区", index = "25" });
        coord.x.Add(new CoordPoint { name = "四川省", index = "26" });
        coord.x.Add(new CoordPoint { name = "西藏自治区", index = "27" });

        for (int i = 5 ; i <= 15; i++)
        {
            coord.y.Add(new CoordPoint { name = (2000 + i).ToString(), index = i.ToString() });
        }

        for (int i = 1; i <= 12; i++)
        {
            coord.z.Add(new CoordPoint { name = i.ToString()+"月", index = i.ToString() });
        }

        freCube.coordinate = coord;
        foreach (var item in freCube.coordinate.x)
	    {
            print(item.name+" "+item.index);
		 
	    }

        //serialize freCube
        var output = JsonConvert.SerializeObject(freCube,Formatting.Indented);

        DB.Log(output.ToString());

        using (StreamWriter file = new StreamWriter(testPath, false))
        {
            file.WriteLine(output.ToString());
        }
    }

    /// <summary>
    /// Json反序列化函数
    /// </summary>
    /// <param name="jsonData">传入的Json串</param>
    public void Deserialize(string jsonData)
    {
        //rawData = JsonConvert.DeserializeObject<FreCube>(_JsonData);
        //因为WebPlayer的原因，起用LitJson
        rawData = JsonMapper.ToObject<FreCube>(jsonData);
        //缓存
        FreCubeCacheList.Add(rawData);
        DeserializeAndTransform();
    }

    /// <summary>
    /// 8.8暂时用来接收从Next和Pre Button传过来的freCube
    /// </summary>
    /// <param name="freCube"></param>
    public void ReceiveFreCube(FreCube freCube)
    {
        GetComponent<Action>().DeleteCubeFromAction();
        GetComponent<Action>().DeleteCoordsFromAction();
        if(freCube==null)
        {
            Debug.Log("freCube is null. Method: " + System.Reflection.MethodBase.GetCurrentMethod().Name);
        }
        rawData = freCube;
        DeserializeAndTransform();
    }
    /// <summary>
    /// 反序列化对象，从json中分离出对象
    /// </summary>
    public void DeserializeAndTransform()
    {
        //坐标 Deserialize
        //将从jsonData中取出的坐标装如相应的dictionary中并且按照Key值进行排序，同样可以使用SortedDictionary
        //成功测试通过，删去了其他的测试方法 7.23
        x_dictionary = new Dictionary<int, string>();
        y_dictionary = new Dictionary<int, string>();
        z_dictionary = new Dictionary<int, string>();
        //装箱
        foreach (var x in rawData.coordinate.x)
        {
            x_dictionary.Add(int.Parse(x.index), x.name);
        }
        foreach (var y in rawData.coordinate.y)
        {
            y_dictionary.Add(int.Parse(y.index), y.name);
        }
        foreach (var z in rawData.coordinate.z)
        {
            z_dictionary.Add(int.Parse(z.index), z.name);
        }
        //排序
        x_dictionary = x_dictionary.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        y_dictionary = y_dictionary.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        z_dictionary = z_dictionary.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        //发送给Action处理
        GetComponent<Action>().SetX(x_dictionary);
        GetComponent<Action>().SetY(y_dictionary);
        GetComponent<Action>().SetZ(z_dictionary);


        //cube数据 Deserialize
        //cubeDataSet = new Dictionary<Dimension, string>();

        //foreach (var item in rawData.cubes)
        //{
        //    cubeDataSet.Add(item.dimension,item.value);
        //}

        //11.10 模拟上卷下钻增
        GetComponent<Action>().DeleteCoordsFromAction();
        GetComponent<Action>().DeleteCubeFromAction();


        //将Cube数据发送给Cube组装生产类
        GetComponent<Action>().SetCubeDataSet(rawData.cubes);

        //8.2 方便第二次的生成加入了调用Action AllCreate方法的语句
        GetComponent<Action>().AllCreate();

        //更新上卷下钻的选择坐标的Label
        MiniCoordContainer.GetComponent<MiniCoord>().SetCoords(rawData.coordinate.xName, rawData.coordinate.yName, rawData.coordinate.zName);

        //更新Info面板
        InfoPanel.GetComponent<InfoHandler>().SetInfo(rawData.coordinate.xName, rawData.coordinate.yName, rawData.coordinate.zName);

        //更新新加的轴向信息 3*3   10.30更
        string[] array_x = { rawData.coordinate.P_xName, rawData.coordinate.xName, rawData.coordinate.N_xName };
        string[] array_y = { rawData.coordinate.N_yName, rawData.coordinate.yName, rawData.coordinate.P_yName };
        string[] array_z = { rawData.coordinate.N_zName, rawData.coordinate.zName, rawData.coordinate.P_zName };
        Script.GetComponent<CoordManager>().SetCoordMessage(array_x, array_y, array_z);
    }

    /// <summary>
    /// 将切片的数据重新封装发送给Action生成Slice
    /// 8.7 因为有了更加通用的CreateDiceData而弃用
    /// </summary>
    /// <param name="coordPoint">选中的维点</param>
    public void CreateSliceData(string coordPoint)
    {
        //主要注释和CreateDiceData方法中相同
        GetComponent<Action>().DeleteCoordsFromAction();
        GetComponent<Action>().DeleteCubeFromAction();
        string coordAxis = "";
        string coordIndex = "";
        foreach (var item in rawData.coordinate.x)
        {
            if(item.name.Equals(coordPoint))
            {
                coordAxis = "x";
                coordIndex = item.index;
            }
        }

        foreach (var item in rawData.coordinate.y)
        {
            if (item.name.Equals(coordPoint))
            {
                coordAxis = "y";
                coordIndex = item.index;
            }
        }

        foreach (var item in rawData.coordinate.z)
        {
            if (item.name.Equals(coordPoint))
            {
                coordAxis = "z";
                coordIndex = item.index;
            }
        }

        if(coordAxis.Equals("x"))
        {
            x_dictionary = new Dictionary<int, string>();
            y_dictionary = new Dictionary<int, string>();
            z_dictionary = new Dictionary<int, string>();
            //装箱
            foreach (var x in rawData.coordinate.x)
            {
                if (x.name.Equals(coordPoint))
                {
                    x_dictionary.Add(int.Parse(x.index), x.name);
                }
            }
            foreach (var y in rawData.coordinate.y)
            {
                y_dictionary.Add(int.Parse(y.index), y.name);
            }
            foreach (var z in rawData.coordinate.z)
            {
                z_dictionary.Add(int.Parse(z.index), z.name);
            }
            //排序
            y_dictionary = y_dictionary.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            z_dictionary = z_dictionary.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            //发送给Action处理
            GetComponent<Action>().SetX(x_dictionary);
            GetComponent<Action>().SetY(y_dictionary);
            GetComponent<Action>().SetZ(z_dictionary);
        }

        if (coordAxis.Equals("y"))
        {
            x_dictionary = new Dictionary<int, string>();
            y_dictionary = new Dictionary<int, string>();
            z_dictionary = new Dictionary<int, string>();
            //装车
            foreach (var y in rawData.coordinate.y)
            {
                if (y.name.Equals(coordPoint))
                {
                    y_dictionary.Add(int.Parse(y.index), y.name);
                }
            }
            foreach (var x in rawData.coordinate.x)
            {
                x_dictionary.Add(int.Parse(x.index), x.name);
            }
            foreach (var z in rawData.coordinate.z)
            {
                z_dictionary.Add(int.Parse(z.index), z.name);
            }
            //排序
            x_dictionary = x_dictionary.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            z_dictionary = z_dictionary.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            //发送给Action处理
            GetComponent<Action>().SetX(x_dictionary);
            GetComponent<Action>().SetY(y_dictionary);
            GetComponent<Action>().SetZ(z_dictionary);
        } 
        
        if (coordAxis.Equals("z"))
        {
            x_dictionary = new Dictionary<int, string>();
            y_dictionary = new Dictionary<int, string>();
            z_dictionary = new Dictionary<int, string>();
            //装车
            foreach (var z in rawData.coordinate.z)
            {
                if (z.name.Equals(coordPoint))
                {
                    z_dictionary.Add(int.Parse(z.index), z.name);
                }
            }
            foreach (var x in rawData.coordinate.x)
            {
                x_dictionary.Add(int.Parse(x.index), x.name);
            }
            foreach (var y in rawData.coordinate.y)
            {
                y_dictionary.Add(int.Parse(y.index), y.name);
            }
            //排序
            x_dictionary = x_dictionary.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            y_dictionary = y_dictionary.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            //发送给Action处理
            GetComponent<Action>().SetX(x_dictionary);
            GetComponent<Action>().SetY(y_dictionary);
            GetComponent<Action>().SetZ(z_dictionary);
        }

        Debug.Log(coordAxis+" "+coordIndex);

        List<Cube> list = new List<Cube>();
        if (coordAxis.Equals("x"))
        {
            foreach (var item in rawData.cubes)
            {
                if(item.dimension.x.Equals(coordIndex))
                {
                    continue;
                }
                else
                {
                    list.Add(item);
                }
            }
        }

        if (coordAxis.Equals("y"))
        {
            foreach (var item in rawData.cubes)
            {
                if (item.dimension.y.Equals(coordIndex))
                {
                    continue;
                }
                else
                {
                    list.Add(item);
                }
            }
        }

        if (coordAxis.Equals("z"))
        {
            foreach (var item in rawData.cubes)
            {
                if (item.dimension.Equals(coordIndex))
                {
                    continue;
                }
                else
                {
                    list.Add(item);
                }
            }
        }

        //8.6因为foreach中是只读的，不能对集合中的元素进行删除，换用for循环正常删除
        for (int i = 0; i < list.Count; i++)
        {
            list.Remove(list[i]);
        }

        GetComponent<Action>().SetCubeDataSet(rawData.cubes);

        GetComponent<Action>().CreateSliceAndDice();
    }

    /// <summary>
    /// 通用的切片及切块的数据组装类，将元Cube的数据重新组装成新的切割后的Cube需要的类
    /// </summary>
    /// <param name="coordPoints">选中的维点CoordPoint</param>
    public void CreateDiceData(List<string> coordPoints)
    {
        currentCube = new FreCube();
        //清空scene中的原Cube及坐标Coords
        GetComponent<Action>().DeleteCoordsFromAction();
        GetComponent<Action>().DeleteCubeFromAction();

        //新的坐标字典，应用于新的封装数据
        Dictionary<int, string> _new_x_dictionary = new Dictionary<int, string>();
        Dictionary<int, string> _new_y_dictionary = new Dictionary<int, string>();
        Dictionary<int, string> _new_z_dictionary = new Dictionary<int, string>();

        //去除这个集合中的重复的元素
        for (int i = 0; i < coordPoints.Count; i++)
        {
            if (coordPoints.IndexOf(coordPoints[i]) != coordPoints.LastIndexOf(coordPoints[i]))
            {
                coordPoints.Remove(coordPoints[i]);
            }
        }

        //用于统计选中的每个轴各有几个坐标，方便之后的生成
        //同时将每个轴对应的坐标的数据放进对应的字典中
        int coord_x = 0;
        int coord_y = 0;
        int coord_z = 0;

        foreach (var item in rawData.coordinate.x)
        {
            foreach (var coordPoint in coordPoints)
            {
                if(coordPoint.Equals(item.name))
                {
                    coord_x++;

                    _new_x_dictionary.Add(int.Parse(item.index), item.name);
                }
            }
        }

        foreach (var item in rawData.coordinate.y)
        {
            foreach (var coordPoint in coordPoints)
            {
                if (coordPoint.Equals(item.name))
                {
                    coord_y++;

                    _new_y_dictionary.Add(int.Parse(item.index), item.name);
                }
            }
        }

        foreach (var item in rawData.coordinate.z)
        {
            foreach (var coordPoint in coordPoints)
            {
                if (coordPoint.Equals(item.name))
                {
                    coord_z++;

                    _new_z_dictionary.Add(int.Parse(item.index), item.name);
                }
            }
        }

        //Debug
        //foreach (var item in _new_x_dictionary)
        //{
        //    Debug.Log(item.Key + " " + item.Value);
        //}

        //foreach (var item in _new_y_dictionary)
        //{
        //    Debug.Log(item.Key + " " + item.Value);
        //}

        //foreach (var item in _new_z_dictionary)
        //{
        //    Debug.Log(item.Key + " " + item.Value);
        //}


        //字典排序，选择了较为简洁的委托写法
        _new_x_dictionary = _new_x_dictionary.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        _new_y_dictionary = _new_y_dictionary.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
        _new_z_dictionary = _new_z_dictionary.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);

        //判断如果这个维度没有被选中则默认将其所有的原始数据写入到字典中
        //主要对应于选中一维和两维的操作之后的正常显示
        if (coord_x == 0)
        {
            foreach (var item in rawData.coordinate.x)
            {
                _new_x_dictionary.Add(int.Parse(item.index), item.name);
            }
        }

        if (coord_y == 0)
        {
            foreach (var item in rawData.coordinate.y)
            {
                _new_y_dictionary.Add(int.Parse(item.index), item.name);
            }
        }

        if (coord_z == 0)
        {
            foreach (var item in rawData.coordinate.z)
            {
                _new_z_dictionary.Add(int.Parse(item.index), item.name);
            }
        }

        //这里重新将切块后的FreCube封装成一个新的currentCube，与rawData解耦
        currentCube.cubes = rawData.cubes;
        Coordinate coordinate = new Coordinate();

        coordinate.xName = rawData.coordinate.xName;
        coordinate.yName = rawData.coordinate.yName;
        coordinate.zName = rawData.coordinate.zName;

        currentCube.coordinate = coordinate;

        List<CoordPoint> x_c = new List<CoordPoint>();
        List<CoordPoint> y_c = new List<CoordPoint>();
        List<CoordPoint> z_c = new List<CoordPoint>();

        foreach (var item in _new_x_dictionary)
	    {
            CoordPoint coord = new CoordPoint();
            coord.index = item.Key.ToString();
            coord.name  = item.Value;
            x_c.Add(coord);
	    }

        foreach (var item in _new_y_dictionary)
	    {
            CoordPoint coord = new CoordPoint();
            coord.index = item.Key.ToString();
            coord.name  = item.Value;
            y_c.Add(coord);
	    }

        foreach (var item in _new_z_dictionary)
	    {
            CoordPoint coord = new CoordPoint();
            coord.index = item.Key.ToString();
            coord.name  = item.Value;
            z_c.Add(coord);
	    }

        currentCube.coordinate.x = x_c;
        currentCube.coordinate.y = y_c;
        currentCube.coordinate.z = z_c;

        ///////////////////////////
        FreCubeDiceList.Add(currentCube);

        ////这句非常非常重要，将切块后的实例赋值给rawData/////////////////////////////////////////////////////////////
        rawData = currentCube;

        //发送给Action处理
        GetComponent<Action>().SetCubeDataSet(rawData.cubes);

        GetComponent<Action>().SetX(_new_x_dictionary);
        GetComponent<Action>().SetY(_new_y_dictionary);
        GetComponent<Action>().SetZ(_new_z_dictionary);


        //生成Cube的数据
        GetComponent<Action>().CreateSliceAndDice();
    }

    //web page调用这个函数查询后的结果发到这里，处理并且开始后续的展示，这些工作将会在unity web player中进行
    public void ReceiveJson(string newJson)
    {
        ////web测试代码段
        //if (newJson.Length > 0)
        //{
        //    Deserialize(newJson);
        //}
        //else
        //{
        //    Debug.Log("send string is empty!");
        //}

        //原始测试和文件的读写代码，在Awake()中进行调用
        //label.GetComponent<UILabel>().text = _JsonData;

        string newPath = testPath + newJson;
        if (!File.Exists(newPath))
        {
            DB.Log("File does not exist");
        }

        using (StreamReader sr = File.OpenText(newPath))
        {
            _JsonData = sr.ReadToEnd();
        }

        Deserialize(_JsonData);
    }
}


// //jsonFx de/serialize FreCube json Test

//using UnityEngine;
//using System.Collections;
//using Pathfinding.Serialization.JsonFx;
//using System.IO;
//using System.Text;
//using System;
//using System.Collections.Generic;
//using DB = UnityEngine.Debug;

//public class DataHandlerTest : MonoBehaviour {

//    string testFile = "assets/testData/test.json";

//    // Use this for initialization
//    void Start()
//    {
//        //LoadAndDeserialize();
//        SerializeAndSave();
//        LoadAndDeserialize();
//    }
	
//    //private void SerializeAndSave()
//    //{
//    //    string data = JsonWriter.Serialize(san)
//    //}

//    /// <summary>
//    /// 序列化对象，并且保存在文件中方便测试
//    /// </summary>
//    private void SerializeAndSave()
//    {
//        FreCube freCube = new FreCube();
//        freCube.cells = new List<Cell>();

//        Cell cell1 = new Cell();
//        cell1.dimension = new Dimension { x = "liaoning", y = "2010", z = "Dec" };

//        cell1.value = 2000;

//        Cell cell2 = new Cell();
//        cell2.dimension = new Dimension { x = "heNan", y = "2009", z = "Feb" };

//        cell2.value = 1000;

//        freCube.cells.Add(cell1);
//        freCube.cells.Add(cell2);

//        Coordinate coord = new Coordinate();
//        coord.x = new List<string> {"ShanDong","HeiLongJiang"};
//        coord.y = new List<string> { "AAA","BBB"};
//        coord.z = new List<string> { "CCC","DDD"};

//        freCube.coordinate = coord;

//        JsonWriterSettings settings = new JsonWriterSettings();
//        settings.PrettyPrint = true;

//        StringBuilder output = new StringBuilder();

//        JsonWriter writer = new JsonWriter(output,settings);

//        writer.Write(freCube);

//        DB.Log(output.ToString());

//        using(StreamWriter file = new StreamWriter(testFile,false))
//        {
//            file.WriteLine(output.ToString());
//        }

//        //cell.dimensions.Add(new Dimensions { x = "henan", y = "2011", z = "Feb" });

//    }

//    /// <summary>
//    /// 反序列化对象，从json中分离出对象
//    /// </summary>
//    private void LoadAndDeserialize()
//    {
//        if (!File.Exists(testFile))
//        {
//            DB.Log("File does not exist");
//        }

//        using (StreamReader sr = File.OpenText(testFile))
//        {
//            string input = sr.ReadToEnd();
//            print(input);
//            var rawData = JsonReader.Deserialize<FreCube>(input);

//            print(rawData.coordinate);

//            foreach (var cell in rawData.cells)
//            {
//                DB.Log("cell"+rawData.cells.IndexOf(cell)+" 's dimension "+cell.dimension.x+" "+cell.dimension.y+" "+cell.dimension.z);
//            }

//            print("Coordinate:");

//            foreach (var x in rawData.coordinate.x)
//            {
//                DB.Log(x.ToString());
//            }
//            foreach (var y in rawData.coordinate.y)
//            {
//                DB.Log(y.ToString());
//            }
//            foreach (var z in rawData.coordinate.z)
//            {
//                DB.Log(z.ToString());
//            }
//        }


//        //var streamReader = new StreamReader(testFile);
//        //string json = streamReader.ReadToEnd();
//        //streamReader.Close();

//        //Dictionary<string, object> Cubes = new Dictionary<string, object>();
//        //List<Dimension> dimensions = new List<Dimension>();

//        //JsonReaderSettings readerSettings = new JsonReaderSettings();
//        //readerSettings.TypeHintName = "__type";
//        //JsonReader reader = new JsonReader(json, readerSettings);

//        //Cubes = null;
//        //Cubes = (Dictionary<string, object>)reader.Deserialize();

//        //foreach (KeyValuePair<string, object> item in Cubes)
//        //{
//        //    string key = item.Key;
//        //    object val = item.Value;

//        //    print(string.Format("Key : {0}, Value : {1}, Type : {2}", key, val.ToString(), val.GetType()));
//        //}
//    }



//    [Serializable]
//    [JsonName("freCube")]
//    public class FreCube
//    {
//        public List<Cell> cells;
//        public Coordinate coordinate;
//    }

//    [Serializable]
//    [JsonName("coordinate")]
//    public class Coordinate
//    {
//        public List<string> x;
//        public List<string> y;
//        public List<string> z;
//    }

//    [Serializable]
//    [JsonName("cell")]
//    public class Cell
//    {
//        public float value;
//        //public List<Dimension> Dimensions = new List<Dimension>();
//        public Dimension dimension;
//    }

//    [Serializable]
//    [JsonName("dimension")]
//    public class Dimension
//    {
//        public string x;
//        public string y;
//        public string z;
//    }
//}


// //jsonFx dictionary sample from stackOverFlow

//using UnityEngine;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Text;
//using Pathfinding.Serialization.JsonFx;

//[Serializable]
//[JsonName("Person")]
//public class Person
//{
//    public string name;
//    public string surname;
//}

//[JsonName("Animal")]
//public class Animal
//{
//    public string name;
//    public string species;
//}

//[Serializable]
//public class Parameters
//{
//    public float floatValue;
//    public string stringValue;
//    public List<Person> listValue;
//}

//public class DataHandlerTest : MonoBehaviour
//{
//    // Use this for initialization
//    void Start()
//    {
//        ScenarioOne();
//    }

//    void ScenarioOne()
//    {
//        Dictionary<string, object> parameters = new Dictionary<string, object>();
//        List<Person> persons = new List<Person>();
//        persons.Add(new Person() { name = "Clayton", surname = "Curmi" });
//        persons.Add(new Person() { name = "Karen", surname = "Attard" });

//        List<Animal> animals = new List<Animal>();
//        animals.Add(new Animal() { name = "Chimpanzee", species = "Pan troglodytes" });
//        animals.Add(new Animal() { name = "Cat", species = "Felis catus" });

//        parameters.Add("floatValue", 3f);
//        parameters.Add("stringValue", "Parameter string info");
//        parameters.Add("persons", persons.ToArray());
//        parameters.Add("animals", animals.ToArray());

//        // ---- SERIALIZATION ----

//        JsonWriterSettings writerSettings = new JsonWriterSettings();
//        writerSettings.TypeHintName = "__type";

//        StringBuilder json = new StringBuilder();
//        JsonWriter writer = new JsonWriter(json, writerSettings);
//        writer.Write(parameters);

//        Debug.Log(json.ToString());

//        // ---- DESERIALIZATION ----

//        JsonReaderSettings readerSettings = new JsonReaderSettings();
//        readerSettings.TypeHintName = "__type";

//        JsonReader reader = new JsonReader(json.ToString(), readerSettings);

//        parameters = null;
//        parameters = (Dictionary<string, object>)reader.Deserialize();

//        foreach (KeyValuePair<string, object> kvp in parameters)
//        {
//            string key = kvp.Key;
//            object val = kvp.Value;
//            Debug.Log(val == null);
//            Debug.Log(string.Format("Key : {0}, Value : {1}, Type : {2}", key, val, val.GetType()));
//        }
//    }
//}