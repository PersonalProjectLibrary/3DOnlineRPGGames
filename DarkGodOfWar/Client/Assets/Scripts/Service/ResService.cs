
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：Service_ResService.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/8/30 10:56
    功能：资源加载服务
***************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 资源加载服务
/// </summary>
public class ResService : MonoBehaviour
{
    public static ResService Instance = null;

    /// <summary>
    /// 初始化资源加载服务
    /// </summary>
    public void InitService()
    {
        Instance = this;
        PECommon.Log("Init ResService...");
        InitRdNameCfg(PathDefine.RdNameCfg);
        InitMcMapCfg(PathDefine.McMapCfg);
        InitGuideCfg(PathDefine.TaskGuideCfg);
        InitStrongCfg(PathDefine.EqptStrongCfg);
        InitTaskRewardCfg(PathDefine.TaskRewardCfg);
    }

    #region Load Resource
    #region Load Scene
    /// <summary>
    /// 设置场景加载进度，控制进度条实时更新
    /// </summary>
    private Action progressAction;

    /// <summary>
    /// 异步加载场景
    /// </summary>
    /// <param name="sceneName"></param>
    public void AsyncLoadScene(string sceneName, Action loaded)
    {
        GameRoot.Instance.loadingWnd.SetWndState();

        AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(sceneName);//执行和获取异步加载场景的操作

        progressAction = () =>
        {
            float val = sceneAsync.progress;//获取当前异步加载的进度
            GameRoot.Instance.loadingWnd.SetProgress(val);//通过GameRoot设置场景加载进度条
            if (val == 1)//进度条加载完成
            {
                progressAction = null;//结束更新进度条事件
                sceneAsync = null;//置空异步操作
                GameRoot.Instance.loadingWnd.SetWndState(false);//关闭加载界面
                if (loaded != null) loaded();//场景加载完成后，有回调事件，执行回调事件
            }
        };
    }

    /// <summary>
    /// 展现场景加载进度条
    /// </summary>
    private void Update()
    {
        if (progressAction != null) progressAction();
    }
    
    #endregion

    #region Load Audio
    /// <summary>
    /// 存储需要缓存的声音资源
    /// </summary>
    private Dictionary<string, AudioClip> audioDic = new Dictionary<string, AudioClip>();

    /// <summary>
    /// 加载声音资源
    /// </summary>
    /// <param name="path">资源的获取路径+资源名</param>
    /// <param name="cache">是否进行缓存，默认false</param>
    /// <returns></returns>
    public AudioClip LoadAudio(string path, bool cache = false)
    {
        AudioClip audioClip = null;
        if (!audioDic.TryGetValue(path, out audioClip))//没有缓存过
        {
            audioClip = Resources.Load<AudioClip>(path);//获取声音资源
            if (cache) audioDic.Add(path, audioClip);//对声音进行缓存
        }
        return audioClip;
    }

    #endregion

    #region Load Prefab
    /// <summary>
    /// 缓存的预制体：预制体获取路径，预制体
    /// </summary>
    private Dictionary<string, GameObject> prefabCacheDic = new Dictionary<string, GameObject>();
    /// <summary>
    /// 根据预制体路径，获取实例化对象
    /// </summary>
    /// <param name="path">预制体加载路径</param>
    /// <param name="cache">是否缓存预制体</param>
    /// <returns></returns>
    public GameObject LoadPrefab(string path,bool cache = false)
    {
        GameObject prefab = null;
        if(!prefabCacheDic.TryGetValue(path,out prefab))
        {
            prefab = Resources.Load<GameObject>(path);
            if (cache) prefabCacheDic.Add(path, prefab);
        }
        GameObject go = null;
        if (prefab != null) go = Instantiate(prefab);
        return go;
    }

    #endregion

    #region Load Sprite
    /// <summary>
    /// 缓存的Sprite：sprite获取路径，Sprite
    /// </summary>
    private Dictionary<string, Sprite> spDic = new Dictionary<string, Sprite>();
    /// <summary>
    /// 加载Sprite
    /// </summary>
    /// <param name="path"></param>
    /// <param name="cache"></param>
    /// <returns></returns>
    public Sprite LoadSprite(string path,bool cache = false)
    {
        Sprite sp = null;
        if(!spDic.TryGetValue(path,out sp))
        {
            sp = Resources.Load<Sprite>(path);
            if (cache) spDic.Add(path, sp);
        }
        return sp;
    }

    #endregion

    #endregion

    #region Init Configs 解析xml配置表

    #region 创建角色界面_随机姓名配置解析——rdname.xml
    private List<string> surNameList = new List<string>();
    private List<string> manList = new List<string>();
    private List<string> womanList = new List<string>();

