
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

    /// <summary>
    /// 网络通信的命令号, 消息类型
    /// </summary>
    /// 对应PEMsg里的cmd字段
    public enum CMD
    {
        None = 0,
        //登录相关 100
        ReqLogin = 101,//登录请求
        RspLogin = 102,//登录回应
        ReqReName = 103,//重命名请求
        RspReName = 104,//重命名回应
    }

    /// <summary>
    /// 错误码，告诉客户端当前有什么问题
    /// </summary>
    /// 对应PEMsg里的err字段
    public enum ErrorCode
    {
        /// <summary>
        /// 没有错误
        /// </summary>
        None = 0,
        /// <summary>
        /// 账号已经上线(重复登录)
        /// </summary>
        AcctIsOnline,
        /// <summary>
        /// 账号密码错误
        /// </summary>
        PassWrong,
        /// <summary>
        /// 名字已经存在
        /// </summary>
        NameIsExist,
        /// <summary>
        /// 数据库数据更新错误
        /// </summary>
        UpdateDataBase,
    }

    [Serializable]
    public class GameMsg : PEMsg
    {
        public ReqLogin reqLogin;
        public RspLogin rspLogin;
        public ReqReName reqReName;
        public RspReName rspReName;
    }

    #region 登录相关
    /// <summary>
    /// 发送请求登录的消息
    /// </summary>
    [Serializable]
    public class ReqLogin
    {
        public string acct;
        public string pass;
    }

    /// <summary>
    /// 发送回应客户端登录的消息
    /// </summary>
    [Serializable]
    public class RspLogin
    {
        public PlayerData playerData;
    }

    /// <summary>
    /// 发送请求进行重命名
    /// </summary>
    /// 新玩家登录注册新账号起名时向服务器发送请求
    [Serializable]
    public class ReqReName
    {
        public string name;
    }

    /// <summary>
    /// 服务器回应重命名请求
    /// </summary>
    /// 判断服务器上其他人有没有使用过该名字
    [Serializable]
    public class RspReName
    {
        public string name;
    }
    #endregion


    /// <summary>
    /// 玩家信息
    /// </summary>
    [Serializable]
    public class PlayerData
    {
        /// <summary>
        /// ID
        /// </summary>
        public int id;
        /// <summary>
        /// 玩家名
        /// </summary>
        public string name;
        /// <summary>
        /// 等级
        /// </summary>
        public int lv;
        /// <summary>
        /// 经验值
        /// </summary>
        public int exp;
        /// <summary>
        /// 体力
        /// </summary>
        public int power;
        /// <summary>
        /// 金币
        /// </summary>
        public int coin;
        /// <summary>
        /// 钻石
        /// </summary>
        public int diamond;
    }
}
