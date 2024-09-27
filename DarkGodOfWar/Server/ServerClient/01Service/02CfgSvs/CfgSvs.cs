
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：02CfgSvs_CfgSvs.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/27 23:53:34
    功能：配置数据服务
***************************************/
#endregion

using System.Collections.Generic;
using System;
using System.Xml;

/// <summary>
/// 配置数据服务
/// </summary>
public class CfgSvs
{
    private static CfgSvs instance = null;
    public static CfgSvs Instance
    {
        get
        {
            if (instance == null) instance = new CfgSvs();
            return instance;
        }
    }

    /// <summary>
    /// 网络服务初始化
    /// </summary>
    public void Init()
    {
        InitGuideCfg();
        PECommon.Log("CfgSvs Init Done.");
    }

    /// <summary>
    /// 引导任务的字典(任务ID，任务配置文件)
    /// </summary>
    private Dictionary<int, GuideCfg> guideDic = new Dictionary<int, GuideCfg>();

    /// <summary>
    /// 任务引导的配置数据读取
    /// </summary>
    private void InitGuideCfg()
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(@"E:\GitLibrary\PersonalProjectLibrary\3DOnlineRPGGames\DarkGodOfWar\Client\Assets\Resources\ResConfigs\taskguide.xml");
        XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;
        for (int i = 0; i < nodeList.Count; i++)
        {
            XmlElement element = nodeList[i] as XmlElement;
            if (element.GetAttributeNode("ID") == null) continue;
            int id = Convert.ToInt32(element.GetAttributeNode("ID").InnerText);
            GuideCfg ag = new GuideCfg { ID = id };
            foreach (XmlElement e in nodeList[i].ChildNodes)
            {
                switch (e.Name)
                {
                    case "coin": ag.coin = int.Parse(e.InnerText); break;
                    case "exp": ag.coin = int.Parse(e.InnerText); break;
                }
            }
            guideDic.Add(id, ag);
        }
    }

    /// <summary>
    /// 获取任务引导的配置文件
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public GuideCfg GetAutoGuideData(int id)
    {
        GuideCfg agc = null;
        guideDic.TryGetValue(id, out agc);
        return agc;
    }
}

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
/// 任务引导配置表
/// </summary>
public class GuideCfg : BaseData<GuideCfg>
{
    /// <summary>
    /// 任务奖励_金币
    /// </summary>
    public int coin;
    /// <summary>
    /// 任务奖励_经验值
    /// </summary>
    public string exp;
}
