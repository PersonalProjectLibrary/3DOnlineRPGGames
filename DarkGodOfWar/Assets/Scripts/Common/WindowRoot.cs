
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：Common_WindowRoot.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/8/30 18:23
    功能：UI界面基类
***************************************/
#endregion

using UnityEngine;
using UnityEngine.UI;

public class WindowRoot : MonoBehaviour
{
    protected ResService resService = null;
    protected AudioService audioService = null;

    /// <summary>
    /// 设置UI界面显示状态
    /// </summary>
    /// <param name="isActive">默认 true 打开窗口</param>
    public void SetWndState(bool isActive = true)
    {
        if (gameObject.activeSelf != isActive) SetActive(gameObject, isActive);
        if (isActive) InitWnd();
        else ClearWnd();
    }

    /// <summary>
    /// 初始化界面
    /// </summary>
    protected virtual void InitWnd()
    {
        resService = ResService.Instance;
        audioService = AudioService.Instance;
    }

    /// <summary>
    /// 清理界面
    /// </summary>
    protected void ClearWnd()
    {
        resService = null;
        audioService = null;
    }

    #region Tools Functions

    /// <summary>
    /// 对物体的激活
    /// </summary>
    /// <param name="go"></param>
    /// <param name="isActive"></param>
    protected void SetActive(GameObject go,bool isActive = true)
    {
        go.SetActive(isActive);
    }
    protected void SetActive(Transform trans, bool state = true)
    {
        trans.gameObject.SetActive(state);
    }
    protected void SetActive(RectTransform rectTrans, bool state = true)
    {
        rectTrans.gameObject.SetActive(state);
    }
    protected void SetActive(Image img, bool state = true)
    {
        img.transform.gameObject.SetActive(state);
    }
    protected void SetActive(Text txt, bool state = true)
    {
        txt.transform.gameObject.SetActive(state);
    }

    /// <summary>
    /// 设置Text内容
    /// </summary>
    /// <param name="txt"></param>
    /// <param name="content"></param>
    protected void SetText(Text txt, string content = "")
    {
        txt.text = content;
    }
    protected void SetText(Text txt, int num = 0)
    {
        SetText(txt, num.ToString());
    }
    protected void SetText(InputField txt, string content = "")
    {
        txt.text = content;
    }
    protected void SetText(InputField txt, int num = 0)
    {
        SetText(txt, num.ToString());
    }
    protected void SetText(Transform trans, string content = "")
    {
        SetText(trans.GetComponent<Text>(), content);
    }
    protected void SetText(Transform trans, int num = 0)
    {
        SetText(trans.GetComponent<Text>(), num);
    }

    #endregion
}