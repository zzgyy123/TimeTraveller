using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    private Animator anim;              // Reference to the animator component.
    private DoneHashIDs hash;			// Reference to the HashIDs.
    private float m_Speed;
    private float m_Direction = 0;

    public float speedDampTime = 0.1f;
    public float directionDampTime = 0.1f;

    // Use this for initialization
    void Start () {
        // Setting up the references.
        anim = GetComponent<Animator>();
        hash = GameObject.FindGameObjectWithTag(DoneTags.gameController).GetComponent<DoneHashIDs>();
    }

    void FixedUpdate()
    {
        // Cache the inputs.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        MovementManagement(h, v);
    }

    // Update is called once per frame
    void Update()
    {
        //AudioManagement
    }

    void MovementManagement(float horizontal, float vertical)
    {
        Vector3 worldDir;
        Vector3 dir = new Vector3(horizontal, 0, vertical);
        worldDir = Camera.main.transform.rotation * dir;
        worldDir.y = 0;
        worldDir.Normalize();

        float speed = Mathf.Clamp(worldDir.magnitude, 0, 1);
        float direction = 0.0f;

        if (speed > 0.01f)
        {
            Vector3 axis = Vector3.Cross(transform.forward, worldDir);
            direction = Vector3.Angle(transform.forward, worldDir) / 180.0f * (axis.y < 0 ? -1 : 1);
        }

        m_Speed = speed;
        m_Direction = direction;
        UpdateDampTime(m_Speed * 6, m_Direction * 180, speedDampTime, directionDampTime);
    }

    // 根据动画状态更新动画参数值
    void UpdateDampTime(float speed, float direction, float speedDampTime, float directionDampTime)
    {
        anim.SetFloat(hash.speedFloat, speed, speedDampTime, Time.deltaTime);
        anim.SetFloat(hash.directionFloat, direction, directionDampTime, Time.deltaTime);
    }
}
