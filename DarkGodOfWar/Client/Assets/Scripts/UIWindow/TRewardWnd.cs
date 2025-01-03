
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：UIWindow_TaskWnd.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/10/19 19:18
    功能：任务奖励界面
***************************************/
#endregion

using PEProtocol;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 任务奖励界面
/// </summary>
public class TRewardWnd : WindowRoot
{
    private PlayerData pData = null;
    private List<TaskRewardData> taskRewardList = new List<TaskRewardData>();
    public Transform itemContentTrans;

    /// <summary>
    /// 初始化任务奖励面板
    /// </summary>
    protected override void InitWnd()
    {
        base.InitWnd();
        pData = GameRoot.Instance.PlayerData;
        RefreshUI();
    }

    /// <summary>
    /// 刷新界面显示
    /// </summary>
    private void RefreshUI()
    {
        SortTRDataList();
        ShowAllTRItem();
    }

    /// <summary>
    /// 点击奖励按钮
    /// </summary>
    /// <param name="btnName"></param>
    private void ClickRewardBtn(string btnName)
    {
        //点击的是哪个Item
        string[] nameArr = btnName.Split('_');
        int index = int.Parse(nameArr[1]);
        //发送网络消息
        GameMsg msg = new GameMsg
        {
            cmd = (int)CMD.ReqTaskReward,
            reqTaskReward = new ReqTaskReward
            {
                rewardid = taskRewardList[index].ID
            }
        };
        netService.SendMsg(msg);
        //根据配置文件信息，显示成功领取奖励的提示
        TaskRewardCfg trc = resService.GetTaskRewardCfgData(taskRewardList[index].ID);
        GameRoot.AddTips(Constants.SetTxtColor("获得奖励：", TxtColor.Blue) + Constants.SetTxtColor(" 金币 + " + trc.coin + " 经验 + " + trc.exp, TxtColor.Green));
    }

    /// <summary>
    /// 关闭任务奖励界面
    /// </summary>
    public void ClickCloseBtn()
    {
        audioService.PlayUIAudio(Constants.UiClickBtn);
        SetWndState(false);
    }

    #region Tool Func
    /// <summary>
    /// 对任务奖励数据进行排序
    /// </summary>
    private void SortTRDataList()
    {
        taskRewardList.Clear();
        List<TaskRewardData> todoList = new List<TaskRewardData>();//待完成的任务
        List<TaskRewardData> doneList = new List<TaskRewardData>();//已经完成的任务
        for (int i = 0; i < pData.taskRewardArr.Length; i++)//遍历解析玩家的任务奖励数据
        {
            string[] taskInfo = pData.taskRewardArr[i].Split('|');
            TaskRewardData trd = new TaskRewardData
            {
                ID = int.Parse(taskInfo[0]),
                prgs = int.Parse(taskInfo[1]),
                taked = taskInfo[2].Equals("1")//0是没被领取，1是被领取
            };
            if (trd.taked) doneList.Add(trd);
            else todoList.Add(trd);
        }
        taskRewardList.AddRange(todoList);
        taskRewardList.AddRange(doneList);
    }
    /// <summary>
    /// 显示所有任务奖励条目
    /// </summary>
    private void ShowAllTRItem()
    {
        int c = itemContentTrans.childCount;//清除旧条目
        for (int i = c; i > 0; i--) Destroy(itemContentTrans.GetChild(i - 1).gameObject);
        for (int i = 0; i < taskRewardList.Count; i++)//生成新条目
        {
            GameObject item = resService.LoadPrefab(PathDefine.TRewardItemPrefab);
            item.transform.SetParent(itemContentTrans);
            item.transform.localPosition = Vector3.zero;
            item.transform.localScale = Vector3.one;
            item.name = "TRewardItem_" + i;
            TaskRewardData trd = taskRewardList[i];//根据奖励id找到对应配置信息cfg
            TaskRewardCfg trc = resService.GetTaskRewardCfgData(trd.ID);
            SetTRItem(item.transform, trd, trc);
        }
    }
    /// <summary>
    /// 设置任务奖励条目
    /// </summary>
    /// <param name="item"></param>
    /// <param name="trd"></param>
    /// <param name="trc"></param>
    private void SetTRItem(Transform item, TaskRewardData trd, TaskRewardCfg trc)
    {
        SetText(FindAndGetTrans(item, "txtName"), trc.taskName);
        SetText(FindAndGetTrans(item, "txtPrg"), trd.prgs + "/" + trc.count);
        SetText(FindAndGetTrans(item, "txtExp"), "奖励：    经验" + trc.exp);
        SetText(FindAndGetTrans(item, "txtCoin"), "金币" + trc.coin);
        Image imgPrg = FindAndGetTrans(item, "prgBar/prgVal").GetComponent<Image>();
        float prgVal = trd.prgs * 0.1f / trc.count;
        imgPrg.fillAmount = prgVal;
        Button btnTake = FindAndGetTrans(item, "btnReward").GetComponent<Button>();
        Transform isTakeImg = FindAndGetTrans(item, "imgComp");
        btnTake.onClick.AddListener(() => { ClickRewardBtn(item.name); });
        if (trd.taked)
        {
            SetActive(isTakeImg);
            btnTake.interactable = false;
        }
        else
        {
            SetActive(isTakeImg, false);
            if (trd.prgs == trc.count) btnTake.interactable = true;
            else btnTake.interactable = true;
        }
    }

    #endregion
}