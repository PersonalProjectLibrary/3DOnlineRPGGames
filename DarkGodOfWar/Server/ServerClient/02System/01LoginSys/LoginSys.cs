
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：01LoginSys_LoginSys.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/6 21:14:07
    功能：登录业务系统
***************************************/
#endregion

using PEProtocol;

/// <summary>
/// 登录业务系统
/// </summary>
public class LoginSys
{
    private static LoginSys instance = null;
    public static LoginSys Instance
    {
        get
        {
            if (instance == null) instance = new LoginSys();
            return instance;
        }
    }

    private CacheSvc cacheSvc = null;
    private TimerSvc timerSvc = null;

    /// <summary>
    /// 登录业务系统初始化
    /// </summary>
    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        timerSvc = TimerSvc.Instance;
        PECommon.Log("LoginSys Init Done.");
    }

    #region 玩家登录
    /// <summary>
    /// 处理客户端的登录请求
    /// </summary>
    /// 对NetSvc里分发过来的登录消息进行处理响应
    /// <param name="msg"></param>
    public void ReqLogin(MsgPack msgPack)
    {
        GameMsg msg = new GameMsg { cmd = (int)CMD.RspLogin };
        //检测当前账号信息
        ReqLogin data = msgPack.m_Msg.reqLogin;
        if (cacheSvc.IsAcctOnLine(data.acct)) msg.err = (int)ErrorCode.AcctIsOnline;//已上线：返回错误信息
        else
        {
            PlayerData pData = cacheSvc.GetPlayerData(data.acct, data.pass);
            if (pData == null) msg.err = (int)ErrorCode.PassWrong;//密码错误，返回错误码
            else//账号存在,进行缓存，并返回玩家信息
            {
                int powerMax = PECommon.GetPowerLimit(pData.lv);//获取体力上线
                int curPower = pData.power;//当前体力的值
                if (curPower < powerMax)//离线前体力小于体力上限，计算离线体力增长
                {
                    long curTime = timerSvc.GetNowTime();//获取本次上线时间
                    long interval = curTime - pData.offlineTime;//计算离线时长
                    //分钟转化为毫秒后计算要增长的体力
                    int addPower = (int)(interval / (1000 * 60 * PECommon.PowerAddSpace)) * PECommon.PowerAddCount;
                    //int addPower = (int)(interval / (1000 * PECommon.PowerAddSpace)) * PECommon.PowerAddCount;//测试用
                    if (addPower > 0)//离线时长大于5分钟，才有体力增长
                    {
                        pData.power += addPower;
                        if (pData.power > powerMax) pData.power = powerMax;
                    }
                }
                if (curPower != pData.power) cacheSvc.UpdatePlayerData(pData.id, pData);//体力有更新，更新数据库
                msg.rspLogin = new RspLogin { playerData = pData };
                cacheSvc.AcctOnline(data.acct, msgPack.m_Session, pData);
            }
        }
        msgPack.m_Session.SendMsg(msg);//向对应客户端回应
    }

    /// <summary>
    /// 处理客户端的重命名请求
    /// </summary>
    /// 对NetSvc里分发过来的重命名消息进行处理响应
    /// <param name="pack"></param>
    public void ReqReName(MsgPack pack)
    {
        ReqReName data = pack.m_Msg.reqReName;
        GameMsg msg = new GameMsg { cmd = (int)CMD.RspReName };
        //名字已经存在，返回错误码
        if (cacheSvc.IsNameExist(data.name)) msg.err = (int)ErrorCode.NameIsExist;
        else //名字不存在：更新缓存，以及数据库，再返回给客户端
        {
            //通过pack的Session，拿到对应玩家的缓存数据（之前创建新账号时，有把数据存到缓存里）
            PlayerData playerData = cacheSvc.GetPlayDataBySession(pack.m_Session);//获取数据
            playerData.name = data.name;//更新缓存里玩家的名字
            if (!cacheSvc.UpdatePlayerData(playerData.id, playerData))
                msg.err = (int)ErrorCode.UpdateDBaseError;
            else msg.rspReName = new RspReName { name = data.name };
        }
        pack.m_Session.SendMsg(msg);//将数据返回客户端
    }

    #endregion

    /// <summary>
    /// 玩家下线，清理缓存
    /// </summary>
    /// <param name="session"></param>
    public void ClearOfflineData(ServerSession session)
    {
        PlayerData pData = cacheSvc.GetPlayDataBySession(session);//获取玩家数据
        if (pData != null)//玩家存在
        {
            pData.offlineTime = timerSvc.GetNowTime();//更新玩家离线时间
            if (!cacheSvc.UpdatePlayerData(pData.id, pData))
                PECommon.Log("Update offline time error", LogType.Error);
            cacheSvc.AccOffLine(session);//缓存清空下线玩家数据，玩家下线
        }
    }

}
