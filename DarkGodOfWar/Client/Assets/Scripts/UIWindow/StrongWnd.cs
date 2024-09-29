
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：UIWindow_StrongWnd.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/30 0:9
    功能：强化升级界面
***************************************/
#endregion

/// <summary>
/// 强化升级界面
/// </summary>
public class StrongWnd : WindowRoot
{
    #region UIDefine

    #endregion

    /// <summary>
    /// 初始化强化神经面板
    /// </summary>
    protected override void InitWnd()
    {
        base.InitWnd();
    }

    /// <summary>
    /// 关闭强化界面
    /// </summary>
    public void ClickCloseBtn()
    {
        audioService.PlayUIAudio(Constants.UiClickBtn);
        SetWndState(false);
    }
}