
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：06PowerSys_PowerSys.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/10/11 23:07:02
    功能：体力恢复系统
***************************************/
#endregion

using PEProtocol;
using System.Collections.Generic;

/// <summary>
/// 体力恢复系统
/// </summary>
public class PowerSys
{
    private static PowerSys instance = null;
    public static PowerSys Instance
    {
        get
        {
            if (instance == null) instance = new PowerSys();
            return instance;
        }
    }

    private CacheSvc cacheSvc = null;
    private TimerSvc timerSvc = null;

    /// <summary>
    /// 交易购买系统初始化
    /// </summary>
    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        timerSvc = TimerSvc.Instance;
        TimerSvc.Instance.AddTimeTask(CalcuatePowerAdd, PECommon.PowerAddSpace, PETimeUnit.Minute, 0);
        //TimerSvc.Instance.AddTimeTask(CalcuatePowerAdd, PECommon.PowerAddSpace, PETimeUnit.Minute, 0);//测试用
        PECommon.Log("PowerSys Init Done.");
    }

    /// <summary>
    /// 增加体力计算任务
    /// </summary>
    /// <param name="tid"></param>
    /// 分为离线玩家与在线玩家两种情况
    private void CalcuatePowerAdd(int tid)
    {
        //设置体力推送消息
        PECommon.Log("All Online Player Calc Power Incress...");
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.PshPower,
            pshPower = new PshPower()
        };
        //获取在线玩家数据，遍历玩家需不需要恢复体力
        Dictionary<ServerSession, PlayerData> onlineDic = cacheSvc.GetOnlineCache();
        foreach (var item in onlineDic)
        {
            PlayerData pData = item.Value;
            ServerSession session = item.Key;
            //判断增长是否达到上限
            int powerMax = PECommon.GetPowerLimit(pData.lv);
            if (pData.power >= powerMax) continue;
            else
            {
                pData.power += PECommon.PowerAddCount;
                pData.offlineTime = timerSvc.GetNowTime();
                if (pData.power > powerMax) pData.power = powerMax;
            }
            //玩家数据更新到缓存，发送到客户端
            if (!cacheSvc.UpdatePlayerData(pData.id, pData)) msg.err = (int)ErrorCode.UpdateDBaseError;
            else
            {
                msg.pshPower.power = pData.power;
                session.SendMsg(msg);
            }
        }
    }
}
