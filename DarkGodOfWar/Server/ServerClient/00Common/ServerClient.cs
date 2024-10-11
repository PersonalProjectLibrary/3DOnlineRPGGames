
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：00Common_ServerClient.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/6 20:55:00
    功能：服务器入口
***************************************/
#endregion

using System.Threading;

/// <summary>
/// 服务器入口
/// </summary>
public class ServerClient
{
    static void Main(string[] args)
    {
        ServerRoot.Instance.Init();
        //死循环保持服务器一直运行，不退出，同时执行服务器上要一直执行的功能
        while (true)
        {
            ServerRoot.Instance.Update();
            Thread.Sleep(20);
        }
    }
}
