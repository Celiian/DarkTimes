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


 void Update()
    {
        var distanceToPlayer = Vector2.Distance(transform.position, m_Player.position);

        if (distanceToPlayer < m_aggroRange)
        {
            var direction = (m_Player.position - transform.position).normalized;
            m_Rigidbody.velocity = new Vector2(direction.x * m_Speed, m_Rigidbody.velocity.y);
            anim.SetBool("Move", true);

            if (direction.x > 0 && m_FacingLeft || direction.x < 0 && !m_FacingLeft)
            {
                Flip();
            }
        }
        else
        {
            m_Rigidbody.velocity = new Vector2(0, m_Rigidbody.velocity.y);
            anim.SetBool("Move", false);
        }
    }

    void Flip()
    {
        m_FacingLeft = !m_FacingLeft;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }
}
