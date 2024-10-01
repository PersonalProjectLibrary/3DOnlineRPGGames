
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：Common_PETools.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/1 13:11
    功能：工具类
***************************************/
#endregion

/// <summary>
/// 工具类
/// </summary>
public class PETools
{
    /// <summary>
    /// 获取随机整数
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <param name="rd">某一个随机类/随机种子生成</param>
    /// <returns></returns>
    public static int RandomInt(int min,int max,System.Random rd =null)
    {
        if (rd == null) rd = new System.Random();
        int val = rd.Next(min, max + 1);
        return val;
    }
}