
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：UIWindow_MainCityWnd.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/13 18:26
    功能：主城UI界面
***************************************/
#endregion


using PEProtocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 主城界面
/// </summary>
public class MainCityWnd : WindowRoot
{
    #region UIDefine
    /// <summary>
    /// 战斗力
    /// </summary>
    public Text txtFight;
    /// <summary>
    /// 体力
    /// </summary>
    public Text txtPower;
    /// <summary>
    /// 体力进度条
    /// </summary>
    public Image imgPowerPrg;
    /// <summary>
    /// 等级
    /// </summary>
    public Text txtLevel;
    /// <summary>
    /// 名字
    /// </summary>
    public Text txtName;
    /// <summary>
    /// 经验进度条
    /// </summary>
    public Text txtExpPrg;
    /// <summary>
    /// 分段经验条的ItemList
    /// </summary>
    public Transform expPrgTrans;

    /// <summary>
    /// 主菜单显示掩藏的动画
    /// </summary>
    public Animation menuAnim;
    /// <summary>
    /// 主菜单按钮
    /// </summary>
    public Button btnMenu;
    /// <summary>
    /// 当前主菜单激活状态
    /// </summary>
    public bool menuState = true;

    /// <summary>
    /// 摇杆触控区域
    /// </summary>
    public Image imgTouch;
    /// <summary>
    /// 摇杆轮盘
    /// </summary>
    public Image imgDirBg;
    /// <summary>
    /// 摇杆杆轴
    /// </summary>
    /// 初始不显示
    public Image imgDirPoint;
    /// <summary>
    /// 摇杆轮盘默认位置
    /// </summary>
    /// 初始化时就要赋值，摇杆复位位置
    private Vector2 defaultPos = Vector2.zero;
    /// <summary>
    /// 摇杆区域起始点击位置
    /// </summary>
    private Vector2 startPos = Vector2.zero;
    /// <summary>
    /// 缩放后摇杆标准距离实际值
    /// </summary>
    private float pointDis;
    #endregion

    /// <summary>
    /// 当前任务引导配置数据
    /// </summary>
    private AutoGuideCfg curTaskData;
    /// <summary>
    /// 自动任务的按钮
    /// </summary>
    public Button btnGuide;

    #region Main Functions
    /// <summary>
    /// 主城UI初始化
    /// </summary>
    protected override void InitWnd()
    {
        base.InitWnd();

        defaultPos = imgDirBg.transform.position;
        pointDis = Screen.height * 1.0f / Constants.ScreenStandardHeight * Constants.ScreenOpDis;
        SetActive(imgDirPoint, false);
        RegisterTouchEvts();

        RefreshUI();
    }

    /// <summary>
    /// 刷新主城UI界面
    /// </summary>
    public void RefreshUI()
    {
        PlayerData pData = GameRoot.Instance.PlayerData;//获取玩家数据

        SetText(txtName, pData.name);
        SetText(txtLevel, pData.lv);
        SetText(txtFight, PECommon.GetFightByProps(pData));
        SetText(txtPower, "体力:" + pData.power + "/" + PECommon.GetPowerLimit(pData.lv));
        imgPowerPrg.fillAmount = pData.power * 1.0f / PECommon.GetPowerLimit(pData.lv);

        #region Set expPrg
        //经验进度条UI自适应
        GridLayoutGroup grid = expPrgTrans.GetComponent<GridLayoutGroup>();

        //当前项目的UI自适应是基于高度做标准的，这里用高度计算缩放比；
        float globalRate = 1.0f * Constants.ScreenStandardHeight / Screen.height;
        float screenWidth = Screen.width * globalRate;//UI在自适应后，实际展现给玩家的缩放宽度
        float width = (screenWidth - 180) / 10;//每小段经验条的长度
        grid.cellSize = new Vector2(width, 7);

        //经验进度条数值显示
        int expPrgVal = (int)(pData.exp * 1.0f / PECommon.GetExpUpValByLv(pData.lv) * 100);
        SetText(txtExpPrg, expPrgVal + "%");

        //设置分段进度条哪些显示或掩藏，展示玩家经验状态
        int index = expPrgVal / 10;
        for (int i = 0; i < expPrgTrans.childCount; i++)
        {
            Image img = expPrgTrans.GetChild(i).GetComponent<Image>();
            if (i < index) img.fillAmount = 1;
            else if (i == index) img.fillAmount = expPrgVal % 10 * 1.0f / 10;
            else img.fillAmount = 0;
        }

        #endregion

        #region Set guideBtn Icon
        curTaskData = resService.GetGuideCfgData(pData.guideid);
        if (curTaskData == null) SetGuideBtnIcon(-1);
        else SetGuideBtnIcon(curTaskData.npcID);

        #endregion
    }

