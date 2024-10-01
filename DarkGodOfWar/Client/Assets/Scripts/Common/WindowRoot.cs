
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：Common_WindowRoot.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/8/30 18:23
    功能：UI界面基类
***************************************/
#endregion

using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WindowRoot : MonoBehaviour
{
    /// <summary>
    /// 资源加载服务
    /// </summary>
    protected ResService resService = null;
    /// <summary>
    /// 声音播放服务
    /// </summary>
    protected AudioService audioService = null;
    /// <summary>
    /// 网络服务
    /// </summary>
    protected NetService netService = null;

    #region Set Wnd：设置界面
    /// <summary>
    /// 初始化界面
    /// </summary>
    protected virtual void InitWnd()
    {
        resService = ResService.Instance;
        audioService = AudioService.Instance;
        netService = NetService.Instance;
    }

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
    /// 清理界面
    /// </summary>
    protected void ClearWnd()
    {
        resService = null;
        audioService = null;
        netService = null;
    }

    #endregion

    #region Tools Functions：设置物体的显掩、设置文本内容、获取组件、设置图片
    /// <summary>
    /// 设置图片
    /// </summary>
    /// <param name="img"></param>
    /// <param name="path"></param>
    protected void SetSprite(Image img,string path)
    {
        Sprite sp = resService.LoadSprite(path, true);//加载图片
        img.sprite = sp;//替换图片
    }

    /// <summary>
    /// 获取组件
    /// </summary>
    /// <typeparam name="T">所需组件</typeparam>
    /// <param name="go">获取组件的物体</param>
    /// <returns>所需的组件</returns>
    /// 判断物体身上有没有某个组件,有则获取，没有则添加
    protected T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        T t = go.GetComponent<T>();
        if (t == null) t = go.AddComponent<T>();
        return t;
    }

    /// <summary>
    /// 设置物体的显掩
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
    /// 设置文本内容
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

    #region Click Evts：点击、触屏事件
    /// <summary>
    /// UI触控点击
    /// </summary>
    /// <param name="go"></param>
    /// <param name="callback"></param>
    protected void OnClick(GameObject go, Action<PointerEventData> callback)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClick = callback;
    }

    /// <summary>
    /// UI触控按下
    /// </summary>
    /// <param name="go">触控的物体</param>
    /// <param name="callback">回调事件</param>
    protected void OnClickDown(GameObject go,Action<PointerEventData> callback)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClickDown = callback;
    }
    
    /// <summary>
    ///  UI触控拖拽
    /// </summary>
    /// <param name="go"></param>
    /// <param name="callback"></param>
    protected void OnDrag(GameObject go, Action<PointerEventData> callback)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onDrag = callback;
    }
    
    /// <summary>
    ///  UI触控松开
    /// </summary>
    /// <param name="go"></param>
    /// <param name="callback"></param>
    protected void OnClickUp(GameObject go, Action<PointerEventData> callback)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClickUp = callback;
    }
    
    #endregion

}