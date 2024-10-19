
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：UIWindow_TaskWnd.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/10/19 19:18
    功能：任务奖励界面
***************************************/
#endregion

/// <summary>
/// 任务奖励界面
/// </summary>
public class TaskWnd : WindowRoot
{
    /// <summary>
    /// 初始化任务奖励面板
    /// </summary>
    protected override void InitWnd()
    {
        base.InitWnd();
    }

    /// <summary>
    /// 关闭任务奖励界面
    /// </summary>
    public void ClickCloseBtn()
    {
        audioService.PlayUIAudio(Constants.UiClickBtn);
        SetWndState(false);
    }

    /// <summary>
    /// 更新UI显示
    /// </summary>
    public void UpdateUI() { }

    /// <summary>
    /// 刷新界面显示
    /// </summary>
    private void RefreshUI() { }
}