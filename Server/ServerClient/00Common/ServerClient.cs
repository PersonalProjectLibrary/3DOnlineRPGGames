
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：00Common_ServerClient.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/6 20:55:00
    功能：服务器入口
***************************************/
#endregion

public class ServerClient
{
    static void Main(string[] args)
    {
        ServerRoot.Instance.Init();
        while (true) { }//保持服务器一直运行，不退出
    }
}
