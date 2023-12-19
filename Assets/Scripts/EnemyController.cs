using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    [SerializeField]
    private float m_Speed;

    [SerializeField]
    private Rigidbody2D m_Rigidbody;

    [SerializeField]
    private Transform m_Player;

    [SerializeField]
    private float m_aggroRange = 5f;

    [SerializeField]
    private bool m_FacingLeft;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private float m_PatrolDistance = 1f;

    private Vector2 m_OriginalPosition;
    private Vector2 m_PatrolTarget;
    private bool m_IsPatrolling = false;

    private bool m_IsFacingPlayer = false;


    void Start()
    {
        m_OriginalPosition = transform.position;
        m_PatrolTarget = m_FacingLeft ? m_OriginalPosition - new Vector2(m_PatrolDistance, 0f) : m_OriginalPosition + new Vector2(m_PatrolDistance, 0f);
    }
    void Update()
    {
        var distanceToPlayer = Vector2.Distance(transform.position, m_Player.position);

        if (distanceToPlayer < m_aggroRange)
        {
            m_IsPatrolling = false;
            var direction = (m_Player.position - transform.position).normalized;
            m_Rigidbody.velocity = new Vector2(direction.x * m_Speed, m_Rigidbody.velocity.y);
            anim.SetBool("Move", true);

            if (!m_IsFacingPlayer)
            {
                Flip();
            }
            m_IsFacingPlayer = true;
        }
        else
        {
            m_IsFacingPlayer = false;
            if (!m_IsPatrolling)
            {
                StartCoroutine(Patrol());
            }
        }
    }

    IEnumerator Patrol()
    {
        m_IsPatrolling = true;

        while (Vector2.Distance(transform.position, m_PatrolTarget) > 0.1f)
        {
            var direction = (m_PatrolTarget - (Vector2)transform.position).normalized;
            m_Rigidbody.velocity = new Vector2(direction.x * m_Speed, m_Rigidbody.velocity.y);
            anim.SetBool("Move", true);

            yield return null;
        }

        m_Rigidbody.velocity = Vector2.zero;
        Flip();

        yield return new WaitForSeconds(0.5f);

        m_PatrolTarget = m_FacingLeft ? m_OriginalPosition - new Vector2(m_PatrolDistance, 0f) : m_OriginalPosition + new Vector2(m_PatrolDistance, 0f);
        m_IsPatrolling = false;
    }

    // void Flip()
    // {
    //     if (m_IsFacingPlayer)
    //     {
    //         m_FacingLeft = !m_FacingLeft;
    //     }
    //     transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    // }

        void Flip()
    {
        // if (m_IsFacingPlayer)
        // {
        //     m_FacingLeft = !m_FacingLeft;
        // }
        if(m_FacingLeft)
        {
            transform.localScale = Vector3.one;
             m_FacingLeft = false;

        }
      else {
            m_FacingLeft = true;
            transform.localScale = new Vector3(-1, 1, 1);
       }
    }  
}
