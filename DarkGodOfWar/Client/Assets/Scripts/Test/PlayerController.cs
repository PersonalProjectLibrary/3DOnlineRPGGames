
#region 脚本功能、创建时间和文件作者
/**************************************
    文件：Test_PlayerController.cs
    作者：LoriaRujoy
    邮箱：2659635618@qq.com
    时间：2024/9/15 22:26
    功能：角色控制器
***************************************/
#endregion

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /// <summary>
    /// 跟随玩家的相机
    /// </summary>
    private Transform camTrans;
    /// <summary>
    /// 相机和角色之间的偏移量
    /// </summary>
    private Vector3 camOffset;

    /// <summary>
    /// 角色动画控制器
    /// </summary>
    public Animator anim;
    /// <summary>
    /// 角色控制器
    /// </summary>
    public CharacterController ctrl;
    /// <summary>
    /// 标记角色是否在移动
    /// </summary>
    private bool isMove = false;
    /// <summary>
    /// 角色的方向朝向
    /// </summary>
    private Vector2 dir = Vector2.zero;
    public Vector2 Dir
    {
        get { return dir; }
        set
        {
            if (value == Vector2.zero) isMove = false;
            else isMove = true;

            dir = value;
        }
    }

    private void Start()
    {
        camTrans = Camera.main.transform;
        camOffset = transform.position - camTrans.position;
    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector2 _dir = new Vector2(h, v).normalized;
        if (_dir != Vector2.zero) Dir = _dir;
        else Dir = Vector2.zero;

        if (isMove)
        {
            SetDir();
            SetMove();
            SetCamMove();
        }
    }

    /// <summary>
    /// 设置角色的方向朝向
    /// </summary>
    private void SetDir()
    {
        //从目标角度dir，到当前（初位置角色z轴正方向/角色正前方）朝向，之间的角度偏移量
        float angle = Vector2.SignedAngle(Dir, new Vector2(0, 1));//计算画布屏幕内角度偏移量
        Vector3 eulerAngles = new Vector3(0, angle, 0);//根据角度偏移量，计算场景里旋转角度
        transform.localEulerAngles = eulerAngles;//设置角色旋转角度
    }

    /// <summary>
    /// 设置角色的移动
    /// </summary>
    private void SetMove()
    {
        //使用角色控制器里的Move接口,前面SetDir()设置好了角色的运动方向，这里直接往前移动就好
        ctrl.Move(transform.forward * Time.deltaTime * Constants.PlayerMoveSpeed);
    }

    /// <summary>
    /// 设置相机的跟随移动
    /// </summary>
    private void SetCamMove()
    {
        if (camTrans != null) camTrans.position = transform.position - camOffset;
    }
}