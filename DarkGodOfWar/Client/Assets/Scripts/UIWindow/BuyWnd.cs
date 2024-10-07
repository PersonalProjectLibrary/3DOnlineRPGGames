
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：UIWindow_BuyWnd.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/10/7 14:4
    功能：购买交易窗口
***************************************/
#endregion

using PEProtocol;
using UnityEngine.UI;

/// <summary>
/// 购买交易窗口
/// </summary>
/// 1、主城场景左上角体力条后边的“购买”按钮，把钻石转化为体力
/// 2、主城场景右下角的“铸造”按钮，把钻石铸造成金币
/// 这两处地方点击打开购买交易窗口
public class BuyWnd : WindowRoot
{
    /// <summary>
    /// 玩家数据
    /// </summary>
    private PlayerData pData;

    /// <summary>
    /// 购买类型，0：钻石购买体力；1：钻石铸造成金币；
    /// </summary>
    private int buyType;
    /// <summary>
    /// 购买窗口显示购买的内容
    /// </summary>
    public Text txtInfo;

    /// <summary>
    /// 初始化购买交易窗口
    /// </summary>
    protected override void InitWnd()
    {
        base.InitWnd();
        pData = GameRoot.Instance.PlayerData;
        RefreshUI();
    }

    /// <summary>
    /// 刷新购买窗口内容显示
    /// </summary>
    private void RefreshUI()
    {
        txtInfo.text = "是否花费" + Constants.SetTxtColor("10钻石", TxtColor.Red);
        switch (buyType)
        {
            case 0://购买体力
                txtInfo.text += "购买" + Constants.SetTxtColor("100体力", TxtColor.Blue)+"?";
                break;
            case 1://铸造金币
                txtInfo.text += "铸造" + Constants.SetTxtColor("1000金币", TxtColor.Blue)+"?";
                break;
        }
    }

    /// <summary>
    /// 设置购买类型，0：购买体力；1：购买金币
    /// </summary>
    /// 在购买窗口打开时使用
    /// 0：钻石购买体力；1：钻石铸造成金币；
    public void SetBuyType(int type) { buyType = type; }

    /// <summary>
    /// 点击确认购买的按钮
    /// </summary>
    public void ClickSureBtn()
    {
        audioService.PlayUIAudio(Constants.UiClickBtn);
        if (pData.diamond < 10)//钻石是否足够进行购买
        {
            GameRoot.AddTips("钻石数量不够，是否进行充值？");
            return;
        }
        GameMsg msg = new GameMsg//发送资源购买的网络消息
        {
            cmd = (int)CMD.ReqBuy,
            reqBuy = new ReqBuy
            {
                buyType = this.buyType,
                diamondCost = 10,
            }
        };
        netService.SendMsg(msg);
    }

    /// <summary>
    /// 关闭购买交易窗口
    /// </summary>
    public void ClickCloseBtn()
    {
        audioService.PlayUIAudio(Constants.UiClickBtn);
        SetWndState(false);
    }
}