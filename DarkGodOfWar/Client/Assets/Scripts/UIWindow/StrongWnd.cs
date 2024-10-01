
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
    /// 左侧选中的图片的索引值
    /// </summary>
    private int curIndex;
    /// <summary>
    /// 左侧图片的父物体
    /// </summary>
    public Transform leftImgPos;
    /// <summary>
    /// 左侧按钮的图片
    /// </summary>
    private Image[] leftImgs = new Image[6];
    
    #endregion

    /// <summary>
    /// 初始化强化神经面板
    /// </summary>
    protected override void InitWnd()
    {
        base.InitWnd();
        RegClickEvts();
        ClickPosItem(0);
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

            leftImgs[i] = img;
        }
    }

    /// <summary>
    /// 点击左侧图片Item
    /// </summary>
    private void ClickPosItem(int index)
    {
        PECommon.Log("ClickItem：" + index);
        curIndex = index;
        for (int i = 0; i < leftImgs.Length; i++)
        {
            Transform trans = leftImgs[i].transform;
            if (i == curIndex)//设置选中的图片
            {
                SetSprite(leftImgs[i], PathDefine.ItemArrowBg);
                trans.localPosition = new Vector3(10, trans.localPosition.y, 0);
                trans.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 95);
            }
            else//设置为默认背景版
            {
                SetSprite(leftImgs[i], PathDefine.ItemPlateBg);
                trans.localPosition = new Vector3(0, trans.localPosition.y, 0);
                trans.GetComponent<RectTransform>().sizeDelta = new Vector2(220, 85);
            }
        }
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