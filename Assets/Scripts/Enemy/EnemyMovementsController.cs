using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementsController : MonoBehaviour
{

    [SerializeField] public bool m_FacingLeft;

    [SerializeField] public Rigidbody2D m_Rigidbody;

    [SerializeField] public float m_Speed;

    [SerializeField] public Animator m_Anim;


    public bool _purchasing = false;

    private int _stunned = 0;

    public void stun(float duration)
    {
        _stunned = (int)duration * 60;
    }

    public float getStunned()
    {
        return _stunned;
    }

    public void Flip()
    {
        if (m_FacingLeft)
        {
            transform.localScale = new Vector3(-1, 1, 1);
            m_FacingLeft = false;
        }
        else
        {
            m_FacingLeft = true;
            transform.localScale = Vector3.one;

        }
    }

    private void Update()
    {
        m_Anim.SetBool("Move", Mathf.Abs(m_Rigidbody.velocity.x) > 0.1f);

        if(_stunned > 0)
        {
            _stunned -= 1;
        }
    }

    public void MoveToLocation(Vector2 targetPosition, float speed)
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

        m_Rigidbody.velocity = new Vector2(direction.x * (m_Speed * speed), m_Rigidbody.velocity.y);
    }

}
