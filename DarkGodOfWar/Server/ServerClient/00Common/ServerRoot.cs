
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：00Common_ServerRoot.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/6 20:58:00
    功能：服务器初始化
***************************************/
#endregion

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
        //数据层TODO
        NetSvc.Instance.Init();//服务层
        LoginSys.Instance.Init();//业务系统层
    }
}
