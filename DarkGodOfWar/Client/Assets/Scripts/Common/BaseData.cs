
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
