
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：Service_TimerService.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/10/10 12:42
    功能：计时服务
***************************************/
#endregion

using System;

public class TimerService : SystemRoot
{
    public static TimerService Instance = null;

    /// <summary>
    /// 定时器
    /// </summary>
    private PETimer pTimer;

    public void InitService()
    {
        Instance = this;
        pTimer = new PETimer();
        //使用公共日志输出方式输出定时器日志
        pTimer.SetLog((string info) => { PECommon.Log(info); });
        PECommon.Log("Init TimerService...");
    }

    /// <summary>
    /// 添加定时任务
    /// </summary>
    /// <param name="cb">新增的定时任务，要传任务id参数</param>
    /// <param name="delay">延迟多久后执行定时任务</param>
    /// <param name="timeUnit">时间单位，默认毫秒</param>
    /// <param name="count">计数，定时任务循环多少次；默认是1；如果是0则一直循环</param>
    /// <returns>返回任务的id</returns>
    public int AddTimeTask(Action<int> cb,double delay,PETimeUnit timeUnit = PETimeUnit.Millisecond,int count =1)
    {
        return pTimer.AddTimeTask(cb,delay,timeUnit,count);
    }
    /// <summary>
    /// 对定时任务进行监测
    /// </summary>
    private void Update() { pTimer.Update(); }
}