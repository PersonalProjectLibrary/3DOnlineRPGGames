
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
        /// 服务器数据异常
        /// </summary>
        /// 一般是客户端开挂了
        /// 解决方法：强制把客户端踢下线
        ServerDataError,
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
        UpdateDBaseError,
        /// <summary>
        /// 等级不够
        /// </summary>
        LockLevel,
        /// <summary>
        /// 钻石不够
        /// </summary>
        LockDiamond,
        /// <summary>
        /// 金币不够
        /// </summary>
        LockCoin,
        /// <summary>
        /// 水晶不够
        /// </summary>
        LockCrystal,
    }

    /// <summary>
    /// 网络通信的命令号, 消息类型
    /// </summary>
    /// 对应PEMsg里的cmd字段
    public enum CMD
    {
        None = 0,
        
        //登录相关 100开始
        ReqLogin = 101,//登录请求
        RspLogin = 102,//登录回应
        ReqReName = 103,//重命名请求
        RspReName = 104,//重命名回应

        //主城相关 200开始
        ReqGuide = 201,//任务引导请求
        RspGuide = 202,//任务引导回应
        ReqStrong = 203,//装备强化升级请求
        RspStrong = 204,//装备强化升级回应
        SndWorldChat = 205,//发送世界聊天消息
        PshWorldChat = 206,//广播世界聊天消息
        ReqBuy = 207,//资源购买请求
        RspBuy = 208,//资源购买回应

        PshPower = 209,//服务端向客户端推送体力恢复消息
    }

    /// <summary>
    /// 消息包
    /// </summary>
    [Serializable]
    public class GameMsg : PEMsg
    {
        /// <summary>
        /// 服务器向客户端推送体力消息
        /// </summary>
        public PshPower pshPower;

        /// <summary>
        /// 客户端发送资源购买请求
        /// </summary>
        public ReqBuy reqBuy;
        /// <summary>
        /// 服务器回应资源购买请求
        /// </summary>
        public RspBuy rspBuy;

        /// <summary>
        /// 客户端发送世界聊天消息
        /// </summary>
        public SndWorldChat sndWorldChat;
        /// <summary>
        /// 服务器广播世界聊天消息
        /// </summary>
        public PshWorldChat pshWorldChat;

        /// <summary>
        /// 客户端发送装备强化升级请求
        /// </summary>
        public ReqStrong reqStrong;
        /// <summary>
        /// 服务器回应装备强化升级请求
        /// </summary>
        public RspStrong rspStrong;

        /// <summary>
        /// 客户端发送引导完成请求
        /// </summary>
        public ReqGuide reqGuide;
        /// <summary>
        /// 服务器回应引导完全请求
        /// </summary>
        public RspGuide rspGuide;

        #region Login And ReName
        /// <summary>
        /// 客户端发送重命名请求
        /// </summary>
        public ReqReName reqReName;
        /// <summary>
        /// 服务器回应重命名请求
        /// </summary>
        public RspReName rspReName;
        /// <summary>
        /// 客户端发送登录请求
        /// </summary>
        public ReqLogin reqLogin;
        /// <summary>
        /// 服务端回应登录请求
        /// </summary>
        public RspLogin rspLogin;

        #endregion
    }

    #region 体力恢复
    /// <summary>
    /// 服务器向客户端推送体力消息
    /// </summary>
    [Serializable]
    public class PshPower { public int power; }

    #endregion

    #region 资源交易相关
    /// <summary>
    /// 客户端发送资源购买的请求消息
    /// </summary>
    /// 花费多少钻石进行购买
    /// 购买的类型数据：购买体力还是金币；
    [Serializable]
    public class ReqBuy
    {
        /// <summary>
        /// 购买类型,0：体力；1：金币；
        /// </summary>
        public int buyType;
        /// <summary>
        /// 花费多少钻石购买
        /// </summary>
        public int diamondCost;
    }

    /// <summary>
    /// 服务器回应资源购买的请求
    /// </summary>
    /// 1、购买返回结果：类型type：买了什么；
    /// 2、购买后花掉钻石，更新钻石数量；
    /// 3、购买后金币体力值更新
    [Serializable]
    public class RspBuy
    {
        /// <summary>
        /// 购买类型,0：体力；1：金币；
        /// </summary>
        public int buyType;
        /// <summary>
        /// 剩余钻石数
        /// </summary>
        public int diamond;
        /// <summary>
        /// 购买后金币数
        /// </summary>
        public int coin;
        /// <summary>
        /// 购买后体力数
        /// </summary>
        public int power;
    }

    #endregion

    #region 世界聊天
    /// <summary>
    /// 客户端发送世界聊天消息
    /// </summary>
    /// 服务器知道谁发来的消息，客户端就不用传送发送者名字信息，只要发送消息内容
    [Serializable]
    public class SndWorldChat { public string chat; }

    /// <summary>
    /// 服务器广播世界聊天消息
    /// </summary>
    /// 向所有客户端广播消息，得指明发送者是谁，和发送的内容
    [Serializable]
    public class PshWorldChat
    {
        public string name;
        public string chat;
    }

    #endregion

    #region 装备强化升级相关
    /// <summary>
    /// 客户端发送装备强化升级的请求的消息
    /// </summary>
    /// 只需要传递位置/装备数据，
    /// 服务器里记录了等级星级金币等数据，其他数据都不用客户端请求时传递
    /// 服务器根据传递pos信息，判断是否满足强化升级条件就可以了
    [Serializable]
    public class ReqStrong { public int pos; }// 位置/装备信息

    /// <summary>
    /// 服务器回应装备强化升级的请求
    /// </summary>
    /// 1、剩余金币数、水晶数；2、升级后属性变化
    /// 3、强化数据strongArr的更新
    [Serializable]
    public class RspStrong
    {
        public int coin;//剩余金币
        public int crystal;//剩余水晶
        public int hp;//强化后属性
        public int ad;
        public int ap;
        public int addef;
        public int apdef;
        public int[] strongArr;//更新强化数据
    }

    #endregion

    #region 任务引导相关
    /// <summary>
    /// 客户端发送引导任务完成的请求的消息
    /// </summary>
    /// 请求引导的id对话，只需发送当前已经完成的引导任务的id
    [Serializable]
    public class ReqGuide { public int guideid; }// 引导任务id

    /// <summary>
    /// 服务器回应引导任务完成的请求
    /// </summary>
    /// 1、更新任务和引导的id；2、发放奖励：金币，经验值数据；
    /// 3、经验值可能会导致等级变化，还有等级数据
    [Serializable]
    public class RspGuide
    {
        public int guideid;//下个任务id
        public int coin;//获得的金币奖励
        public int exp;//获得的经验值奖励
        public int lv;//获得奖励后当前等级
    }

    #endregion

    #region 登录相关
    /// <summary>
    /// 客户端发送请求登录的消息
    /// </summary>
    [Serializable]
    public class ReqLogin
    {
        public string acct;//账号
        public string pass;//密码
    }

    /// <summary>
    /// 服务器回应登录请求的消息
    /// </summary>
    [Serializable]
    public class RspLogin { public PlayerData playerData; }//玩家数据

    /// <summary>
    /// 客户端发送重命名请求的消息
    /// </summary>
    /// 新玩家登录注册新账号起名时向服务器发送请求
    [Serializable]
    public class ReqReName { public string name; }//重命名的名字

    /// <summary>
    /// 服务器回应重命名请求的消息
    /// </summary>
    /// 判断服务器上其他人有没有使用过该名字
    [Serializable]
    public class RspReName { public string name; }//更新后的名字
    #endregion

    /// <summary>
    /// 玩家信息
    /// </summary>
    [Serializable]
    public class PlayerData
    {
        #region 用户数据
        /// <summary>
        /// ID
        /// </summary>
        public int id;
        /// <summary>
        /// 玩家名
        /// </summary>
        public string name;

        #endregion

        #region 角色属性
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
        /// <summary>
        /// 水晶
        /// </summary>
        public int crystal;
        /// <summary>
        /// 血量
        /// </summary>
        public int hp;
        /// <summary>
        /// 物理伤害
        /// </summary>
        public int ad;
        /// <summary>
        /// 法术伤害
        /// </summary>
        public int ap;
        /// <summary>
        /// 物理防御
        /// </summary>
        public int addef;
        /// <summary>
        /// 法术防御
        /// </summary>
        public int apdef;
        /// <summary>
        /// 闪避概率
        /// </summary>
        public int dodge;
        /// <summary>
        /// 穿透比率
        /// </summary>
        public int pierce;
        /// <summary>
        /// 暴击概率
        /// </summary>
        public int critical;

        #endregion

        /// <summary>
        /// 任务引导进度id
        /// </summary>
        public int guideid;

        /// <summary>
        /// 索引号即pos值，索引号里的数据即starLv
        /// </summary>
        /// 索引号，代表强化面板上选择点击了第几个位置的图片/装置/pos，要获取哪个装备数据
        /// 索引号里的数据，即配置表里对应装备的星级starLv
        /// 装备pos和装备星级starlv确定唯一的配置数据
        public int[] strongArr;
    }
}
