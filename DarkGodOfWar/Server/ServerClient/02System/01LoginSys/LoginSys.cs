
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：01LoginSys_LoginSys.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/6 21:14:07
    功能：登录业务系统
***************************************/
#endregion

using PENet;

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
        PETool.LogMsg("LoginSys Init Done.");
    }
}
