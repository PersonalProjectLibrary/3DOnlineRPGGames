
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：UIWindow_StrongWnd.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/30 0:9
    功能：强化升级界面
***************************************/
#endregion

using PEProtocol;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 强化升级界面
/// </summary>
public class StrongWnd : WindowRoot
{
    /// <summary>
    /// 玩家数据
    /// </summary>
    private PlayerData pData;
    /// <summary>
    /// 下一星级的强化数据
    /// </summary>
    private EqptStrongCfg nextEsg;

    #region Left Btns：左侧装备按钮
    /// <summary>
    /// 左侧选中的图片的索引值
    /// </summary>
    private int curIndex;
    /// <summary>
    /// 左侧图片的父物体
    /// </summary>
    public Transform leftImgPos;
    /// <summary>
    /// 左侧按钮的图片
    /// </summary>
    private Image[] leftImgs = new Image[6];

    #endregion

    #region UIDefine
    /// <summary>
    /// 右侧显示的当前装备图片
    /// </summary>
    public Image imgCurtPos;
    /// <summary>
    /// 装备星级
    /// </summary>
    public Text txtStarLv;
    /// <summary>
    /// 显示的星级图标的父物体
    /// </summary>
    public Transform starTransGrp;

    /// <summary>
    /// 升级前血值
    /// </summary>
    public Text propHp1;
    /// <summary>
    /// 升级前伤害值
    /// </summary>
    public Text propHurt1;
    /// <summary>
    /// 升级前防御值
    /// </summary>
    public Text propDef1;
    /// <summary>
    /// 升级后血值
    /// </summary>
    public Text propHp2;
    /// <summary>
    /// 升级后伤害值
    /// </summary>
    public Text propHurt2;
    /// <summary>
    /// 升级后防御值
    /// </summary>
    public Text propDef2;
    /// <summary>
    /// 生命值提升箭头
    /// </summary>
    public Image propArrow1;
    /// <summary>
    /// 伤害值提升箭头
    /// </summary>
    public Image propArrow2;
    /// <summary>
    /// 防御值提升箭头
    /// </summary>
    public Image propArrow3;

    /// <summary>
    /// 升级消耗面板
    /// </summary>
    public Transform costInfo;
    /// <summary>
    /// 升级所需最低等级
    /// </summary>
    public Text txtNeedLv;
    /// <summary>
    /// 升级所需金币
    /// </summary>
    public Text txtCostCoin;
    /// <summary>
    /// 升级所需水晶
    /// </summary>
    public Text txtCostCrystal;
    /// <summary>
    /// 升级后的剩余金币
    /// </summary>
    public Text txtCoin;

    #endregion

    /// <summary>
    /// 初始化强化升级面板
    /// </summary>
    protected override void InitWnd()
    {
        base.InitWnd();
        pData = GameRoot.Instance.PlayerData;
        RegClickEvts();
        ClickPosItem(0);
    }

    /// <summary>
    /// 点击左侧装备图片，刷新UI，显示对应装备数据
    /// </summary>
    private void ClickPosItem(int index)
    {
        PECommon.Log("ClickItem：" + index);
        curIndex = index;
        for (int i = 0; i < leftImgs.Length; i++)
        {
            Transform trans = leftImgs[i].transform;
            if (i == curIndex)//设置选中的图片
            {
                SetSprite(leftImgs[i], PathDefine.ItemArrowBg);
                trans.localPosition = new Vector3(10, trans.localPosition.y, 0);
                trans.GetComponent<RectTransform>().sizeDelta = new Vector2(250, 95);
            }
            else//设置为默认背景版
            {
                SetSprite(leftImgs[i], PathDefine.ItemPlateBg);
                trans.localPosition = new Vector3(0, trans.localPosition.y, 0);
                trans.GetComponent<RectTransform>().sizeDelta = new Vector2(220, 85);
            }
        }
        RefreshUI();
    }

    /// <summary>
    /// 点击强化升级按钮
    /// </summary>
    public void ClickStrongBtn()
    {
        audioService.PlayUIAudio(Constants.UiClickBtn);
        //对本地数据进行筛选过滤
        if (pData.strongArr[curIndex] >= 10)
        {
            GameRoot.AddTips("满星已经升满");
            return;
        }
        if (pData.lv < nextEsg.minLv)
        {
            GameRoot.AddTips("角色等级不够");
            return;
        }
        if (pData.coin < nextEsg.coin)
        {
            GameRoot.AddTips("金币数量不够");
            return;
        }
        if (pData.crystal < nextEsg.crystal)
        {
            GameRoot.AddTips("水晶数量不够");
            return;
        }
        //通过筛选过滤，发送强化请求
        netService.SendMsg(new GameMsg
        {
            cmd = (int)CMD.ReqStrong,
            reqStrong = new ReqStrong { pos = curIndex }
        });
    }

