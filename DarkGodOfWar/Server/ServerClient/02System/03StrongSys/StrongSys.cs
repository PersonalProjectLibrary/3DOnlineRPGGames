
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：03StrongSys_StrongSys.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/10/4 15:14:02
    功能：强化升级业务系统
***************************************/
#endregion

using PEProtocol;

/// <summary>
/// 强化升级业务系统
/// </summary>
public class StrongSys
{
    private static StrongSys instance = null;
    public static StrongSys Instance
    {
        get
        {
            if (instance == null) instance = new StrongSys();
            return instance;
        }
    }

    private CacheSvc cacheSvc = null;
    private CfgSvs cfgSvs = null;

    /// <summary>
    /// 强化升级系统初始化
    /// </summary>
    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        cfgSvs = CfgSvs.Instance;
        PECommon.Log("StrongSys Init Done.");
    }

    /// <summary>
    /// 对NetSvc里分发过来的引导消息进行处理响应
    /// </summary>
    /// <param name="pack"></param>
    public void ReqStrong(MsgPack pack)
    {
        ReqStrong data = pack.m_Msg.reqStrong;//数据转接
        GameMsg msg = new GameMsg { cmd = (int)CMD.RspGuide };//回应客户端的消息
        PlayerData pData = cacheSvc.GetPlayDataBySession(pack.m_Session);//获取缓存层里玩家数据
        //判断升级条件
        int curStarLv = pData.strongArr[data.pos];
        StrongCfg nextSg = cfgSvs.GetStrongCfg(data.pos, curStarLv+1);//获取升级所需的条件的数据
        if (pData.lv < nextSg.minLv) msg.err = (int)ErrorCode.LockLevel;//等级不够
        else if (pData.coin < nextSg.coin) msg.err = (int)ErrorCode.LockCoin;//金币不够
        else if (pData.crystal < nextSg.crystal) msg.err = (int)ErrorCode.LockCrystal;//水晶不够
        else//通过筛选，扣除升级所用的资源，相关属性值更新
        {
            pData.coin -= nextSg.coin;
            pData.crystal -= nextSg.crystal;
            pData.strongArr[data.pos] += 1;//星级升级
            pData.hp += nextSg.addHp;
            pData.ad += nextSg.addHurt;
            pData.ap += nextSg.addHurt;
            pData.addef += nextSg.addDef;
            pData.apdef += nextSg.addDef;
        }
        //根据id号，把玩家数据更新到数据库里
        if (!cacheSvc.UpdatePlayerData(pData.id, pData))
            msg.err = (int)ErrorCode.UpdateDBaseError;
        else
        {
            msg.rspStrong = new RspStrong
            {
                coin = pData.coin,
                crystal = pData.crystal,
                hp = pData.hp,
                ad = pData.ad,
                ap = pData.ap,
                addef = pData.addef,
                apdef = pData.apdef,
                strongArr = pData.strongArr
            };
        }
        pack.m_Session.SendMsg(msg);//将数据返回客户端
    }
}
