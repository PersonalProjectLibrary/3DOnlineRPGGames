
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：PEProtocol_PECommon.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/7 11:08:46
    功能：客户端服务端共用工具类
***************************************/
#endregion

using PENet;

/// <summary>
/// 消息等级
/// </summary>
public enum LogType
{
    Log = 0,
    Warn =1,
    Error =2,
    Info =3
}

public class PECommon
{
    public static void Log(string msg="",LogType tp = LogType.Log)
    {
        LogLevel lv = (LogLevel)tp;
        PETool.LogMsg(msg, lv);
    }
}
