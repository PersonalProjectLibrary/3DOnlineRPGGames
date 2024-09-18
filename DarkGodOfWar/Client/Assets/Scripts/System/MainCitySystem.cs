
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

    public MainCityWnd mainCityWnd;

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
            LoadPlayer(mcMapData);//加载游戏主角、设置人物展示相机
            mainCityWnd.SetWndState();//打开主城UI界面
            audioService.PlayBgMusic(Constants.BgmMainCity);//播放主城的背景音乐

        });
    }

    /// <summary>
    /// 加载角色
    /// </summary>
    /// <param name="mcMapData">主城角色相机配置</param>
    private void LoadPlayer(McMapCfg mcMapData)
    {
        GameObject player = resService.LoadPrefab(PathDefine.AssissnCityPrefab, true);
        player.transform.position = mcMapData.playerBornPos;
        player.transform.localEulerAngles = mcMapData.playerBornRote;
        player.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);

        Camera.main.transform.position = mcMapData.mainCamPos;
        Camera.main.transform.localEulerAngles = mcMapData.mainCamRote;
    }
}