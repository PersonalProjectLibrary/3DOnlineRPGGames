
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：UIWindow_BuyWnd.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/10/7 14:4
    功能：购买交易窗口
***************************************/
#endregion

/// <summary>
/// 购买交易窗口
/// </summary>
/// 1、主城场景左上角体力条后边的“购买”按钮，
/// 2、主城场景右下角的“铸造”按钮，把钻石铸造成金币
/// 这两处地方点击打开购买交易窗口
public class BuyWnd : WindowRoot
{
    /// <summary>
    /// 初始化购买交易窗口
    /// </summary>
    protected override void InitWnd()
    {
        base.InitWnd();
    }

    /// <summary>
    /// 关闭购买交易窗口
    /// </summary>
    public void ClickCloseBtn()
    {
        audioService.PlayUIAudio(Constants.UiClickBtn);
        SetWndState(false);
    }
}