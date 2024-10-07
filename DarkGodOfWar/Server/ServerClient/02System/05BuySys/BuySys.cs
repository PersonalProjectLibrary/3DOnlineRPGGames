
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：05BuySys_BuySys.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/10/7 22:35:20
    功能：交易购买系统
***************************************/
#endregion

using PEProtocol;

/// <summary>
/// 交易购买系统
/// </summary>
public class BuySys
{
    private static BuySys instance = null;
    public static BuySys Instance
    {
        get
        {
            if (instance == null) instance = new BuySys();
            return instance;
        }
    }

    private CacheSvc cacheSvc = null;
    private CfgSvs cfgSvs = null;

    /// <summary>
    /// 交易购买系统初始化
    /// </summary>
    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        cfgSvs = CfgSvs.Instance;
        PECommon.Log("BuySys Init Done.");
    }

    /// <summary>
    /// 对NetSvc里分发过来的交易购买消息进行处理响应
    /// </summary>
    /// <param name="pack"></param>
    public void ReqBuy(MsgPack pack)
    {
        ReqBuy data = pack.m_Msg.reqBuy;//数据转接
        GameMsg msg = new GameMsg { cmd = (int)CMD.RspBuy };//回应客户端的消息
        PlayerData pData = cacheSvc.GetPlayDataBySession(pack.m_Session);//获取缓存层里玩家数据
        //判断升级条件
        if (pData.diamond < data.diamondCost) msg.err = (int)ErrorCode.LockDiamond;//钻石不够
        else//通过筛选，扣除花费的钻石数，相关属性值更新
        {
            pData.diamond -= data.diamondCost;
            switch (data.buyType)
            {
                case 0:pData.power += 100; break;
                case 1:pData.coin += 1000; break;
            }
        }
        //根据id号，把玩家数据更新到数据库里
        if (!cacheSvc.UpdatePlayerData(pData.id, pData))
            msg.err = (int)ErrorCode.UpdateDBaseError;
        else
        {
            msg.rspBuy = new RspBuy
            {
                buyType = data.buyType,
                diamond = pData.diamond,
                coin = pData.coin,
                power = pData.power,
            };
        }
        pack.m_Session.SendMsg(msg);//将数据返回客户端
    }
}
