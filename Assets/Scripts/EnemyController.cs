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
    private bool m_PatrolEnabled;

    [SerializeField]
    private bool m_RandomMovements;

    [SerializeField]
    private int m_RandomMovementsDistance;

    [SerializeField]
    private Transform m_Player;

    [SerializeField]
    private float m_aggroRange = 5f;

    [SerializeField]
    private bool m_FacingLeft;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private Animator animAttack;

    [SerializeField]
    private float m_PatrolDistance = 1f;

    private Vector2 m_OriginalPosition;
    private Vector2 m_PatrolTarget;

    private bool m_IsFacingPlayer = false;
    private Vector2 _RandomLocation = Vector2.zero;

    private System.Random random = new System.Random();


    void Start()
    {
        m_OriginalPosition = transform.position;
        m_PatrolTarget = m_FacingLeft ? m_OriginalPosition - new Vector2(m_PatrolDistance, 0f) : m_OriginalPosition + new Vector2(m_PatrolDistance, 0f);
    }

    void Update()
    {
        var distanceToPlayer = Vector2.Distance(transform.position, m_Player.position);

        var isPlayerRight = Vector2.Distance(m_Player.position, transform.position) <= 0.1f;

        UpdateFacingPlayer(isPlayerRight);

        anim.SetBool("Move", Mathf.Abs(m_Rigidbody.velocity.x) > 1);


        if (distanceToPlayer < m_aggroRange)
        {            
            MoveToLocation(m_Player.position);
            // Trigger event for animation enemy attack
            animAttack.SetTrigger("Attack");
        }
        else if(m_PatrolEnabled)
        {
            Patrol();    
        }
        else if(m_RandomMovements)
        {
            MoveRandom();
        }
    }

    void MoveRandom()
    {
        if (_RandomLocation != Vector2.zero)
        {
            MoveToLocation(_RandomLocation);

            if (Vector2.Distance(transform.position, _RandomLocation) <= 0.1f)
            {
                _RandomLocation = Vector2.zero;
            }
        }

        else
        {
            int randomNumber = random.Next(1, 11);
            if (randomNumber < 3)
            {
                int randDistance = random.Next(1, m_RandomMovementsDistance * 2);

                randDistance -= m_RandomMovementsDistance;

                _RandomLocation = new Vector2(transform.position.x + randDistance, transform.position.y);
            }


        }


    }

    void Patrol()
    {

        if (Vector2.Distance(transform.position, m_PatrolTarget) <= 0.1f)
        {
            m_PatrolTarget = !m_FacingLeft ? m_OriginalPosition - new Vector2(m_PatrolDistance, 0f) : m_OriginalPosition + new Vector2(m_PatrolDistance, 0f);
        }

        MoveToLocation(m_PatrolTarget);
    }

    void UpdateFacingPlayer(bool isPlayerRight)
    {
        if (!m_FacingLeft && isPlayerRight || m_FacingLeft && !isPlayerRight)
        {
            m_IsFacingPlayer = true;
        }
        else
        {
            m_IsFacingPlayer = false;
        }
    }

   
    void Flip()
    {
        if(m_FacingLeft)
        {
            transform.localScale = Vector3.one;
             m_FacingLeft = false;
        }
        else 
        {
            m_FacingLeft = true;
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void MoveToLocation(Vector2 targetPosition)
    {

        var isTargetRight = targetPosition.x - transform.position.x > 0;

        if (isTargetRight && m_FacingLeft)
        {
            Flip();
        }
        else if (!isTargetRight && !m_FacingLeft)
        {
            Flip();
        }


        var direction = (targetPosition - (Vector2)transform.position).normalized;

        m_Rigidbody.velocity = new Vector2(direction.x * m_Speed, m_Rigidbody.velocity.y);
    }
}
