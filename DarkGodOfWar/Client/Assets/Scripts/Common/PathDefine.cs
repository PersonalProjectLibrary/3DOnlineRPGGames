
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：Common_PathDefine.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/1 0:30
    功能：路径常量定义
***************************************/
#endregion

/// <summary>
/// 路径常量定义
/// </summary>
public class PathDefine 
{
    #region ConfigsFile Path：配置文件的路径
    /// <summary>
    /// 创建角色界面_随机姓名配置文件
    /// </summary>
    public const string RdNameCfg = "ResConfigs/rdname";
    /// <summary>
    /// 主城_地图配置文件
    /// </summary>
    public const string McMapCfg = "ResConfigs/mcmap";
    /// <summary>
    /// 主城_任务引导配置文件
    /// </summary>
    public const string TaskGuideCfg = "ResConfigs/taskguide";
    /// <summary>
    /// 主城_强化升级配置文件
    /// </summary>
    public const string EqptStrongCfg = "ResConfigs/eqptstrong";

    #endregion

    #region PlayerPrefab Path：角色预制体的路径
    /// <summary>
    /// 主城角色预制体路径
    /// </summary>
    public const string AssissnCityPrefab = "PrefabPlayer/AssassinCity";

    #endregion

    #region Chat Image Path：聊天窗口按钮背景
    /// <summary>
    /// 聊天按钮选中时图片
    /// </summary>
    public const string SelectChat = "ResImages/btntype1";
    /// <summary>
    /// 聊天按钮没选中时图片
    /// </summary>
    public const string UnSelectChat = "ResImages/btntype2";

    #endregion

    #region Strong Image Path：强化界面里按钮背景、装备图片、星级的路径
    //左侧背景图片的路径
    /// <summary>
    /// 默认背景版
    /// </summary>
    public const string ItemPlateBg = "ResImages/charbg3";
    /// <summary>
    /// 选中后箭头背景版
    /// </summary>
    public const string ItemArrowBg = "ResImages/btnstrong";

    //右侧当前装备图片的路径
    public const string ItemToukui = "ResImages/toukui";
    public const string ItemBody = "ResImages/body";
    public const string ItemYaobu = "ResImages/yaobu";
    public const string ItemHand = "ResImages/hand";
    public const string ItemLeg = "ResImages/leg";
    public const string ItemFoot = "ResImages/foot";

    //右侧星级图片的路径
    /// <summary>
    /// 空星星图片
    /// </summary>
    public const string SpStar1 = "ResImages/star1";
    /// <summary>
    /// 实星星图片
    /// </summary>
    public const string SpStar2 = "ResImages/star2";
    #endregion

    #region Dialog CharacterIcon Path：与npc对话显示的对话的角色图片的路径
    /// <summary>
    /// 刺客角色图标
    /// </summary>
    public const string SelfIcon = "ResImages/assassin";
    /// <summary>
    /// 默认引导npc图标
    /// </summary>
    public const string GuideIcon = "ResImages/npcguide";
    /// <summary>
    /// npc智者图标
    /// </summary>
    public const string WiseIcon = "ResImages/npc0";
    /// <summary>
    /// npc将军图标
    /// </summary>
    public const string GeneralIcon = "ResImages/npc1";
    /// <summary>
    /// npc工匠图标
    /// </summary>
    public const string ArtisanIcon = "ResImages/npc2";
    /// <summary>
    /// npc商贩图标
    /// </summary>
    public const string TraderIcon = "ResImages/npc3";

    #endregion

    #region AutoGuide HeadImage Path：任务引导的图标的路径
    /// <summary>
    /// 自动引导_默认头像
    /// </summary>
    public const string TaskHead = "ResImages/task";
    /// <summary>
    /// 自动引导_智者头像
    /// </summary>
    public const string WisekHead = "ResImages/wiseman";
    /// <summary>
    /// 自动引导_将军头像
    /// </summary>
    public const string GeneralHead = "ResImages/general";
    /// <summary>
    /// 自动引导_工匠头像
    /// </summary>
    public const string ArtisanHead = "ResImages/artisan";
    /// <summary>
    /// 自动引导_货商头像
    /// </summary>
    public const string TraderHead = "ResImages/trader";

    #endregion

}