    /// <summary>
    /// 生成随机姓名的配置文件解析
    /// </summary>
    private void InitRdNameCfg(string path)
    {
        TextAsset nameXml = Resources.Load<TextAsset>(path);
        if (!nameXml) PECommon.Log("Xml file:" + path + "not exist!", LogType.Error);
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(nameXml.text);
            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement element = nodeList[i] as XmlElement;
                if (element.GetAttributeNode("ID") == null) continue;
                //int id = Convert.ToInt32(element.GetAttributeNode("ID").InnerText);
                foreach (XmlElement e in nodeList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "surname": surNameList.Add(e.InnerText); break;
                        case "man": manList.Add(e.InnerText); break;
                        case "woman": womanList.Add(e.InnerText); break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 获取随机姓名
    /// </summary>
    /// <param name="woman">是否是女性</param>
    /// <returns></returns>
    public string GetRdNameData(bool woman = true)
    {
        //System.Random rd = new System.Random();
        //string rdName = surNameList[PETools.RandomInt(0, surNameList.Count - 1, rd)];
        string rdName = surNameList[PETools.RandomInt(0, surNameList.Count - 1)];
        if (woman) rdName += womanList[PETools.RandomInt(0, womanList.Count - 1)];
        else rdName += manList[PETools.RandomInt(0, manList.Count - 1)];
        return rdName;
    }

    #endregion

    #region 主城_地图配置解析——mcmap.xml
    /// <summary>
    /// （id号，对应map配置表数据）存储配置表解析后的地图数据
    /// </summary>
    private Dictionary<int, McMapCfg> mcMapCfgDataDic = new Dictionary<int, McMapCfg>();

    /// <summary>
    /// 主城地图配置解析
    /// </summary>
    private void InitMcMapCfg(string path)
    {
        TextAsset nameXml = Resources.Load<TextAsset>(path);
        if (!nameXml) PECommon.Log("Xml file:" + path + "not exist!", LogType.Error);
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(nameXml.text);
            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement element = nodeList[i] as XmlElement;
                if (element.GetAttributeNode("ID") == null) continue;
                int id = Convert.ToInt32(element.GetAttributeNode("ID").InnerText);
                McMapCfg mc = new McMapCfg { ID = id };
                foreach (XmlElement e in nodeList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "mapName":mc.mapName = e.InnerText; break;
                        case "sceneName": mc.sceneName = e.InnerText; break;
                        case "mainCamPos":
                            {
                                string[] valArr = e.InnerText.Split(',');
                                mc.mainCamPos = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                            }
                            break;
                        case "mainCamRote":
                            {
                                string[] valArr = e.InnerText.Split(',');
                                mc.mainCamRote = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                            }
                            break;
                        case "playerBornPos":
                            {
                                string[] valArr = e.InnerText.Split(',');
                                mc.playerBornPos = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                            }
                            break;
                        case "playerBornRote":
                            {
                                string[] valArr = e.InnerText.Split(',');
                                mc.playerBornRote = new Vector3(float.Parse(valArr[0]), float.Parse(valArr[1]), float.Parse(valArr[2]));
                            }
                            break;
                    }
                }
                mcMapCfgDataDic.Add(id, mc);
            }
        }
    }

    /// <summary>
    /// 根据Id号获取配置数据的接口
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public McMapCfg GetMapCfgData(int id)
    {
        McMapCfg data;
        if (mcMapCfgDataDic.TryGetValue(id, out data)) return data;
        return null;
    }

    #endregion

    #region 主城_任务引导配置解析--taskguide.xml
    /// <summary>
    /// 引导任务的字典(任务ID，任务配置文件)
    /// </summary>
    private Dictionary<int, AutoGuideCfg> taskGuideDic = new Dictionary<int, AutoGuideCfg>();

    /// <summary>
    /// 任务引导配置文件解析
    /// </summary>
    private void InitGuideCfg(string path)
    {
        TextAsset nameXml = Resources.Load<TextAsset>(path);
        if (!nameXml) PECommon.Log("Xml file:" + path + "not exist!", LogType.Error);
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(nameXml.text);
            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement element = nodeList[i] as XmlElement;
                if (element.GetAttributeNode("ID") == null) continue;
                int id = Convert.ToInt32(element.GetAttributeNode("ID").InnerText);
                AutoGuideCfg ag = new AutoGuideCfg { ID = id };
                foreach (XmlElement e in nodeList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "npcID": ag.npcID = int.Parse(e.InnerText); break;
                        case "dialogArr": ag.dialogArr = e.InnerText; break;//具体对话窗口再处理对话
                        case "actID": ag.actID = int.Parse(e.InnerText); break;
                        case "coin": ag.coin = int.Parse(e.InnerText); break;
                        case "exp": ag.exp = int.Parse(e.InnerText); break;
                    }
                }
                taskGuideDic.Add(id, ag);
            }
        }
    }

    /// <summary>
    /// 获取任务引导的配置文件
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public AutoGuideCfg GetGuideCfgData(int id)
    {
        AutoGuideCfg agc = null;
        taskGuideDic.TryGetValue(id, out agc);
        return agc;
    }

    #endregion

    #region 主城_强化升级配置解析——eqptstrong.xml
    /// <summary>
    /// 强化升级的字典(装备pos,(装备星级startLv，配置文件esg))
    /// </summary>
    private Dictionary<int,Dictionary<int, EqptStrongCfg>> eqptStrongDic;

    /// <summary>
    /// 强化升级配置文件解析
    /// </summary>
    private void InitStrongCfg(string path)
    {
        TextAsset nameXml = Resources.Load<TextAsset>(path);
        if (!nameXml) PECommon.Log("Xml file:" + path + "not exist!", LogType.Error);
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(nameXml.text);
            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;
            eqptStrongDic = new Dictionary<int, Dictionary<int, EqptStrongCfg>>();
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement element = nodeList[i] as XmlElement;
                if (element.GetAttributeNode("ID") == null) continue;
                int id = Convert.ToInt32(element.GetAttributeNode("ID").InnerText);
                EqptStrongCfg esg = new EqptStrongCfg { ID = id };
                foreach (XmlElement e in nodeList[i].ChildNodes)
                {
                    int val = int.Parse(e.InnerText);
                    switch (e.Name)
                    {
                        case "pos": esg.pos = val; break;
                        case "starlv": esg.starLv = val; break;
                        case "addhp": esg.addHp = val; break;
                        case "addhurt": esg.addHurt = val; break;
                        case "adddef": esg.addDef = val; break;
                        case "minlv": esg.minLv = val; break;
                        case "coin": esg.coin = val; break;
                        case "crystal": esg.crystal = val; break;
                    }
                }
                Dictionary<int, EqptStrongCfg> sDic =null;
                if(eqptStrongDic.TryGetValue(esg.pos,out sDic)) 
                    sDic.Add(esg.starLv, esg);
                else
                {
                    sDic = new Dictionary<int, EqptStrongCfg> { { esg.starLv, esg } };
                    eqptStrongDic.Add(esg.pos, sDic);
                };
            }
        }
    }

    /// <summary>
    /// 获取某个装备某个星级下的加成数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public EqptStrongCfg GetStrongCfgData(int pos,int startLv)
    {
        EqptStrongCfg esg = null;
        Dictionary<int, EqptStrongCfg> sDic = null;
        if (eqptStrongDic.TryGetValue(pos, out sDic))
        {
            if(sDic.ContainsKey(startLv)) sDic.TryGetValue(startLv, out esg);
        }
        return esg;
    }

    /// <summary>
    /// 获取某个装备当前星级下某个属性的所有星级加成
    /// </summary>
    /// <param name="pos">位置/装备</param>
    /// <param name="starlv">星级/等级</param>
    /// <param name="prop">属性：血值 hp，伤害值 hurt，防御值 def</param>
    /// <returns></returns>
    public int GetPropAddValPreLv(int pos,int starlv,string prop)
    {
        Dictionary<int, EqptStrongCfg> posDic = null;
        int val = 0;
        if(eqptStrongDic.TryGetValue(pos,out posDic)){
            for(int i = 0; i < starlv; i++)
            {
                EqptStrongCfg esg;
                if(posDic.TryGetValue(i,out esg))
                {
                    switch (prop) 
                    {
                        case "hp":val += esg.addHp; break;
                        case "hurt": val += esg.addHurt; break;
                        case "def": val += esg.addDef; break;
                    }
                }
            }
        }
        return val;
    }
    #endregion

    #region 主城_任务奖励配置解析——taskreward.xml
    /// <summary>
    /// 任务奖励的字典(任务ID，奖励配置文件)
    /// </summary>
    private Dictionary<int, TaskRewardCfg> taskRewardDic = new Dictionary<int, TaskRewardCfg>();
    /// <summary>
    /// 任务奖励配置文件解析
    /// </summary>
    private void InitTaskRewardCfg(string path)
    {
        TextAsset nameXml = Resources.Load<TextAsset>(path);
        if (!nameXml) PECommon.Log("Xml file:" + path + "not exist!", LogType.Error);
        else
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(nameXml.text);
            XmlNodeList nodeList = doc.SelectSingleNode("root").ChildNodes;
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlElement element = nodeList[i] as XmlElement;
                if (element.GetAttributeNode("ID") == null) continue;
                int id = Convert.ToInt32(element.GetAttributeNode("ID").InnerText);
                TaskRewardCfg trc = new TaskRewardCfg { ID = id };
                foreach (XmlElement e in nodeList[i].ChildNodes)
                {
                    switch (e.Name)
                    {
                        case "taskName": trc.taskName = e.InnerText; break;
                        case "count": trc.count = int.Parse(e.InnerText); break;
                        case "coin": trc.coin = int.Parse(e.InnerText); break;
                        case "exp": trc.exp = int.Parse(e.InnerText); break;
                    }
                }
                taskRewardDic.Add(id, trc);
            }
        }
    }
    /// <summary>
    /// 获取任务奖励的配置文件
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public TaskRewardCfg GetTaskRewardCfgData(int id)
    {
        TaskRewardCfg trc = null;
        taskRewardDic.TryGetValue(id, out trc);
        return trc;
    }
    
    #endregion

    #endregion

}