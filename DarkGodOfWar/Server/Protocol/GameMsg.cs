
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：PEProtocol_GameMsg.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/6 20:58:00
    功能：网络通信协议（Unity客户端和服务端共用）
***************************************/
#endregion

using PENet;
using System;

namespace PEProtocol
{
    /// <summary>
    /// 服务器配置类
    /// </summary>
    public class SrvCfg
    {
        public const string srvIP = "127.0.0.1";//服务器IP
        public const int srvPort = 17666;//服务器端口
    }

    [Serializable]
    public class GameMsg : PEMsg
    {
        public ReqLogin reqLogin;
    }

    /// <summary>
    /// 网络通信的命令号, 消息类型
    /// </summary>
    /// 对应PEMsg里的cmd字段
    public enum CMD
    {
        None =0,
        //登录相关 100
        ReqLogin =101,//登录请求
        RspLogin =102,//登录回应
    }

    /// <summary>
    /// 发送请求登录的消息
    /// </summary>
    [Serializable]
    public class ReqLogin
    {
        public string acct;
        public string pass;
    }
}
