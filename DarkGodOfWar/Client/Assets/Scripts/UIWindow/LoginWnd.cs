
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：UIWindow_LoginWnd.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/8/30 17:03
    功能：登录注册界面
***************************************/
#endregion

using PEProtocol;
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

    /// <summary>
    /// 点击进入游戏按钮
    /// </summary>
    public void ClickEntBtn()
    {
        string _acct = iptAcct.text;
        string _pass = iptPass.text;

        audioService.PlayUIAudio(Constants.UiLoginBtn);
        if (_acct != "" && _pass != "")//更新本地存储和账号密码
        {
            PlayerPrefs.SetString("Acct", _acct);
            PlayerPrefs.SetString("Pass", _pass);
            //发送网络消息请求登录
            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.ReqLogin,//消息类型
                reqLogin = new ReqLogin//消息内容
                {
                    acct = _acct,
                    pass = _pass
                }
            };

            netService.SendMsg(msg);

            //这里模拟请求成功，当前是新账号，打开创建角色面板
            //LoginSystem.Instance.RespondLogin();
            //已有的账号时，直接进入游戏
        }
        else GameRoot.AddTips("账号或密码为空");
    }

    public void ClickNoticeBtn()
    {
        audioService.PlayUIAudio(Constants.UiClickBtn);
        GameRoot.AddTips("功能正在开发中...");
    }
}