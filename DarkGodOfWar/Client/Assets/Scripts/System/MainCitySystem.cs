
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：System_MainCitySystem.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/13 18:52
    功能：主城业务系统
***************************************/
#endregion

using PEProtocol;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 主城系统
/// </summary>
public class MainCitySystem : SystemRoot
{
    public static MainCitySystem Instance = null;

    /// <summary>
    /// 主城界面
    /// </summary>
    public MainCityWnd mainCityWnd;
    /// <summary>
    /// 角色信息界面
    /// </summary>
    public InfoWnd infoWnd;
    /// <summary>
    /// 引导对话界面
    /// </summary>
    public GuideWnd guideWnd;
    /// <summary>
    /// 强化升级界面
    /// </summary>
    public StrongWnd strongWnd;
    /// <summary>
    /// 聊天界面
    /// </summary>
    public ChatWnd chatWnd;
    /// <summary>
    /// 购买交易窗口
    /// </summary>
    public BuyWnd buyWnd;

    /// <summary>
    /// 初始化主城系统
    /// </summary>
    public override void InitSystem()
    {
        base.InitSystem();
        Instance = this;
        PECommon.Log("Init MainCitySystem...");
    }

    /// <summary>
    /// 进入主城
    /// </summary>
    public void EnterMainCity()
    {
        McMapCfg mcMapData = resService.GetMapCfgData(Constants.IDMainCityMap);
        resService.AsyncLoadScene(mcMapData.sceneName, () =>
        {
            PECommon.Log("Enter MainCity...");//输出日志
            mainCityWnd.SetWndState();//打开主城UI界面
            audioService.PlayBgMusic(Constants.BgmMainCity);//播放主城的背景音乐
            //获取主城的npc位置信息
            GameObject mapObj = GameObject.FindGameObjectWithTag("MapRoot");
            MainCityMap mainCityMap = mapObj.GetComponent<MainCityMap>();
            npcPosTrans = mainCityMap.npcPosTrans;

            LoadPlayer(mcMapData);//加载游戏主角、设置主相机跟随角色
            //设置用于角色信息界面显示角色的相机
            if (charaterCam == null) 
                charaterCam = GameObject.FindGameObjectWithTag("CharacterCam").transform;
            charaterCam.gameObject.SetActive(false);
        });
    }

    /// <summary>
    /// 根据服务器推送的体力消息设置玩家的体力信息
    /// </summary>
    /// <param name="msg"></param>
    public void PshPower(GameMsg msg)
    {
        PshPower data = msg.pshPower;
        GameRoot.Instance.SetPlayerDataByPower(data);
        mainCityWnd.RefreshUI();
    }

    #region Buy Wnd：购买交易窗口设置
    /// <summary>
    /// 打开购买窗口，0：购买体力；1：购买金币；
    /// </summary>
    /// <param name="buyType">0：购买体力；1：购买金币；</param>
    public void OpenBuyWnd(int buyType)
    {
        buyWnd.SetBuyType(buyType);
        buyWnd.SetWndState();
    }

    /// <summary>
    /// 处理服务器回应资源购买请求的消息
    /// </summary>
    /// <param name="msg"></param>
    public void RspBuy(GameMsg msg)
    {
        //更新玩家数据
        GameRoot.Instance.SetPlayerDataByBuy(msg.rspBuy);
        GameRoot.AddTips("购买成功");
        //更新UI显示，只刷新体力UI即可，直接在主城UI里看不到金币数量
        //可打开强化界面，强化界面打开时也会根据玩家信息更新金币信息，不用这里设置
        mainCityWnd.RefreshUI();
        buyWnd.SetWndState(false);
    }

    #endregion

    #region Chat Wnd：聊天界面设置
    /// <summary>
    /// 打开聊天界面
    /// </summary>
    public void OpenChatWnd() { chatWnd.SetWndState(); }

    /// <summary>
    /// 处理服务器广播的世界聊天消息
    /// </summary>
    /// <param name="msg"></param>
    public void PshWorldChat(GameMsg msg)
    {
        chatWnd.AddWorldChatMsg(msg.pshWorldChat.name, msg.pshWorldChat.chat);
    }

    #endregion

    #region Strong Wnd：强化升级界面设置
    /// <summary>
    /// 打开强化升级界面
    /// </summary>
    public void OpenStrongWnd() { strongWnd.SetWndState(); }

