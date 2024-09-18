
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
            mainCityWnd.SetWndState();//打开主城UI界面
            audioService.PlayBgMusic(Constants.BgmMainCity);//播放主城的背景音乐
            LoadPlayer(mcMapData);//加载游戏主角、设置人物展示相机
        });
    }

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
        player.SetActive(true);
    }
}