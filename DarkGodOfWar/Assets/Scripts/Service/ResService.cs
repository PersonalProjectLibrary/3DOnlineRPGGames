
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
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResService : MonoBehaviour
{
    public static ResService Instance = null;

    private Action progressAction;//控制进度条实时更新

    /// <summary>
    /// 初始化资源加载服务
    /// </summary>

    public void InitService()
    {
        Instance = this;

        Debug.Log("Init ResService...");
    }

    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <param name="sceneName"></param>
    public void AsyncLoadScene(string sceneName,Action loaded)
    {
        GameRoot.Instance.loadingWnd.gameObject.SetActive(true);//打开场景加载界面
        GameRoot.Instance.loadingWnd.InitWnd();//重置加载界面

        AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(sceneName);//执行和获取异步加载场景的操作

        progressAction = () =>
        {
            float val = sceneAsync.progress;//获取当前异步加载的进度
            GameRoot.Instance.loadingWnd.SetProgress(val);//通过GameRoot设置场景加载进度条
            if (val == 1)//进度条加载完成
            {
                progressAction = null;//结束更新进度条事件
                sceneAsync = null;//置空异步操作
                GameRoot.Instance.loadingWnd.gameObject.SetActive(false);//关闭加载界面
                if (loaded != null) loaded();//场景加载完成后，有回调事件，执行回调事件
            }
        };
    }

    private void Update()
    {
        if (progressAction != null)progressAction();
    }
}