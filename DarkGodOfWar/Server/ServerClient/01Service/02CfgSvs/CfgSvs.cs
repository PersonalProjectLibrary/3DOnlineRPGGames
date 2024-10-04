
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
        InitStrongCfg();
        PECommon.Log("CfgSvs Init Done.");
    }

    #region 强化升级配置文件获取解析
    /// <summary>
    /// 强化升级的字典(装备pos,(装备星级startLv，配置文件sg))
    /// </summary>
    private Dictionary<int, Dictionary<int, StrongCfg>> strongDic = new Dictionary<int, Dictionary<int, StrongCfg>>();

    /// <summary>
    /// 强化升级的配置数据读取
    /// </summary>
    private void InitStrongCfg()
    {
        XmlDocument doc = new XmlDocument();
        doc.Load(@"E:\GitLibrary\PersonalProjectLibrary\3DOnlineRPGGames\DarkGodOfWar\Client\Assets\Resources\ResConfigs\eqptstrong.xml");
        XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;
        for (int i = 0; i < nodeList.Count; i++)
        {
            XmlElement element = nodeList[i] as XmlElement;
            if (element.GetAttributeNode("ID") == null) continue;
            int id = Convert.ToInt32(element.GetAttributeNode("ID").InnerText);
            StrongCfg sg = new StrongCfg { ID = id };
            foreach (XmlElement e in nodeList[i].ChildNodes)
            {
                int val = int.Parse(e.InnerText);
                switch (e.Name)
                {
                    case "pos": sg.pos = val; break;
                    case "starlv": sg.starLv = val; break;
                    case "addhp": sg.addHp = val; break;
                    case "addhurt": sg.addHurt = val; break;
                    case "adddef": sg.addDef = val; break;
                    case "minlv": sg.minLv = val; break;
                    case "coin": sg.coin = val; break;
                    case "crystal": sg.crystal = val; break;
                }
            }
            Dictionary<int, StrongCfg> sDic = null;
            if (strongDic.TryGetValue(sg.pos, out sDic))
                sDic.Add(sg.starLv, sg);
            else
            {
                sDic = new Dictionary<int, StrongCfg> { { sg.starLv, sg } };
                strongDic.Add(sg.pos, sDic);
            };
        }
        PECommon.Log("StrongCfg Init Done");
    }

    /// <summary>
    /// 获取强化升级的配置文件
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public StrongCfg GetStrongCfg(int pos, int startLv)
    {
        StrongCfg sg = null;
        Dictionary<int, StrongCfg> sDic = null;
        if (strongDic.TryGetValue(pos, out sDic))
        {
            if (sDic.ContainsKey(startLv)) sDic.TryGetValue(startLv, out sg);
        }
        return sg;
    }
    #endregion

    #region 引导任务配置文件获取解析
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
        doc.Load(@"E:\GitLibrary\PersonalProjectLibrary\3DOnlineRPGGames\DarkGodOfWar\Client\Assets\Resources\ResConfigs\taskguide.xml");
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
                    case "exp": ag.exp = int.Parse(e.InnerText); break;
                }
            }
            guideDic.Add(id, ag);
        }
        PECommon.Log("GuideCfg Init Done");
    }

    /// <summary>
    /// 获取任务引导的配置文件
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public GuideCfg GetGuideCfg(int id)
    {
        GuideCfg agc = null;
        if (guideDic.TryGetValue(id, out agc)) return agc;
        return null;
    }

    #endregion

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
/// 强化升级配置表
/// </summary>
public class StrongCfg : BaseData<StrongCfg>
{
    /// <summary>
    /// 几号装备
    /// </summary>
    public int pos;
    /// <summary>
    /// 星级等级
    /// </summary>
    public int starLv;
    /// <summary>
    /// 可增加的生命加成值
    /// </summary>
    public int addHp;
    /// <summary>
    /// 可增加的伤害加成值
    /// </summary>
    public int addHurt;
    /// <summary>
    /// 可增加的防御加成值
    /// </summary>
    public int addDef;
    /// <summary>
    /// 所需最小等级
    /// </summary>
    public int minLv;
    /// <summary>
    /// 所需金币
    /// </summary>
    public int coin;
    /// <summary>
    /// 所需水晶
    /// </summary>
    public int crystal;
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
    public int exp;
}
