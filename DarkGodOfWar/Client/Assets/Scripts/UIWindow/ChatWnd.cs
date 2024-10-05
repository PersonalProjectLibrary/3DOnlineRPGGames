
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：UIWindow_ChatWnd.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/10/5 15:57
    功能：聊天界面
***************************************/
#endregion


public class ChatWnd : WindowRoot
{
    protected override void InitWnd()
    {
        base.InitWnd();
    }

    /// <summary>
    /// 关闭聊天界面
    /// </summary>
    public void ClickCloseBtn()
    {
        audioService.PlayUIAudio(Constants.UiClickBtn);
        SetWndState(false);
    }
}