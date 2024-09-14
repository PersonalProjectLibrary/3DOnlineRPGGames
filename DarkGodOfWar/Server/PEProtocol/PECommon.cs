
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：PEProtocol_PECommon.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/7 11:08:46
    功能：客户端服务端共用工具类
***************************************/
#endregion

using PENet;
using PEProtocol;

/// <summary>
/// 消息等级
/// </summary>
public enum LogType
{
    Log = 0,
    Warn =1,
    Error =2,
    Info =3
}

public class PECommon
{
    public static void Log(string msg="",LogType tp = LogType.Log)
    {
        LogLevel lv = (LogLevel)tp;
        PETool.LogMsg(msg, lv);
    }

    /// <summary>
    /// 计算玩家战斗力
    /// </summary>
    /// <param name="pData"></param>
    /// <returns></returns>
    /// 玩家战斗力计算公式一般由策划给出
    /// 等级*100+物理伤害+法术伤害+物理防御+法术防御
    public static int GetFightByProps(PlayerData pData)
    {
        return pData.lv * 100 + pData.ad + pData.ap + pData.addef + pData.apdef;
    }

    /// <summary>
    /// 获取玩家体力值上限
    /// </summary>
    /// <param name="lv"></param>
    /// <returns></returns>
    /// 计算公式一般由策划提供
    /// 等级每上涨10级，体力值获得150的上涨；
    public static int GetPowerLimit(int lv)
    {
        return ((lv - 1) / 10) * 150 + 150;
    }
}
