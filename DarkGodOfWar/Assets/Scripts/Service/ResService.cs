
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：Service_ResService.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/8/30 10:56
    功能：资源加载服务
***************************************/
#endregion

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResService : MonoBehaviour
{
    public static ResService Instance = null;

    /// <summary>
    /// 初始化资源加载服务
    /// </summary>
    public void InitService()
    {
        Instance = this;

        Debug.Log("Init ResService...");
    }

    #region LoadScene

    /// <summary>
    /// 设置场景加载进度，控制进度条实时更新
    /// </summary>
    private Action progressAction;

    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <param name="sceneName"></param>
    public void AsyncLoadScene(string sceneName, Action loaded)
    {
        GameRoot.Instance.loadingWnd.SetWndState();

        AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(sceneName);//执行和获取异步加载场景的操作

        progressAction = () =>
        {
            float val = sceneAsync.progress;//获取当前异步加载的进度
            GameRoot.Instance.loadingWnd.SetProgress(val);//通过GameRoot设置场景加载进度条
            if (val == 1)//进度条加载完成
            {
                progressAction = null;//结束更新进度条事件
                sceneAsync = null;//置空异步操作
                GameRoot.Instance.loadingWnd.SetWndState(false);//关闭加载界面
                if (loaded != null) loaded();//场景加载完成后，有回调事件，执行回调事件
            }
        };
    }

    private void Update()
    {
        if (progressAction != null) progressAction();
    }
    #endregion


    #region Load Audio

    /// <summary>
    /// 存储需要缓存的声音资源
    /// </summary>
    private Dictionary<string,AudioClip> audioDic = new Dictionary<string,AudioClip>();

    /// <summary>
    /// 加载声音资源
    /// </summary>
    /// <param name="path">资源的获取路径+资源名</param>
    /// <param name="cache">是否进行缓存，默认false</param>
    /// <returns></returns>
    public AudioClip LoadAudio(string path,bool cache = false)
    {
        AudioClip audioClip = null;
        if(!audioDic.TryGetValue(path, out audioClip))//没有缓存过
        {
            audioClip = Resources.Load<AudioClip>(path);//获取声音资源
            if(cache)audioDic.Add(path, audioClip);//对声音进行缓存
        }
        return audioClip;
    }
    #endregion

}