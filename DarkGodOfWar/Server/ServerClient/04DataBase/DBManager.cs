
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：ServerClient._04DataBase_DBManager.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/10 22:50:32
    功能：数据库管理类
***************************************/
#endregion

using MySql.Data.MySqlClient;
using PEProtocol;

public class DBManager
{
    private static DBManager instance = null;
    public static DBManager Instance
    {
        get
        {
            if (instance == null) instance = new DBManager();
            return instance;
        }
    }

    private MySqlConnection SqlConnection;

    /// <summary>
    /// 数据库管理初始化
    /// </summary>
    public void Init()
    {
        //填写连接信息
        SqlConnection = new MySqlConnection("server=localhost;User Id=root;password=;Database=sql_darkgod;Charset=utf8");
        PECommon.Log("DBManager Init Done.");
    }

    /// <summary>
    /// 根据账号密码查询玩家信息
    /// </summary>
    /// <param name="acct"></param>
    /// <param name="pass"></param>
    /// <returns></returns>
    public PlayerData QueryPlayerData(string acct,string pass)
    {
        PlayerData playerData = null;
        return playerData;
    }
}
