
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：Service_NetService.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/7 0:59
    功能：网络服务
***************************************/
#endregion

using PENet;
using PEProtocol;
using UnityEngine;

public class NetService : MonoBehaviour
{
    public static NetService Instance = null;

    PESocket<ClientSession, GameMsg> client;

    /// <summary>
    /// 网络服务初始化
    /// </summary>
    public void InitService()
    {
        Instance = this;
        PECommon.Log("Init NetService...");

        client = new PESocket<ClientSession, GameMsg>();
        
        //设置客户端日志接口
        client.SetLog(true, (string msg, int lv) =>
        {
            switch (lv)
            {
                case 0:
                    msg = "Log：" + msg;
                    Debug.Log(msg);
                    break;
                case 1:
                    msg = "Warn：" + msg;
                    Debug.LogWarning(msg);
                    break;
                case 2:
                    msg = "Error：" + msg;
                    Debug.LogError(msg);
                    break;
                case 3:
                    msg = "Info：" + msg;
                    Debug.Log(msg);
                    break;
            }
        });
        client.StartAsClient(SrvCfg.srvIP, SrvCfg.srvPort);//启动客户端
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))//测试和服务器通信
            client.session.SendMsg(new GameMsg { text = "hello unity" });
    }
}