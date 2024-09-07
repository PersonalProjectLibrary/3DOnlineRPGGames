
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

    /// <summary>
    /// 登录业务系统初始化
    /// </summary>
    public void Init()
    {
        PECommon.Log("LoginSys Init Done.");
    }

    /// <summary>
    /// 对NetSvc里分发过来的登录消息进行处理响应
    /// </summary>
    /// <param name="msg"></param>
    public void ReqLogin(MsgPack msgPack)
    {
        //TODO，检测当前账号是否以及上线
        /* 已上线：返回错误信息
         * 未上线：检测账号是否存在
         *      存在：检测密码
         *      不存在：创建默认的账号密码（使用sdk，接第三方登录创建账号）
         */
        //回应客户端
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.RspLogin,
            rspLogin = new RspLogin { }
        };
        msgPack.m_Session.SendMsg(msg);
    }
}