    /// <summary>
    /// 处理服务器回应引导任务请求的消息
    /// </summary>
    /// <param name="msg"></param>
    public void RspStrong(GameMsg msg)
    {
        //计算战力
        int zhanliPre = PECommon.GetFightByProps(GameRoot.Instance.PlayerData);
        GameRoot.Instance.SetPlayerDataByStrong(msg.rspStrong);
        int zhanliNow = PECommon.GetFightByProps(GameRoot.Instance.PlayerData);
        GameRoot.AddTips(Constants.SetTxtColor("战力提升 " + (zhanliNow - zhanliPre), TxtColor.Blue));
        //更新UI显示
        strongWnd.UpdateUI();//刷新强化界面UI显示
        mainCityWnd.RefreshUI();//刷新主城界面UI显示
    }

    #endregion

    #region Guide Wnd：引导任务界面设置
    /// <summary>
    /// 当前引导任务数据
    /// </summary>
    private AutoGuideCfg curTaskData;
    /// <summary>
    /// npc位置信息数组
    /// </summary>
    private Transform[] npcPosTrans;
    /// <summary>
    /// 角色自动导航组件
    /// </summary>
    private NavMeshAgent navAgent;
    /// <summary>
    /// 是否在自动寻路中
    /// </summary>
    private bool isNavGuide = false;

    /// <summary>
    /// 获取当前任务数据
    /// </summary>
    /// <returns></returns>
    public AutoGuideCfg GetCurTaskData() { return curTaskData; }

    /// <summary>
    /// 执行引导任务
    /// </summary>
    /// <param name="agc">任务引导数据</param>
    public void RunGuideTask(AutoGuideCfg agc)
    {
        if (agc != null) curTaskData = agc;
        navAgent.enabled = true;
        if (curTaskData.npcID == -1) OpenGuideWnd();//id为-1，不需要找npc，直接打开任务对话窗口
        else//解析任务数据，执行相关操作
        {
            //guide.xml表里npcID和npcPosTrans数组索引一一对应，指向相同的npc；
            //计算目标位置与玩家角色之间位置距离
            float dis = Vector3.Distance(playerCtrl.transform.position, npcPosTrans[agc.npcID].position);
            if (dis < 0.5f)
            {
                isNavGuide = false;
                navAgent.isStopped = true;
                playerCtrl.SetBlend(Constants.BlendIdle);
                navAgent.enabled = false;
                OpenGuideWnd();
            }
            else
            {
                isNavGuide = true;
                navAgent.enabled = true;
                navAgent.speed = Constants.PlayerMoveSpeed;
                navAgent.SetDestination(npcPosTrans[agc.npcID].position);
                playerCtrl.SetBlend(Constants.BlendWalk);
            }
        }
    }

    /// <summary>
    /// 处理服务器回应引导任务请求的消息
    /// </summary>
    /// <param name="msg"></param>
    public void RspGuide(GameMsg msg)
    {
        RspGuide data = msg.rspGuide;//获取服务器数据
        //Tips弹出提示获取的奖励
        //GameRoot.AddTips("任务奖励 金币+" + curTaskData.coin + " 经验+" + curTaskData.exp);
        GameRoot.AddTips(Constants.SetTxtColor("任务奖励 金币+" + curTaskData.coin + " 经验+" + curTaskData.exp, TxtColor.Blue));
        //读取任务的actID，执行相应的操作
        switch (curTaskData.actID)
        {
            case 0: break;//与智者对话
            case 1: break;//TODO 进入副本
            case 2: break;//TODO 进行装备强化
            case 3: break;//TODO 进行体力购买
            case 4: break;//TODO 进行金币购买
            case 5: break;//TODO 进行世界聊天
        }
        GameRoot.Instance.SetPlayerDataByGuide(data);//把更新的玩家数据，更新到GameRoot里
        mainCityWnd.RefreshUI();//刷新主城UI显示
    }

    /// <summary>
    /// 进行自动导航
    /// </summary>
    private void Update()
    {
        if (isNavGuide)
        {
            IsArriveNavPos();//寻路中实时检测是否到目标位置，到位置结束寻路
            playerCtrl.SetCamMove();// 自动任务时相机跟随
        }
    }

    #region Tools Function
    /// <summary>
    /// 打开引导任务的对话界面
    /// </summary>
    private void OpenGuideWnd() { guideWnd.SetWndState(); }

