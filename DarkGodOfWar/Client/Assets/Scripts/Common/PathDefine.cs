
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：Common_PathDefine.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/1 0:30
    功能：路径常量定义
***************************************/
#endregion

using System;
using UnityEngine;

public class PathDefine 
{
    #region Configs Path
    /// <summary>
    /// 创建角色界面_随机姓名配置文件
    /// </summary>
    public const string RdNameCfg = "ResConfigs/rdname";
    
    /// <summary>
    /// 主城_地图配置文件
    /// </summary>
    public const string McMapCfg = "ResConfigs/mcmap";

    /// <summary>
    /// 主城_任务引导配置文件
    /// </summary>
    public const string TaskGuideCfg = "ResConfigs/taskguide";

    #endregion

    #region Player Path
    /// <summary>
    /// 主城角色预制体路径
    /// </summary>
    public const string AssissnCityPrefab = "PrefabPlayer/AssassinCity";

    #endregion
}