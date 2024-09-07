
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：01NetSvc_NetSvc.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/6 21:13:15
    功能：网络服务
***************************************/
#endregion

using PENet;
using PEProtocol;

public class NetSvc
{
    private static NetSvc instance = null;
    public static NetSvc Instance
    {
        get
        {
            if (instance == null) instance = new NetSvc();
            return instance;
        }
    }
    /// <summary>
    /// 网络服务初始化
    /// </summary>
    public void Init()
    {
        PESocket<ServerSession, GameMsg> server = new PESocket<ServerSession, GameMsg>();//生成服务器
        server.StartAsServer(SrvCfg.srvIP, SrvCfg.srvPort);//开启服务器

        PECommon.Log("NetSvc Init Done.");
    }
}
