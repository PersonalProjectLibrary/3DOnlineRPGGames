
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：ServerClient._04DataBase_DBManager.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/10 22:50:32
    功能：数据库管理层
***************************************/
#endregion

using MySql.Data.MySqlClient;
using PEProtocol;
using System;

/// <summary>
/// 数据库管理层
/// </summary>
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
        SqlConnection.Open();
        PECommon.Log("DBManager Init Done.");

        //QueryPlayerData("xxx", "oooo");//测试查询玩家数据（账号不存在则新建账号）
    }

    #region 数据库相关操作
    /// <summary>
    /// 查询玩家信息
    /// </summary>
    /// <param name="acct"></param>
    /// <param name="pass"></param>
    /// <returns></returns>
    public PlayerData QueryPlayerData(string acct, string pass)
    {
        PlayerData playerData = null;
        MySqlDataReader reader = null;//存储查询出来的数据
        bool isNew = true;//区分新旧账号
        try
        {
            /* Sql查询语句：选择指令 查找的数据是什么，从哪里查找，选择条件
            //其中：查找的数据用*表示，表示找出所有满足条件的数据；选择条件 这里不写的话，就是将表中所有数据列出来
            //使用@变量方式，不把数据写死，后面使用AddWithValue()替换参数，进行获取、更新数据；
            */
            MySqlCommand cmd = new MySqlCommand("select * from account where acct = @acct", SqlConnection);
            cmd.Parameters.AddWithValue("acct", acct);//获取acct对应的数据
            reader = cmd.ExecuteReader();//执行Sql语句
            if (reader.Read())//处理数据
            {
                isNew = false;//有旧账号
                string _pass = reader.GetString("pass");
                if (_pass.Equals(pass)) GetAcctData(reader, ref playerData);
            }
        }
        catch (Exception e) { PECommon.Log("Query PlayerData By Acct&Pass Error：" + e, LogType.Error); }
        finally
        {
            //防止前面查数据时，没有关闭reader，导致下面插入数据报错
            if (reader != null) reader.Close();
            if (isNew) CreateNewAcct(acct, pass, ref playerData);//没有旧账号，创建新账号
        }
        return playerData;
    }

    /// <summary>
    /// 存储玩家信息
    /// </summary>
    /// <param name="acct"></param>
    /// <param name="pass"></param>
    /// <param name="pData"></param>
    /// <returns></returns>
    private int InsertNewAcctData(string acct, string pass, PlayerData pData)
    {
        int id = -1;//插入的玩家数据的初始id，插入到数据库里正式赋值生成
        try
        {
            MySqlCommand cmd = new MySqlCommand("insert into account set acct=@acct,pass=@pass,name=@name,"
                + "level=@level,exp=@exp,power=@power,coin=@coin,diamond=@diamond,crystal=@crystal,"
                + "hp=@hp,ad=@ad,ap=@ap,addef=@addef,apdef=@apdef,dodge=@dodge,pierce=@pierce,"
                + "critical=@critical,guideid=@guideid,strong=@strong,offlinetime = @offlinetime,"
                + "taskreward=@taskreward", SqlConnection);

            #region 玩家账号
            cmd.Parameters.AddWithValue("acct", acct);
            cmd.Parameters.AddWithValue("pass", pass);
            cmd.Parameters.AddWithValue("name", pData.name);

            #endregion

            #region 玩家财产
            cmd.Parameters.AddWithValue("level", pData.lv);
            cmd.Parameters.AddWithValue("exp", pData.exp);
            cmd.Parameters.AddWithValue("power", pData.power);
            cmd.Parameters.AddWithValue("coin", pData.coin);
            cmd.Parameters.AddWithValue("diamond", pData.diamond);
            cmd.Parameters.AddWithValue("crystal", pData.crystal);

            #endregion

            #region 玩家属性
            cmd.Parameters.AddWithValue("hp", pData.hp);
            cmd.Parameters.AddWithValue("ad", pData.ad);
            cmd.Parameters.AddWithValue("ap", pData.ap);
            cmd.Parameters.AddWithValue("addef", pData.addef);
            cmd.Parameters.AddWithValue("apdef", pData.apdef);
            cmd.Parameters.AddWithValue("dodge", pData.dodge);
            cmd.Parameters.AddWithValue("pierce", pData.pierce);
            cmd.Parameters.AddWithValue("critical", pData.critical);

            #endregion

            #region 强化升级
            string strongInfo = "";
            for (int i = 0; i < pData.strongArr.Length; i++)
            {
                strongInfo += pData.strongArr[i];
                strongInfo += "#";
            }
            cmd.Parameters.AddWithValue("strong", strongInfo);

            #endregion

            #region 任务奖励
            string taskRewardInfo = "";
            for (int i = 0; i < pData.taskRewardArr.Length; i++)
            {
                taskRewardInfo += pData.taskRewardArr[i];
                taskRewardInfo += "#";
            }
            cmd.Parameters.AddWithValue("taskreward", taskRewardInfo);

            #endregion

            cmd.Parameters.AddWithValue("guideid", pData.guideid);//任务引导
            cmd.Parameters.AddWithValue("offlinetime", pData.offlineTime);//最后某一上线/离线时间

            cmd.ExecuteNonQuery();
            id = (int)cmd.LastInsertedId;
        }
        catch (Exception e) { PECommon.Log("Insert PlayerData Error：" + e, LogType.Error); }
        return id;
    }

    /// <summary>
    /// 更新玩家数据
    /// </summary>
    /// <param name="id"></param>
    /// <param name="playerData"></param>
    /// <returns></returns>
    public bool UpdatePlayerData(int id, PlayerData playerData)
    {
        try
        {
            MySqlCommand cmd = new MySqlCommand("update account set name=@name,level=@level,"
                + "exp=@exp,power=@power,coin=@coin,diamond=@diamond,crystal=@crystal,hp=@hp,"
                + "ad=@ad,ap=@ap,addef=@addef,apdef=@apdef,dodge=@dodge,pierce=@pierce,"
                + "critical=@critical,guideid=@guideid,strong=@strong,offlinetime = @offlinetime,"
                + "taskreward=@taskreward where id =@id", SqlConnection);

            #region 玩家账号
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("name", playerData.name);

            #endregion

            #region 玩家财产
            cmd.Parameters.AddWithValue("level", playerData.lv);
            cmd.Parameters.AddWithValue("exp", playerData.exp);
            cmd.Parameters.AddWithValue("power", playerData.power);
            cmd.Parameters.AddWithValue("coin", playerData.coin);
            cmd.Parameters.AddWithValue("diamond", playerData.diamond);
            cmd.Parameters.AddWithValue("crystal", playerData.crystal);

            #endregion

            #region 玩家属性
            cmd.Parameters.AddWithValue("hp", playerData.hp);
            cmd.Parameters.AddWithValue("ad", playerData.ad);
            cmd.Parameters.AddWithValue("ap", playerData.ap);
            cmd.Parameters.AddWithValue("addef", playerData.addef);
            cmd.Parameters.AddWithValue("apdef", playerData.apdef);
            cmd.Parameters.AddWithValue("dodge", playerData.dodge);
            cmd.Parameters.AddWithValue("pierce", playerData.pierce);
            cmd.Parameters.AddWithValue("critical", playerData.critical);

            #endregion

            #region 强化升级
            string strongInfo = "";
            for (int i = 0; i < playerData.strongArr.Length; i++)
            {
                strongInfo += playerData.strongArr[i];
                strongInfo += "#";
            }
            cmd.Parameters.AddWithValue("strong", strongInfo);

            #endregion

            #region 任务奖励
            string taskRewardInfo = "";
            for (int i = 0; i < playerData.taskRewardArr.Length; i++)
            {
                taskRewardInfo += playerData.taskRewardArr[i];
                taskRewardInfo += "#";
            }
            cmd.Parameters.AddWithValue("taskreward", taskRewardInfo);

            #endregion

            cmd.Parameters.AddWithValue("guideid", playerData.guideid);//任务引导
            cmd.Parameters.AddWithValue("offlinetime", playerData.offlineTime);//最后某一上线/离线时间

            cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
            PECommon.Log("Update PlayerData Error：" + e, LogType.Error);
            return false;
        }
        return true;
    }

    #endregion

    #region Tool Function
    /// <summary>
    /// 获取账号数据
    /// </summary>
    /// <param name="playerData"></param>
    private void GetAcctData(MySqlDataReader reader, ref PlayerData playerData)
    {
        playerData = new PlayerData
        {
            id = reader.GetInt32("id"),
            name = reader.GetString("name"),
            lv = reader.GetInt32("level"),
            exp = reader.GetInt32("exp"),
            power = reader.GetInt32("power"),
            coin = reader.GetInt32("coin"),
            diamond = reader.GetInt32("diamond"),
            crystal = reader.GetInt32("crystal"),
            hp = reader.GetInt32("hp"),
            ad = reader.GetInt32("ad"),
            ap = reader.GetInt32("ap"),
            addef = reader.GetInt32("addef"),
            apdef = reader.GetInt32("apdef"),
            dodge = reader.GetInt32("dodge"),
            pierce = reader.GetInt32("pierce"),
            critical = reader.GetInt32("critical"),
            guideid = reader.GetInt32("guideid"),
            offlineTime = reader.GetInt64("offlinetime"),
        };

        #region StrongArr 获取数据库强化升级数据
        //解析数据库里的强化数据，数据库里字符格式存储，如：1#2#2#4#3#7#
        string[] strongStrArr = reader.GetString("strong").Split('#');
        int[] _strongArr = new int[6];
        for (int i = 0; i < strongStrArr.Length; i++)
        {
            if (strongStrArr[i] == "") continue;
            if (int.TryParse(strongStrArr[i], out int starLv))
                _strongArr[i] = starLv;
            else PECommon.Log("Parse strong Data Error", LogType.Error);
        }
        playerData.strongArr = _strongArr;
        #endregion

        #region TaskRewardArr 获取数据库里的任务奖励数据
        //数据库里字符格式存储，如：2|0|0#3|1|0#6|0|0#7|1|1#8|1|1#9|0|0#
        string[] taskRewardStrArr = reader.GetString("taskreward").Split('#');
        playerData.taskRewardArr = new string[6];
        //任务多时字符串可能会很长，避免数据错误，做个安全性的校验避免异常数据
        //如分割后的任务项目数据：2|0|0 ，数据如任务id，可能不止一位但至少一位，总长度至少5位
        for (int i = 0; i < taskRewardStrArr.Length; i++)
        {
            if (taskRewardStrArr[i] != "")
            {
                if (taskRewardStrArr[i].Length < 5) throw new Exception("DataError");
                else playerData.taskRewardArr[i] = taskRewardStrArr[i];
            }
        }
        #endregion
    }

    /// <summary>
    /// 创建新账号
    /// </summary>
    /// <param name="acct"></param>
    /// <param name="pass"></param>
    /// <param name="playerData"></param>
    private void CreateNewAcct(string acct, string pass, ref PlayerData playerData)
    {
        playerData = new PlayerData
        {
            id = -1,
            name = "",
            lv = 1,
            exp = 0,
            power = 150,
            coin = 5000,
            diamond = 500,
            crystal = 500,
            hp = 2000,
            ad = 275,
            ap = 265,
            addef = 67,
            apdef = 43,
            dodge = 7,
            pierce = 5,
            critical = 2,
            guideid = 1001,
            strongArr = new int[6],//新账号默认都是0
            offlineTime = TimerSvc.Instance.GetNowTime(),//新账号默认使用创建时间
            taskRewardArr = new string[6],
        };
        //对任务奖励数据进行初始化设置，不能直接插入
        for (int i = 0; i < playerData.taskRewardArr.Length; i++)
        {
            playerData.taskRewardArr[i] = (i + 1) + "|0|0";
        }
        playerData.id = InsertNewAcctData(acct, pass, playerData);
    }

    /// <summary>
    /// 查询数据库里是否存在某个名字
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public bool QueryNameData(string name)
    {
        bool exist = false;
        MySqlDataReader reader = null;
        try //名字是否已经存在 
        {
            MySqlCommand cmd = new MySqlCommand("select * from account where name= @name", SqlConnection);
            cmd.Parameters.AddWithValue("name", name);
            reader = cmd.ExecuteReader();
            if (reader.Read()) exist = true;
        }
        catch (Exception e) { PECommon.Log("Query Name State Error：" + e, LogType.Error); }
        finally
        {
            if (reader != null) reader.Close();
        }
        return exist;
    }

    #endregion
}
