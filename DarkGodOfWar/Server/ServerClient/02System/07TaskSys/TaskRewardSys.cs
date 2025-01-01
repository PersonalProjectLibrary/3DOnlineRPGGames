
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：TaskRewardSys.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2025/1/1 17:00:00
    功能：任务奖励系统
***************************************/
#endregion

public class TaskRewardSys
{
    private static TaskRewardSys instance = null;
    public static TaskRewardSys Instance
    {
        get
        {
            if (instance == null) instance = new TaskRewardSys();
            return instance;
        }
    }

    private CacheSvc cacheSvc = null;
    private CfgSvs cfgSvs = null;

    /// <summary>
    /// 任务奖励系统初始化
    /// </summary>
    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        cfgSvs = CfgSvs.Instance;
        PECommon.Log("TaskRewardSys Init Done.");
    }
}
