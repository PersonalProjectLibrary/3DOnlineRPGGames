
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：00Common_ServerRoot.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/6 20:58:00
    功能：服务器初始化
***************************************/
#endregion

/// <summary>
/// 服务器根节点
/// </summary>
public class ServerRoot
{
    private static ServerRoot instance = null;
    public static ServerRoot Instance
    {
        get
        {
            if (instance == null) instance = new ServerRoot();
            return instance;
        }
    }

    /// <summary>
    /// 服务器初始化
    /// </summary>
    public void Init()
    {
        //数据层
        DBManager.Instance.Init();
        //服务层
        CfgSvs.Instance.Init();
        NetSvc.Instance.Init();
        CacheSvc.Instance.Init();
        TimerSvc.Instance.Init();
        //业务系统层
        LoginSys.Instance.Init();
        GuideSys.Instance.Init();
        StrongSys.Instance.Init();
        WorldChatSys.Instance.Init();
        BuySys.Instance.Init();
        PowerSys.Instance.Init();
        //TestTimer2();
    }

    /// <summary>
    /// 服务器里要一直执行的功能
    /// </summary>
    public void Update()
    {
        NetSvc.Instance.Update();
        TimerSvc.Instance.Update();
    }

    private int SessionID = 0;//默认为0，每次调用获取加一
    
    /// <summary>
    /// 每次调用时，生成一个唯一的id
    /// </summary>
    /// <returns></returns>
    public int GetSessionID()
    {
        if (SessionID == int.MaxValue) SessionID = 0;//避免服务器运行很久后id值越界
        return SessionID += 1;
    }

    #region Test Function
    /// <summary>
    /// 测试PETimer定时器
    /// </summary>
    private void TestTimer()
    {
        TimerSvc.Instance.AddTimeTask((int tid) => { PECommon.Log("xxxxxxxx"); }, 1000);
    }

    /// <summary>
    /// 测试优化后的定时器定时任务
    /// </summary>
    private void TestTimer2()
    {
        TimerSvc.Instance.AddTimeTask((int tid) =>
        {
            PECommon.Log("yyyyy");
        }, 1000, PETimeUnit.Millisecond, 0);//0表示一直执行
    }
    #endregion
}
