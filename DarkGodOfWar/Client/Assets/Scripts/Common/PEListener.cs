
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：Common_PEListener.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/15 12:52
    功能：UI事件监听插件
***************************************/
#endregion

using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 点击、拖拽、松开UI的事件监听
/// </summary>
public class PEListener : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDropHandler
{
    public Action<PointerEventData> onClickDown;
    public Action<PointerEventData> onDrag;
    public Action<PointerEventData> onClickUp;

    /// <summary>
    /// 按下事件
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void OnPointerDown(PointerEventData eventData)
    {
        if (onClickDown != null) onClickDown(eventData);
    }

    /// <summary>
    /// 拖动事件
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void OnDrop(PointerEventData eventData)
    {
        if (onDrag != null) onDrag(eventData);
    }

    /// <summary>
    /// 松开事件
    /// </summary>
    /// <param name="eventData"></param>
    /// <exception cref="NotImplementedException"></exception>
    public void OnPointerUp(PointerEventData eventData)
    {
        if (onClickUp != null) onClickUp(eventData);
    }
}