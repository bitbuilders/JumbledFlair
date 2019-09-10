using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField] Animator m_animator = null;

    [SerializeField] Rigidbody2D m_rigidbody = null;

    [Tooltip("You will never go any faster than this, sorry")]
    [SerializeField, Range(0.0f, 100.0f)] float m_maxSpeed = 0.75f;

    [Tooltip("Percentage of how fast it will take to get up to Max Speed (1.0 = 1 second, 2.0 = 0.5 seconds)")]
    [SerializeField, Range(0.01f, 10.0f)] float m_acceleration = 1.0f;

    Player m_player = null;
    Vector2 m_velocity = Vector2.zero;
    float m_xInput = 0.0f;
    float m_yInput = 0.0f;

    private void Awake()
    {
        m_player = GetComponent<Player>();
    }

    private void Update()
    {
        float xInpt = Input.GetAxis("Horizontal");
        float yInpt = Input.GetAxis("Vertical");
        float absXInpt = Mathf.Abs(xInpt);
        float absYInpt = Mathf.Abs(yInpt);
        
        if (absXInpt > 0.0f) m_xInput = Mathf.Sign(xInpt);
        if (absYInpt > 0.0f) m_yInput = Mathf.Sign(yInpt);
        
        if (absXInpt > 0.0f || absYInpt > 0.0f)
        {
            if (absXInpt >= absYInpt)
            {
                m_yInput = 0.0f;
            }
            else
            {
                m_xInput = 0.0f;
            }
        }

        m_animator.SetFloat("xInpt", m_xInput);
        m_animator.SetFloat("yInpt", m_yInput);
        m_animator.SetFloat("runSpeed", m_velocity.sqrMagnitude / (m_maxSpeed * m_maxSpeed));
        m_animator.SetBool("running", m_velocity.sqrMagnitude > 5.0f);
        if (Input.GetButtonDown("Attack"))
        {
            m_animator.SetTrigger("attack");
            UpdateLookDirection();
        }
    }

    void UpdateLookDirection()
    {
        Vector2 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mPos - (Vector2)m_player.transform.position;
        m_player.LookDir = dir.normalized;

        m_animator.SetFloat("xLook", m_player.LookDir.x);
        m_animator.SetFloat("yLook", m_player.LookDir.y);
    }

    private void FixedUpdate()
    {
        Vector2 force = Vector2.zero;

        float xInpt = Input.GetAxis("Horizontal");
        float yInpt = Input.GetAxis("Vertical");

        float acc = m_maxSpeed * m_acceleration;

        if (Mathf.Abs(xInpt) > 0.0f)
        {
            m_velocity.x += acc * Time.deltaTime * Mathf.Sign(xInpt);
            force.x = m_velocity.x;
        }
        else
        {
            m_velocity.x = m_rigidbody.velocity.x;
        }

        if (Mathf.Abs(yInpt) > 0.0f)
        {
            m_velocity.y += acc * Time.deltaTime * Mathf.Sign(yInpt);
            force.y = m_velocity.y;
        }
        else
        {
            m_velocity.y = m_rigidbody.velocity.y;
        }

        if (m_velocity.sqrMagnitude > m_maxSpeed * m_maxSpeed)
        {
            m_velocity = m_velocity.normalized * m_maxSpeed;
        }

        m_rigidbody.AddForce(force, ForceMode2D.Force);
    }
}