    /// <summary>
    /// 关闭强化界面
    /// </summary>
    public void ClickCloseBtn()
    {
        audioService.PlayUIAudio(Constants.UiClickBtn);
        SetWndState(false);
    }

    #region Tools Function
    /// <summary>
    /// 强化后刷新游戏的UI显示
    /// </summary>
    /// 每次点击位置时，都会刷新强化界面的显示，
    /// 这里也直接使用：点击位置的方式获取最新数据，刷新强化界面
    public void UpdateUI()
    {
        audioService.PlayUIAudio(Constants.FuBenEnter);
        ClickPosItem(curIndex);//点击装备，显示对应装备数据
    }

    /// <summary>
    /// 刷新强化界面
    /// </summary>
    private void RefreshUI()
    {
        SetText(txtCoin, pData.coin);//金币
        switch (curIndex)//当前装备图片
        {
            case 0: SetSprite(imgCurtPos, PathDefine.ItemToukui); break;
            case 1: SetSprite(imgCurtPos, PathDefine.ItemBody); break;
            case 2: SetSprite(imgCurtPos, PathDefine.ItemYaobu); break;
            case 3: SetSprite(imgCurtPos, PathDefine.ItemHand); break;
            case 4: SetSprite(imgCurtPos, PathDefine.ItemLeg); break;
            case 5: SetSprite(imgCurtPos, PathDefine.ItemFoot); break;
        }
        //星级
        int curStarLv = pData.strongArr[curIndex];
        SetText(txtStarLv, curStarLv + "星级");
        for (int i = 0; i < starTransGrp.childCount; i++)
        {
            Image img = starTransGrp.GetChild(i).GetComponent<Image>();
            if (i < curStarLv) SetSprite(img, PathDefine.SpStar2);
            else SetSprite(img, PathDefine.SpStar1);
        }
        //当前星级属性的总加成
        int nextStarLv = curStarLv + 1;
        int sumAddHp = resService.GetPropAddValPreLv(curIndex, nextStarLv, "hp");
        int sumAddHurt = resService.GetPropAddValPreLv(curIndex, nextStarLv, "hurt");
        int sumAddDef = resService.GetPropAddValPreLv(curIndex, nextStarLv, "def");
        SetText(propHp1, "生命 +" + sumAddHp);
        SetText(propHurt1, "伤害 +" + sumAddHurt);
        SetText(propDef1, "防御 +" + sumAddDef);
        //下一级星级可获得的属性加成
        nextEsg = resService.GetStrongCfgData(curIndex, nextStarLv);
        if (nextEsg != null)//没升满星
        {
            SetActive(propHp2);
            SetActive(propDef2);
            SetActive(propHurt2);

            SetActive(costInfo);
            SetActive(propArrow1);
            SetActive(propArrow2);
            SetActive(propArrow3);

            SetText(propHp2, "+" + nextEsg.addHp);
            SetText(propDef2, "+" + nextEsg.addDef);
            SetText(propHurt2, "+" + nextEsg.addHurt);
            SetText(txtNeedLv, nextEsg.minLv);
            SetText(txtCostCoin, nextEsg.coin);
            SetText(txtCostCrystal, nextEsg.crystal + "/" + pData.crystal);
        }
        else//升满星，无下一级
        {
            SetActive(propHp2, false);
            SetActive(propDef2, false);
            SetActive(propHurt2, false);

            SetActive(costInfo, false);
            SetActive(propArrow1, false);
            SetActive(propArrow2, false);
            SetActive(propArrow3, false);
        }
    }

    /// <summary>
    /// 左侧按钮图片添加点击事件监听
    /// </summary>
    private void RegClickEvts()
    {
        for (int i = 0; i < leftImgPos.childCount; i++)
        {
            Image img = leftImgPos.GetChild(i).GetComponent<Image>();

            OnClick(img.gameObject, (object args) =>
            {
                ClickPosItem((int)args);
                audioService.PlayUIAudio(Constants.UiClickBtn);
            }, i);

            leftImgs[i] = img;
        }
    }

    #endregion

}