
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

    /// <summary>
    /// 登录业务系统初始化
    /// </summary>
    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        PECommon.Log("LoginSys Init Done.");
    }

    /// <summary>
    /// 对NetSvc里分发过来的登录消息进行处理响应
    /// </summary>
    /// <param name="msg"></param>
    public void ReqLogin(MsgPack msgPack)
    {
        //设置回应客户端的消息
        GameMsg msg = new GameMsg { cmd = (int)CMD.RspLogin };
        //检测当前账号信息
        ReqLogin data = msgPack.m_Msg.reqLogin;
        if (cacheSvc.IsAcctOnLine(data.acct))
            msg.err = (int)ErrorCode.AcctIsOnline;//已上线：返回错误信息
        else
        {
            PlayerData pData = cacheSvc.GetPlayerData(data.acct, data.pass);
            if (pData == null)//密码错误，返回错误码
                msg.err = (int)ErrorCode.PassWrong;
            else//账号存在,进行缓存，并返回玩家信息
            {
                msg.rspLogin = new RspLogin { playerData = pData };
                cacheSvc.AcctOnline(data.acct, msgPack.m_Session, pData);
            }
        }
        //向对应客户端回应
        msgPack.m_Session.SendMsg(msg);
    }
}
