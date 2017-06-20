using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterPushingStyle
{
    CharacterTranslateStyle,
    CharacterControllerMoveStyle
}

public enum BoxPushingStyle
{
    BoxTranslateStyle,
    BoxAddForceStyle
}

public enum VarMoveStyle
{
    moveDir,
    characterDirection
}
public class PushNew : MonoBehaviour
{
    Animator anim;
    Collider box;
    DoneHashIDs hash;
    CharacterController characterController;
    AnimatorStateInfo nowState;
    Vector3 varMoveVector;

    float speed;
    bool readyPush;
    bool hitBox;
    bool pushing;

    public CharacterPushingStyle characterPushingStyle = CharacterPushingStyle.CharacterTranslateStyle;
    public BoxPushingStyle boxPushingStyle = BoxPushingStyle.BoxTranslateStyle;
    public VarMoveStyle varMoveStyle = VarMoveStyle.moveDir;
    public float pushPower = 0.1f;
    public float speedDampTime = 0.1f;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        hash = GameObject.FindGameObjectWithTag(DoneTags.gameController).GetComponent<DoneHashIDs>();
        readyPush = anim.GetBool("ReadyPush");
    }

    // Update is called once per frame
    void Update()
    {
        if (anim == null)
            return;

        // 按下鼠标左键，若切换push变量
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            print("?");
            if (!readyPush)
                readyPush = true;
            else
                readyPush = false;
            anim.SetBool(hash.readyPushBool, readyPush);
        }

        nowState = anim.GetCurrentAnimatorStateInfo(0);
        // 因为readyPushState状态时可以按方向键脱离进入readyPushBackState
        // 所以要设定readyPush = false
        if (nowState.fullPathHash == hash.readyPushBackState)
        {
            readyPush = false;
            anim.SetBool(hash.readyPushBool, readyPush);
        }
    }

    void FixedUpdate()
    {
        // Cache the inputs.
        float vertical = Input.GetAxis("Vertical");

        // 只有当处于pushingState且按着W键时，人物才会移动
        // 不然处于pushingState不按W键的话就脱离
        if (nowState.fullPathHash == hash.pushingState)
        {
            if (vertical > 0.01f)
            {
                //SpeedManagement(vertical);
                Quaternion r = transform.rotation;
                Vector3 characterDirection = (transform.position + (r * transform.forward));
                print(transform.forward);
                Vector3 moveDir = transform.forward * vertical;

                if (varMoveStyle == VarMoveStyle.moveDir)
                {
                    varMoveVector = moveDir;
                }
                else if (varMoveStyle == VarMoveStyle.characterDirection)
                {
                    varMoveVector = characterDirection;
                }

                // 角色推箱子时角色的运动模式
                if (characterPushingStyle == CharacterPushingStyle.CharacterTranslateStyle)
                {
                    CharacterTranslateStyle(varMoveVector);
                }
                else if (characterPushingStyle == CharacterPushingStyle.CharacterControllerMoveStyle)
                {
                    CharacterControllerMoveStyle(varMoveVector);
                }

                // 角色推箱子时箱子的运动模式
                if (boxPushingStyle == BoxPushingStyle.BoxTranslateStyle)
                {
                    BoxTranslateStyle(varMoveVector);
                }
                else if (boxPushingStyle == BoxPushingStyle.BoxAddForceStyle)
                {
                    BoxAddForceStyle(varMoveVector);
                }
            }
            else
            {
                readyPush = false;
                anim.SetBool(hash.readyPushBool, readyPush);
            }
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Box")
        {
            box = other;
            hitBox = true;
            anim.SetBool(hash.hitBoxBool, hitBox);
        }
        //else return;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Box")
        {
            hitBox = false;
            anim.SetBool(hash.hitBoxBool, hitBox);
        }
    }

    void CharacterTranslateStyle(Vector3 moveDir)
    {
        transform.Translate(moveDir * Time.deltaTime);
    }

    void CharacterControllerMoveStyle(Vector3 moveDir)
    {
        characterController.Move(moveDir * Time.deltaTime);
    }

    void BoxTranslateStyle(Vector3 moveDir)
    {
        box.transform.Translate(moveDir * Time.deltaTime);
    }

    void BoxAddForceStyle(Vector3 moveDir)
    {
        Rigidbody rb = box.GetComponent<Rigidbody>();
        rb.AddForce(moveDir);
    }

    /*
    void SpeedManagement(float vertical)
    {
        Vector3 worldDir;
        Vector3 dir = new Vector3(0, 0, vertical);
        worldDir = Camera.main.transform.rotation * dir;
        worldDir.y = 0;
        worldDir.Normalize();

        speed = Mathf.Clamp(worldDir.magnitude, 0, 1);
        anim.SetFloat(hash.speedFloat, speed, speedDampTime, Time.deltaTime);
    }*/
}