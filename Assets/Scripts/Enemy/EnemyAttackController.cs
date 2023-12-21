using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackController : MonoBehaviour
{
    [SerializeField] public float m_Strengh;
    [SerializeField] private float m_AttackCoolDown;
    [SerializeField] public float m_AttackRange;
    [SerializeField] public float m_MinAttackRange;
    [SerializeField] private string m_AttackType;
    [SerializeField] private float m_Timing;
    [SerializeField] private bool m_Impulse;
    [SerializeField] private float m_ImpulseTiming;
    [SerializeField] private bool m_Tp;
    [SerializeField] private float m_TpTiming;
    [SerializeField] private float m_TpDistance;
    [SerializeField] private bool m_AdditionalEffect;
    [SerializeField] private Animator m_AnimatorEffect;
    [SerializeField] private float m_EffectTiming;
    [SerializeField] private string m_Trigger;
    [SerializeField] private bool m_KeepVelocity;


    public bool _attackInCoolDown = false;
    private bool _facingLeft;
    private bool _isPlayerRight;
    private Transform _player;


    private EnemyMovementsController _movements;

    private void Start()
    {
        _movements = gameObject.GetComponent<EnemyMovementsController>();
    }


    private void Update()
    {
        if (_movements.wait)
        {
            return;
        }


        if (_movements.getStunned() > 0)
        {
            return;
        }

        var agroController = gameObject.GetComponent<EnemyAggroController>();

        _player = agroController.m_Player;

        _facingLeft = _movements.m_FacingLeft;

        var distanceToPlayer = Vector2.Distance(transform.position, _player.position);

        _isPlayerRight = _player.position.x - transform.position.x >= 0.1f;

        var facingPlayer = _isPlayerRight && !_facingLeft || !_isPlayerRight && _facingLeft;

        Vector2 raycastOrigin = transform.position;

        Vector2 raycastDirection = _isPlayerRight ? Vector2.right : Vector2.left;

        RaycastHit2D[] hits = Physics2D.RaycastAll(raycastOrigin, raycastDirection, m_AttackRange);

        if (distanceToPlayer < m_MinAttackRange)
        {
            return;
        }

        foreach (var hit in hits)
        {
            if (hit.collider != null && hit.collider.CompareTag("Joueur"))
            {
                if (!facingPlayer)
                {
                    _movements.Flip();
                }
                if (!_attackInCoolDown)
                {
                    if(m_Impulse)
                    {
                        Invoke(nameof(dash), m_ImpulseTiming);
                    }
                    if (m_Tp)
                    {
                        Invoke(nameof(tp), m_TpTiming);
                    }
                    if (m_AdditionalEffect)
                    {
                        Invoke(nameof(effect), m_EffectTiming);
                    }
                    _movements.m_Anim.SetTrigger(m_AttackType);

                    var controller = hit.collider.GetComponent<PlayerActionsController>();

                    var attackDirection = _movements.m_FacingLeft ? -1 : 1;

                    Vector2 currentVelocity = _movements.m_Rigidbody.velocity;

                    var velocity = currentVelocity.x > 2f ? currentVelocity.x / 2f : 1f;

                    var attackStrength = (m_Strengh) * velocity;

                    controller.takeHit(attackDirection, attackStrength, m_Timing);

                    _attackInCoolDown = true;
                    Invoke(nameof(coolDown), m_AttackCoolDown);


                    break;
                }
            }
        }

    }

    private void effect()
    {
        m_AnimatorEffect.SetTrigger(m_Trigger);
    }

    private void dash()
    {
        var attackDirection = _facingLeft ? -1 : 1;

        Vector2 currentVelocity = _movements.m_Rigidbody.velocity;

        Vector2 attackForce = new Vector2(attackDirection * Mathf.Abs(currentVelocity.x), currentVelocity.y);

        _movements.m_Rigidbody.AddForce(attackForce, ForceMode2D.Impulse);
    }

    private void tp()
    {
        var attackDirection = _facingLeft ? -1 : 1;

        Vector2 newPosition = new Vector2(_player.position.x + (m_TpDistance * attackDirection), transform.position.y);

        transform.position = newPosition;

        if (!m_KeepVelocity)
        {
            _movements.m_Rigidbody.velocity = new Vector2(0, 0);
        }
    }


    void coolDown()
    {
        _attackInCoolDown = false;
    }

}
