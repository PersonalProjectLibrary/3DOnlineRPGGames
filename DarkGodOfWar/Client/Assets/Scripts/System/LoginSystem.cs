
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：System_LoginSystem.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/8/30 10:58
    功能：登录注册业务系统
***************************************/
#endregion

using PEProtocol;
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

        PECommon.Log("Init LoginSystem...");
    }

    /// <summary>
    /// 进入登录场景
    /// </summary>
    public void EnterLogin()
    {
        resService.AsyncLoadScene(Constants.SceneLogin, () => 
        {
            loginWnd.SetWndState();//加载登录界面
            audioService.PlayBgMusic(Constants.BgmLogin);//播放登录界面背景音乐
        });
    }

    /// <summary>
    /// 接收登录请求的服务器回应
    /// </summary>
    public void RespondLogin(GameMsg msg)
    {
        GameRoot.AddTips("登录成功");
        GameRoot.Instance.SetPlayerDataByLogin(msg.rspLogin);//保存返回的玩家信息
        if (msg.rspLogin.playerData.name == "") createWnd.SetWndState();//进入角色创建界面
        else MainCitySystem.Instance.EnterMainCity();//进入主城
        loginWnd.SetWndState(false);//关闭登录界面
    }

    /// <summary>
    /// 接收玩家创建角色重命名请求的服务器回应
    /// </summary>
    /// <param name="msg"></param>
    public void RspRename(GameMsg msg)
    {
        GameRoot.Instance.SetPlayerName(msg.rspReName.name);//客户端更新玩家姓名
        MainCitySystem.Instance.EnterMainCity();//进入主城
        createWnd.SetWndState(false);//关闭当前创建界面
    }
}