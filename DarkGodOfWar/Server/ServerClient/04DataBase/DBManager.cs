
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
using System;

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
        MySqlDataReader reader = null;//存储查询出来的数据
        bool isNew = true;//区分新旧账号
        try
        {
            //Sql查询语句：选择指令 查找的数据是什么，从哪里查找，选择条件
            //其中：查找的数据用*表示，表示找出所有满足条件的数据；选择条件 这里不写的话，就是将表中所有数据列出来
            //使用@变量方式，不把数据写死，后面使用AddWithValue()替换参数，进行获取、更新数据；
            MySqlCommand cmd = new MySqlCommand("select * from account where acct = @acct", SqlConnection);
            cmd.Parameters.AddWithValue("acct", acct);//获取acct对应的数据
            reader = cmd.ExecuteReader();//执行Sql语句
            if (reader.Read())//处理数据
            {
                isNew = false;//有旧账号
                string _pass = reader.GetString("pass");
                if (_pass.Equals(pass))
                {
                    playerData = new PlayerData//密码正确，返回玩家数据
                    {
                        id = reader.GetInt32("id"),
                        name = reader.GetString("name"),
                        lv = reader.GetInt32("level"),
                        exp = reader.GetInt32("exp"),
                        power = reader.GetInt32("power"),
                        coin = reader.GetInt32("coin"),
                        diamond = reader.GetInt32("diamond")
                    };
                }
            }
        }
        catch (Exception e) { PECommon.Log("Query PlayerData By Acct&Pass Error：" + e, LogType.Error); }
        finally
        {
            if (isNew)//没有旧账号，创建新账号
            {
                playerData = new PlayerData
                {
                    id = -1,
                    name = "",
                    lv = 1,
                    exp = 0,
                    power = 150,
                    coin = 5000,
                    diamond = 500
                };
                //新账号存到数据库，并将返回的id更新到playerData里
                playerData.id = InsertNewAcctData(acct, pass, playerData);
            }
        }
        return playerData;
    }

    //将新账号数据存到数据库中
    private int InsertNewAcctData(string acct,string pass,PlayerData pData)
    {
        return 0;
    }
}
