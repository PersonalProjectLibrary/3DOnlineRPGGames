
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

/// <summary>
/// 服务器缓存层
/// </summary>
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
    /// 已上线的账号及其Session
    /// </summary>
    private Dictionary<string, ServerSession> onLineAcctDic = new Dictionary<string, ServerSession>();

    /// <summary>
    /// 已上线的账号的玩家信息及Session
    /// </summary>
    private Dictionary<ServerSession, PlayerData> onLineSessionDic = new Dictionary<ServerSession, PlayerData>();

    private DBManager dbManager;

    /// <summary>
    /// 缓存层初始化
    /// </summary>
    public void Init()
    {
        dbManager = DBManager.Instance;
        PECommon.Log("CacheSvc Init Done.");
    }

    #region 玩家登录注册上线
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
    public PlayerData GetPlayerData(string acct, string pass)
    {
        //当前账号还未上线，故缓存里不存在，要去数据库里进行查找
        return dbManager.QueryPlayerData(acct, pass);
    }

    /// <summary>
    /// 账号上线，缓存数据
    /// </summary>
    /// <param name="acct"></param>
    /// <param name="session"></param>
    /// <param name="playerData"></param>
    public void AcctOnline(string acct, ServerSession session, PlayerData playerData)
    {
        onLineAcctDic.Add(acct, session);
        onLineSessionDic.Add(session, playerData);
    }

    /// <summary>
    /// 判断某个名字是否已经存在
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool IsNameExist(string name)
    {
        return DBManager.Instance.QueryNameData(name);
    }

    /// <summary>
    /// 根据玩家连接信息，获取玩家信息
    /// </summary>
    /// <param name="session"></param>
    /// <returns></returns>
    public PlayerData GetPlayDataBySession(ServerSession session)
    {
        if (onLineSessionDic.TryGetValue(session, out PlayerData playerData)) return playerData;
        else return null;
    }

    /// <summary>
    /// 将玩家数据同步更新到数据库
    /// </summary>
    /// <param name="id"></param>
    /// <param name="playerData"></param>
    /// <returns></returns>
    public bool UpdatePlayerData(int id, PlayerData playerData)
    {
        return dbManager.UpdatePlayerData(id, playerData);
    }
    #endregion

    /// <summary>
    /// 玩家下线清理数据
    /// </summary>
    /// <param name="session"></param>
    public void AccOffLine(ServerSession session)
    {
        foreach (var item in onLineAcctDic)
        {
            if(item.Value == session)
            {
                onLineAcctDic.Remove(item.Key);
                break;
            }
        }
        bool succ = onLineSessionDic.Remove(session);
        PECommon.Log("SessionID：" + session.sessionID + " Offline Result " + succ);
    }
}
