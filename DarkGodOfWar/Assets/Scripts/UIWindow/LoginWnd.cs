
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：UIWindow_LoginWnd.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/8/30 17:03
    功能：登录注册界面
***************************************/
#endregion

using UnityEngine;
using UnityEngine.UI;

public class LoginWnd : WindowRoot
{
    public InputField iptAcct;
    public InputField iptPass;
    public Button btnEnter;
    public Button btnNotice;

    /// <summary>
    /// 界面打开时初始化
    /// </summary>
    protected override void InitWnd()
    {
        base.InitWnd();

        //获取本地存储的账号和密码
        if (PlayerPrefs.HasKey("Acct") && PlayerPrefs.HasKey("Pass"))
        {
            SetText(iptAcct, PlayerPrefs.GetString("Acct"));
            SetText(iptPass, PlayerPrefs.GetString("Pass"));
        }
        else
        {
            SetText(iptAcct, "");
            SetText(iptPass, "");
        }
    }

    //TODO 更新本地存储和账号密码
}