    #endregion

    #region ClickEvents
    /// <summary>
    /// 点击打开强化升级界面
    /// </summary>
    public void ClickStrongBtn()
    {
        audioService.PlayUIAudio(Constants.UiOpenPage);
        MainCitySystem.Instance.OpenStrongWnd();
    }

    /// <summary>
    /// 点击自动任务按钮
    /// </summary>
    public void ClickGuideBtn()
    {
        audioService.PlayUIAudio(Constants.UiClickBtn);
        if (curTaskData == null) GameRoot.AddTips("更多引导任务，正在开发中...");
        else MainCitySystem.Instance.RunGuideTask(curTaskData);
    }

    /// <summary>
    /// 点击角色头像按钮
    /// </summary>
    /// 打开角色信息面板
    public void ClickHeadBtn()
    {
        audioService.PlayUIAudio(Constants.UiOpenPage);
        MainCitySystem.Instance.OpenInfoWnd();
    }

    /// <summary>
    /// 点击主菜单按钮
    /// </summary>
    public void ClickMenuBtn()
    {
        AnimationClip clip = null;//动画播放的文件
        audioService.PlayUIAudio(Constants.UiExtenBtn);//更新音乐
        menuState = !menuState;//每次点击都修改主菜单UI的状态
        //根据当前状态来选择设置主菜单的显示掩藏
        if (menuState) clip = menuAnim.GetClip("MCMenuOpenAnim");
        else clip = menuAnim.GetClip("MCMenuCloseAnim");
        menuAnim.Play(clip.name);
    }

    #endregion

    #region Tools Functions
    /// <summary>
    /// 注册摇杆事件
    /// </summary>
    public void RegisterTouchEvts()
    {
        /* old
        PEListener listener = imgTouch.gameObject.AddComponent<PEListener>();
        listener.onClickDown = (PointerEventData evt) =>
        {
            imgDirBg.transform.position = evt.position;
        };
        //*/

        //按下摇杆
        OnClickDown(imgTouch.gameObject, (PointerEventData evt) =>
        {
            startPos = evt.position;
            SetActive(imgDirPoint);
            imgDirBg.transform.position = evt.position;//传入的是全局坐标
        });

        //拖拽摇杆
        OnDrag(imgTouch.gameObject, (PointerEventData evt) =>
        {
            Vector2 dir = evt.position - startPos;//拖拽方向
            float len = dir.magnitude;
            if (len > pointDis)
            {
                Vector2 clampDir = Vector2.ClampMagnitude(dir, pointDis);
                imgDirPoint.transform.position = startPos + clampDir;
            }
            else imgDirPoint.transform.position = evt.position;
            //向玩家传递方向信息，设置玩家角色移动
            MainCitySystem.Instance.SetMoveDir(dir.normalized);
        });

        //松开摇杆
        OnClickUp(imgTouch.gameObject, (PointerEventData evt) =>
        {
            imgDirBg.transform.position = defaultPos;
            SetActive(imgDirPoint, false);
            imgDirPoint.transform.localPosition = Vector2.zero;//相对父物体还原使用本地坐标
            //向玩家传递方向信息，停止玩家角色移动
            MainCitySystem.Instance.SetMoveDir(Vector2.zero);
        });
    }

    /// <summary>
    /// 设置 自动任务UI 的图标
    /// </summary>
    /// <param name="npcId">-1为没有任务，设置默认Icon</param>
    private void SetGuideBtnIcon(int npcId)
    {
        //获取目标图片加载路径
        string spPath = "";
        Image img = btnGuide.GetComponent<Image>();
        switch (npcId)
        {
            case Constants.NpcWiseMan: spPath = PathDefine.WisekHead;break;
            case Constants.NpcGeneral: spPath = PathDefine.GeneralHead; break;
            case Constants.NpcArtisan: spPath = PathDefine.ArtisanHead; break;
            case Constants.NpcTrader: spPath = PathDefine.TraderHead; break;
            default: spPath = PathDefine.TaskHead; break;
        }
        SetSprite(img, spPath);//加载图片
    }
    
    #endregion
}