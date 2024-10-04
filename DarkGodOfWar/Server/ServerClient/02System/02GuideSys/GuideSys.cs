
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：02GuideSys_GuideSys.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/27 20:36:05
    功能：引导业务系统
***************************************/
#endregion

using PEProtocol;

/// <summary>
/// 引导业务系统
/// </summary>
public class GuideSys
{
    private static GuideSys instance = null;
    public static GuideSys Instance
    {
        get
        {
            if (instance == null) instance = new GuideSys();
            return instance;
        }
    }

    private CacheSvc cacheSvc = null;
    private CfgSvs cfgSvs = null;

    /// <summary>
    /// 引导业务系统初始化
    /// </summary>
    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        cfgSvs = CfgSvs.Instance;
        PECommon.Log("GuideSys Init Done.");
    }

    /// <summary>
    /// 对NetSvc里分发过来的引导消息进行处理响应
    /// </summary>
    /// <param name="pack"></param>
    public void ReqGuide(MsgPack pack)
    {
        ReqGuide data = pack.m_Msg.reqGuide;
        GameMsg msg = new GameMsg { cmd = (int)CMD.RspGuide };
        PlayerData pData = cacheSvc.GetPlayDataBySession(pack.m_Session);//获取缓存层里玩家数据
        if (pData.guideid == data.guideid)//确认客户端数据和玩家数据相等同步
        {
            pData.guideid += 1;//更新任务id
            /* 读取配置表文件，更新玩家数据：
             * 获取当前任务的奖励，并把奖励数据更新到数据库里，最后把更新的结果发回给客户端
             * 当前服务器只有任务id，没有任务对应的奖励信息，奖励信息在配置文件里，服务器里不存在；
             * 也没有让客户端把奖励数据传到服务器，只传了引导任务id
             * 原因是：客户端和服务器通信只穿有必要的数据，
             * 一方面保证安全性，避免服务器数据篡改异常；另一方面减少服务器带宽流量；
             * 像奖励金币经验这种，可以通过配置文件读取，没必要在网络通信时传递；
             * 后续添加模块服务器统一读取配置文件
            //*/
            GuideCfg guideCfg = cfgSvs.GetGuideCfg(data.guideid);//获取引导数据
            pData.coin += guideCfg.coin;//更新玩家金币数值
            CaculateExp(pData, guideCfg.exp);//更新玩家等级和经验值
            //根据id号，把玩家数据更新到数据库里
            if (!cacheSvc.UpdatePlayerData(pData.id, pData))
                msg.err = (int)ErrorCode.UpdateDBaseError;
            else
            {
                msg.rspGuide = new RspGuide//玩家更新后的数据赋值到数据库
                {
                    guideid = pData.guideid,
                    coin = pData.coin,
                    lv = pData.lv,
                    exp = pData.exp
                };
            }
        }
        else msg.err = (int)ErrorCode.ServerDataError;
        pack.m_Session.SendMsg(msg);//将数据返回客户端
    }

    /// <summary>
    /// 经验升级计算
    /// </summary>
    /// <param name="pData">玩家数据</param>
    /// <param name="addExp">新增经验值</param>
    private void CaculateExp(PlayerData pData,int addExp)
    {
        int curLv = pData.lv;//当前级别
        int curExp = pData.exp;//当前经验值
        int addRestExp = addExp;//剩余经验值
        while (true)//循环判断是否升级
        {
            int upNeedExp = PECommon.GetExpUpValByLv(curLv) - curExp;//当前升级所需要的经验值
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
}
