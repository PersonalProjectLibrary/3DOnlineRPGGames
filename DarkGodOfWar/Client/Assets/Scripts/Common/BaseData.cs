
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：Common_BaseData.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/16 23:6
    功能：配置表数据的基类
***************************************/
#endregion

using UnityEngine;

/// <summary>
/// 配置表数据的基类
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseData<T>
{
    /// <summary>
    /// 配置表里的ID
    /// </summary>
    public int ID;
}

/// <summary>
/// 主城地图配置表数据类
/// </summary>
public class McMapCfg : BaseData<McMapCfg>
{
    /// <summary>
    /// 地图名
    /// </summary>
    public string mapName;

    /// <summary>
    /// 场景名
    /// </summary>
    public string sceneName;

    /// <summary>
    /// （跟随角色的）主相机的位置
    /// </summary>
    public Vector3 mainCamPos;

    /// <summary>
    /// （跟随角色的）主相机的角度
    /// </summary>
    public Vector3 mainCamRote;

    /// <summary>
    /// 玩家角色的出生位置
    /// </summary>
    public Vector3 playerBornPos;

    /// <summary>
    /// 玩家角色的出生角度
    /// </summary>
    public Vector3 playerBornRote;
}

/// <summary>
/// 任务引导配置表
/// </summary>
public class AutoGuideCfg : BaseData<AutoGuideCfg>
{
    /// <summary>
    /// 触发任务目标NPC索引号
    /// </summary>
    /// 后面利用npc的ID做导航
    public int npcID;

    /// <summary>
    /// 对话数据
    /// </summary>
    /// 后面根据分割符对对话进行处理
    public string dialogArr;

    /// <summary>
    /// 目标任务ID
    /// </summary>
    /// 完成引导后做什么
    public int actID;

    /// <summary>
    /// 金币
    /// </summary>
    /// 引导完成后的奖励
    public int coin;

    /// <summary>
    /// 经验值
    /// </summary>
    /// 引导完成后的奖励
    public int exp;
}
