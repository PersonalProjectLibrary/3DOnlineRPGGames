
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：Scripts_GameRoot.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/8/30 10:39
    功能：游戏启动入口，初始化各个系统，保存核心数据
***************************************/
#endregion

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
        Debug.Log("Game Start...");
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

        //进入登录场景
        login.EnterLogin();
    }

    public static void AddTips(string tips)
    {
        Instance.dynamicWnd.AddTips(tips);
    }
}