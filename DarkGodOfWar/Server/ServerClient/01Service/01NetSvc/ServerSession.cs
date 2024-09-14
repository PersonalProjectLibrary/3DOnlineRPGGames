
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：01NetSvc_ServerSession.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/6 21:32:17
    功能：网络会话连接
***************************************/
#endregion

using PENet;
using PEProtocol;

/// <summary>
/// 用于服务器和客户端建立联系，一个客户端对应一个Session
/// </summary>
public class ServerSession : PESession<GameMsg>
{
    public int sessionID = 0;

    protected override void OnConnected()
    {
        sessionID = ServerRoot.Instance.GetSessionID();//生成当前客户端和服务器连接的唯一id
        PECommon.Log("SessionID：" + sessionID + " Client Connect");
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        PECommon.Log("SessionID：" + sessionID + "  RcvPack CMD：" + ((CMD)msg.cmd).ToString());
        NetSvc.Instance.AddMsgQue(new MsgPack(this,msg));
    }

    protected override void OnDisConnected()
    {
        LoginSys.Instance.ClearOfflineData(this);
        PECommon.Log("SessionID：" + sessionID + " Client Offline");
    }
}
