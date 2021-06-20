using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public Animator m_Animator;
    // = [SerializeField] public ... 과 동일
    // Inspector창에 띄우기 싫으면 [System.NonSerialize]를 붙임

    public CharacterController m_Controller;
    public Transform m_CameraTransform;
    public float m_MoveSpeed;
    public float m_RotateSpeed;
    public float m_Gravity = 9.81f;
    public float m_JumpHeight = 2.0f;

    public Transform m_AimPoint;
    public GameObject m_BulletPrefab;
    public float m_ShootPower;

    public Transform m_BodyTransform;

    private Vector2 m_RotateAxis;
    private float m_FixRotateX = 0;
    private float m_GravityTimer = 0;
    private float m_VerticalVelocity = 0;
    private bool m_IsEnableCursor = false;

    private void Awake()
    {
        Vector3 euler = m_CameraTransform.eulerAngles;

        m_RotateAxis = new Vector2(euler.y, euler.x);
        m_FixRotateX = m_RotateAxis.x;

        EnableCursor(m_IsEnableCursor);
    }
    private void Update()
    {
        UpdateRotate();
        UpdateMove();

        if (Input.GetButtonDown("Fire1"))
        {
            GameObject go = Instantiate(m_BulletPrefab, m_AimPoint.position, m_AimPoint.rotation, null);
            go.GetComponent<Bullet>().Shoot(m_ShootPower);
        }



        if (Input.GetKeyDown(KeyCode.Escape))
            EnableCursor(!m_IsEnableCursor);
        
    }

    private void EnableCursor(bool isEnable)
    {
        Cursor.lockState = isEnable ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.visible = isEnable;

        m_IsEnableCursor = isEnable;

    }

    private void UpdateRotate()
    {
        m_RotateAxis.x += Input.GetAxis("Mouse X") * m_RotateSpeed;
        m_RotateAxis.y += Input.GetAxis("Mouse Y") * m_RotateSpeed;

        m_CameraTransform.localRotation = Quaternion.Euler(-m_RotateAxis.y, m_FixRotateX, 0);
        transform.localRotation = Quaternion.Euler(0, m_FixRotateX + m_RotateAxis.x, 0);

        m_BodyTransform.localRotation = Quaternion.Euler(m_RotateAxis.y, 0, 0);
    }
    private void UpdateMove()
    {
        //캐릭터가 땅에 붙었나?
        if (m_Controller.isGrounded) m_GravityTimer = 0.2f;
        if (m_GravityTimer > 0) m_GravityTimer -= Time.deltaTime;

        //캐릭터가 땅에 있고, 캐릭터가 점프하지 않고 있다면 (수직 낙하 값이 0보다 작다면)
        if (m_Controller.isGrounded & m_VerticalVelocity < 0)
            m_VerticalVelocity = 0;

        if(Input.GetButtonDown("Jump"))
        {
            //점프 중인지 아닌지 검사
            if(m_GravityTimer>0)
            {
                m_GravityTimer = 0;
                m_VerticalVelocity += Mathf.Sqrt(m_JumpHeight * 2.0f * m_Gravity);
            }
        }

        m_VerticalVelocity -= m_Gravity * Time.deltaTime;

        float vertical = Input.GetAxis("Vertical"); 
        float horizontal = Input.GetAxis("Horizontal");

        //수직과 수평 값의 방향을 갖기 위해 Vector3에다 넣어준다
        Vector3 direction = new Vector3(-horizontal, 0, -vertical);
        //transform.TransformDirection(Vector3)
        // > 로컬 공간에서 월드 공간으로의 방향을 준다
        // 로컬 = 내 자기 자신의 공간
        // 월드 = 부모를 포함한 공간
        direction = transform.TransformDirection(direction);
        direction *= m_MoveSpeed;

        direction.y = m_VerticalVelocity;
        if (direction.x != 0 && direction.z != 0) m_Animator.SetBool("is Move", true);
        else m_Animator.SetBool("is Move", false);

        m_Controller.Move(direction * Time.deltaTime);

    }
}
