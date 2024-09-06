
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：Common_SystemRoot.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/8/31 15:39
    功能：业务系统基类
***************************************/
#endregion

using UnityEngine;

public class SystemRoot : MonoBehaviour
{
    protected ResService resService;
    protected AudioService audioService;

    public virtual void InitSystem()
    {
        resService = ResService.Instance;
        audioService = AudioService.Instance;

    }
}