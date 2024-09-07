
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：03Cache_CacheSvc.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/7 22:06:40
    功能：缓存层
***************************************/
#endregion

using PEProtocol;
using System.Collections.Generic;

public class CacheSvc
{
    private static CacheSvc instance = null;
    public static CacheSvc Instance
    {
        get
        {
            if (instance == null) instance = new CacheSvc();
            return instance;
        }
    }

    /// <summary>
    /// 缓存层初始化
    /// </summary>
    public void Init()
    {
        PECommon.Log("CacheSvc Init Done.");
    }

    /// <summary>
    /// 已上线的账号及其Session
    /// </summary>
    private Dictionary<string, ServerSession> onLineAcctDic = new Dictionary<string, ServerSession>();
    /// <summary>
    /// 已上线的账号的玩家信息及Session
    /// </summary>
    private Dictionary<ServerSession, PlayerData> onLineSessionDic = new Dictionary<ServerSession, PlayerData>();

    /// <summary>
    /// 判断账号是否已上线
    /// </summary>
    /// <param name="acct"></param>
    /// <returns></returns>
    public bool IsAcctOnLine(string acct)
    {
        return onLineAcctDic.ContainsKey(acct);
    }

    /// <summary>
    /// 根据密码获取账号数据，错误返回null，账号不存在则默认创建新账号
    /// </summary>
    /// <param name="acct"></param>
    /// <param name="pass"></param>
    /// <returns></returns>
    public PlayerData GetPlayerData(string acct,string pass)
    {
        //当前账号还未上线，故缓存里不存在，要去数据库里进行查找
        return null;
    }

    /// <summary>
    /// 账号上线，缓存数据
    /// </summary>
    /// <param name="acct"></param>
    /// <param name="session"></param>
    /// <param name="playerData"></param>
    public void AcctOnline(string acct,ServerSession session,PlayerData playerData)
    {
        onLineAcctDic.Add(acct, session);
        onLineSessionDic.Add(session, playerData);
    }
}
