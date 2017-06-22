using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ACStyle
{
    AnimWithStealthOffsetStyle,
    AnimWithTTTranslateStyle,
    AnimWithTTRigidVelocityStyle,
    AnimWithTTCControllerStyle
}

public class PlayerMovement02 : MonoBehaviour
{

    Animator anim;              // Reference to the animator component.
    DoneHashIDs hash;			// Reference to the HashIDs.
    CharacterController characteController;  // Reference to the CharacterController component.
    Rigidbody rigidbody;

    public ACStyle acStyle = ACStyle.AnimWithStealthOffsetStyle;
    public float turnSmoothing = 15f;	// A smoothing value for turning the player.
    public float movePower = 2f;
    public float speedDampTime = 0.1f;
    public float directionDampTime = 0.1f;

    // Use this for initialization
    void Start()
    {
        // Setting up the references.
        anim = GetComponent<Animator>();
        hash = GameObject.FindGameObjectWithTag(DoneTags.gameController).GetComponent<DoneHashIDs>();
        rigidbody = GetComponent<Rigidbody>();
        characteController = GetComponent<CharacterController>();
    }

    void FixedUpdate()
    {
        // Cache the inputs.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        switch (acStyle)
        {
            case ACStyle.AnimWithStealthOffsetStyle:
                OffsetMovementManagement(h, v);
                break;
            case ACStyle.AnimWithTTTranslateStyle:
                NoOffsetTranslateMovement(h, v);
                break;
            case ACStyle.AnimWithTTRigidVelocityStyle:
                NoOffsetRigidVelocityMovement(h, v);
                break;
            case ACStyle.AnimWithTTCControllerStyle:
                NoOffsetCControllerMovement(h, v);
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {

        //AudioManagement
    }

    void OffsetMovementManagement(float horizontal, float vertical)
    {
        Vector3 worldDir;
        Vector3 dir = new Vector3(horizontal, 0, vertical);
        //worldDir = Camera.main.transform.rotation * dir;
        worldDir = dir;
        worldDir.y = 0;
        worldDir.Normalize();

        float speed = Mathf.Clamp(worldDir.magnitude, 0, 1);
        float direction = 0.0f;

        if (speed > 0.01f)
        {
            Vector3 axis = Vector3.Cross(transform.forward, worldDir);
            direction = Vector3.Angle(transform.forward, worldDir) / 180.0f * (axis.y < 0 ? -1 : 1);
        }
        UpdateSpeedParam(speed * 6, speedDampTime);
        UpdateDirectionParam(direction * 180, directionDampTime);
    }

    // 运用此模式需开启CharacterController组件，无需禁用rigidbody组件
    void NoOffsetCControllerMovement(float horizontal, float vertical)
    {
        Vector3 moveDir = new Vector3(horizontal * movePower, 0, vertical * movePower);
        characteController.Move(moveDir * Time.deltaTime);

        // If there is some axis input...
        if (horizontal != 0f || vertical != 0f)
        {
            // ... set the players rotation and set the speed parameter to 5.5f.
            Rotating(horizontal, vertical);         
        }

        float speed = Mathf.Clamp(moveDir.magnitude, 0, 1);
        UpdateSpeedParam(speed * 6, speedDampTime);
    }

    void NoOffsetTranslateMovement(float horizontal, float vertical)
    {
        Vector3 moveDir = new Vector3(horizontal * movePower, 0, vertical * movePower);

        transform.Translate(moveDir * Time.deltaTime);
        /* 这里应该有主角面向旋转才对，但一转本地坐标系就发生变化，
         * 行走方向就又变了，不再是原来一开始瞄准的方向
        // If there is some axis input...
        if (horizontal != 0f || vertical != 0f)
        {
            // ... set the players rotation and set the speed parameter to 5.5f.
            Rotating(horizontal, vertical);
        }
        */

        float speed = Mathf.Clamp(moveDir.magnitude, 0, 1);
        UpdateSpeedParam(speed * 6, speedDampTime);
    }

    // 运用此模式需禁用CharacterController组件
    void NoOffsetRigidVelocityMovement(float horizontal, float vertical)
    {
        
        Vector3 moveDir = new Vector3(horizontal * movePower, 0, vertical * movePower);
        if (horizontal != 0f || vertical != 0f)
        {
            rigidbody.velocity = moveDir;
            Rotating(horizontal, vertical);
        }
           
        float speed = Mathf.Clamp(moveDir.magnitude, 0, 1);
        UpdateSpeedParam(speed * 6, speedDampTime);
    }


    // 更新Speed动画参数
    void UpdateSpeedParam(float speed, float speedDampTime)
    {
        anim.SetFloat(hash.speedFloat, speed, speedDampTime, Time.deltaTime);
    }

    // 更新Direction动画参数
    void UpdateDirectionParam(float direction, float directionDampTime)
    {
        anim.SetFloat(hash.directionFloat, direction, directionDampTime, Time.deltaTime);
    }

    void Rotating(float horizontal, float vertical)
    {
        // Create a new vector of the horizontal and vertical inputs.
        Vector3 targetDirection = new Vector3(horizontal, 0f, vertical);

        // Create a rotation based on this new vector assuming that up is the global y axis.
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, Vector3.up);

        // Create a rotation that is an increment closer to the target rotation from the player's rotation.
        Quaternion newRotation = Quaternion.Lerp(rigidbody.rotation, targetRotation, turnSmoothing * Time.deltaTime);

        // Change the players rotation to this new rotation.
        rigidbody.MoveRotation(newRotation);
    }
}
