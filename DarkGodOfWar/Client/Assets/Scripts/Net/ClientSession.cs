
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：Net_ClientSession.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/7 1:14
    功能：客户端网络会话
***************************************/
#endregion

using PENet;
using PEProtocol;

public class ClientSession : PESession<GameMsg>
{
    /// <summary>
    /// 和服务端建立连接
    /// </summary>
    protected override void OnConnected()
    {
        GameRoot.AddTips("连接服务器成功");
        PECommon.Log("Connect To Server Succ");
    }

    /// <summary>
    /// 接收消息时处理
    /// </summary>
    /// <param name="msg"></param>
    protected override void OnReciveMsg(GameMsg msg)
    {
        PECommon.Log("RcvPack CMD：" + ((CMD)msg.cmd).ToString());
        NetService.Instance.AddNetPkg(msg);
    }

    /// <summary>
    /// 和服务端断开连接
    /// </summary>
    protected override void OnDisConnected()
    {
        GameRoot.AddTips("服务器断开连接");
        PECommon.Log("DisConnect To Server");
    }
}