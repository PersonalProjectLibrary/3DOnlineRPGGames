
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：System_MainCitySystem.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/13 18:52
    功能：主城业务系统
***************************************/
#endregion


using UnityEngine;

public class MainCitySystem : SystemRoot
{
    public static MainCitySystem Instance = null;

    /// <summary>
    /// 主城界面
    /// </summary>
    public MainCityWnd mainCityWnd;

    /// <summary>
    /// npc位置信息数组
    /// </summary>
    private Transform[] npcPosTrans;

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

    #region 引导任务设置
    /// <summary>
    /// 当前引导任务数据
    /// </summary>
    private AutoGuideCfg curTaskData;

    /// <summary>
    /// 执行引导任务
    /// </summary>
    /// <param name="agc">任务引导数据</param>
    public void RunGuideTask(AutoGuideCfg agc)
    {
        if (agc != null) curTaskData = agc;
        if (curTaskData.npcID == -1) OpenGuideWnd();//id为-1，不需要找npc，直接打开任务对话窗口
        else//解析任务数据，执行相关操作
        {
            //NPC寻路：调用主角在主城的系统去寻路
            
            //在寻路过程中，隔段时间就会进行检测计算主角和目标npc之间的距离
            //当距离大于某限定值，就一直寻路下去：如何实现寻路系统
            //当距离小于某限定值，判断找到目标npc，打开对话界面：怎么判定找到npc
            
            //使用Unity提供的Nevigation导航系统，实现寻路
            //把场景里的npc的exam transform信息传递到mainCitySystem里

        }
    }

    /// <summary>
    /// 打开引导任务的对话界面
    /// </summary>
    private void OpenGuideWnd()
    {
        //TODO
    }

    #endregion


    #region 角色信息界面设置
    /// <summary>
    /// 角色信息界面
    /// </summary>
    public InfoWnd infoWnd;

    /// <summary>
    /// 角色初始角度
    /// </summary>
    private float startRotate = 0;

    /// <summary>
    /// 打开角色信息面板
    /// </summary>
    public void OpenInfoWnd()
    {
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
    public void SetStartRotate()
    {
        startRotate = playerCtrl.transform.localEulerAngles.y;
    }

    /// <summary>
    /// 设置角色信息界面里角色的旋转
    /// </summary>
    /// <param name="rotate"></param>
    public void SetPlayerRotate(float rotate)
    {
        playerCtrl.transform.localEulerAngles = new Vector3(0, startRotate + rotate, 0);
    }

    #endregion

    #region 主城的角色设置
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
    }

    /// <summary>
    /// 向玩家传递方向信息，设置玩家角色移动和停止
    /// </summary>
    /// <param name="dir">摇杆区域点击触发的坐标</param>
    public void SetMoveDir(Vector2 dir)
    {
        //设置角色动画：如果传入的是0，则停止移动，否则角色进行移动
        if (dir == Vector2.zero) playerCtrl.SetBlend(Constants.BlendIdle);
        else playerCtrl.SetBlend(Constants.BlendWalk);

        //设置角色方向，控制移动
        playerCtrl.Dir = dir;
    }

    #endregion

}