    /// <summary>
    /// 判断是否自动导航到目标位置
    /// </summary>
    private void IsArriveNavPos()
    {
        float dis = Vector3.Distance(playerCtrl.transform.position, npcPosTrans[curTaskData.npcID].position);
        if (dis < 0.5f)
        {
            isNavGuide = false;
            navAgent.isStopped = true;
            playerCtrl.SetBlend(Constants.BlendIdle);
            navAgent.enabled = false;
            OpenGuideWnd();
        }
    }

    /// <summary>
    /// 停止自动任务
    /// </summary>
    private void StopNavTask()
    {
        if (isNavGuide)
        {
            isNavGuide = false;
            navAgent.isStopped = true;
            navAgent.enabled = false;
            playerCtrl.SetBlend(Constants.BlendIdle);
        }
    }
    #endregion

    #endregion

    #region Info Wnd：角色信息界面设置
    /// <summary>
    /// 角色初始角度
    /// </summary>
    private float startRotate = 0;

    /// <summary>
    /// 打开角色信息面板
    /// </summary>
    public void OpenInfoWnd()
    {
        StopNavTask();//结束自动寻路
        if (charaterCam == null)
            charaterCam = GameObject.FindGameObjectWithTag("CharacterCam").transform;

        //设置角色的相对位置(反复手动测试测出来的合适位置)
        charaterCam.localPosition = playerCtrl.transform.position + playerCtrl.transform.forward * 3.8f + new Vector3(0, 1.2f, 0);
        charaterCam.localEulerAngles = new Vector3(0, 180 + playerCtrl.transform.localEulerAngles.y, 0);
        charaterCam.localScale = Vector3.one;
        charaterCam.gameObject.SetActive(true);
        infoWnd.SetWndState();
    }

    /// <summary>
    /// 关闭角色信息面板
    /// </summary>
    public void CloseInfoWnd()
    {
        if (charaterCam != null) charaterCam.gameObject.SetActive(false);
        infoWnd.SetWndState(false);
    }

    /// <summary>
    /// 记录角色初始角度
    /// </summary>
    public void SetStartRotate() { startRotate = playerCtrl.transform.localEulerAngles.y; }

    /// <summary>
    /// 设置角色信息界面里角色的旋转
    /// </summary>
    /// <param name="rotate"></param>
    public void SetPlayerRotate(float rotate)
    {
        playerCtrl.transform.localEulerAngles = new Vector3(0, startRotate + rotate, 0);
    }

    #endregion

    #region Character：主城的角色设置
    /// <summary>
    /// 角色相机
    /// </summary>
    private Transform charaterCam;

    /// <summary>
    /// 玩家角色控制器
    /// </summary>
    private PlayerController playerCtrl;

    /// <summary>
    /// 加载角色
    /// </summary>
    /// <param name="mcMapData">主城角色相机配置</param>
    /// 注：存在角色加载比碰撞体加载快，
    /// 即使添加主城碰撞体和根据配置设置了角色位置，还是出现角色穿透主城的问题；
    /// 故这里实例化角色后，先掩藏角色，等角色和相机根据配置表设置好后，再把角色显示出来，
    /// 这样角色就不会穿透主城一直掉落了。
    private void LoadPlayer(McMapCfg mcMapData)
    {
        GameObject player = resService.LoadPrefab(PathDefine.AssissnCityPrefab, true);
        player.SetActive(false);
        player.transform.position = mcMapData.playerBornPos;
        player.transform.localEulerAngles = mcMapData.playerBornRote;
        player.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        Camera.main.transform.position = mcMapData.mainCamPos;
        Camera.main.transform.localEulerAngles = mcMapData.mainCamRote;

        playerCtrl = player.GetComponent<PlayerController>();
        playerCtrl.Init();
        player.SetActive(true);
        navAgent = player.GetComponent<NavMeshAgent>();
    }

    /// <summary>
    /// 向玩家传递方向信息，设置玩家角色移动和停止
    /// </summary>
    /// <param name="dir">摇杆区域点击触发的坐标</param>
    public void SetMoveDir(Vector2 dir)
    {
        StopNavTask(); //取消自动寻路
        //设置角色动画：如果传入的是0，则停止移动，否则角色进行移动
        if (dir == Vector2.zero) playerCtrl.SetBlend(Constants.BlendIdle);
        else playerCtrl.SetBlend(Constants.BlendWalk);

        //设置角色方向，控制移动
        playerCtrl.Dir = dir;
    }

    #endregion

}