
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：Service_NetService.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/7 0:59
    功能：网络服务
***************************************/
#endregion

using PENet;
using PEProtocol;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 网络服务
/// </summary>
public class NetService : MonoBehaviour
{
    public static NetService Instance = null;

    PESocket<ClientSession, GameMsg> client;

    private static readonly string lockObj = "lock";
    private Queue<GameMsg> msgQue = new Queue<GameMsg>();

    /// <summary>
    /// 网络服务初始化
    /// </summary>
    public void InitService()
    {
        Instance = this;
        PECommon.Log("Init NetService...");
        client = new PESocket<ClientSession, GameMsg>();
        //设置客户端日志接口
        client.SetLog(true, (string msg, int lv) =>
        {
            switch (lv)
            {
                case 0:
                    msg = "Log：" + msg;
                    Debug.Log(msg);
                    break;
                case 1:
                    msg = "Warn：" + msg;
                    Debug.LogWarning(msg);
                    break;
                case 2:
                    msg = "Error：" + msg;
                    Debug.LogError(msg);
                    break;
                case 3:
                    msg = "Info：" + msg;
                    Debug.Log(msg);
                    break;
            }
        });
        client.StartAsClient(SrvCfg.srvIP, SrvCfg.srvPort);//启动客户端
    }

    public void Update()
    {
        if (msgQue.Count > 0)
        {
            lock (lockObj) { ProcessMsg(msgQue.Dequeue()); }
        }
    }

    /// <summary>
    /// 具体的发送函数
    /// </summary>
    /// <param name="msg"></param>
    public void SendMsg(GameMsg msg)
    {
        if (client.session != null) client.session.SendMsg(msg);
        else
        {
            GameRoot.AddTips("服务器未连接");
            InitService();
        }
    }

    /// <summary>
    /// 将从服务器接收到消息放到消息队列中
    /// </summary>
    /// <param name="msg"></param>
    public void AddNetPkg(GameMsg msg)
    {
        lock (lockObj) { msgQue.Enqueue(msg); }
    }

    /// <summary>
    /// 对消息进行分发
    /// </summary>
    /// <param name="msg"></param>
    public void ProcessMsg(GameMsg msg)
    {
        if (msg.err != (int)ErrorCode.None)//返回的是错误信息
        {
            switch ((ErrorCode)msg.err)
            {
                case ErrorCode.AcctIsOnline: GameRoot.AddTips("当前账号已经上线"); break;
                case ErrorCode.PassWrong: GameRoot.AddTips("密码错误"); break;
                case ErrorCode.UpdateDBaseError:
                    PECommon.Log("数据库更新异常", LogType.Error);
                    GameRoot.AddTips("网络不稳定");
                    break;
                case ErrorCode.ServerDataError:
                    PECommon.Log("服务器数据异常", LogType.Error);
                    GameRoot.AddTips("客户端数据异常");
                    break;
                case ErrorCode.ClientDataError:
                    PECommon.Log("客户端数据异常", LogType.Error);
                    GameRoot.AddTips("客户端数据异常");
                    break;
                case ErrorCode.LockLevel: GameRoot.AddTips("角色等级不够"); break;
                case ErrorCode.LockCoin: GameRoot.AddTips("金币数量不够"); break;
                case ErrorCode.LockCrystal: GameRoot.AddTips("水晶数量不够"); break;
                case ErrorCode.LockDiamond:GameRoot.AddTips("钻石数量不够");break;
            }
            return;
        }

        switch ((CMD)msg.cmd)//将信息分发出去
        {
            case CMD.RspLogin: LoginSystem.Instance.RespondLogin(msg); break;
            case CMD.RspReName: LoginSystem.Instance.RspRename(msg); break;
            case CMD.RspGuide: MainCitySystem.Instance.RspGuide(msg); break;
            case CMD.RspStrong: MainCitySystem.Instance.RspStrong(msg); break;
            case CMD.PshWorldChat:MainCitySystem.Instance.PshWorldChat(msg);break;
            case CMD.RspBuy:MainCitySystem.Instance.RspBuy(msg);break;
            case CMD.PshPower:MainCitySystem.Instance.PshPower(msg);break;
            case CMD.RspTaskReward:MainCitySystem.Instance.RspTReward(msg);break;
            default:break;
        }
    }
}