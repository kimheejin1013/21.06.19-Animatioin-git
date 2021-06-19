using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlayer : MonoBehaviour
{
    public Animator m_Animator;
    // = [SerializeField] public ... �� ����
    // Inspectorâ�� ���� ������ [System.NonSerialize]�� ����

    public float m_Speed;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        m_Animator.SetBool("is Move", false);

        if (Input.GetKey(KeyCode.W))
        {
            m_Animator.SetBool("is Move", true);
            transform.Translate(0, 0, -m_Speed);
        }
        else if(Input.GetKey(KeyCode.A))
        {
            m_Animator.SetBool("is Move", true);
            transform.Translate(m_Speed, 0, 0);
        }
        else if(Input.GetKey(KeyCode.S))
        {
            m_Animator.SetBool("is Move", true);
            transform.Translate(0, 0, m_Speed);
        }
        else if(Input.GetKey(KeyCode.D))
        {
            m_Animator.SetBool("is Move", true);
            transform.Translate(-m_Speed, 0, 0);
        }
        
}
