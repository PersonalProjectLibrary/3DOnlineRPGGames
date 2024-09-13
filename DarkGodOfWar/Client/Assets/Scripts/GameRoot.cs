
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：Scripts_GameRoot.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/8/30 10:39
    功能：游戏启动入口，初始化各个系统，保存核心数据
***************************************/
#endregion

using PEProtocol;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    public static GameRoot Instance=null;

    public LoadingWnd loadingWnd;
    public DynamicWnd dynamicWnd;

    void Start()
    {
        Instance = this;
        DontDestroyOnLoad(this);//游戏中一直不销毁GameRoot
        PECommon.Log("Game Start...");
        ClearUIRoot();
        Init();
    }

    /// <summary>
    /// 初始化场景UI显示状态
    /// </summary>
    private void ClearUIRoot()
    {
        Transform canvas = transform.Find("Canvas");
        for (int i = 0; i < canvas.childCount; i++)
        {
            canvas.GetChild(i).gameObject.SetActive(false);
        }
        dynamicWnd.SetWndState();
    }

    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        //优先服务模块初始化
        NetService net = GetComponent<NetService>();
        net.InitService();
        ResService res = GetComponent<ResService>();
        res.InitService();

        //声音播放服务初始化
        AudioService audio = GetComponent<AudioService>();
        audio.InitService();

        //业务系统初始化
        LoginSystem login = GetComponent<LoginSystem>();
        login.InitSystem();
        MainCitySystem mainCity = GetComponent<MainCitySystem>();
        mainCity.InitSystem();

        //进入登录场景
        login.EnterLogin();
    }

    /// <summary>
    /// 显示弹窗提示
    /// </summary>
    /// <param name="tips"></param>
    public static void AddTips(string tips)
    {
        Instance.dynamicWnd.AddTips(tips);
    }

    #region PlayerData
    private PlayerData playerData = null;
    /// <summary>
    /// 获取玩家数据
    /// </summary>
    public PlayerData PlayerData
    {
        get { return playerData; }
    }
    /// <summary>
    /// 更新修改玩家数据
    /// </summary>
    /// <param name="data"></param>
    public void SetPlayerData(RspLogin data)
    {
        playerData = data.playerData;
    }

    /// <summary>
    /// 更新玩家名字
    /// </summary>
    /// <param name="name"></param>
    public void SetPlayerName(string name)
    {
        PlayerData.name = name;
    }
    #endregion

}