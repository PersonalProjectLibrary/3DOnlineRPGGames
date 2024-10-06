
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
using System.Collections.Generic;

/// <summary>
/// 网络服务
/// </summary>
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

    public static readonly string lockObj = "lock";
    private Queue<MsgPack> msgPackQue = new Queue<MsgPack>();
    
    /// <summary>
    /// 把接收的消息传到消息队列中
    /// </summary>
    /// <param name="msg"></param>
    /// 然后去ServerSession里进行调用.
    public void AddMsgQue(MsgPack msgPack)
    {
        lock (lockObj)
        {
            msgPackQue.Enqueue(msgPack);
        }
    }

    /// <summary>
    /// 处理消息队列里的消息
    /// </summary>
    public void Update()
    {
        if (msgPackQue.Count > 0)
        {
            //PECommon.Log("PackCount：" + msgPackQue.Count);
            lock (lockObj)
            {
                MsgPack msgPack = msgPackQue.Dequeue();
                HandOutMsg(msgPack);
            }
        }
    }

    /// <summary>
    /// 根据消息类型，分发到不同业务系统中进行处理
    /// </summary>
    /// <param name="msg"></param>
    private void HandOutMsg(MsgPack msgPack)
    {
        switch ((CMD)msgPack.m_Msg.cmd)
        {
            case CMD.None: break;
            case CMD.ReqLogin: LoginSys.Instance.ReqLogin(msgPack); break;
            case CMD.ReqReName: LoginSys.Instance.ReqReName(msgPack); break;
            case CMD.ReqGuide:GuideSys.Instance.ReqGuide(msgPack);break;
            case CMD.ReqStrong:StrongSys.Instance.ReqStrong(msgPack);break;
            case CMD.SndWorldChat:WorldChatSys.Instance.SndWorldChat(msgPack);break;
            default: break;
        }
    }
}

/// <summary>
/// 登录消息的消息包
/// </summary>
public class MsgPack
{
    public ServerSession m_Session;
    public GameMsg m_Msg;
    public MsgPack(ServerSession session,GameMsg msg)
    {
        m_Session = session;
        m_Msg = msg;
    }
}