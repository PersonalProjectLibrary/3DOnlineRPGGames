
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：UIWindow_ChatWnd.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/10/5 15:57
    功能：聊天界面
***************************************/
#endregion


using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// 聊天界面
/// </summary>
public class ChatWnd : WindowRoot
{
    #region UIDefine
    /// <summary>
    /// 世界聊天按钮图片
    /// </summary>
    public Image imgWorld;
    /// <summary>
    /// 工会聊天按钮图片
    /// </summary>
    public Image imgGuild;
    /// <summary>
    /// 好友聊天按钮图片
    /// </summary>
    public Image imgFriend;
    /// <summary>
    /// 当前聊天类型：世界0，工会1，好友2
    /// </summary>
    private int chatType;
    /// <summary>
    /// 显示聊天内容的文本
    /// </summary>
    public Text txtChat;
    /// <summary>
    /// 聊天消息输入框
    /// </summary>
    public InputField iptChat;
    #endregion

    /// <summary>
    /// 记录存储世界聊天消息
    /// </summary>
    /// 后面读取list，把聊天记录显示到聊天界面里
    private List<string> worldChatList = new List<string>();

    /// <summary>
    /// 初始化聊天界面
    /// </summary>
    protected override void InitWnd()
    {
        base.InitWnd();
        chatType = 0;
        RefreshUI();
    }

    /// <summary>
    /// 刷新界面UI显示
    /// </summary>
    private void RefreshUI()
    {
        string chatMsg = "";
        if (chatType == 0)
        {
            //设置显示的聊天消息
            for (int i = 0; i < worldChatList.Count; i++) chatMsg += worldChatList[i] + "\n";
            //聊天按钮显示控制
            SetSprite(imgWorld, PathDefine.SelectChat);
            SetSprite(imgGuild, PathDefine.UnSelectChat);
            SetSprite(imgFriend, PathDefine.UnSelectChat);
        }
        else if (chatType == 1)
        {
            chatMsg = "尚未加入工会";
            SetSprite(imgWorld, PathDefine.UnSelectChat);
            SetSprite(imgGuild, PathDefine.SelectChat);
            SetSprite(imgFriend, PathDefine.UnSelectChat);
        }
        else if (chatType == 2)//这里也用else if，后面方便扩展添加其他聊天窗口选项设置
        {
            chatMsg = "暂无好友信息";
            SetSprite(imgWorld, PathDefine.UnSelectChat);
            SetSprite(imgGuild, PathDefine.UnSelectChat);
            SetSprite(imgFriend, PathDefine.SelectChat);
        }
        SetText(txtChat, chatMsg);
    }

    /// <summary>
    /// 点击世界聊天按钮
    /// </summary>
    public void ClicWorldkBtn()
    {
        audioService.PlayUIAudio(Constants.UiClickBtn);
        chatType = 0;
        RefreshUI();
    }

    /// <summary>
    /// 点击公会聊天按钮
    /// </summary>
    public void ClickGuildBtn()
    {
        audioService.PlayUIAudio(Constants.UiClickBtn);
        chatType = 1;
        RefreshUI();
    }

    /// <summary>
    /// 点击好友聊天按钮
    /// </summary>
    public void ClickFriendBtn()
    {
        audioService.PlayUIAudio(Constants.UiClickBtn);
        chatType = 2;
        RefreshUI();
    }

    /// <summary>
    /// 发送聊天消息
    /// </summary>
    public void ClickSendBtn()
    {
        if (iptChat.text == null || iptChat.text == "" || iptChat.text == " ")
            GameRoot.AddTips("尚未输入聊天信息");
        else if (iptChat.text.Length > 12) GameRoot.AddTips("输入信息不能超过12个字");
        else//发送网络消息到服务器
        {

        }
    }

    /// <summary>
    /// 关闭聊天界面
    /// </summary>
    public void ClickCloseBtn()
    {
        audioService.PlayUIAudio(Constants.UiClickBtn);
        chatType = 0;
        SetWndState(false);
    }
}