using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Push : MonoBehaviour
{
    // 该脚本要配合animator里的状态机及其转换条件一起看
    // 思路是push为true，进入readyPush状态，若此时hitBox也为true，便进入pushing状态
    // 但由于pushing动画不带实际位移，需按W键或up键使人和箱子一起前进
    // 人和箱子的实际位移是独立的，我不怎么会写……效果不太理想，
    // 一旦pushPower比较大，人就追不上箱子了，不过也没有必要那么大吧…………
    /* ====================================== */

    // animator组件 
    // 动画状态机的状态
    // CharacterController组件
    Animator animator;
    AnimatorStateInfo state;
    CharacterController characterController;

    // 判断是否进入推箱子准备状态，用来调整animator里的同名参数
    // 判断是否碰撞到箱子，用来调整animator里的同名参数
    // 判断是否处于pushing状态
    // 竖直轴，设定按W或up键才能推得动箱子  
    bool push;
    bool hitBox;
    bool pushing;
    float vertical;

    // 推动力
    public float pushPower = 2.0f;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (animator == null)
            return;

        // 获取animator参数push
        // 获取竖直轴输入
        // 获取当前动画状态
        // 判断当前是否为pushing状态
        push = animator.GetBool("Push");
        vertical = Input.GetAxis("Vertical");
        state = animator.GetCurrentAnimatorStateInfo(0);
        bool pushing = state.IsName("Pushing");

        // 按下鼠标左键，若切换push变量
        if (Input.GetMouseButton(0))
        {
            if (!push)
            {
                push = true;              
            }
            else
            {
                push = false;
               // if (pushing)
                 //   animator.SetBool("HitBox", false);
            }
            animator.SetBool("Push", push);
        }

        // 只有当处于pushing状态且按着W键时，人物才会移动
        if (pushing && (vertical > 0.0f))
            CharacterPushing();
    }

    // 此函数于CharacterController碰到一个可执行移动的碰撞器时被调用
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // 也是获取当前状态，为避免混淆，弄多一个，感觉应该可以更简单
        AnimatorStateInfo nowState = animator.GetCurrentAnimatorStateInfo(0);
        bool pushing = nowState.IsName("Pushing");

        Rigidbody body = hit.collider.attachedRigidbody;
        if (body == null || body.isKinematic)
        {
            hitBox = false;
            return;
        }   
        if (hit.moveDirection.y < -0.3f)
        {
            hitBox = false;
            return;
        }

        // 判断碰撞体是否带有标签box,是的话就更改HitBox参数
        // 有个小瑕疵是，当玩家碰撞到box后不论什么状态，HitBox参数仍为true
        // 这样就可以产生空推现象，玩家可凭空进入pushing状态并可以向前推
        // 除非玩家碰到另一个不是box的碰撞体,改变HitBox参数
        GameObject hitObj = hit.gameObject;
        if (hitObj.tag == "Box")
        {
            hitBox = true;
            animator.SetBool("HitBox", hitBox);
        }
        else
        {
            hitBox = false;
            animator.SetBool("HitBox", hitBox);
            return;
        }

        // 同上，改变箱子的位置
        if (pushing && (vertical > 0.0f))
        {
            // CharacterPushing(); // 不知道为什么，两个移动放在一起，就推不了了
            Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);
            body.velocity = pushDir * pushPower;
        }       
    }
   
    void CharacterPushing()
    {
        Vector3 moveDir = transform.forward * vertical;
        characterController.Move(moveDir * Time.deltaTime);
    }
}