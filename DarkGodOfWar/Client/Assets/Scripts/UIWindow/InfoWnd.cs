
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：UIWindow_InfoWnd.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/19 18:39
    功能：角色信息展示界面
***************************************/
#endregion

using PEProtocol;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 角色信息界面
/// </summary>
public class InfoWnd : WindowRoot
{
    #region UIDefine
    /// <summary>
    /// 角色信息
    /// </summary>
    public Text txtInfo;
    /// <summary>
    /// 经验值
    /// </summary>
    public Text txtExp;
    /// <summary>
    /// 经验值进度条
    /// </summary>
    public Image imgExpPrg;
    /// <summary>
    /// 体力值
    /// </summary>
    public Text txtPower;
    /// <summary>
    /// 体力值进度条
    /// </summary>
    public Image imgPowerPrg;
    /// <summary>
    /// 角色职业
    /// </summary>
    public Text txtJob;
    /// <summary>
    /// 战斗力
    /// </summary>
    public Text txtFight;
    /// <summary>
    /// 血量值
    /// </summary>
    public Text txtHp;
    /// <summary>
    /// 伤害值
    /// </summary>
    public Text txtHurt;
    /// <summary>
    /// 防御值
    /// </summary>
    public Text txtDef;
    /// <summary>
    /// 关闭信息面板按钮
    /// </summary>
    public Button btnClose;
    #endregion

    #region 角色旋转展示
    /// <summary>
    /// 角色展示图片
    /// </summary>
    /// 监控触摸情况，实现对展现的角色旋转查看
    public RawImage imgChar;
    /// <summary>
    /// 鼠标手指点击位置
    /// </summary>
    private Vector2 startPos;

    #endregion

    /// <summary>
    /// 初始化角色信息界面
    /// </summary>
    protected override void InitWnd()
    {
        base.InitWnd();
        RefreshUI();
        RegTouchEvts();
    }

    /// <summary>
    /// 刷新UI界面
    /// </summary>
    private void RefreshUI()
    {
        PlayerData pData = GameRoot.Instance.PlayerData;
        SetText(txtInfo, pData.name + " LV." + pData.lv);
        SetText(txtExp, pData.exp + "/" + PECommon.GetExpUpValByLv(pData.lv));
        imgExpPrg.fillAmount = pData.exp * 1.0f / PECommon.GetExpUpValByLv(pData.lv);
        SetText(txtPower, pData.power + "/" + PECommon.GetPowerLimit(pData.lv));
        imgPowerPrg.fillAmount = pData.power * 1.0f / PECommon.GetPowerLimit(pData.lv);

        SetText(txtJob, " 职业   " + "暗夜刺客");
        SetText(txtFight, " 战力   " + PECommon.GetFightByProps(pData));
        SetText(txtHp, " 血量   " + pData.hp);
        SetText(txtHurt, " 伤害   " + (pData.ad+pData.ap));
        SetText(txtDef, " 防御   " + (pData.addef + pData.apdef));
    }

    /// <summary>
    /// 触摸监听事件注册
    /// </summary>
    private void RegTouchEvts()
    {
        //开始点击触屏
        OnClickDown(imgChar.gameObject, (PointerEventData evt) =>
        {
            startPos = evt.position;
            MainCitySystem.Instance.SetStartRotate();
        });
        OnDrag(imgChar.gameObject, (PointerEventData evt) =>
        {
            float rotate = -(evt.position.x - startPos.x) * 0.4f;
            MainCitySystem.Instance.SetPlayerRotate(rotate);
        });
    }

    /// <summary>
    /// 关闭角色信息面板
    /// </summary>
    public void ClickCloseBtn()
    {
        audioService.PlayUIAudio(Constants.UiClickBtn);
        MainCitySystem.Instance.CloseInfoWnd();
    }
}