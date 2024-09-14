
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
    /// <summary>
    /// 分段经验条的ItemList
    /// </summary>
    public Transform expPrgTrans;

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

        //expPrg
        GridLayoutGroup grid = expPrgTrans.GetComponent<GridLayoutGroup>();
        //当前项目的UI自适应是基于高度做标准的，这里用高度计算缩放比；
        float globalRate = 1.0f * Constants.ScreenStandardHeight / Screen.height;
        float screenWidth = Screen.width * globalRate;//UI在自适应后，实际展现给玩家的缩放宽度
        float width = (screenWidth - 180) / 10;//每小段经验条的长度
        grid.cellSize = new Vector2(width, 7);
    }
}