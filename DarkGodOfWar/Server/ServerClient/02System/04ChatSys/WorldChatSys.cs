
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：04ChatSys_WorldChatSys.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/10/6 17:49:11
    功能：世界聊天系统
***************************************/
#endregion

using PEProtocol;
using System.Collections.Generic;

/// <summary>
/// 世界聊天系统
/// </summary>
public class WorldChatSys
{
    private static WorldChatSys instance = null;
    public static WorldChatSys Instance
    {
        get
        {
            if (instance == null) instance = new WorldChatSys();
            return instance;
        }
    }

    private CacheSvc cacheSvc = null;

    /// <summary>
    /// 世界聊天系统初始化
    /// </summary>
    public void Init()
    {
        cacheSvc = CacheSvc.Instance;
        PECommon.Log("WorldChatSys Init Done.");
    }

    /// <summary>
    /// 把聊天消息广播给在线玩家
    /// </summary>
    public void SndWorldChat(MsgPack pack)
    {
        SndWorldChat data = pack.m_Msg.sndWorldChat;//数据转接
        PlayerData pData = cacheSvc.GetPlayDataBySession(pack.m_Session);//获取缓存层里玩家数据
        UpdateTaskPrgs(pData);
        GameMsg msg = new GameMsg//广播给客户端的消息
        {
            cmd = (int)CMD.PshWorldChat,
            pshWorldChat = new PshWorldChat
            {
                name = pData.name,
                chat = data.chat
            }
        };
        //把消息广播给所有在线的玩家客户端
        List<ServerSession> list = cacheSvc.GetOnlineServerSessions();
        
        /* old 直接发送msg给所有在线玩家
         * 
         * for (int i = 0; i < list.Count; i++) list[i].SendMsg(msg);
         * 
         * 存在问题：
         * 网络里是把msg序列化二进制再进行传输的，但序列化消耗cpu
         * 如果直接SendMsg(msg)，则会经历几千次序列化，性能消耗大
         * 这里SendMsg()有重载，可以直接发送二进制数据，而不是必须msg类消息
         * 同时这里发送给玩家的消息都是一样的，故可进行下面的优化方法发送消息
         * 先使用PEProtocol里将msg序列化为二进制的方法，序列化msg，
         * 再一遍遍把二进制数据发送给各个客户端，减少序列化操作，减少性能消耗
         */
        
        //先将msg序列化为二进制，再发送给所有在线玩家
        byte[] bytes = PENet.PETool.PackNetMsg(msg);
        for (int i = 0; i < list.Count; i++) list[i].SendMsg(bytes);
    }

    /// <summary>
    /// 任务进度数据更新
    /// </summary>
    /// <param name="pData"></param>
    public void UpdateTaskPrgs(PlayerData pData)
    {
        //对应在任务奖励配置里能言善辩任务的id
        int tid = int.Parse(pData.taskRewardArr[5].Split('|')[0]);
        TaskRewardSys.Instance.CalcuteTaskPrgs(pData, tid);
    }
}
