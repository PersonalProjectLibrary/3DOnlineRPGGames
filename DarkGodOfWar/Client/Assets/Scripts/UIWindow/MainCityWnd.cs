
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
using UnityEngine.UI;

public class MainCityWnd : WindowRoot
{
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

    protected override void InitWnd()
    {
        base.InitWnd();
        RefreshUI();
    }

    /// <summary>
    /// 刷新主城UI界面
    /// </summary>
    private void RefreshUI()
    {
        PlayerData pData = GameRoot.Instance.PlayerData;//获取玩家数据

        SetText(txtName, pData.name);
        SetText(txtLevel, pData.lv);
        SetText(txtFight, PECommon.GetFightByProps(pData));
        SetText(txtPower, "体力:" + pData.power + "/" + PECommon.GetPowerLimit(pData.lv));
        imgPowerPrg.fillAmount = pData.power * 1.0f / PECommon.GetPowerLimit(pData.lv);
    }
}