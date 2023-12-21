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
    [SerializeField] public float m_TpDistance = 0;
    [SerializeField] public float m_TpCoolDown = 0;

    [SerializeField] private bool m_AdditionalEffect;
    [SerializeField] private Animator m_AnimatorEffect;
    [SerializeField] private float m_EffectTiming;
    [SerializeField] private string m_Trigger;



    public bool _purchasing = false;
    public bool wait = false;
    public bool _tpCoolDown = false;
    private int _stunned = 0;
    public Vector2 _targetPosition;
    private EnemyAggroController _aggro;
    private EnemyTpController _tp;

    public void Start()
    {
        _aggro = gameObject.GetComponent<EnemyAggroController>();
        _tp = gameObject.GetComponent<EnemyTpController>();
    }

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

        if (wait)
        {
            m_Rigidbody.velocity = new Vector2(0, 0);
            return;
        }

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


        var playerGrounded = _aggro.m_Player.GetComponent<PlayerMovementController>()._isGrounded;

        if (m_Tp && distance > m_TpTriggerDistance && !_tpCoolDown)
        {
            if (m_AdditionalEffect)
            {
                Invoke(nameof(effect), m_EffectTiming);
            }
            _tp.tp(_targetPosition, m_TpDistance, m_TpCoolDown);
        }
        else if (m_Tp && Mathf.Abs(_targetPosition.x - transform.position.x) < 0.5f && _targetPosition.y - transform.position.y > 2f && playerGrounded && !_tpCoolDown)
        {
            if (m_AdditionalEffect)
            {
                Invoke(nameof(effect), m_EffectTiming);
            }
            _tp.Disapear();
            _tp.m_AnimatorInitEffect.SetTrigger("tp");
            wait = true;

        }
        else
        {
            m_Rigidbody.velocity = new Vector2(direction.x * (m_Speed * speed), m_Rigidbody.velocity.y);
        }
    }


   

    private void effect()
    {
        m_AnimatorEffect.SetTrigger(m_Trigger);
    }


}
