
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：UIWindow_DynamicWnd.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/8/31 17:16
    功能：动态UI元素界面
***************************************/
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicWnd : WindowRoot
{
    public Animation tipsAnim;
    public Text txtTips;

    private Queue<string> tipsQueue = new Queue<string>();
    private bool isTipsShow = false;

    protected override void InitWnd()
    {
        base.InitWnd();

        SetActive(txtTips, false);//默认关闭，需要时再显示
    }

    private void Update()
    {
        if (tipsQueue.Count > 0&& !isTipsShow)
        {
            lock (tipsQueue)
            {
                string tips = tipsQueue.Dequeue();
                isTipsShow = true;
                SetTips(tips);
            }
        }
    }

    /// <summary>
    /// 添加要显示的Tips信息
    /// </summary>
    /// <param name="tips"></param>
    public void AddTips(string tips)
    {
        lock (tipsQueue) tipsQueue.Enqueue(tips);
    }

    /// <summary>
    /// 显示Tips
    /// </summary>
    /// <param name="tips">显示的内容</param>
    private void SetTips(string tips)
    {
        SetActive(txtTips, true);
        SetText(txtTips, tips);

        //设置tips的动画
        AnimationClip clip = tipsAnim.GetClip("TipsShowAnim");
        tipsAnim.Play();
        StartCoroutine(AnimPlayDone(clip.length, () =>
        {
            SetActive(txtTips, false);
            isTipsShow = false;
        }));
    }

    /// <summary>
    /// 延时执行callback
    /// </summary>
    /// <param name="sec">延时的时长</param>
    /// <param name="callback">延时后执行的事件</param>
    /// <returns></returns>
    private IEnumerator AnimPlayDone(float sec,Action callback)
    {
        yield return new WaitForSeconds(sec);
        if(callback != null) callback();
    }
}