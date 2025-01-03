
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
    /// <summary>
    /// 日志输出
    /// </summary>
    /// <param name="msg">日志内容</param>
    /// <param name="tp">日志类型</param>
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
    /// 根据等级计算下次升级所需要的经验值
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    /// 实际工作也有对应的公式
    public static int GetExpUpValByLv(int level) { return 100 * level * level; }

    /// <summary>
    /// 增加经验并更新等级
    /// </summary>
    /// <param name="pData"></param>
    /// <param name="addExp"></param>
    public static void AddExpAndUpdateLv(PlayerData pData, int addExp)
    {
        int curLv = pData.lv;//当前级别
        int curExp = pData.exp;//当前经验值
        int addRestExp = addExp;//剩余经验值
        while (true)//循环判断是否升级
        {
            int upNeedExp = GetExpUpValByLv(curLv) - curExp;//当前升级所需要的经验值
            if (addRestExp >= upNeedExp)//进行升级
            {
                curLv += 1;//升级
                curExp = 0;
                addRestExp -= upNeedExp;
            }
            else//更新数据，结束升级
            {
                pData.lv = curLv;
                pData.exp = curExp + addRestExp;
                break;
            }
        }
    }

    #region Power Recovery：体力恢复系统
    /* 每隔5分钟，恢复2点体力 
     */

    /// <summary>
    /// 体力增加的间隙，单位是分钟
    /// </summary>
    public const int PowerAddSpace = 5;
    /// <summary>
    /// 每个间隙恢复的体力个数
    /// </summary>
    public const int PowerAddCount = 2;
    /// <summary>
    /// 获取玩家体力值上限
    /// </summary>
    /// <param name="lv"></param>
    /// <returns></returns>
    /// 计算公式一般由策划提供
    /// 等级每上涨10级，体力值获得150的上涨；
    public static int GetPowerLimit(int lv) { return ((lv - 1) / 10) * 150 + 150; }

    #endregion
}
