
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：01NetSvc_ServerSession.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/6 21:32:17
    功能：网络会话连接
***************************************/
#endregion

//用于服务器和客户端建立联系，一个客户端对应一个Session
using PENet;
using PEProtocol;

public class ServerSession:PESession<GameMsg>
{
    protected override void OnConnected()
    {
        PETool.LogMsg("Client Connect");
        SendMsg(new GameMsg { text = "Welcome to connect." });
    }

    protected override void OnReciveMsg(GameMsg msg)
    {
        PETool.LogMsg("Client Req：" + msg.text);
        SendMsg(new GameMsg { text = "SrvRsp：" + msg.text });
    }

    protected override void OnDisConnected()
    {
        PETool.LogMsg("Client DisConnect");
    }
}
