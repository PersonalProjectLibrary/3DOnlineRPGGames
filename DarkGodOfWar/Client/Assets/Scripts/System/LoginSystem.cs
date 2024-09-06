
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：System_LoginSystem.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/8/30 10:58
    功能：登录注册业务系统
***************************************/
#endregion

using UnityEngine;

public class LoginSystem : SystemRoot
{
    public static LoginSystem Instance =null;

    public LoginWnd loginWnd;
    public CreateWnd createWnd;

    /// <summary>
    /// 初始化登录注册系统
    /// </summary>
    public override void InitSystem()
    {
        base.InitSystem();

        Instance=this;

        Debug.Log("Init LoginSystem...");
    }

    /// <summary>
    /// 进入登录场景
    /// </summary>
    public void EnterLogin()
    {
        resService.AsyncLoadScene(Constants.SceneLogin, () => 
        {
            loginWnd.SetWndState();//加载登录界面
            audioService.PlayBgMusic(Constants.BgLogin);//播放登录界面背景音乐
        });
    }

    /// <summary>
    /// 接收网络回应
    /// </summary>
    public void RespondLogin()
    {
        GameRoot.AddTips("登录成功");

        createWnd.SetWndState();
        loginWnd.SetWndState(false);
    }
}