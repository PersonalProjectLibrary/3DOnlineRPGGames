
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：03TimerSvc_TimerSvc.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/10/10 16:48:09
    功能：定时服务
***************************************/
#endregion

using System;
using System.Collections.Generic;

/// <summary>
/// 定时服务
/// </summary>
public class TimerSvc
{
    private static TimerSvc instance = null;
    public static TimerSvc Instance
    {
        get
        {
            if (instance == null) instance = new TimerSvc();
            return instance;
        }
    }

    PETimer pTimer = null;

    Queue<TaskPack> taskPackQue = new Queue<TaskPack>();
    static readonly string tpqLock = "tpqLock";

    /// <summary>
    /// 定时服务初始化
    /// </summary>
    public void Init()
    {
        pTimer = new PETimer(100);
        //使用公共日志输出方式输出定时器日志
        pTimer.SetLog((string info) => { PECommon.Log(info); });
        //使用SetHandle()处理具体的定时业务逻辑，保证独立线程数据安全性
        //使用Handle，会把所有已经满足目标时间的定时任务发出来，我们再执行具体的业务逻辑即可
        pTimer.SetHandle((Action<int> cb, int tid) =>
        {
            //把定时任务放到任务队列里，后面主线程里一一处理
            if (cb != null)
            {
                lock (tpqLock) { taskPackQue.Enqueue(new TaskPack(tid, cb)); }
            }
        });
        PECommon.Log("TimerSvc Init Done.");
    }

    /// <summary>
    /// 获取当前时间，单位毫秒
    /// </summary>
    /// 获取的时间是非常大的毫秒数：从计算机纪元时间1970年到现在为止的总毫秒数）
    /// <returns></returns>
    public long GetNowTime() { return (long)pTimer.GetMillisecondsTime(); }

    /// <summary>
    /// 添加定时任务
    /// </summary>
    /// <param name="cb">新增的定时任务，要传任务id参数</param>
    /// <param name="delay">延迟多久后执行定时任务</param>
    /// <param name="timeUnit">时间单位，默认毫秒</param>
    /// <param name="count">计数，定时任务循环多少次；默认是1；如果是0则一直循环</param>
    /// <returns>返回任务的id</returns>
    public int AddTimeTask(Action<int> cb, double delay, PETimeUnit timeUnit = PETimeUnit.Millisecond, int count = 1)
    {
        return pTimer.AddTimeTask(cb, delay, timeUnit, count);
    }

    /// <summary>
    /// 对定时任务进行监测
    /// </summary>
    public void Update()
    {
        /* 检测任务队列不空，则取任务
         * 使用if判断，每帧只取出一个任务处理，
         * 使用while，可在同一帧里把所有满足条件的定时任务取出进行处理
         */
        while (taskPackQue.Count > 0)
        {
            TaskPack tp = null;
            lock (tpqLock) { tp = taskPackQue.Dequeue(); }
            if (tp != null) { tp.cb(tp.tid); }//处理已经满足条件的定时任务
        }
    }

    /// <summary>
    /// 定时任务包
    /// </summary>
    /// 负责定时任务具体业务逻辑的包
    class TaskPack
    {
        /// <summary>
        /// >定时任务的id
        /// </summary>
        public int tid;
        /// <summary>
        /// 定时任务具体执行的Action
        /// </summary>
        public Action<int> cb;
        /// <summary>
        /// 定时任务包
        /// </summary>
        /// <param name="tId">定时任务的id</param>
        /// <param name="callback">定时任务具体执行的Action</param>
        public TaskPack(int tId,Action<int> callback)
        {
            tid = tId;
            cb = callback;
        }
    }
}
