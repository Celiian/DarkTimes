using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementsController : MonoBehaviour
{

    [SerializeField] public bool m_FacingLeft;

    [SerializeField] public Rigidbody2D m_Rigidbody;

    [SerializeField] public float m_Speed;

    [SerializeField] public Animator m_Anim;

    [SerializeField] private bool m_Tp;
    [SerializeField] private float m_TpTriggerDistance;
    [SerializeField] private float m_TpTiming = 0;
    [SerializeField] private float m_TpDistance;
    [SerializeField] private float m_TpCoolDown = 0;


    [SerializeField] private bool m_AdditionalEffect;
    [SerializeField] private Animator m_AnimatorEffect;
    [SerializeField] private float m_EffectTiming;
    [SerializeField] private string m_Trigger;



    public bool _purchasing = false;
    private bool _tpCoolDown = false;
    private int _stunned = 0;
    private Vector2 _targetPosition;

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

        _targetPosition = targetPosition;

        var isTargetRight = _targetPosition.x - transform.position.x > 0;

        if (isTargetRight && m_FacingLeft)
        {
            Flip();
        }
        else if (!isTargetRight && !m_FacingLeft)
        {
            Flip();
        }


        var direction = (_targetPosition - (Vector2)transform.position).normalized;

        

        var distance = Vector2.Distance(_targetPosition, transform.position);

        if (m_Tp && distance > m_TpTriggerDistance && !_tpCoolDown)
        {
            
            if (m_AdditionalEffect)
            {
                Invoke(nameof(effect), m_EffectTiming);
            }
            Invoke(nameof(tp), m_TpTiming);
            
        }
        else
        {
            m_Rigidbody.velocity = new Vector2(direction.x * (m_Speed * speed), m_Rigidbody.velocity.y);
        }
    }


    private void tp()
    {
        

        var direction = m_FacingLeft ? -1 : 1;

        Vector2 newPosition = new Vector2(_targetPosition.x + (m_TpDistance * direction), _targetPosition.y);

        transform.position = newPosition;

        m_Rigidbody.velocity = new Vector2(0, 0);

        _tpCoolDown = true;
        Invoke(nameof(removeCoolDown), m_TpCoolDown);
    }

    private void removeCoolDown()
    {
        _tpCoolDown = false;
    }

    private void effect()
    {
        m_AnimatorEffect.SetTrigger(m_Trigger);
    }


}
