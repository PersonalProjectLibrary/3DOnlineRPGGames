
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：UIWindow_CreateWnd.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/8/31 20:9
    功能：角色创建界面
***************************************/
#endregion

using PEProtocol;
using UnityEngine.UI;

/// <summary>
/// 角色创建界面
/// </summary>
public class CreateWnd : WindowRoot
{
    public InputField iptName;

    protected override void InitWnd()
    {
        base.InitWnd();
        iptName.text = resService.GetRdNameData();
    }

    public void ClickRandomBtn()
    {
        audioService.PlayUIAudio(Constants.UiClickBtn);
        string rdName = resService.GetRdNameData();
        iptName.text = rdName;
    }

    public void ClickEnterBtn()
    {
        audioService.PlayUIAudio(Constants.UiClickBtn);
        if (iptName.text == "") GameRoot.AddTips("当前名字不合法");
        else
        {
            //发送名字数据到服务器，登录主城
            GameMsg msg = new GameMsg
            {
                cmd = (int)CMD.ReqReName,
                reqReName = new ReqReName { name = iptName.text }
            };
            netService.SendMsg(msg);
            //GameRoot.AddTips("登录主城");
        }
    }
}