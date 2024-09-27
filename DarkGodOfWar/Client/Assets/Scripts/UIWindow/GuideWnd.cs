
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：UIWindow_GuideWnd.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/27 0:27
    功能：引导对话界面
***************************************/
#endregion

using PEProtocol;
using UnityEngine.UI;

/// <summary>
/// 引导对话界面
/// </summary>
public class GuideWnd : WindowRoot
{
    #region UIDefine
    /// <summary>
    /// 角色名字
    /// </summary>
    public Text txtName;
    /// <summary>
    /// 对话内容
    /// </summary>
    public Text txtTalk;
    /// <summary>
    /// 人物图标
    /// </summary>
    public Image imgIcon;

    #endregion

    /// <summary>
    /// 玩家数据
    /// </summary>
    private PlayerData pData;
    /// <summary>
    /// 当前任务数据
    /// </summary>
    private AutoGuideCfg curTaskData;
    /// <summary>
    /// 任务对话数据
    /// </summary>
    private string[] dialogArr;
    /// <summary>
    /// 当前对话索引号，从1开始
    /// </summary>
    private int dialogIndex;

    /// <summary>
    /// 初始化对话窗口
    /// </summary>
    protected override void InitWnd()
    {
        base.InitWnd();
        pData = GameRoot.Instance.PlayerData;
        curTaskData = MainCitySystem.Instance.GetCurTaskData();
        dialogArr = curTaskData.dialogArr.Split('#');//分隔对话数据
        dialogIndex = 1;
        SetTalk();
    }

    /// <summary>
    /// 点击切换下一条对话
    /// </summary>
    public void ClickNextBtn()
    {
        audioService.PlayUIAudio(Constants.UiClickBtn);
        dialogIndex += 1;
        if (dialogIndex == dialogArr.Length)
        {
            GameMsg msg = new GameMsg//设置引导请求消息
            {
                cmd = (int)CMD.ReqGuide,
                reqGuide = new ReqGuide { guideid = curTaskData.ID }
            };
            netService.SendMsg(msg);//向服务器发送引导请求信息
            SetWndState(false);
        }
        else SetTalk();
    }

    /// <summary>
    /// 解析设置对话内容
    /// </summary>
    private void SetTalk()
    {
        string[] talkArr = dialogArr[dialogIndex].Split('|');//分割对话编号和对话内容
        if (talkArr[0] == "0")//角色的对话
        {
            SetSprite(imgIcon, PathDefine.SelfIcon);
            SetText(txtName, pData.name);
        }
        else//npc的对话
        {
            switch (curTaskData.npcID)
            {
                case 0:
                    SetSprite(imgIcon, PathDefine.WiseIcon);
                    SetText(txtName, "智者");
                    break;
                case 1:
                    SetSprite(imgIcon, PathDefine.GeneralIcon);
                    SetText(txtName, "将军");
                    break;
                case 2:
                    SetSprite(imgIcon, PathDefine.ArtisanIcon);
                    SetText(txtName, "工匠");
                    break;
                case 3:
                    SetSprite(imgIcon, PathDefine.TraderIcon);
                    SetText(txtName, "商人");
                    break;
                default:
                    SetSprite(imgIcon, PathDefine.GuideIcon);
                    SetText(txtName, "小仙女");
                    break;
            }
        }
        imgIcon.SetNativeSize();//图标icon自适应，不随尺寸进行拉伸变形
        SetText(txtTalk, talkArr[1].Replace("$name", pData.name));//文本中的$name，替换为玩家名字
    }

}