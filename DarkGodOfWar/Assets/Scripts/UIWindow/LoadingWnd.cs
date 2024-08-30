
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：UIWindow_LoadingWnd.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/8/30 14:14
    功能：加载进度界面
***************************************/
#endregion

using UnityEngine;
using UnityEngine.UI;

public class LoadingWnd : MonoBehaviour
{
    public Text txtTips;
    public Image loadingFg;
    public Image loadingPoint;
    public Text txtPrg;

    private float fgWidth;//用于设置进度条里imgPoint位置

    /// <summary>
    /// 界面打开时初始化
    /// </summary>
    public void InitWnd()
    {
        fgWidth = loadingFg.GetComponent<RectTransform>().sizeDelta.x;//获取进度条宽度

        txtTips.text = "这是一条游戏Tips";
        txtPrg.text = "0%";
        loadingFg.fillAmount = 0;
        loadingPoint.transform.localPosition = new Vector3(-545f,0,0);
    }

    /// <summary>
    /// 控制进度条进度
    /// </summary>
    /// <param name="progress"></param>
    public void SetProgress(float progress)
    {
        txtPrg.text = (int)(progress * 100) + "%";
        loadingFg.fillAmount = progress;

        float pointPox = progress* fgWidth - 545;//545是imgPoint在进度条0位置上的偏移量
        loadingPoint.GetComponent<RectTransform>().anchoredPosition = new Vector2(pointPox, 0);//设置进度条位置
    }
}