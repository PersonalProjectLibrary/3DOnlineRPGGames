
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：UIWindow_StrongWnd.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/30 0:9
    功能：强化升级界面
***************************************/
#endregion

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 强化升级界面
/// </summary>
public class StrongWnd : WindowRoot
{
    #region UIDefine
    /// <summary>
    /// 左侧一列按钮图片的父物体
    /// </summary>
    public Transform leftImgPos;

    #endregion

    /// <summary>
    /// 初始化强化神经面板
    /// </summary>
    protected override void InitWnd()
    {
        base.InitWnd();
        RegClickEvts();
    }

    /// <summary>
    /// 左侧按钮图片添加点击事件监听
    /// </summary>
    private void RegClickEvts()
    {
        for (int i = 0; i < leftImgPos.childCount; i++)
        {
            Image img = leftImgPos.GetChild(i).GetComponent<Image>();

            OnClick(img.gameObject, (object args) =>
            {
                ClickPosItem((int)args);
                audioService.PlayUIAudio(Constants.UiClickBtn);
            },i);
        }
    }

    /// <summary>
    /// 点击左侧图片Item
    /// </summary>
    private void ClickPosItem(int index) { PECommon.Log("ClickItem：" + index); }

    /// <summary>
    /// 关闭强化界面
    /// </summary>
    public void ClickCloseBtn()
    {
        audioService.PlayUIAudio(Constants.UiClickBtn);
        SetWndState(false);
    }
}