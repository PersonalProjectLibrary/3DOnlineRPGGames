
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：TaskRewardSys.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2025/1/1 17:00:00
    功能：任务奖励系统
***************************************/
#endregion

using PEProtocol;

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

    /// <summary>
    /// 对NetSvc里分发过来的领取任务奖励消息进行处理响应
    /// </summary>
    /// <param name="pack"></param>
    public void ReqTaskReward(MsgPack pack)
    {
        ReqTaskReward data = pack.m_Msg.reqTaskReward;
        GameMsg msg = new GameMsg { cmd = (int)CMD.RspTaskReward };
        PlayerData pData = cacheSvc.GetPlayDataBySession(pack.m_Session);
        TaskRewardCfg trc = cfgSvs.GetTaskRewardCfg(data.rewardid);
        TaskRewardData trd = GetTReward(pData, data.rewardid);
        //安全校验，是否满足领取条件
        if (trd.prgs == trc.count && !trd.taked)
        {
            pData.coin += trc.coin;
            PECommon.AddExpAndUpdateLv(pData, trc.exp);
            trd.taked = true;
            //更新玩家的任务进度数据
            TRewardToPlayer(pData, trd);
            //更新数据库
            if (!cacheSvc.UpdatePlayerData(pData.id, pData))
                msg.err = (int)ErrorCode.UpdateDBaseError;
            else
            {
                RspTaskReward rspTaskReward = new RspTaskReward
                {
                    coin = pData.coin,
                    lv = pData.lv,
                    exp = pData.exp,
                    taskArr = pData.taskRewardArr,
                };
                msg.rspTaskReward = rspTaskReward;
            }
        }
        else msg.err = (int)ErrorCode.ClientDataError;//客户端数据不满领取奖励条件
        pack.m_Session.SendMsg(msg);//将数据返回客户端
    }

    /// <summary>
    /// 查找玩家数据的某个任务奖励数据
    /// </summary>
    /// <param name="pData"></param>
    /// <param name="tRewardId"></param>
    /// <returns></returns>
    public TaskRewardData GetTReward(PlayerData pData, int tRewardId)
    {
        TaskRewardData trd = null;
        for (int i = 0; i < pData.taskRewardArr.Length; i++)
        {
            string[] taskInfo = pData.taskRewardArr[i].Split('|');//1|0|0
            if (int.Parse(taskInfo[0]) == tRewardId)
            {
                trd = new TaskRewardData
                {
                    ID = tRewardId,
                    prgs = int.Parse(taskInfo[1]),
                    taked = taskInfo[2].Equals("1"),
                };
                break;
            }
        }
        return trd;
    }

    /// <summary>
    /// 更新玩家数据的任务奖励数据
    /// </summary>
    /// <param name="pData"></param>
    /// <param name="tRData"></param>
    public void TRewardToPlayer(PlayerData pData, TaskRewardData tRData)
    {
        string res = tRData.ID + "|" + tRData.prgs + "|" + (tRData.taked ? 1 : 0);
        int index = -1;
        for (int i = 0; i < pData.taskRewardArr.Length; i++)
        {
            string[] taskInfo = pData.taskRewardArr[i].Split('|');
            if (int.Parse(taskInfo[0]) == tRData.ID)
            {
                index = i;
                break;
            }
        }
        pData.taskRewardArr[index] = res;
    }

}
