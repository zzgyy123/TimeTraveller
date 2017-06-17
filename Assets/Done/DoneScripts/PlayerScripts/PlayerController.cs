using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    // 走着走着就开始飘了…………

    private Animator m_Animator;
    private Locomotion m_Locomotion;
    private float m_Speed;
    private float m_Direction = 0;

    public bool hasLog = false;

    // Use this for initialization
    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Animator.logWarnings = false;
        m_Locomotion = new Locomotion(m_Animator);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Animator && Camera.main)
        {
            Vector3 worldDir;
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
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
        }

        m_Locomotion.UpdateDamp(m_Speed * 6, m_Direction * 360);
        m_Animator.SetBool("HoldLog", hasLog);
    }
}
