
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：Common_LoopDragonAnim.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/8/31 16:41
    功能：飞龙循环动画
***************************************/
#endregion

using UnityEngine;

public class LoopDragonAnim : MonoBehaviour
{
    private Animation anim;

    private void Awake()
    {
        anim = GetComponent<Animation>();
    }
    void Start()
    {
        if (anim != null) InvokeRepeating("PlayDragonAnim", 0, 20);//以一定频率重复执行PlayDragonAnim()方法；
    }

    private void PlayDragonAnim()
    {
        if (anim != null) anim.Play();
    }
}