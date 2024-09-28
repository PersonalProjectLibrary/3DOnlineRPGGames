
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：Common_Constants.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/8/30 13:46
    功能：常量配置
***************************************/
#endregion

public class Constants
{
    //场景名称
    public const string SceneLogin = "SceneLogin";//登录场景
    public const int IDMainCityMap = 10000;//主城场景地图配置数据id
    //public const string SceneMainCity = "SceneMainCity";//主城场景

    //背景音乐
    public const string BgmLogin = "bgLogin";//登录界面的背景音乐
    public const string BgmMainCity = "bgMainCity";//主城界面的背景音乐

    //按钮音效
    public const string UiLoginBtn = "uiLoginBtn";//点击登录按钮的音效
    public const string UiClickBtn = "uiClickBtn";//常规UI的点击音效
    public const string UiExtenBtn = "uiExtenBtn";//点击主城里的主菜单按钮的音效
    public const string UiOpenPage = "uiOpenBtn";//打开页面/大的窗口界面的音效

    //屏幕标准宽高
    public const int ScreenStandardWidth = 1334;
    public const int ScreenStandardHeight = 750;

    //摇杆点标准距离
    public const int ScreenOpDis = 90;//摇杆杆轴可偏移距离

    //移动速度
    public const int PlayerMoveSpeed = 8;//角色移动速度
    public const int MonsterMoveSpeed = 4;//怪物移动速度

    //Blend动画混合参数
    public const int BlendIdle = 0;//idle动画时Blend值
    public const int BlendWalk = 1;//walk动画时Blend值
    public const float AccelerateSpeed = 5;//角色blend动画变化的加速度

    //AutoGuide NPC
    public const int NpcWiseMan = 0;//智者npc
    public const int NpcGeneral = 1;//将军npc
    public const int NpcArtisan = 2;//工匠npc
    public const int NpcTrader = 3;//货商npc

    //动态弹窗文字颜色：<color=#FF0000FF>Tips文字</color>
    private const string ColorDefault = "<color=#FFAF36>";//最初默认橙色
    private const string ColorRed = "<color=#FF0000FF>";//红色
    private const string ColorGreen = "<color=#00FF00FF>";//绿色
    private const string ColorBlue = "<color=#00B4FFFF>";//蓝色
    private const string ColorYellow = "<color=#FFFF00FF>";//黄色
    private const string ColorEnd = "</color>";//颜色结束标签
    /// <summary>
    /// 设置文本颜色
    /// </summary>
    /// <param name="str"></param>
    /// <param name="txtColor"></param>
    /// <returns></returns>
    public static string SetTxtColor(string str, TxtColor txtColor)
    {
        string result = "";
        switch (txtColor)
        {
            case TxtColor.Red: result = ColorRed + str + ColorEnd; break;
            case TxtColor.Green: result = ColorGreen + str + ColorEnd; break;
            case TxtColor.Blue: result = ColorBlue + str + ColorEnd; break;
            case TxtColor.Yellow: result = ColorYellow + str + ColorEnd; break;
            default: result = ColorDefault + str + ColorEnd; break;
        }
        return result;
    }
}

/// <summary>
/// 文本颜色枚举类
/// </summary>
public enum TxtColor
{
    Red,
    Green,
    Blue,
    Yellow
}